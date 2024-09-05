using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grille.ConsoleTestLib.IO;

namespace Grille.ConsoleTestLib;

public class TestSystem
{
    /// <summary>
    /// Rethows all exceptions except TestFailedExceptions.<br/><br/>
    /// Default value is <see langword="false"/>
    /// </summary>
    public bool RethrowExeptions { get; set; } = false;

    /// <summary>
    /// Rethow all TestFailedException, usually thrown by assert statements.<br/><br/>
    /// Default value is <see langword="false"/>
    /// </summary>
    public bool RethrowFailed { get; set; } = false;

    /// <summary>
    /// Tests are immediately executed after being created, helpful for debugging.<br/><br/>
    /// Default value is <see langword="false"/>
    /// </summary>
    public bool ExecuteImmediately { get; set; } = false;

    /// <summary>
    /// Default value is <see langword="true"/>
    /// </summary>
    public bool RunAsync { get; set; } = true;

    /// <summary>
    /// Sets the Rethrow and ExecuteImmediately flags that are helpful while debugging, all flags can be set individually.<br/><br/>
    /// Default value is <see langword="false"/>
    /// </summary>
    public bool DebugMode
    {
        get { return RethrowExeptions && RethrowFailed && ExecuteImmediately; }
        set { RethrowExeptions = RethrowFailed = ExecuteImmediately = value; }
    }

    private Section section;
    private List<Section> sections;

    public ReadOnlyCollection<Section> Sections => sections.AsReadOnly();

    public ITestPrinter Printer { get; set; }

    public TestSystem(ITestPrinter printer)
    {
        section = new Section("default");
        sections = [section];

        Printer = printer;
    }

    public TestSystem() : this(new DefaultTestPrinter()) { }

    private TestCaseCreateOptions CreateInfo(string name)
    {
        return new TestCaseCreateOptions(name, RethrowExeptions, RethrowFailed, ExecuteImmediately);
    }

    public void Test(string name, Action action) => section.Add(new TestCase(CreateInfo(name), action));

    public void Test(string name, Action<TestCase> action) => section.Add(new TestCase(CreateInfo(name), action));

    public void Test(string name, Func<TestStatus> action) => section.Add(new TestCase(CreateInfo(name), action));

    public void Test(string name, Func<TestCase, TestStatus> action) => section.Add(new TestCase(CreateInfo(name), action));

    public void Section(string name) {
        section = new Section(name);
        sections.Add(section);
    }

    public void RunTests()
    {
        var task = Printer.PrintSectionsAsync(sections);

        if (RunAsync)
        {
            RunTestsAsync();
        }
        else
        {
            RunTestsSynchronously();
        }

        task.Wait();

        Printer.PrintSummary(TestSummary.NewCount(sections));

        if (RethrowExeptions)
        {
            Rethrow();
        }
    }

    /// <summary>
    /// Run all enqueued tests parallel.
    /// </summary>
    private void RunTestsAsync()
    {
        foreach (var section in sections)
        {
            section.RunAsync();
        }
    }

    /// <summary>
    /// Run all enqueued tests synchronous.
    /// </summary>
    private void RunTestsSynchronously()
    {
        foreach (var section in sections)
        {
            section.RunSynchronously();
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
