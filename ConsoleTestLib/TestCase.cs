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

    public TestStatus Status { get; set; }

    public Exception? Exception { get; private set; }    

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

    public TestCase(TestCaseCreateOptions options, Func<TestStatus> action) : this(options)
    {

        this.action = () => Status = action();
        CreateTask();
    }

    public TestCase(TestCaseCreateOptions options, Func<TestCase, TestStatus> action) : this(options)
    {
        this.action = () => Status = action(this);
        CreateTask();
    }

    private void CreateTask()
    {
        if (CreateOptions.ExecuteImmediately)
        {
            task = Task.CompletedTask;
            Run();
            //CreateOptions.Printer.PrintTest(this);
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
            if (Status == TestStatus.None)
            {
                Status = TestStatus.Success;
            }
            else if (Status == TestStatus.Error)
            {
                throw new Exception("Status on exit is Error.");
            }
            else if ((int)Status > 3 || (int)Status < 0)
            {
                throw new Exception($"Invalid status '{Status}' on exit.");
            }
        }
        catch (TestSuccessException e)
        {
            Exception = e;
            Message = e.Message;
            Status = TestStatus.Success;
        }
        catch (TestFailedException e)
        {
            Exception = e;
            Message = e.Message;
            Status = TestStatus.Failure;

            if (CreateOptions.ExecuteImmediately && CreateOptions.RethrowFailed)
            {
                throw;
            }
        }
        catch (Exception e)
        {
            Exception = e;
            Message = e.Message;
            Status = TestStatus.Error;

            exceptionDispatchInfo = ExceptionDispatchInfo.Capture(e);

            if (CreateOptions.ExecuteImmediately && CreateOptions.RethrowExeptions)
            {
                throw;
            }
        }
        
        watch.Stop();
    }

    /// <inheritdoc cref="Task.Start()"/>
    public void Start()
    {
        if (!task.IsCompleted)
        {
            task.Start();
        }
    }

    /// <inheritdoc cref="Task.RunSynchronously()"/>
    public void RunSynchronously()
    {
        if (!task.IsCompleted)
        {
            task.RunSynchronously();
        }
    }

    /// <inheritdoc cref="Task.Wait()"/>
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
