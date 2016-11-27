using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;


public class Test
{
    public Guid TestID {get; set;}
    public string Subject { get; set; }
    public int TestNumber { get; set; }

    List<Test> Tests = new List<Test>();

    public Test()
    {
        this.TestID = Guid.NewGuid(); 
    }
               
    #region TestRefreshEvent
    public delegate void TestRefreshEventHandler(object sender, EventArgs args);

    public event TestRefreshEventHandler TestRefreshing;


    protected virtual void OnTestRefreshing()
    {
        if (TestRefreshing != null)
        {
            TestRefreshing(this, EventArgs.Empty);
        }
    }
    #endregion

    public List<Test> TestList()
    {
        if (Tests.Count == 0)
        {
            Tests.Add(new Test()
            {
                Subject = "Computer Programming",
                TestNumber = 1
            });

            Tests.Add(new Test()
            {
                Subject = "Computer Programming",
                TestNumber = 2
            });
            Tests.Add(new Test()
            {
                Subject = "Science",
                TestNumber = 1
            });

            Tests.Add(new Test()
            {
                Subject = "Science",
                TestNumber = 2
            });
            Tests.Add(new Test()
            {
                Subject = "Science",
                TestNumber = 3
            });
        }

        return Tests;
    }

    public List<Test> TestList(string newSubject, int newNumberOfTests)
    {
        //first max test number if subject exists

        int maxTestNumber = -1;
        int testNumberSeed = 0;
            
        foreach (Test test in Tests)
        {
            if (test.Subject == newSubject && test.TestNumber > maxTestNumber)
            {
                maxTestNumber = test.TestNumber;
            }
        }

        if (maxTestNumber == -1)
        {
            testNumberSeed = 1;
        }
        else
        {
            testNumberSeed = maxTestNumber + 1;
        }
            
        for(int i = 0; i < newNumberOfTests; i++)
        {
            Tests.Add(new Test()
            {
                Subject = newSubject,
                TestNumber = testNumberSeed
            });

            testNumberSeed= testNumberSeed + 1;
        }

        OnTestRefreshing();

        return Tests;
    }
}

