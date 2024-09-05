using System;
using System.Diagnostics.CodeAnalysis;
using Grille.ConsoleTestLib.IO;

namespace Grille.ConsoleTestLib;

/// <summary>
/// Static interface to an global TestSystem instance.
/// </summary>
public static class GlobalTestSystem
{
    public static TestSystem Instance { get; set; }

    static GlobalTestSystem()
    {
        Instance = new TestSystem();
    }

    /// <inheritdoc cref="TestSystem.RethrowExeptions"/>
    public static bool RethrowExceptions { get => Instance.RethrowExeptions; set => Instance.RethrowExeptions = value; }

    /// <inheritdoc cref="TestSystem.RethrowFailed"/>
    public static bool RethrowFailed { get => Instance.RethrowFailed; set => Instance.RethrowFailed = value; }

    /// <inheritdoc cref="TestSystem.ExecuteImmediately"/>
    public static bool ExecuteImmediately { get => Instance.ExecuteImmediately; set => Instance.ExecuteImmediately = value; }

    /// <inheritdoc cref="TestSystem.DebugMode"/>
    public static bool DebugMode { get => Instance.DebugMode; set => Instance.DebugMode = value; }

    /// <inheritdoc cref="TestSystem.RunAsync"/>
    public static bool RunAsync { get => Instance.RunAsync; set => Instance.RunAsync = value; }

    /// <inheritdoc cref="TestSystem.Printer"/>
    public static ITestPrinter Printer { get => Instance.Printer; set => Instance.Printer = value; }

    /// <inheritdoc cref="TestSystem.Test(string, Action)"/>
    public static void Test(string name, Action action) => Instance.Test(name, action);

    /// <inheritdoc cref="TestSystem.Test(string, Action{TestCase})"/>
    public static void Test(string name, Action<TestCase> action) => Instance.Test(name, action);

    /// <inheritdoc cref="TestSystem.Test(string, Func{TestStatus})"/>
    public static void Test(string name, Func<TestStatus> action) => Instance.Test(name, action);

    /// <inheritdoc cref="TestSystem.Test(string, Func{TestCase, TestStatus})"/>
    public static void Test(string name, Func<TestCase, TestStatus> action) => Instance.Test(name, action);

    /// <inheritdoc cref="TestSystem.Section"/>
    public static void Section(string name) => Instance.Section(name);

    /// <inheritdoc cref="TestSystem.RunTests"/>
    public static void RunTests() => Instance.RunTests();
}
