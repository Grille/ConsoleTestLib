using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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

    public void Start()
    {
        foreach (var test in TestCases)
        {
            test.Start();
        }
    }

    public void Add(TestCase test)
    {
        TestCases.Add(test);
    }

    public void RunSynchronouslyAndPrint(ITestPrinter printer)
    {
        printer.PrintSectionBegin(this);

        foreach (var test in TestCases)
        {
            test.RunSynchronously();
            printer.PrintTest(test);
        }

        printer.PrintSectionEnd(this);
    }

    public void Print(ITestPrinter printer)
    {
        printer.PrintSectionBegin(this);

        for (var i = 0; i < TestCases.Count; i++)
        {
            var test = TestCases[i];
            test.Wait();
            printer.PrintTest(test);
        }

        printer.PrintSectionEnd(this);
    }

    public void Rethrow()
    {
        foreach (var test in TestCases)
        {
            test.Rethrow();
        }
    }
}
