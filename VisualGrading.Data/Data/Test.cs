namespace VisualGrading.Model.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Test")]
    public partial class Test : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(2147483647)]
        public string Name { get; set; }

        [StringLength(2147483647)]
        public string Subject { get; set; }

        [StringLength(2147483647)]
        public string SubCategory { get; set; }

        public int? Date { get; set; }

        public long? SeriesNumber { get; set; }

        public long? MaximumPoints { get; set; }
    }
}
