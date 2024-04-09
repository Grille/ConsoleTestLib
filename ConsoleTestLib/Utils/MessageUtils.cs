using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grille.ConsoleTestLib.Utils;

public static class MessageUtils
{
    public static string IListToString<T>(IList<T> ilist) => IListToString(ilist, 16);

    public static string IListToString<T>(IList<T> ilist, int maxsize)
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
}
