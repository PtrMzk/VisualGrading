namespace VisualGrading.Model.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Grade")]
    public partial class GradeDTO : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public int? Points { get; set; }

        public virtual TestDTO Test { get; set; }

        [InverseProperty("ID")]
        [ForeignKey("Test")]
        public long TestID { get; set; }

        public virtual StudentDTO Student { get; set; }

        [InverseProperty("ID")]
        [ForeignKey("Student")]
        public long StudentID { get; set; }
    }
}
