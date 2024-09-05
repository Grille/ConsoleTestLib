using Grille.ConsoleTestLib.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grille.ConsoleTestLib.Asserts;

public static class Assert
{
    [DoesNotReturn]
    public static void Throws<T>(Action action) where T : Exception
    {
        var e = ThrowsContinue<T>(action);
        throw new TestSuccessException(e.Message, e);
    }

    [DoesNotReturn]
    public static void Throws<T>(Action action, string expectedMessage) where T : Exception
    {
        var e = ThrowsContinue<T>(action);
        IsEqual(expectedMessage, e.Message, null, (s) => $"'{s}'");
        throw new TestSuccessException(e.Message, e);
    }

    /// <summary>Assert that <see cref="Exception"/> was thrown and continue execution.</summary>
    public static T ThrowsContinue<T>(Action action) where T : Exception
    {
        try
        {
            action();
        }
        catch (T err)
        {
            return err;
        }
        throw new TestFailedException($"Expected exception '{typeof(T).Name}' not thrown.");
    }

    public static void IsTrue(bool value) => IsTrue(value, null);

    public static void IsTrue(bool value, string? message)
    {
        if (!value)
            throw new TestFailedException(message);
    }


    public static void IsNotEqual<T>(T expected, T actual) => IsEqual(true, expected, actual);

    public static void IsNotEqual<T>(T expected, T actual, string? message) => IsEqual(true, expected, actual, message);

    public static void IsNotEqual<T>(T expected, T actual, string? message, Func<T, string>? toString) => IsEqual(true, expected, actual, message, toString);

    public static void IsNotEqual<T>(T expected, T actual, Func<T, string>? toString) => IsEqual(true, expected, actual, null, toString);


    public static void IsEqual<T>(T expected, T actual) => IsEqual(false, expected, actual);

    public static void IsEqual<T>(T expected, T actual, string? message) => IsEqual(false, expected, actual, message);

    public static void IsEqual<T>(T expected, T actual, string? message, Func<T, string>? toString) => IsEqual(false, expected, actual, message, toString);

    public static void IsEqual<T>(T expected, T actual, Func<T, string>? toString) => IsEqual(false, expected, actual, null, toString);


    private static void IsEqual<T>(bool invert, T expected, T actual, string? message = null, Func<T, string>? toString = null)
    {
        if (expected == null && actual == null)
            return;

        bool equal = expected != null ? expected.Equals(actual) : false;
        bool fail = !equal ^ invert;
        if (fail)
        {
            var text = MessageUtils.EqualFailText(invert, expected, actual, message, toString);
            throw new TestFailedCompareException(expected, actual, text);
        }
    }


    public static void ListsAreEqual<T>(IReadOnlyList<T> expected, IReadOnlyList<T> actual) => ListsAreEqual(expected, actual, null, MessageUtils.ListToString);

    public static void ListsAreEqual<T>(IReadOnlyList<T> expected, IReadOnlyList<T> actual, string? message) => ListsAreEqual(expected, actual, message, MessageUtils.ListToString);

    public static void ListsAreEqual<T>(IReadOnlyList<T> expected, IReadOnlyList<T> actual, Func<IReadOnlyList<T>, string> toString) => ListsAreEqual(expected, actual, null, MessageUtils.ListToString);

    public static void ListsAreEqual<T>(IReadOnlyList<T> expected, IReadOnlyList<T> actual, string? message, Func<IReadOnlyList<T>, string> toString)
    {
        var result = CompareUtils.ListsAreEqual(expected, actual);

        if (result.Code == CompareUtils.EnumerablesEqualsResultCode.Equals)
        {
            return;
        }
        else if (result.Code == CompareUtils.EnumerablesEqualsResultCode.Count)
        {
            TestResult.Fail($"Item count mismatch at position {result.Position}.");
        }
        else if (result.Code == CompareUtils.EnumerablesEqualsResultCode.Item)
        {
            var text = MessageUtils.EqualFailText(false, expected, actual, message, toString);
            throw new TestFailedCompareException(expected, actual, text);
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    public static void EnumerablesAreEqual(IEnumerable expected, IEnumerable actual)
    {
        var result = CompareUtils.EnumerablesAreEqual(expected, actual);

        if (result.Code == CompareUtils.EnumerablesEqualsResultCode.Equals)
        {
            return;
        }
        else if (result.Code == CompareUtils.EnumerablesEqualsResultCode.Count)
        {
            TestResult.Fail($"Item count mismatch at position {result.Position}.");
        }
        else if (result.Code == CompareUtils.EnumerablesEqualsResultCode.Item)
        {
            TestResult.Fail($"Item mismatch at position {result.Position}.");
        }
        else
        {
            throw new InvalidOperationException();
        }
    }
}
