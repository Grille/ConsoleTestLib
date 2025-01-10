using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grille.ConsoleTestLib;

public static class TestResult
{
    /// <summary>Throws a <see langword="new"/> <see cref="TestFailedException"/>().</summary>
    [DoesNotReturn]
    public static void Fail() => throw new TestFailedException();

    /// <summary>Throws a <see langword="new"/> <see cref="TestWarnException"/>().</summary>
    [DoesNotReturn]
    public static void Warn() => throw new TestWarnException();

    /// <summary>Throws a <see langword="new"/> <see cref="TestSuccessException"/>().</summary>
    [DoesNotReturn]
    public static void Success() => throw new TestSuccessException();

    /// <summary>Throws a <see langword="new"/> <see cref="TestFailedException"/>(message).</summary>
    [DoesNotReturn]
    public static void Fail(string message) => throw new TestFailedException(message);

    /// <summary>Throws a <see langword="new"/> <see cref="TestWarnException"/>(message).</summary>
    [DoesNotReturn]
    public static void Warn(string message) => throw new TestWarnException(message);

    /// <summary>Throws a <see langword="new"/> <see cref="TestSuccessException"/>(message).</summary>
    [DoesNotReturn]
    public static void Success(string message) => throw new TestSuccessException(message);
}
