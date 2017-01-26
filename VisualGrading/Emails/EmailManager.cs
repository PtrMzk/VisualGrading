using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Unity;
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

        private const string EMAIL_SUCCESS = "Email sent successfully.";
        private const string TEST_SUBJECT = "Test Message.";
        private const string STUDENT_SUBJECT = "Student Results";
        private const string VISUAL_GRADING_PREFIX = "Visual Grading: {0}";
        private const string MESSAGE_FORMAT = "{0} <\br> {1}";

        private readonly IDataManager _dataManager;

        #endregion

        #region Constructors

        public EmailManager()
        {
            _dataManager = ContainerHelper.Container.Resolve<IDataManager>();
        }

        #endregion

        #region Public Methods

        public async Task SendEmail(Student student, List<Grade> grades)
        {
            var stringBuilder = new StringBuilder();

            foreach (var grade in grades)
                stringBuilder.AppendFormat("<\br> {0} || {1} || {2} || {3} || {4}", grade.Test.Name,
                    grade.Test.MaximumPoints, grade.Points, grade.PercentAverage, grade.Student.FullName);

            var dataPoints = stringBuilder.ToString();

            await SendEmail(student.ParentEmailAddress, student.EmailAddress, STUDENT_SUBJECT, dataPoints);
        }

        public async Task SendTestEmail(SettingsProfile customSettingsProfile)
        {
            await SendEmail(customSettingsProfile.EmailAddress, string.Empty, TEST_SUBJECT, string.Empty,
                customSettingsProfile);
        }

        #endregion

        #region Private Methods

        private async Task SendEmail(string to, string cc, string subject, string dataPoints,
            SettingsProfile customSettingsProfile = null)
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
                        message.Body = string.Format(MESSAGE_FORMAT, settingsProfile.EmailMessage, dataPoints);
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