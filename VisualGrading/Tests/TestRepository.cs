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
using VisualGrading.DataAccess;
using VisualGrading.DataAccess;
using VisualGrading.Helpers;
using Microsoft.Practices.Unity;
using StudentTestReporting.Helpers;

namespace VisualGrading.Tests
{
    public sealed class TestRepository : AbstractRepository, INotifyPropertyChanged, ITestRepository
    {
        #region Singleton Implementation

        private static TestRepository _instance = new TestRepository();

        private TestRepository()
        {
            _dataManager = ContainerHelper.Container.Resolve<IDataManager>();
            InitializeTestList();
        }

        public static TestRepository Instance
        {
            get
            {
                return _instance;
            }
        }
        #endregion

        #region Properties

        private IDataManager _dataManager;

        //private string TestFileLocation { get { return SettingRepository.TestFileLocation; } }

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
                    PropertyChanged(null, new PropertyChangedEventArgs("TestList"));
                }

            }
        }
        #endregion

        #region Methods

        private async void InitializeTestList()
        {
            if (!File.Exists(settingRepository.GetFileLocationByType<List<Test>>()))
            {
                List<Test> emptyTestList= new List<Test>();
                _dataManager.Save<List<Test>>(emptyTestList);
            }

            //TestList = _dataManager.Load<List<Test>>();
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
                tests = await _dataManager.LoadAsync<List<Test>>();
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

            //TODO: Come up with a better design for keeping TestRepository in line with wherever else needs this information
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
            await _dataManager.SaveAsync<List<Test>>(TestList);
        }

        public async void AddTestAsync(Test test)
        {
            TestList.Add(test);
            await _dataManager.SaveAsync<List<Test>>(TestList);
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
            await _dataManager.SaveAsync<List<Test>>(TestList);
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
            await _dataManager.SaveAsync<List<Test>>(TestList);
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
