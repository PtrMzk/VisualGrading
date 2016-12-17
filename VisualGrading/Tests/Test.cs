using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using VisualGrading.Presentation;

namespace VisualGrading.Tests
{
    public class Test
    {
        #region Constructors

        public Test()
        {
            //this.TestID = Guid.NewGuid();
        }

        public Test(string name, string subject, string subCategory, int seriesNumber)
        {
            this.TestID = Guid.NewGuid();
            this.Name = name;
            this.Subject = subject;
            this.SubCategory = subCategory;
            this.SeriesNumber = seriesNumber;

        }

        #endregion

        #region Properties

        public Guid TestID { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string SubCategory { get; set; }
        public DateTime Date { get; set; }
        public int SeriesNumber { get; set; }
        public int MaximumPoints { get; set; }

        #endregion
    }

}