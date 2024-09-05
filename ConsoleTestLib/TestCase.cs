using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Grille.ConsoleTestLib;

#if NET6_0_OR_GREATER
[StackTraceHidden]
#endif
public class TestCase
{
    public TestCaseDates Dates { get; private set; }

    private Action action;

    private Task task;

    public string Name { get => CreateOptions.Name; }

    public TestCaseCreateOptions CreateOptions { get; private set; }

    public string Message { get; set; }

    public TestStatus Status { get; set; }

    public Exception? Exception { get; private set; }    

    private ExceptionDispatchInfo? exceptionDispatchInfo;

    public TestCase(TestCaseCreateOptions options, Action action)
    {
        Setup(options);

        this.action = action;
        CreateTask();
    }

    public TestCase(TestCaseCreateOptions options, Action<TestCase> action)
    {
        Setup(options);

        this.action = () => action(this);
        CreateTask();
    }

    public TestCase(TestCaseCreateOptions options, Func<TestStatus> action)
    {
        Setup(options);

        this.action = () => Status = action();
        CreateTask();
    }

    public TestCase(TestCaseCreateOptions options, Func<TestCase, TestStatus> action)
    {
        Setup(options);

        this.action = () => Status = action(this);
        CreateTask();
    }

    [MemberNotNull(nameof(CreateOptions), nameof(Message), nameof(Dates))]
    private void Setup(TestCaseCreateOptions options)
    {
        Dates = new TestCaseDates();
        CreateOptions = options;
        Message = string.Empty;
    }

    [MemberNotNull(nameof(task))]
    private void CreateTask()
    {
        if (CreateOptions.ExecuteImmediately)
        {
            task = Task.CompletedTask;
            Execute();
        }
        else
        {
            task = new Task(Execute, TaskCreationOptions.PreferFairness);
        }
    }

    private void Execute()
    {
        Dates.StartNow();
        try
        {
            action();
            if (Status == TestStatus.None)
            {
                Status = TestStatus.Success;
            }
        }
        catch (TestResultException e)
        {
            Exception = e;
            Message = e.Message;
            Status = e.Status;

            if (Status == TestStatus.Failure && CreateOptions.ExecuteImmediately && CreateOptions.RethrowFailed)
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
        
        Dates.EndNow();
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
