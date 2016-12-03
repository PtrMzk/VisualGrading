using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using VisualGrading.Students;

namespace VisualGrading.Grades
{
    public interface IGradeManager
    {
        string GradeFileLocation { get; }
        List<Grade> GradeList { get; set; }

        event PropertyChangedEventHandler PropertyChanged;

        void AddGradeAsync(Grade Grade);
        void AddGradeByStudentAsync(Student student);
        void AddGradeByTestAsync(Test test);
        List<Grade> GenerateGrades(List<Student> students, List<Test> tests);
        Task<List<Grade>> GetGradesAsync();
        void RemoveGradeByStudentAsync(Student studentToRemove);
        void RemoveGradeByTestAsync(Test testToRemove);
        void UpdateGradeAsync(Grade updatedGrade);
    }
}