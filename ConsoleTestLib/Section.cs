using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Grille.ConsoleTestLib.IO;

namespace Grille.ConsoleTestLib;

public class Section
{
    public string Name { get; }

    public List<TestCase> TestCases { get; }

    public Section(string name)
    {
        Name = name;
        TestCases = new();
    }

    public void Add(TestCase test)
    {
        TestCases.Add(test);
    }

    public void RunAsync()
    {
        foreach (var test in TestCases)
        {
            test.Start();
        }
    }

    public void RunSynchronously()
    {
        foreach (var test in TestCases)
        {
            test.RunSynchronously();
        }
    }

    public void Rethrow()
    {
        foreach (var test in TestCases)
        {
            test.Rethrow();
        }
    }
}
