using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grille.ConsoleTestLib.Utils;

public static class MessageUtils
{
    public static string ListToString<T>(IReadOnlyList<T> ilist) => ListToString(ilist, 16);

    public static string ListToString<T>(IReadOnlyList<T> ilist, int maxsize)
    {
        var sb = new StringBuilder();
        int count = ilist.Count;
        sb.Append($"[{count}]{{");
        int size = Math.Min(count, maxsize);
        int rest = count - size;
        for (int i = 0; i < size; i++)
        {
            sb.Append(ilist[i]);
            if (i < size - 1)
                sb.Append(",");
            else if (rest > 0)
                sb.Append($"...");
        }
        sb.Append("}");
        return sb.ToString();
    }

    public static string EqualFailText<T>(bool invert, T expected, T actual, string? message = null, Func<T, string>? toString = null)
    {
        string ToString(T obj)
        {
            if (toString == null)
                return obj == null ? "null" : obj.ToString()!;
            else
                return toString(obj);
        }

        var sb = new StringBuilder();
        if (invert)
        {
            sb.Append("Not ");
        }
        sb.Append("Expected: ");
        sb.Append(ToString(expected));
        sb.Append(" Actual: ");
        sb.Append(ToString(actual));
        if (!string.IsNullOrEmpty(message))
        {
            sb.Append(" ");
            sb.Append(message);
        }

        return sb.ToString();
    }
}
