namespace VisualGrading.Model.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class VisualGradingDBContext : DbContext
    {
        public VisualGradingDBContext()
            : base("name=VisualGradingDBContext")
        {
        }

        public virtual DbSet<Grade> Grades { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Test> Tests { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
