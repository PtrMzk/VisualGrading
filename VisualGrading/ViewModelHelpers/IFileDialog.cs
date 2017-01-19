using Microsoft.Win32;

namespace VisualGrading.ViewModelHelpers
{
    public interface IFileDialog
    {
        OpenFileDialog OpenFileDialog { get; set; }
        SaveFileDialog SaveFileDialog { get; set; }
    }
}