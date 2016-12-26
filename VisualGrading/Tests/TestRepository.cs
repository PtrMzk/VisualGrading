//using VisualGrading.Grades;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.IO;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Text;
//using System.Threading.Tasks;
//using VisualGrading.DataAccess;
//using VisualGrading.DataAccess;
//using VisualGrading.Helpers;
//using Microsoft.Practices.Unity;
//using StudentTestReporting.Helpers;

//namespace VisualGrading.Tests
//{
//    public sealed class TestRepository : AbstractRepository, INotifyPropertyChanged, ITestRepository
//    {
//        #region Singleton Implementation

//        private static TestRepository _instance = new TestRepository();

//        private TestRepository()
//        {
//            _dataManager = ContainerHelper.Container.Resolve<IDataManager>();
//            InitializeTestList();
//        }

//        public static TestRepository Instance
//        {
//            get
//            {
//                return _instance;
//            }
//        }
//        #endregion

//        #region Properties

//        private IDataManager _dataManager;

//        //private string TestFileLocation { get { return SettingRepository.TestFileLocation; } }

//        private List<Test> _testList;

//        public List<Test> TestList
//        {
//            get
//            {
//                return _testList;
//            }
//            set
//            {
//                if (_testList != value)
//                {
//                    _testList = value;
//                    PropertyChanged(null, new PropertyChangedEventArgs("TestList"));
//                }

//            }
//        }
//        #endregion

//        #region Methods

//        private async void InitializeTestList()
//        {
//            if (!File.Exists(settingRepository.GetFileLocationByType<List<Test>>()))
//            {
//                _dataManager.UpdateTest(new Test());
//            }

//            //TestList = _dataManager.Load<List<TestDTO>>();
//        }

//        public async Task<List<Test>> GetTestsAsync()
//        {

//            //TODO: This whole method needs a rewrite
//            if (TestList != null && TestList.Count > 0)
//            {
//                return TestList;
//            }

//            List<Test> tests = new List<Test>();

//            //TODO: Make this file location dependent on a setting
//            //TODO: TestDTO when the file and/or folder don't exist - causes issues

//            try
//            {
//                tests = await _dataManager.GetTestsAsync();
//            }
//            catch
//            {
//            }

//            if (tests == null || tests.Count == 0)
//            {
//                tests = new List<Test>();
//                //ObservableTests = new List<TestDTO>();
//                Test test = new Test() { Subject = "TestDTO TestDTO", SeriesNumber = 1 };
//                //test = BinarySerialization.ReadFromBinaryFile<TestDTO>(@"C:\Visual Studio Code\VisualGrading\VisualGrading\SaveFiles\test.json");
//                tests.Add(test);
//            }

//            //TODO: Come up with a better design for keeping TestRepository in line with wherever else needs this information
//            if (TestList == null)
//            {
//                TestList = tests;
//            }

//            return tests;
//        }

//        public async void UpdateTestAsync(Test updatedTest)
//        {
//            //foreach (Test cachedTest in TestList)
//            //{
//            //    if (cachedTest.ID == updatedTest.ID)
//            //    {
//            //        TestList.Remove(cachedTest);
//            //        TestList.Add(updatedTest);
//            //        break;
//            //    }
//            //}
//            _dataManager.UpdateTest(updatedTest);
//        }

//        public async void AddTestAsync(Test test)
//        {
//            _dataManager.UpdateTest(test);
//        }

//        public async void AddTestSeriesAsync(TestSeries testSeries)
//        {
//            var newTests = new List<Test>();
//            for (int i = 0; i < testSeries.TestCount; i++)
//            {
//                int seriesNumber = i + 1;
//                string testName = string.Format("{0}{1}", testSeries.Name, seriesNumber);
//                var test = new Test(testName, testSeries.Subject, testSeries.SubCategory, seriesNumber);
//                _dataManager.UpdateTest(test);

//            }
//        }

//        public async void RemoveTest(Test testToDelete)
//        {
//            _dataManager.DeleteTest(testToDelete);
//        }

//        public Test GetTestByID(int ID)
//        {
//            foreach (Test cachedTest in TestList)
//            {
//                if (cachedTest.ID == ID)
//                {
//                    return cachedTest;
//                }
//            }
//            return null;
//        }

//        public event PropertyChangedEventHandler PropertyChanged = delegate { };

//        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
//        {
//            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//        }

//        #endregion
//    }
//}
