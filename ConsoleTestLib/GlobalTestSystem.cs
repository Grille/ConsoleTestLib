using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Grille.ConsoleTestLib;

/// <summary>
/// Static interface to an global TestSystem instance.
/// </summary>
public static class GlobalTestSystem
{
    public static TestSystem System { get; set; }

    static GlobalTestSystem()
    {
        System = new TestSystem();
    }

    /// <inheritdoc cref="TestSystem.RethrowExeptions"/>
    public static bool RethrowExceptions { get => System.RethrowExeptions; set => System.RethrowExeptions = value; }

    /// <inheritdoc cref="TestSystem.RethrowFailed"/>
    public static bool RethrowFailed { get => System.RethrowFailed; set => System.RethrowFailed = value; }

    /// <inheritdoc cref="TestSystem.ExecuteImmediately"/>
    public static bool ExecuteImmediately { get => System.ExecuteImmediately; set => System.ExecuteImmediately = value; }

    /// <inheritdoc cref="TestSystem.DebugMode"/>
    public static bool DebugMode { get => System.DebugMode; set => System.DebugMode = value; }

    /// <inheritdoc cref="TestSystem.Printer"/>
    public static ITestPrinter Printer { get => System.Printer; set => System.Printer = value; }

    public static void Test(string name, Action action) => System.Test(name, action);

    public static void Test(string name, Action<TestCase> action) => System.Test(name, action);

    public static void Test(string name, Func<TestStatus> action) => System.Test(name, action);

    public static void Test(string name, Func<TestCase, TestStatus> action) => System.Test(name, action);

    /// <inheritdoc cref="TestSystem.Section"/>
    public static void Section(string name) => System.Section(name);

    /// <inheritdoc cref="TestSystem.RunTests"/>
    public static void RunTests() => System.RunTests();

    /// <inheritdoc cref="TestSystem.RunTestsSynchronously"/>
    public static void RunTestsSynchronously() => System.RunTestsSynchronously();
}
