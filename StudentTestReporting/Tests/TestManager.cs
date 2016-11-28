using StudentTestReporting.Grades;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using StudentTestReporting.Helpers;

namespace StudentTestReporting.Tests
{
    public sealed class TestManager : INotifyPropertyChanged, ITestManager
    {
        #region Singleton Implementation

        static readonly TestManager instance = new TestManager();

        static TestManager()
        {
            //TODO change this to read a list of ObservableTests
            //ObservableTests = new List<Test>();
            try
            {
                //TODO: Make this not use the the TestManager name
                //GradeManager.GenerateGrades(StudentManager.students, TestManager.ObservableTests);
            }
            catch
            {

            }

        }

        public static TestManager Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        private List<Test> _testList;

        public List<Test> TestList
        {
            get
            {
                return _testList;
            }
            set
            {
                if (_testList != value)
                {
                    _testList = value;
                    Instance.PropertyChanged(null, new PropertyChangedEventArgs("TestList"));
                }

            }
        }

        //public static List<Test> GetTestsCopy()
        //{
        //    List<Test> testsCopy = new List<Test>(ObservableTests);
        //    return testsCopy;
        //}
        

        public async Task<List<Test>> GetTestsAsync(string fileLocation)
        {
            //TODO: This whole method needs a rewrite
            if (TestList != null && TestList.Count > 0)
            {
                return TestList;
            }

            List<Test> tests = new List<Test>();

            //TODO: Make this file location dependent on a setting
            //TODO: Test when the file and/or folder don't exist - causes issues

            try
            {
                if (!File.Exists(fileLocation))
                {
                    await Helpers.JSONSerialization.SerializeJSONAsync(fileLocation, tests);
                }

                tests = await Helpers.JSONSerialization.DeserializeJSONAsync<List<Test>>(fileLocation);
            }
            catch
            {
            }

            if (tests == null || tests.Count == 0)
            {
                tests = new List<Test>();
                //ObservableTests = new List<Test>();
                Test test = new Test() { Subject = "Test Test", TestNumber = 1 };
                //test = BinarySerialization.ReadFromBinaryFile<Test>(@"C:\Visual Studio Code\StudentTestReporting\StudentTestReporting\SaveFiles\test.json");
                tests.Add(test);
            }

            //TODO: Come up with a better design for keeping TestManager in line with wherever else needs this information
            if (TestList == null)
            {
                TestList = tests; 
            }

            return tests;
        }

        public async void UpdateTestAsync(Test updatedTest)
        {
            foreach (Test currentTest in TestList)
            {
                if (currentTest.TestID == updatedTest.TestID)
                {
                    TestList.Remove(currentTest);
                    TestList.Add(updatedTest);
                    break;
                }
            }
            GradeManager.GenerateGrades(TestList);
            await JSONSerialization.SerializeJSONAsync(
    @"C:\Visual Studio Code\StudentTestReporting\StudentTestReporting\SaveFiles\test.json", TestList);
        }

        public async void AddTestAsync(Test test)
        {
            TestList.Add(test);
            GradeManager.GenerateGrades(TestList);
            await JSONSerialization.SerializeJSONAsync(
                @"C:\Visual Studio Code\StudentTestReporting\StudentTestReporting\SaveFiles\test.json", TestList);

        }

        public void RemoveTest(Test test)
        {
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
