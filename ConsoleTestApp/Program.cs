using System;
using System.Threading.Tasks;

using Grille.ConsoleTestLib;
using System.Diagnostics;

using Grille.ConsoleTestLib.Utils;
using Grille.ConsoleTestLib.IO;
using Grille.ConsoleTestLib.Asserts;

using static Grille.ConsoleTestLib.GlobalTestSystem;
using static Grille.ConsoleTestLib.TestResult;
using System.Reflection;

Console.WriteLine(Environment.Version);
Console.WriteLine();

ExecuteImmediately = false;
RethrowExceptions = false;
RethrowFailed = false;
RunAsync = true;

Printer = new DefaultTestPrinter()
{
    PrintDates = false,
    ColoredNames = false,
    PrintFailAsException = false,
    PrintFullExceptions = false,
};


int i = 0;

//DebugMode = true;

Test($"value on task create '{i}'", () =>
{
    Fail("hm");
    Succes($"value on task run '{i}'");
});

Section("Basic");

for (i = 0; i < 4; i++)
{
    
    int number = i;
    Test($"test_{number}", () =>
    {
        if (number == 1)
            Succes("number is 1");
        if (number == 3)
            throw new Exception("number is 3");
        if (number > 1)
            Fail("number is > 1");
    });
}

Test($"test_return_fail", () =>
{
    return TestStatus.Failure;
});

Test($"read/write prefix", () =>
{
    Assert.IsEqual(0, long.MaxValue, "Value");
});
Test($"read/write prefix", () =>
{
    Assert.IsEqual(0, long.MaxValue, "Value");
});
Test($"read/write prefix", () =>
{
    Assert.IsEqual(0, long.MaxValue, "________________________________________________________");
});
Test($"read/write prefix", () =>
{
    Succes();
});

Test($"test_return_invalid", () =>
{
    return (TestStatus)8;
});

Test($"test_ok", Succes);
Test($"test_warn", Warn);
Test($"test_fail", Fail);

Test("linebreak", () => Succes("Hello, \nWorld"));

Test($"test_ok", ()=>Succes("msg"));
Test($"test_warn", () => Warn("msg"));
Test($"test_fail", () => Fail("msg"));

var sw = Stopwatch.StartNew();

Section("Async");
Test("async_test_1", () =>
{
    Succes($"{sw.ElapsedMilliseconds}ms");
});
Test("async_test_2", () =>
{
    Task.Delay(400).Wait();
    Succes($"{sw.ElapsedMilliseconds}ms");
});
Test("async_test_3", () =>
{
    Task.Delay(400).Wait();
    Succes($"{sw.ElapsedMilliseconds}ms");
});
Test("async_test_4", () =>
{
    Task.Delay(200).Wait();
    Succes($"{sw.ElapsedMilliseconds}ms");
});
Test("async_test_5", () =>
{
    Succes($"{sw.ElapsedMilliseconds}ms");
});

Section("Selftest");

Test("selftest_IsEqual", () =>
{
    Assert.IsEqual(1, 1);
    Assert.Throws<TestFailedException>(() => Assert.IsEqual(0, 1));

    Assert.IsNotEqual(0, 1);
    Assert.Throws<TestFailedException>(() => Assert.IsNotEqual(1, 1));
});
void MessageIs(Action action, string message)
{
    var err = Assert.ThrowsContinue<TestFailedException>(action);
    Assert.IsEqual(err.Message, message);
}

Test("selftest_IsEqualMessage", () =>
{
    MessageIs(() => Assert.IsEqual(0, 1), "Expected: 0 Actual: 1");
    MessageIs(() => Assert.IsEqual(0, 1, "message"), "Expected: 0 Actual: 1 message");
    MessageIs(() => Assert.IsEqual(0, 1, null, (x) => $"[{x}]"), "Expected: [0] Actual: [1]");
    MessageIs(() => Assert.IsEqual(0, 1, "message", (x) => $"[{x}]"), "Expected: [0] Actual: [1] message");
});

Test("selftest_IsNotEqualMessage", () =>
{
    MessageIs(() => Assert.IsNotEqual(1, 1), "Not Expected: 1 Actual: 1");
    MessageIs(() => Assert.IsNotEqual(1, 1, "message"), "Not Expected: 1 Actual: 1 message");
    MessageIs(() => Assert.IsNotEqual(1, 1, null, (x) => $"[{x}]"), "Not Expected: [1] Actual: [1]");
    MessageIs(() => Assert.IsNotEqual(1, 1, "message", (x) => $"[{x}]"), "Not Expected: [1] Actual: [1] message");
});

Test("selftest_IListToString", () =>
{
    var list = new int[] { 0, 1, 2, 3 };

    Assert.IsEqual(MessageUtils.ListToString(list, 6), "[4]{0,1,2,3}");
    Assert.IsEqual(MessageUtils.ListToString(list, 4), "[4]{0,1,2,3}");
    Assert.IsEqual(MessageUtils.ListToString(list, 2), "[4]{0,1...}");
});

Test("selftest_AssertException", () =>
{
    var ex = new IndexOutOfRangeException();
    Assert.Throws<IndexOutOfRangeException>(() =>
    {
        throw ex;
    }, ex.Message);
});

Test("selftest", () =>
{
    var list1 = new int[] { 0, 1, 2, 3 };
    var list2 = new int[] { 0, 1, 2, 3 };
    var list3 = new int[] { 0, 1, 4, 5 };
    var list4 = new int[] { 0, 1 };

    void Fails(Action action, string? message = null)
    {
        var e = Assert.ThrowsContinue<TestFailedException>(action);
        if (message != null)
        {
            Assert.IsEqual(message, e.Message);
        }
    }

    Assert.EnumerablesAreEqual(list1, list1);
    Assert.EnumerablesAreEqual(list1, list2);
    Fails(() => Assert.EnumerablesAreEqual(list1, list3), "Item mismatch at position 2.");
    Fails(() => Assert.EnumerablesAreEqual(list1, list4), "Item count mismatch at position 2.");

    Assert.ListsAreEqual(list1, list1);
    Assert.ListsAreEqual(list1, list2);
    Fails(() => Assert.ListsAreEqual(list1, list3), "Expected: [4]{0,1,2,3} Actual: [4]{0,1,4,5}");
    Fails(() => Assert.ListsAreEqual(list1, list4), "Item count mismatch at position 2.");
});

RunTests();

Printer.PrintSummary(new TestSummary());