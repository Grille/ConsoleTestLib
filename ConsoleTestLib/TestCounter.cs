using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grille.ConsoleTestLib;

public struct TestCounter
{
    public int Total { get; private set; }

    public int Unexecuted { get; private set; }
    public int Success { get; private set; }
    public int Failure { get; private set; }
    public int Error { get; private set; }

    public void Result(TestState result)
    {
        switch (result)
        {
            case TestState.Success:
                Success++;
                break;
            case TestState.Failure:
                Failure++;
                break;
            case TestState.Error:
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
            Result(test.Result);
        }
    }
}
