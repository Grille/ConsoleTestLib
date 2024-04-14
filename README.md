# ConsoleTestLib
Library to write standalone console programs for testing.
This library relies heavily on delayed execution of lambdas, so knowledge how variable capture works is useful.

## Debugging
The parallel nature of RunTests() can lead to problems while debugging.
Its better to use RunTestsSynchronously() for that purpose.

## Disclaimer 
I created this as a private experiment, you probably shouldn't use this over something proper like XUnit.
Also, I don't necessarily care much about braking changes here, so keep that in mind before updating.

## Example
```cs
using static Grille.ConsoleTestLib.GlobalTestSystem;
using static Grille.ConsoleTestLib.TestResult;
using Grille.ConsoleTestLib.Asserts;

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
```
## Result

```
default
value on task create '0': OK value on task run '4'

Basic
test_0: OK
test_1: OK number is 1
test_2: FAIL number is > 1
test_3: ERROR
System.Exception: number is 3
   at ConsoleTestApp.Program.<>c__DisplayClass0_1.<Main>b__6() in C:\Users\Grille\Documents\GitHub\ConsoleTestLib\ConsoleTestApp\Program.cs:line 30
   at Grille.ConsoleTestLib.TestCase.<>c__DisplayClass25_0.<CreateTask>b__0() in C:\Users\Grille\Documents\GitHub\ConsoleTestLib\ConsoleTestLib\TestCase.cs:line 74

Async
async_test_1: OK 72ms
async_test_2: OK 4118ms
async_test_3: OK 4118ms
async_test_4: OK 2108ms
async_test_5: OK 70ms

Results:
Testcases: 10
* Success: 8
* failure: 2
```
