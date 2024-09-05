using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grille.ConsoleTestLib;

public class TestResultException : Exception
{
    public TestStatus Status { get; }

    public TestResultException(string? msg, TestStatus status) : base(msg == null ? string.Empty : msg)
    {
        Status = status;
    }
    public TestResultException(string? msg, TestStatus status, Exception exception) : base(msg == null ? string.Empty : msg, exception)
    {
        Status = status;
    }
}

public class TestFailedException : TestResultException
{
    public TestFailedException() : base(string.Empty, TestStatus.Failure) { }
    public TestFailedException(string? msg) : base(msg, TestStatus.Failure) { }
    public TestFailedException(string? msg, Exception exception) : base(msg, TestStatus.Failure, exception) { }
}

public class TestFailedCompareException : TestFailedException
{
    public object? Expected { get; }
    public object? Actual { get; }

    public TestFailedCompareException(object? expected, object? actual) : base()
    {
        Expected = expected;
        Actual = actual;
    }

    public TestFailedCompareException(object? expected, object? actual, string? message) : base(message)
    {
        Expected = expected;
        Actual = actual;
    }
}

public class TestWarnException : TestResultException
{
    public TestWarnException() : base(string.Empty, TestStatus.Warning) { }
    public TestWarnException(string? msg) : base(msg, TestStatus.Warning) { }
    public TestWarnException(string? msg, Exception exception) : base(msg, TestStatus.Warning, exception) { }
}

public class TestSuccessException : TestResultException
{
    public TestSuccessException() : base(string.Empty, TestStatus.Success) { }
    public TestSuccessException(string? msg) : base(msg, TestStatus.Success) { }
    public TestSuccessException(string? msg, Exception exception) : base(msg, TestStatus.Success, exception) { }
}