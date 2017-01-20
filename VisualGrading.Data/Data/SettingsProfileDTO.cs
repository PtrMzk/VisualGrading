using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VisualGrading.Model.Data
{
    [Table("SettingsProfile")]
    public class SettingsProfileDTO : IEntity
    {
        public SettingsProfileDTO()
        {
            
            this.EmailAddress = string.Empty;
            this.EncryptedEmailPassword = new byte[1];
            this.EmailPort = string.Empty;
            this.EmailUsesSSL = false;
            this.SMTPAddress = string.Empty;
            this.EmailMessage = string.Empty;

        }
        #region Properties

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public string EmailAddress { get; set; }
        public byte[] EncryptedEmailPassword { get; set; }
        public string EmailPort { get; set; }
        public bool EmailUsesSSL { get; set; }
        public string SMTPAddress { get; set; }
        public string EmailMessage { get; set; }

        #endregion
    }
}