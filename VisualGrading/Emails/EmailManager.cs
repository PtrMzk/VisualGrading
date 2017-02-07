using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Unity;
using OxyPlot.Wpf;
using VisualGrading.Charts;
using VisualGrading.DataAccess;
using VisualGrading.Grades;
using VisualGrading.Helpers;
using VisualGrading.Settings;
using VisualGrading.Students;

namespace VisualGrading.Emails
{
    public class EmailManager : IEmailManager
    {
        #region Fields

        //todo: format HTML so its easier to read
        private const string EMAIL_SUCCESS = "Email sent successfully.";
        private const string TEST_SUBJECT = "Test Message.";
        private const string STUDENT_SUBJECT = "{0} Results";
        private const string VISUAL_GRADING_PREFIX = "Visual Grading: {0}";

        private const string MESSAGE_FORMAT =
            @"<div style=""text-align: left; font-family: 'Segoe UI', 'sans-serif'""> {0} </div> <br/> {1}";

        private const string TEST = "Test";
        private const string POINTS = "Points";
        private const string GRADE = "Grade";

        private const string HTML_WRAPPER_TABLE_CONSTRUCTOR =
            @"<table><tr><td>{0}</td></tr> <tr><td>{1}</td></tr> <tr><td><img src=""cid:{2}""/></td></tr> </table>";

        private const string HTML_GRADES_TABLE_CONSTRUCTOR =
            @"<div class=""container"" align: center>
            <table border = ""0"" align=""left"" style=""font-family: 'Segoe UI', 'sans-serif'; border-color: #cccccc; width:500px"">
            {0}
            {1}
            {2}
            </table>
            </div>";

        private const string HTML_HEADER_ROW =
            @"<tr style = ""background-color: #195E63; color: #FFFFFF; font-weight: lighter; text-align: center"">
            <th width=""34%"">{0}</th><th width=""33%"">{1}</th><th width=""33%"">{2}</th></tr>";
        
        private const string HTML_DATA_ROWS =
            @"<tr style = "" text-align: center""><td>{0}</td><td>{1}/{2}</td><td>{3}</td></tr>";

        private const string HTML_OVERALL_GRADE =
            @"<tr style=""text-align: center""><div style=""font-family: 'Segoe UI', 'sans-serif'"">
            <td></td>
            <td><b>Overall Grade: {0}</b></td>
            <td></td>
            </div></tr>";

        private readonly ChartGenerator _chartGenerator;

        private readonly IDataManager _dataManager;

        #endregion

        #region Constructors

        public EmailManager()
        {
            _dataManager = ContainerHelper.Container.Resolve<IDataManager>();
            _chartGenerator = new ChartGenerator();
        }

        #endregion

        #region Public Methods

        public async Task SendEmail(Student student, List<Grade> grades)
        {
            var gradesTable = GenerateGradesTable(grades, student.OverallGrade);

            var studentSubject = string.Format(STUDENT_SUBJECT, student.FullName);

            //todo: this pull grades from the DB a second time, maybe an overload that just takes grades
            var chart = _chartGenerator.ChartByTests(student);

            using (var memoryStream = new MemoryStream())
            {
                var pngExporter = new PngExporter() {Width= (grades.Count * 256), Height= 700 };
                pngExporter.Export(chart, memoryStream);

                var gradesChart = new LinkedResource(memoryStream, "image/png");

                gradesChart.ContentStream.Position = 0;

                gradesChart.ContentId = Guid.NewGuid().ToString();

                await SendEmail(student.ParentEmailAddress, student.EmailAddress, studentSubject, gradesTable, null,
                    gradesChart);
            }
        }

        public async Task SendTestEmail(SettingsProfile customSettingsProfile)
        {
            await SendEmail(customSettingsProfile.EmailAddress, string.Empty, TEST_SUBJECT, string.Empty,
                customSettingsProfile);
        }

        #endregion

        #region Private Methods

        private string GenerateGradesTable(List<Grade> grades, decimal overallGrade)
        {
            var dataRows = new StringBuilder();

            //overall grade
            var overallGradeRow = string.Format(HTML_OVERALL_GRADE, overallGrade.ToString("0.0%"));

            //header rows
            var headerRow = string.Format(HTML_HEADER_ROW, TEST, POINTS, GRADE);

            //data rows
            foreach (var grade in grades)
                dataRows.AppendFormat(HTML_DATA_ROWS, grade.Test.Name,
                    grade.Points, grade.Test.MaximumPoints, grade.PercentAverage.ToString("0.0%"));

            //table construction
            var gradesTable = string.Format(HTML_GRADES_TABLE_CONSTRUCTOR, overallGradeRow, headerRow,
                dataRows);

            return gradesTable;
        }

        private async Task SendEmail(string to, string cc, string subject, string htmlContent,
            SettingsProfile customSettingsProfile = null, LinkedResource linkedResource = null)
        {
            SettingsProfile settingsProfile;
            var usingCustomSettingsProfile = false;

            if (customSettingsProfile == null)
            {
                settingsProfile = _dataManager.GetSettingsProfileWithPassword();
            }
            else
            {
                settingsProfile = customSettingsProfile;
                usingCustomSettingsProfile = true;
            }

            if (string.IsNullOrEmpty(to))
                to = settingsProfile.EmailAddress;

            try
            {
                using (var client = new SmtpClient())
                {
                    client.Port = settingsProfile.EmailPort;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(settingsProfile.EmailAddress,
                        settingsProfile.EmailPassword);
                    client.Host = settingsProfile.SMTPAddress;
                    client.EnableSsl = settingsProfile.EmailUsesSSL;

                    using (var message = new MailMessage())
                    {
                        message.IsBodyHtml = true;
                        message.From = new MailAddress(settingsProfile.EmailAddress);
                        message.To.Add(new MailAddress(to));
                        if (!string.IsNullOrEmpty(cc))
                            message.CC.Add(new MailAddress(cc));
                        message.Subject = string.Format(VISUAL_GRADING_PREFIX, subject);

                        if (linkedResource != null)
                        {
                            var messageHeader = string.Format(MESSAGE_FORMAT, settingsProfile.EmailMessage,
                                string.Empty);

                            var completeMessage = string.Format(HTML_WRAPPER_TABLE_CONSTRUCTOR, messageHeader,
                                htmlContent, linkedResource.ContentId);

                            var alternateView = AlternateView.CreateAlternateViewFromString(completeMessage, null,
                                "text/html");

                            alternateView.LinkedResources.Add(linkedResource);

                            message.AlternateViews.Add(alternateView);
                        }
                        else
                        {
                            var messageWithoutChart = string.Format(MESSAGE_FORMAT, settingsProfile.EmailMessage, htmlContent);

                            message.Body = messageWithoutChart;
                        }

                        await client.SendMailAsync(message);

                        //todo: this is not MVVM friendly, needs to route exceptions back up to view instead
                        MessageBox.Show(EMAIL_SUCCESS);
                    }
                }
            }
            catch (Exception ex)
            {
                //todo: this is not MVVM friendly, needs to route exceptions back up to view instead
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (!usingCustomSettingsProfile)
                    settingsProfile.EmailPassword.Dispose();
            }
        }

        #endregion
    }
}