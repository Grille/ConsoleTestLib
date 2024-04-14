using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grille.ConsoleTestLib;

public class TestSummary
{
    public int Total { get; private set; }

    public int Unexecuted { get; private set; }
    public int Success { get; private set; }
    public int Warning { get; private set; }
    public int Failure { get; private set; }
    public int Error { get; private set; }

    public void Result(TestStatus result)
    {
        switch (result)
        {
            case TestStatus.Success:
                Success++;
                break;
            case TestStatus.Warning:
                Warning++;
                break;
            case TestStatus.Failure:
                Failure++;
                break;
            case TestStatus.Error:
                Error++;
                break;
            default:
                Unexecuted++;
                break;
        }

        Total = Success + Failure + Error + Unexecuted;
    }

    public void Count(Section section)
    {
        foreach (var test in section.TestCases) {
            Result(test.Status);
        }
    }
}
