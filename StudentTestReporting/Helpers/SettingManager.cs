using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualGrading.Helpers
{
    public sealed class SettingManager
    {
        #region Singleton Implementation

        private static readonly SettingManager _instance = new SettingManager();

        private SettingManager()
        {
            //TODO: first load settings file..

            if (string.IsNullOrEmpty(SaveFileFolder))
            {
                SaveFileFolder = string.Format(LONG_APPEND_STRING, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), SLASH, VISUAL_GRADING, SLASH, SAVE_FILES, SLASH);
                
            }
        }

        public static SettingManager Instance
        {
            get { return _instance; }
        }

        #endregion

        #region Properties

        private const string SLASH = "/";
        private const string VISUAL_GRADING = "Visual Grading";
        private const string SAVE_FILES = "Save Files";
        private const string TEST_JSON = "test.json";
        private const string GRADE_JSON = "grade.json";
        private const string STUDENT_JSON = "student.json";
        private const string APPEND_STRING = "{0}{1}";
        private const string LONG_APPEND_STRING = "{0}{1}{2}{3}{4}{5}";

        public string SaveFileFolder { get; set; }

        public string TestFileLocation
        {
            get { return string.Format(APPEND_STRING, SaveFileFolder, TEST_JSON); }
        }

        public string GradeFileLocation
        {
            get { return string.Format(APPEND_STRING, SaveFileFolder, GRADE_JSON); }
        }

        public string StudentFileLocation
        {
            get { return string.Format(APPEND_STRING, SaveFileFolder, STUDENT_JSON); }
        }

        #endregion

        #region Methods
        public string GetFileLocationByType<T>()
        {
             return string.Format(APPEND_STRING, SaveFileFolder,(typeof(T).ToString().ToLower())); 
        }
        #endregion
    }
}
