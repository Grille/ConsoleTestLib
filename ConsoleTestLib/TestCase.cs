using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Grille.ConsoleTestLib;

public class TestCase
{
    private Stopwatch watch;

    public TimeSpan Elapsed
    {
        get => watch.Elapsed;
    }


    public double ElapsedMilliseconds
    {
        get => watch.Elapsed.TotalMilliseconds;
    }

    public Task Task { get; }

    public string Name { get; }

    public string Message { get; set; }

    public TestResult Result { get; set; }

    private ExceptionDispatchInfo? exceptionDispatchInfo;

    private TestCase(TestCaseCreateInfo info)
    {
        Name = info.Name;
        Message = string.Empty;
        watch = new();
        Task = null!;
    }

    public TestCase(TestCaseCreateInfo info, Action action) : this(info)
    {
        Task = CreateTask(action);
    }

    public TestCase(TestCaseCreateInfo info, Action<TestCase> action) : this(info)
    {
        Task = CreateTask(() => action(this));
    }

    public TestCase(TestCaseCreateInfo info, Func<TestResult> action) : this(info)
    {
        Task = CreateTask(() => Result = action());
    }

    public TestCase(TestCaseCreateInfo info, Func<TestCase, TestResult> action) : this(info)
    {
        Task = CreateTask(() => Result = action(this));
    }

    private Task CreateTask(Action action)
    {
        return new Task(() =>
        {
            watch.Start();
            try
            {
                action();
                if (Result == TestResult.None)
                {
                    Result = TestResult.Success;
                }
            }
            catch (TestSuccessException e)
            {
                Message = e.Message;
                Result = TestResult.Success;
            }
            catch (TestFailException e)
            {
                Message = e.Message;
                Result = TestResult.Failure;
            }
            catch (Exception e)
            {
                Message = e.ToString();
                Result = TestResult.Error;

                exceptionDispatchInfo = ExceptionDispatchInfo.Capture(e);
            }
            watch.Stop();
        }, TaskCreationOptions.PreferFairness);
    }

    public void Start()
    {
        Task.Start();
    }

    public void RunSynchronously()
    {
        Task.RunSynchronously();
    }

    public void Wait()
    {
        Task.Wait();
    }

    public void Rethrow()
    {
        if (exceptionDispatchInfo != null)
        {
            exceptionDispatchInfo.Throw();
        }
    }
}
