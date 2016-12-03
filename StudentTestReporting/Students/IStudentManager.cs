using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;


namespace VisualGrading.Students
{
    public interface IStudentManager
    {
        List<Student> StudentList { get; set; }

        event Action OnStudentDelete;
        event Action OnStudentUpdate;
        event PropertyChangedEventHandler PropertyChanged;
        //TODO: This probably shouldnt reference the studentManager class...
        event StudentManager.OnStudentChangedEventHandler StudentAdded;

        void AddStudentAsync(Student Student);
        Task<List<Student>> GetStudentsAsync();
        void RemoveStudent(Student StudentToDelete);
        void UpdateStudentAsync(Student updatedStudent);
        Student GetStudentByID(Guid studentID);
        
    }
}