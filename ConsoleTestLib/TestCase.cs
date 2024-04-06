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

    private Action action;

    private Task task;

    public string Name { get => CreateOptions.Name; }

    public TestCaseCreateOptions CreateOptions { get; }

    public string Message { get; set; }

    public TestState Result { get; set; }

    private ExceptionDispatchInfo? exceptionDispatchInfo;

    private TestCase(TestCaseCreateOptions options)
    {
        CreateOptions = options;
        Message = string.Empty;
        watch = new();
    }

    public TestCase(TestCaseCreateOptions options, Action action) : this(options)
    {
        this.action = action;
        CreateTask();
    }

    public TestCase(TestCaseCreateOptions options, Action<TestCase> action) : this(options)
    {
        this.action = () => action(this);
        CreateTask();
    }

    public TestCase(TestCaseCreateOptions options, Func<TestState> action) : this(options)
    {

        this.action = () => Result = action();
        CreateTask();
    }

    public TestCase(TestCaseCreateOptions options, Func<TestCase, TestState> action) : this(options)
    {
        this.action = () => Result = action(this);
        CreateTask();
    }

    private void CreateTask()
    {
        if (CreateOptions.ExecuteImmediately)
        {
            task = Task.CompletedTask;
            Run();
            CreateOptions.Printer.PrintTest(this);
        }
        else
        {
            task = new Task(Run, TaskCreationOptions.PreferFairness);
        }
    }

    private void Run()
    {
        watch.Start();
        try
        {
            action();
            if (Result == TestState.None)
            {
                Result = TestState.Success;
            }
        }
        catch (TestSuccessException e)
        {
            Message = e.Message;
            Result = TestState.Success;
        }
        catch (TestFailException e)
        {
            Message = e.Message;
            Result = TestState.Failure;
        }
        catch (Exception e)
        {
            Message = e.ToString();
            Result = TestState.Error;

            exceptionDispatchInfo = ExceptionDispatchInfo.Capture(e);

            if (CreateOptions.ExecuteImmediately && CreateOptions.RethrowExeptions)
            {
                throw;
            }
        }
        watch.Stop();
    }

    public void Start()
    {
        if (!task.IsCompleted)
        {
            task.Start();
        }
    }

    public void RunSynchronously()
    {
        task.RunSynchronously();
    }

    public void Wait()
    {
        task.Wait();
    }

    public void Rethrow()
    {
        if (exceptionDispatchInfo != null)
        {
            exceptionDispatchInfo.Throw();
        }
    }
}
