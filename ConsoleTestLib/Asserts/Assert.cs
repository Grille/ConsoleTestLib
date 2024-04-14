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
        var e = ThrowsAndReturn<T>(action);
        throw new TestSuccessException(e.Message, e);
    }

    [DoesNotReturn]
    public static void Throws<T>(string expectedMessage, Action action) where T : Exception
    {
        var e = ThrowsAndReturn<T>(action);
        IsEqual(expectedMessage, e.Message, null, (s) => $"'{s}'");
        throw new TestSuccessException(e.Message, e);
    }

    public static T ThrowsAndReturn<T>(Action action) where T : Exception
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

    public static void IsTrue(bool value) => IsTrue(value, string.Empty);

    public static void IsTrue(bool value, string message)
    {
        if (!value)
            throw new TestFailedException(message);
    }

    public static void IsNotEqual<T>(T expected, T actual) => AssertIsEqual(expected, actual, true, null, null);

    public static void IsNotEqual<T>(T expected, T actual, string? message) => AssertIsEqual(expected, actual, true, null, message);

    public static void IsNotEqual<T>(T expected, T actual, string? message, Func<T, string>? toString) => AssertIsEqual(expected, actual, true, toString, message);


    public static void IsEqual<T>(T expected, T actual) => AssertIsEqual(expected, actual, false, null, null);

    public static void IsEqual<T>(T expected, T actual, string? message) => AssertIsEqual(expected, actual, false, null, message);

    public static void IsEqual<T>(T expected, T actual, string? message, Func<T, string>? toString) => AssertIsEqual(expected, actual, false, toString, message);


    private static void AssertIsEqual<T>(T expected, T actual, bool invert, Func<T, string>? toString, string? message)
    {
        if (expected == null && actual == null)
            return;

        if (expected == null)
            throw new ArgumentNullException(nameof(expected));

        if (actual == null)
            throw new ArgumentNullException(nameof(actual));

        bool equal = expected.Equals(actual);
        bool fail = !equal ^ invert;
        if (fail)
        {
            var sb = new StringBuilder();
            if (invert)
            {
                sb.Append("Not ");
            }
            sb.Append("Expected: ");
            sb.Append(toString == null ? expected.ToString() : toString(expected));
            sb.Append(" Actual: ");
            sb.Append(toString == null ? actual.ToString() : toString(actual));
            if (!string.IsNullOrEmpty(message))
            {
                sb.Append(" ");
                sb.Append(message);
            }
            throw new TestFailedCompareException(expected, actual, sb.ToString());
        }
    }

    public static void IListIsEqual<T>(IList<T> expected, IList<T> actual) => IListIsEqual(expected, actual, null, MessageUtils.IListToString);

    public static void IListIsEqual<T>(IList<T> expected, IList<T> actual, string message) => IListIsEqual(expected, actual, message, MessageUtils.IListToString);
    

    public static void IListIsEqual<T>(IList<T> expected, IList<T> actual, string? message, Func<IList<T>, string> toString)
    {
        if (!CompareUtils.IsIListEqual(expected, actual))
        {
            var sb = new StringBuilder();
            sb.Append($"Expected: {toString(expected)} == Actual: {toString(actual)}");
            if (!string.IsNullOrEmpty(message))
            {
                sb.Append(" ");
                sb.Append(message);
            }
            throw new TestFailedCompareException(expected, actual, sb.ToString());
        }
    }


    public static void IEnumerableIsEqual(IEnumerable expected, IEnumerable actual)
    {
        if (expected == actual)
            return;
        
        var enumerator1 = expected.GetEnumerator();
        var enumerator2 = actual.GetEnumerator();

        int count = 0;

        while (true)
        {
            var res1 = enumerator1.MoveNext();
            var res2 = enumerator2.MoveNext();

            if (res1 != res2)
                TestResult.Fail($"Item count mismatch at position [{count}].");

            if (!(res1 && res2))
                break;

            IsEqual(enumerator1.Current, enumerator2.Current, $"Item mismatch at position [{count}].");

            count += 1;
        }
    }
}
