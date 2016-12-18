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
        public int ID { get; set; }

        public int? Points { get; set; }

        public int TestID { get; set; }
        
        public int StudentID { get; set; }
    }
}
