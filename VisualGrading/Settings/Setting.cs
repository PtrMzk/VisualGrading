using System.ComponentModel;
using System.Runtime.CompilerServices;
using StudentTestReporting.Annotations;

namespace VisualGrading.Settings
{
    public sealed class SettingsProfile : INotifyPropertyChanged
    {
        #region Fields

        private string _emailAddress;
        private string _emailMessage;
        private string _emailPassword;
        private string _emailPort;
        private bool _emailUsesSsl;
        private string _smtpAddress;

        #endregion

        #region Constructors

        private SettingsProfile()
        {
            //TODO: first load settings file..
        }

        #endregion

        #region Properties

        public static SettingsProfile Instance { get; } = new SettingsProfile();

        public string EmailAddress
        {
            get { return _emailAddress; }
            set
            {
                if (value == _emailAddress) return;
                _emailAddress = value;
                OnPropertyChanged();
            }
        }

        public string EmailPassword
        {
            get { return _emailPassword; }
            set
            {
                if (value == _emailPassword) return;
                _emailPassword = value;
                OnPropertyChanged();
            }
        }

        public string EmailPort
        {
            get { return _emailPort; }
            set
            {
                if (value == _emailPort) return;
                _emailPort = value;
                OnPropertyChanged();
            }
        }

        public bool EmailUsesSSL
        {
            get { return _emailUsesSsl; }
            set
            {
                if (value == _emailUsesSsl) return;
                _emailUsesSsl = value;
                OnPropertyChanged();
            }
        }

        public string SMTPAddress
        {
            get { return _smtpAddress; }
            set
            {
                if (value == _smtpAddress) return;
                _smtpAddress = value;
                OnPropertyChanged();
            }
        }

        public string EmailMessage
        {
            get { return _emailMessage; }
            set
            {
                if (value == _emailMessage) return;
                _emailMessage = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Methods

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Interface Implementations

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}