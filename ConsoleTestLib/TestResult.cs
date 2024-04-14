using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grille.ConsoleTestLib;

public static class TestResult
{

    [DoesNotReturn]
    public static void Fail() => throw new TestFailedException();


    [DoesNotReturn]
    public static void Warn() => throw new TestWarnException();


    [DoesNotReturn]
    public static void Succes() => throw new TestSuccessException();


    [DoesNotReturn]
    public static void Fail(string message) => throw new TestFailedException(message);


    [DoesNotReturn]
    public static void Warn(string message) => throw new TestWarnException(message);


    [DoesNotReturn]
    public static void Succes(string message) => throw new TestSuccessException(message);
}
