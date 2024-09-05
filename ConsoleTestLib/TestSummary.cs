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
    public int Executed => Total - Unexecuted;

    public void Count(TestStatus result)
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

        Total = Success + Warning + Failure + Error + Unexecuted;
    }

    public void Count(IEnumerable<Section> sections)
    {
        foreach (var section in sections)
        {
            Count(section);
        }
    }

    public void Count(Section section)
    {
        foreach (var test in section.TestCases)
        {
            Count(test.Status);
        }
    }

    public static TestSummary NewCount(IEnumerable<Section> sections)
    {
        var summary = new TestSummary();
        summary.Count(sections);
        return summary;
    }
}
