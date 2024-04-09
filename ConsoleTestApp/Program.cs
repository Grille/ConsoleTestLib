using Grille.ConsoleTestLib;
using System.Diagnostics;
using Grille.ConsoleTestLib.Utils;

using static Grille.ConsoleTestLib.GlobalTestSystem;
using static Grille.ConsoleTestLib.Asserts;

RethrowExceptions = false;
RethrowFailed = false;

ExecuteImmediately = true;

Printer = new StandardConsolePrinter()
{
    PrintFullExceptions = true,
};

int i = 0;

//DebugMode = true;

Test($"value on task create '{i}'", () =>
{
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

Test($"test_invalid", () =>
{
    return (TestStatus)4;
});

var sw = Stopwatch.StartNew();

Section("Async");
Test("async_test_1", () =>
{
    Succes($"{sw.ElapsedMilliseconds}ms");
});
Test("async_test_2", () =>
{
    Task.Delay(40).Wait();
    Succes($"{sw.ElapsedMilliseconds}ms");
});
Test("async_test_3", () =>
{
    Task.Delay(40).Wait();
    Succes($"{sw.ElapsedMilliseconds}ms");
});
Test("async_test_4", () =>
{
    Task.Delay(20).Wait();
    Succes($"{sw.ElapsedMilliseconds}ms");
});
Test("async_test_5", () =>
{
    Succes($"{sw.ElapsedMilliseconds}ms");
});

Section("Selftest");
Test("selftest_IsEqual", () =>
{
    AssertValueIsEqual(1, 1);
    AssertException<TestFailedException>(() => AssertValueIsEqual(0, 1));

    AssertValueIsNotEqual(0, 1);
    AssertException<TestFailedException>(() => AssertValueIsNotEqual(1, 1));
});

Test("selftest_IsEqualMessage", () =>
{
    void MessageIs(Action action, string message)
    {
        var err = AssertException<TestFailedException>(action);
        AssertValueIsEqual(err.Message, message);
    }

    MessageIs(() => AssertValueIsEqual(0, 1), "Expected: 0 Actual: 1");
    MessageIs(() => AssertValueIsEqual(0, 1, "message"), "Expected: 0 Actual: 1 message");
    MessageIs(() => AssertValueIsEqual(0, 1, (x) => $"[{x}]"), "Expected: [0] Actual: [1]");
    MessageIs(() => AssertValueIsEqual(0, 1, (x) => $"[{x}]", "message"), "Expected: [0] Actual: [1] message");

    MessageIs(() => AssertValueIsNotEqual(1, 1), "Not Expected: 1 Actual: 1");
    MessageIs(() => AssertValueIsNotEqual(1, 1, "message"), "Not Expected: 1 Actual: 1 message");
    MessageIs(() => AssertValueIsNotEqual(1, 1, (x) => $"[{x}]"), "Not Expected: [1] Actual: [1]");
    MessageIs(() => AssertValueIsNotEqual(1, 1, (x) => $"[{x}]", "message"), "Not Expected: [1] Actual: [1] message");
});

Test("selftest_IListToString", () =>
{
    var list = new int[] { 0, 1, 2, 3 };

    AssertValueIsEqual(MessageUtils.IListToString(list, 6), "[4]{0,1,2,3}");
    AssertValueIsEqual(MessageUtils.IListToString(list, 4), "[4]{0,1,2,3}");
    AssertValueIsEqual(MessageUtils.IListToString(list, 2), "[4]{0,1...}");
});

RunTests();
//RunTestsSynchronously();