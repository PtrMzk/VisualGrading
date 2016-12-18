using System.Threading.Tasks;
using VisualGrading.Students;
using VisualGrading.Tests;

namespace VisualGrading.Business
{
    public interface IBusinessManager
    {
        Task AddTestAsync(Test test);


        Task AddStudentAsync(Student student);
    }
}