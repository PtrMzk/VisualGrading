using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security;
using StudentTestReporting.Annotations;
using VisualGrading.Presentation;

namespace VisualGrading.Settings
{
    public sealed class SettingsProfile : ValidatableBaseViewModel
    {
        #region Fields

        private string _emailAddress;
        private string _emailMessage;
        private SecureString _emailPassword;
        private int _emailPort;
        private bool _emailUsesSSL;
        private string _smtpAddress;

        #endregion

        public SettingsProfile()
        {
            //todo: without this, email password is sent as null since PasswordBox doesnt bind to it
            this.EmailPassword = new SecureString();
        }

        #region Properties

        public long ID { get; set; }

        [EmailAddress][Required]
        public string EmailAddress
        {
            get { return _emailAddress; }
            set
            {
                if (value.Equals(_emailAddress)) return;
                SetProperty(ref _emailAddress, value);
            }
        }

        [NotNull][Required]
        public SecureString EmailPassword
        {
            get { return _emailPassword; }
            set
            {
                if (value == _emailPassword) return;
                SetProperty(ref _emailPassword, value);
            }
        }

        [Required]
        public int EmailPort
        {
            get { return _emailPort; }
            set
            {
                if (value == _emailPort) return;
                SetProperty(ref _emailPort, value);
            }
        }

        public bool EmailUsesSSL
        {
            get { return _emailUsesSSL; }
            set
            {
                if (value == _emailUsesSSL) return;
                SetProperty(ref _emailUsesSSL, value);
            }
        }

        [Required]
        public string SMTPAddress
        {
            get { return _smtpAddress; }
            set
            {
                if (value == _smtpAddress) return;
                SetProperty(ref _smtpAddress, value);
            }
        }

        public string EmailMessage
        {
            get { return _emailMessage; }
            set
            {
                if (value == _emailMessage) return;
                SetProperty(ref _emailMessage, value);
            }
        }

        #endregion
    }
}