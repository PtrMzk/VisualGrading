using Microsoft.Win32;

namespace VisualGrading.Presentation
{
    public interface IFileDialog
    {
        OpenFileDialog OpenFileDialog { get; set; }
        SaveFileDialog SaveFileDialog { get; set; }
    }
}