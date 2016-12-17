using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;


namespace VisualGrading.Students
{
    public interface IStudentRepository
    {
        List<Student> StudentList { get; set; }

        event Action OnStudentDelete;
        event Action OnStudentUpdate;
        event PropertyChangedEventHandler PropertyChanged;
        //TODO: This probably shouldnt reference the studentRepository class...
        event StudentRepository.OnStudentChangedEventHandler StudentAdded;

        void AddStudentAsync(Student Student);
        Task<List<Student>> GetStudentsAsync();
        void RemoveStudent(Student student);
        void UpdateStudentAsync(Student updatedStudent);
        Student GetStudentByID(int studentID);
        
    }
}