﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentTestReporting.Tests
{
    public class TestSeries
    {
        #region Constructor 

        public TestSeries()
        {
            this.SeriesID = Guid.NewGuid();
        }
        #endregion

        #region Properties
        public Guid SeriesID { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string SubCategory { get; set; }
        public int Length { get; set; }
        #endregion

    }
}
