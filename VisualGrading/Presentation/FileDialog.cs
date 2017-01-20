using Microsoft.Win32;

namespace VisualGrading.Presentation
{
    public class FileDialog : IFileDialog
    {
        #region Constructors

        public FileDialog()
        {
            OpenFileDialog = new OpenFileDialog();
            SaveFileDialog = new SaveFileDialog();
        }

        #endregion

        #region Properties

        public OpenFileDialog OpenFileDialog { get; set; }

        public SaveFileDialog SaveFileDialog { get; set; }

        #endregion
    }


}