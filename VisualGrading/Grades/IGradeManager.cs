using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using VisualGrading.Students;
using VisualGrading.Tests;

namespace VisualGrading.Grades
{
    public interface IGradeRepository
    {
        List<Grade> GradeList { get; set; }

        event PropertyChangedEventHandler PropertyChanged;

        void AddGradeAsync(Grade Grade);
        void AddGradesByStudentAsync(Student student);
        void AddGradesByTestAsync(Test test);
        //List<GradeDTO> GenerateGrades(List<StudentDTO> students, List<TestDTO> tests);
        Task<List<Grade>> GetGradesAsync();
        void RemoveGradesByStudentAsync(Student studentToRemove);
        void RemoveGradesByTestAsync(Test testToRemove);
        void UpdateGradeAsync(Grade updatedGrade);
    }
}