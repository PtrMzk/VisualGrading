#region Header

// +===========================================================================+
// Visual Grading Source Code
// 
// Copyright (C) 2016-2017 Piotr Mikolajczyk
// 
// 2017-03-15
// Encryption.cs
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

using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

#endregion

namespace VisualGrading.DataAccess
{
    //https://weblogs.asp.net/jongalloway/encrypting-passwords-in-a-net-app-config-file

    internal sealed class Encryption
    {
        #region Fields

        private readonly byte[] _entropy;

        #endregion

        #region Constructors

        public Encryption()
        {
            var currentWindowsUser = WindowsIdentity.GetCurrent().Name;
            _entropy = Encoding.Unicode.GetBytes(currentWindowsUser);
        }

        #endregion

        #region Public Methods

        public SecureString DecryptByteArray(byte[] encryptedData)
        {
            try
            {
                var decryptedData = ProtectedData.Unprotect(encryptedData,
                    _entropy,
                    DataProtectionScope.CurrentUser);
                return ToSecureString(Encoding.Unicode.GetString(decryptedData));
            }
            catch
            {
                return new SecureString();
            }
        }

        public byte[] EncryptSecureString(SecureString input)
        {
            var encryptedData = ProtectedData.Protect(
                Encoding.Unicode.GetBytes(ToInsecureString(input)),
                _entropy,
                DataProtectionScope.CurrentUser);
            return encryptedData;
        }

        #endregion

        #region Private Methods

        private string ToInsecureString(SecureString input)
        {
            var returnValue = string.Empty;
            var ptr = Marshal.SecureStringToBSTR(input);
            try
            {
                returnValue = Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                Marshal.ZeroFreeBSTR(ptr);
            }
            return returnValue;
        }

        private SecureString ToSecureString(string input)
        {
            var secure = new SecureString();
            foreach (var c in input)
                secure.AppendChar(c);
            secure.MakeReadOnly();
            return secure;
        }

        #endregion
    }
}