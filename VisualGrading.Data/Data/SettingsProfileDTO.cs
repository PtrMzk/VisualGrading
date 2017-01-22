using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VisualGrading.Model.Data
{
    [Table("SettingsProfile")]
    public class SettingsProfileDTO : IEntity
    {
        public SettingsProfileDTO()
        {
            
        }
        #region Properties

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public string EmailAddress { get; set; }
        public byte[] EncryptedEmailPassword { get; set; }
        public int EmailPort { get; set; }
        public bool EmailUsesSSL { get; set; }
        public string SMTPAddress { get; set; }
        public string EmailMessage { get; set; }

        #endregion
    }
}