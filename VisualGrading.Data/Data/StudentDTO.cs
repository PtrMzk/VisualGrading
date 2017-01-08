namespace VisualGrading.Model.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Student")]
    public partial class StudentDTO : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [StringLength(2147483647)]
        public string FirstName { get; set; }

        [StringLength(2147483647)]
        public string LastName { get; set; }

        [StringLength(2147483647)]
        public string Nickname { get; set; }

        [StringLength(2147483647)]
        public string EmailAddress { get; set; }

        [StringLength(2147483647)]
        public string ParentEmailAddress { get; set; }
        
    }
}
