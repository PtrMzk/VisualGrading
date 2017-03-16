#region Header

// +===========================================================================+
// Visual Grading Source Code
// 
// Copyright (C) 2016-2017 Piotr Mikolajczyk
// 
// 2017-03-15
// SettingsProfile.cs
// 
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
// OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//  +===========================================================================+

#endregion

#region Namespaces

using System.ComponentModel.DataAnnotations;
using System.Security;
using StudentTestReporting.Annotations;
using VisualGrading.Presentation;

#endregion

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

        #region Constructors

        public SettingsProfile()
        {
            //todo: without this, email password is sent as null since PasswordBox doesnt bind to it
            EmailPassword = new SecureString();
        }

        #endregion

        #region Properties

        public long ID { get; set; }

        [EmailAddress]
        [Required]
        public string EmailAddress
        {
            get { return _emailAddress; }
            set
            {
                if (value.Equals(_emailAddress)) return;
                SetProperty(ref _emailAddress, value);
            }
        }

        [NotNull]
        [Required]
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