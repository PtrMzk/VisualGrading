//https://weblogs.asp.net/jongalloway/encrypting-passwords-in-a-net-app-config-file

using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace VisualGrading.DataAccess
{
    internal class Encryption
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

        #region Methods

        public byte[] EncryptSecureString(SecureString input)
        {
            var encryptedData = ProtectedData.Protect(
                Encoding.Unicode.GetBytes(ToInsecureString(input)),
                _entropy,
                DataProtectionScope.CurrentUser);
            return encryptedData;
        }

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

        private SecureString ToSecureString(string input)
        {
            var secure = new SecureString();
            foreach (var c in input)
                secure.AppendChar(c);
            secure.MakeReadOnly();
            return secure;
        }

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

        #endregion
    }
}