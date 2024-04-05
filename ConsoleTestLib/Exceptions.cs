using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grille.ConsoleTestLib;

public class TestFailException : Exception
{
    public TestFailException() : base()
    {

    }
    public TestFailException(string msg) : base(msg)
    {

    }
    public TestFailException(string msg, Exception exception) : base(msg, exception)
    {

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