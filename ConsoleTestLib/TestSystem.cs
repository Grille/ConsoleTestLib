using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grille.ConsoleTestLib;

public class TestSystem
{
    public bool RethrowExeptions { get; set; } = false;
    public bool ExecuteImmediately { get; set; } = false;

    private Section section;
    private List<Section> sections;

    public ICollection<Section> Sections => sections.AsReadOnly();

    public ITestPrinter Printer { get; set; }

    public TestSystem(ITestPrinter printer)
    {
        section = new Section("default");
        sections = [section];

        Printer = printer;
    }

    public TestSystem() : this(new ConsoleTestPrinter())
    { }

    private TestCaseCreateOptions CreateInfo(string name)
    {
        return new TestCaseCreateOptions(name, RethrowExeptions, ExecuteImmediately, Printer);
    }

    public void Test(string name, Action action) => section.Add(new TestCase(CreateInfo(name), action));

    public void Test(string name, Action<TestCase> action) => section.Add(new TestCase(CreateInfo(name), action));

    public void Test(string name, Func<TestState> action) => section.Add(new TestCase(CreateInfo(name), action));

    public void Test(string name, Func<TestCase, TestState> action) => section.Add(new TestCase(CreateInfo(name), action));

    public void Section(string name) {
        section = new Section(name);
        sections.Add(section);

        if (ExecuteImmediately)
        {
            Printer.PrintSectionEnd(section);
            Printer.PrintSectionBegin(section);
        }
    }

    public void RunTests()
    {
        foreach (var section in sections)
        {
            section.Start();
        }

        var counter = new TestCounter();

        foreach (var section in sections)
        {
            if (section.TestCases.Count > 0)
            {
                section.Print(Printer);
                counter.Count(section);
            }
        }

        Printer.PrintSumary(counter);

        if (RethrowExeptions)
        {
            Rethrow();
        }
    }

    public void RunTestsSynchronously()
    {
        var counter = new TestCounter();

        foreach (var section in sections)
        {
            section.RunSynchronouslyAndPrint(Printer);
            counter.Count(section);
        }

        Printer.PrintSumary(counter);

        if (RethrowExeptions)
        {
            Rethrow();
        }
    }

    public void Rethrow()
    {
        foreach (var section in sections)
        {
            section.Rethrow();
        }
    }

}
