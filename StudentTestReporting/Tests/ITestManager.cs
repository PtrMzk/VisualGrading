using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentTestReporting.Tests
{
    public interface ITestManager
    {
        List<Test> TestList { get; set; }
        Task<List<Test>> GetTestsAsync(string fileLocation);
        void AddTestAsync(Test test);
        void RemoveTest(Test test);
        void UpdateTestAsync(Test test);

        event PropertyChangedEventHandler PropertyChanged;
    }
}
