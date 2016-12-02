using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace VisualGrading.Students
{
    public interface IStudentManager
    {
        List<Student> StudentList { get; set; }

        event PropertyChangedEventHandler PropertyChanged;

        void AddStudentAsync(Student Student);
        Task<List<Student>> GetStudentsAsync();
        void RemoveStudent(Student StudentToDelete);
        void UpdateStudentAsync(Student updatedStudent);
    }
}