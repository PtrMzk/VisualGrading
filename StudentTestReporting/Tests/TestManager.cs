using VisualGrading.Grades;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VisualGrading.Helpers;

namespace VisualGrading.Tests
{
    public sealed class TestManager : INotifyPropertyChanged, ITestManager
    {
        #region Singleton Implementation

        private static TestManager _instance = new TestManager();

        static TestManager()
        {
            Instance.InitializeTestList();
        }

        public static TestManager Instance
        {
            get
            {
                return _instance;
            }
        }
        #endregion

        #region Properties

        private string TestFileLocation { get { return SettingManager.Instance.TestFileLocation; } }

        private List<Test> _testList;

        public List<Test> TestList
        {
            get
            {
                return Instance._testList;
            }
            set
            {
                if (Instance._testList != value)
                {
                    Instance._testList = value;
                    Instance.PropertyChanged(null, new PropertyChangedEventArgs("TestList"));
                }

            }
        }
        #endregion

        #region Methods

        private async void InitializeTestList()
        {
            if (!File.Exists(TestFileLocation))
            {
                List<Test> emptyTestList= new List<Test>();
                await Helpers.JSONSerialization.SerializeJSONAsync(TestFileLocation, emptyTestList);
            }

            TestList = Helpers.JSONSerialization.DeserializeJSON<List<Test>>(TestFileLocation);
        }

        public async Task<List<Test>> GetTestsAsync()
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
                tests = await Helpers.JSONSerialization.DeserializeJSONAsync<List<Test>>(TestFileLocation);
            }
            catch
            {
            }

            if (tests == null || tests.Count == 0)
            {
                tests = new List<Test>();
                //ObservableTests = new List<Test>();
                Test test = new Test() { Subject = "Test Test", SeriesNumber = 1 };
                //test = BinarySerialization.ReadFromBinaryFile<Test>(@"C:\Visual Studio Code\VisualGrading\VisualGrading\SaveFiles\test.json");
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
            foreach (Test cachedTest in TestList)
            {
                if (cachedTest.TestID == updatedTest.TestID)
                {
                    TestList.Remove(cachedTest);
                    TestList.Add(updatedTest);
                    break;
                }
            }
            await JSONSerialization.SerializeJSONAsync(
            SettingManager.Instance.TestFileLocation, TestList);
        }

        public async void AddTestAsync(Test test)
        {
            TestList.Add(test);
            await JSONSerialization.SerializeJSONAsync(
                SettingManager.Instance.TestFileLocation, TestList);
        }

        public async void AddTestSeriesAsync(TestSeries testSeries)
        {
            var newTests = new List<Test>();
            for (int i = 0; i < testSeries.Length; i++)
            {
                int seriesNumber = i + 1;
                string testName = string.Format("{0}{1}", testSeries.Name, seriesNumber);
                var test = new Test(testName, testSeries.Subject, testSeries.SubCategory, seriesNumber);
                newTests.Add(test);
            }
            TestList.AddRange(newTests);
            await JSONSerialization.SerializeJSONAsync(
                SettingManager.Instance.TestFileLocation, TestList);
        }

        public async void RemoveTest(Test testToDelete)
        {
            foreach (Test cachedTest in TestList)
            {
                if (cachedTest.TestID == testToDelete.TestID)
                {
                    TestList.Remove(cachedTest);
                    break;
                }
            }
            await JSONSerialization.SerializeJSONAsync(
            SettingManager.Instance.TestFileLocation, TestList);
        }

        public Test GetTestByID(Guid testID)
        {
            foreach (Test cachedTest in TestList)
            {
                if (cachedTest.TestID == testID)
                {
                    return cachedTest;
                }
            }
            return null;
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
