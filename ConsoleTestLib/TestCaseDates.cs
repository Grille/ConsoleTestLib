using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grille.ConsoleTestLib;

public class TestCaseDates
{
    public DateTime Created { get; private set; }
    public DateTime Started { get; private set; }
    public DateTime Ended { get; private set; }
    public TimeSpan Elapsed { get; private set; }

    public TestCaseDates()
    {
        Created = DateTime.Now;
    }

    internal void StartNow()
    {
        Started = DateTime.Now;
    }

    internal void EndNow()
    {
        Ended = DateTime.Now;
        Elapsed = Ended - Started;
    }

    public double ElapsedMilliseconds
    {
        get => Elapsed.TotalMilliseconds;
    }
}
