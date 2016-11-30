using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;


public class Test
{
    #region Constructor
    public Test()
    {
        this.TestID = Guid.NewGuid();
    }
    #endregion

    #region Properties
    public Guid TestID {get; set;}
    public string Name { get; set; }
    public string Subject { get; set; }
    public string SubCategory { get; set; }
    public DateTime Date { get; set; }
    public int SeriesNumber { get; set; }
    #endregion
}

