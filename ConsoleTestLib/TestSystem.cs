using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grille.ConsoleTestLib;

public class TestSystem
{
    /// <summary>
    /// Rethows all exceptions except TestFailedExceptions.
    /// </summary>
    public bool RethrowExeptions { get; set; } = false;

    /// <summary>
    /// Rethow all TestFailedException, usually thrown by assert statements.
    /// </summary>
    public bool RethrowFailed { get; set; } = false;

    /// <summary>
    /// Tests are immediately executed after being created, helpful for debugging.
    /// </summary>
    public bool ExecuteImmediately { get; set; } = false;

    /// <summary>
    /// Sets the Rethrow and ExecuteImmediately flags that are helpful while debugging, all flags can be set individually.
    /// </summary>
    public bool DebugMode
    {
        get { return RethrowExeptions && RethrowFailed && ExecuteImmediately; }
        set { RethrowExeptions = RethrowFailed = ExecuteImmediately = value; }
    }

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

    public TestSystem() : this(new StandardConsolePrinter())
    { }

    private TestCaseCreateOptions CreateInfo(string name)
    {
        return new TestCaseCreateOptions(name, RethrowExeptions, RethrowFailed, ExecuteImmediately, Printer);
    }

    public void Test(string name, Action action) => section.Add(new TestCase(CreateInfo(name), action));

    public void Test(string name, Action<TestCase> action) => section.Add(new TestCase(CreateInfo(name), action));

    public void Test(string name, Func<TestStatus> action) => section.Add(new TestCase(CreateInfo(name), action));

    public void Test(string name, Func<TestCase, TestStatus> action) => section.Add(new TestCase(CreateInfo(name), action));

    public void Section(string name) {
        section = new Section(name);
        sections.Add(section);
    }

    /// <summary>
    /// Run all enqueued tests parallel.
    /// </summary>
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

        Printer.PrintSummary(counter);

        if (RethrowExeptions)
        {
            Rethrow();
        }
    }

    /// <summary>
    /// Run all enqueued tests synchronous.
    /// </summary>
    public void RunTestsSynchronously()
    {
        var counter = new TestCounter();

        foreach (var section in sections)
        {
            section.RunSynchronouslyAndPrint(Printer);
            counter.Count(section);
        }

        Printer.PrintSummary(counter);

        if (RethrowExeptions)
        {
            Rethrow();
        }
    }

    /// <summary>
    /// Rethrow exceptions caught inside tests.
    /// </summary>
    public void Rethrow()
    {
        foreach (var section in sections)
        {
            section.Rethrow();
        }
    }

}
