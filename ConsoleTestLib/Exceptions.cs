using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grille.ConsoleTestLib;

public class TestFailedException : Exception
{
    public TestFailedException() : base()
    {

    }
    public TestFailedException(string msg) : base(msg)
    {

    }
    public TestFailedException(string msg, Exception exception) : base(msg, exception)
    {

    }
}


public class TestFailedCompareException : TestFailedException
{
    public object Expected { get; }
    public object Actual { get; }

    public TestFailedCompareException(object expected, object actual) : base()
    {
        Expected = expected;
        Actual = actual;
    }

    public TestFailedCompareException(object expected, object actual, string message) : base(message)
    {
        Expected = expected;
        Actual = actual;
    }
}

public class TestSuccessException : Exception
{
    public TestSuccessException() : base()
    {

    }
    public TestSuccessException(string msg) : base(msg)
    {

    }
    public TestSuccessException(string msg, Exception exception) : base(msg, exception)
    {

    }
}