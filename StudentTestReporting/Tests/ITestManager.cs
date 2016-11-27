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
        void Add(Test test);
        void Remove(Test test);
        event PropertyChangedEventHandler PropertyChanged;
    }
}
