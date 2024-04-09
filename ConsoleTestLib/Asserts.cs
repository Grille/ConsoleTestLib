using Grille.ConsoleTestLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grille.ConsoleTestLib;

public class Asserts
{
    public static T AssertException<T>(Action action) where T : Exception
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


    public static void Assert(bool value, string message)
    {
        if (!value)
            throw new TestFailedException(message);
    }

    public static void AssertValueIsNotEqual<T>(T expected, T actual) => AssertIsEqual(expected, actual, true, null, null);

    public static void AssertValueIsNotEqual<T>(T expected, T actual, Func<T, string>? toString, string? message) => AssertIsEqual(expected, actual, true, toString, message);

    public static void AssertValueIsNotEqual<T>(T expected, T actual, string? message) => AssertIsEqual(expected, actual, true, null, message);

    public static void AssertValueIsNotEqual<T>(T expected, T actual, Func<T,string>? toString) => AssertIsEqual(expected, actual, true, toString, null);


    public static void AssertValueIsEqual<T>(T expected, T actual) => AssertIsEqual(expected, actual, false, null, null);

    public static void AssertValueIsEqual<T>(T expected, T actual, string? message) => AssertIsEqual(expected, actual, false, null, message);

    public static void AssertValueIsEqual<T>(T expected, T actual, Func<T, string>? toString) => AssertIsEqual(expected, actual, false, toString, null);

    public static void AssertValueIsEqual<T>(T expected, T actual, Func<T, string>? toString, string? message) => AssertIsEqual(expected, actual, false, toString, message);


    private static void AssertIsEqual<T>(T expected, T actual, bool invert, Func<T, string>? toString, string? message)
    {
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

    public static void AssertIListIsEqual<T>(IList<T> expected, IList<T> actual)
    {
        AssertIListIsEqual(expected, actual, MessageUtils.IListToString, null);
    }

    public static void AssertIListIsEqual<T>(IList<T> expected, IList<T> actual, string message)
    {
        AssertIListIsEqual(expected, actual, MessageUtils.IListToString, message);
    }

    public static void AssertIListIsEqual<T>(IList<T> expected, IList<T> actual, Func<IList<T>, string> toString)
    {
        AssertIListIsEqual(expected, actual, toString, null);
    }

    public static void AssertIListIsEqual<T>(IList<T> expected, IList<T> actual, Func<IList<T>, string> toString, string? message)
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

    public static void Fail()
    {
        throw new TestFailedException();
    }

    public static void Succes()
    {
        throw new TestSuccessException();
    }

    public static void Fail(string message)
    {
        throw new TestFailedException(message);
    }

    public static void Succes(string message)
    {
        throw new TestSuccessException(message);
    }
}
