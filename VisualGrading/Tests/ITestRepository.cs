using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualGrading.Tests
{
    public interface ITestRepository
    {
        List<Test> TestList { get; set; }
        Task<List<Test>> GetTestsAsync();
        void AddTestAsync(Test test);
        void RemoveTest(Test test);
        void UpdateTestAsync(Test test);
        void AddTestSeriesAsync(TestSeries testSeries);
        event PropertyChangedEventHandler PropertyChanged;
        Test GetTestByID(int ID);

    }
}
