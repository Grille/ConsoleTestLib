using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grille.ConsoleTestLib;

public class Asserts
{
    public static void AssertException<T>(Action action) where T : Exception
    {
        try
        {
            action();
        }
        catch (T err)
        {
            throw new TestSuccessException($"{err.Message}");
        }
        throw new TestFailException($"expected Exception not thrown");
    }

    public static bool IsIListEqual<T>(IList<T> array1, IList<T> array2)
    {
        if (array1.Count != array2.Count)
            return false;
        for (int i = 0; i < array2.Count; i++)
            if (!array2[i].Equals(array1[i]))
                return false;
        return true;
    }

    public static string IListToString<T>(IList<T> array)
    {
        var sb = new StringBuilder();
        sb.Append($"[{array.Count}]{{");
        int size = Math.Min(array.Count, 16);
        for (int i = 0; i < size; i++)
        {
            sb.Append(array[i]);
            if (i < size - 1)
                sb.Append(",");
            else if (size < array.Count)
                sb.Append("...");
        }
        sb.Append("}");
        return sb.ToString();
    }

    public static void Assert(bool value, string message)
    {
        if (!value)
            throw new TestFailException(message);
    }

    public static void AssertValueIsNotEqual<T>(T value, T expected, string msg = "")
    {
        if (value.Equals(expected))
            throw new TestFailException($"value: {value} == expected: {expected} {msg}");
    }

    public static void AssertValueIsEqual<T>(T value, T expected, string msg = "")
    {
        if (!value.Equals(expected))
            throw new TestFailException($"value: {value} != expected: {expected} {msg}");
    }

    public static void AssertIListIsEqual<T>(IList<T> refarray0, IList<T> dataarray1, string msg = "") where T : unmanaged
    {
        if (!IsIListEqual(refarray0, dataarray1))
            throw new TestFailException($"expected: {IListToString(refarray0)} data: {IListToString(dataarray1)} {msg}");
    }

    public static void Fail()
    {
        throw new TestFailException();
    }

    public static void Succes()
    {
        throw new TestSuccessException();
    }

    public static void Fail(string msg)
    {
        throw new TestFailException(msg);
    }

    public static void Succes(string msg)
    {
        throw new TestSuccessException(msg);
    }
}
