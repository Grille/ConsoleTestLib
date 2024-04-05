using static Grille.ConsoleTestLib.GlobalTestSystem;
using static Grille.ConsoleTestLib.Asserts;
using Grille.ConsoleTestLib;
using System.Diagnostics;

namespace ConsoleTestApp;

internal class Program
{
    static void Main(string[] args)
    {
        RethrowExceptions = false;

        int i = 0;

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

        var sw = Stopwatch.StartNew();

        Section("Async");
        Test("async_test_1", () =>
        {
            Succes($"{sw.ElapsedMilliseconds}ms");
        });
        Test("async_test_2", () =>
        {
            Task.Delay(4000).Wait();
            Succes($"{sw.ElapsedMilliseconds}ms");
        });
        Test("async_test_3", () =>
        {
            Task.Delay(4000).Wait();
            Succes($"{sw.ElapsedMilliseconds}ms");
        });
        Test("async_test_4", () =>
        {
            Task.Delay(2000).Wait();
            Succes($"{sw.ElapsedMilliseconds}ms");
        });
        Test("async_test_5", () =>
        {
            Succes($"{sw.ElapsedMilliseconds}ms");
        });

        RunTests();
    }
}
