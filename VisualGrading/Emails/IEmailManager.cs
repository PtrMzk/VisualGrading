using System.Collections.Generic;
using System.Threading.Tasks;
using VisualGrading.Grades;
using VisualGrading.Settings;
using VisualGrading.Students;

namespace VisualGrading.Emails
{
    public interface IEmailManager
    {
        Task SendEmail(Student student, List<Grade> grades);
        Task SendTestEmail(SettingsProfile customSettingsProfile);
    }
}