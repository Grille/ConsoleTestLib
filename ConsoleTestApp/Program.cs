using System;
using System.Threading.Tasks;

using Grille.ConsoleTestLib;
using System.Diagnostics;
using Grille.ConsoleTestLib.Utils;

using static Grille.ConsoleTestLib.GlobalTestSystem;
using static Grille.ConsoleTestLib.TestResult;
using static Grille.ConsoleTestLib.Asserts.Assert;

//ExecuteImmediately = true;
RethrowExceptions = false;
RethrowFailed = false;

//ExecuteImmediately = true;

Printer = new StandardConsolePrinter()
{
    ColoredNames = false,
    PrintFailAsException = false,
    PrintFullExceptions = true,
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
    IsEqual(0, long.MaxValue, "Value");
});
Test($"read/write prefix", () =>
{
    IsEqual(0, long.MaxValue, "Value");
});
Test($"read/write prefix", () =>
{
    IsEqual(0, long.MaxValue, "________________________________________________________");
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
    IsEqual(1, 1);
    Throws<TestFailedException>(() => IsEqual(0, 1));

    IsNotEqual(0, 1);
    Throws<TestFailedException>(() => IsNotEqual(1, 1));
});

void MessageIs(Action action, string message)
{
    var err = ThrowsAndReturn<TestFailedException>(action);
    IsEqual(err.Message, message);
}

Test("selftest_IsEqualMessage", () =>
{
    MessageIs(() => IsEqual(0, 1), "Expected: 0 Actual: 1");
    MessageIs(() => IsEqual(0, 1, "message"), "Expected: 0 Actual: 1 message");
    MessageIs(() => IsEqual(0, 1, null, (x) => $"[{x}]"), "Expected: [0] Actual: [1]");
    MessageIs(() => IsEqual(0, 1, "message", (x) => $"[{x}]"), "Expected: [0] Actual: [1] message");
});

Test("selftest_IsNotEqualMessage", () =>
{
    MessageIs(() => IsNotEqual(1, 1), "Not Expected: 1 Actual: 1");
    MessageIs(() => IsNotEqual(1, 1, "message"), "Not Expected: 1 Actual: 1 message");
    MessageIs(() => IsNotEqual(1, 1, null, (x) => $"[{x}]"), "Not Expected: [1] Actual: [1]");
    MessageIs(() => IsNotEqual(1, 1, "message", (x) => $"[{x}]"), "Not Expected: [1] Actual: [1] message");
});

Test("selftest_IListToString", () =>
{
    var list = new int[] { 0, 1, 2, 3 };

    IsEqual(MessageUtils.IListToString(list, 6), "[4]{0,1,2,3}");
    IsEqual(MessageUtils.IListToString(list, 4), "[4]{0,1,2,3}");
    IsEqual(MessageUtils.IListToString(list, 2), "[4]{0,1...}");
});

Test("selftest_AssertException", () =>
{
    Throws<IndexOutOfRangeException>("",() =>
    {
        throw new IndexOutOfRangeException();
    });
});

Test("selftest", () =>
{
    var list1 = new int[] { 0, 1, 2, 3 };
    var list2 = new int[] { 0, 1, 2, 3 };
    var list3 = new int[] { 0, 1, 4, 5 };
    var list4 = new int[] { 0, 1 };

    IEnumerableIsEqual(list1, list2);
});

RunTestsSynchronously();
//RunTests();
//RunTestsSynchronously();