using StudentTestReporting.Grades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StudentTestReporting.Tests
{
    public sealed class TestManager : INotifyPropertyChanged
    {
        #region Singleton Implementation

        static readonly TestManager instance = new TestManager();

        static TestManager()
        {
            //TODO change this to read a list of tests
            //tests = new List<Test>();
            try
            {
                //TODO: Make this not use the the TestManager name
                //GradeManager.GenerateGrades(StudentManager.students, TestManager.tests);
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

        public List<Test> TestList
        {
            get
            {
                return TestList;
            }
            set
            {
                if (TestList != value)
                {
                    TestList = value;
                    Instance.PropertyChanged(this, new PropertyChangedEventArgs("TestList"));
                }

            }
        }

        //public static List<Test> GetTestsCopy()
        //{
        //    List<Test> testsCopy = new List<Test>(tests);
        //    return testsCopy;
        //}
        

        public async Task<List<Test>> GetTestsAsync(string fileLocation)
        {
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
                //tests = new List<Test>();
                Test test = new Test() { Subject = "Test Test", TestNumber = 1 };
                //test = BinarySerialization.ReadFromBinaryFile<Test>(@"C:\Visual Studio Code\StudentTestReporting\StudentTestReporting\SaveFiles\test.bin");
                tests.Add(test);

            }

            return tests;
        }



        public void Add(Test test)
        {
            TestList.Add(test);
            GradeManager.GenerateGrades(TestList);
        }

        public void Remove(Test test)
        {
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
