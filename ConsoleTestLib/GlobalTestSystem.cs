using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Grille.ConsoleTestLib;

public static class GlobalTestSystem
{
    public static TestSystem System { get; set; }

    static GlobalTestSystem()
    {
        System = new TestSystem();
    }

    public static bool RethrowExceptions { get => System.RethrowExeptions; set => System.RethrowExeptions = value; }

    public static bool ExecuteImmediately { get => System.ExecuteImmediately; set => System.ExecuteImmediately = value; }

    public static void Test(string name, Action action) => System.Test(name, action);

    public static void Test(string name, Action<TestCase> action) => System.Test(name, action);

    public static void Test(string name, Func<TestState> action) => System.Test(name, action);

    public static void Test(string name, Func<TestCase, TestState> action) => System.Test(name, action);

    public static void Section(string name) => System.Section(name);

    public static void RunTests() => System.RunTests();

    public static void RunTestsSynchronously() => System.RunTestsSynchronously();
}
