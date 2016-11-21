using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StudentTestReporting.GraphicFrontEnd
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public virtual void SetProperty<T>(ref T member, T val,
            [CallerMemberName] string propertName = null)
        {
            if (object.Equals(member, val)) return;
            member = val;
            PropertyChanged(this, new PropertyChangedEventArgs(propertName));
        }

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
