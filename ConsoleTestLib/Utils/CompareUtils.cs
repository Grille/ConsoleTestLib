using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grille.ConsoleTestLib.Utils;

public static class CompareUtils
{
    public enum EnumerablesEqualsResultCode
    {
        Equals,
        Count,
        Item,
    }

    public record struct EnumerablesEqualsResult(EnumerablesEqualsResultCode Code, int Position = 0);

    public static EnumerablesEqualsResult ListsAreEqual<T>(IReadOnlyList<T> ilist1, IReadOnlyList<T> ilist2)
    {
        if (ilist1.Count != ilist2.Count)
        {
            return new(EnumerablesEqualsResultCode.Count, Math.Min(ilist1.Count, ilist2.Count));
        }

        for (int i = 0; i < ilist2.Count; i++)
        {
            var item1 = ilist1[i];
            var item2 = ilist2[i];

            if (item1 == null)
            {
                if (item2 == null)
                {
                    continue;
                }
                else
                {
                    return new(EnumerablesEqualsResultCode.Item, i);
                }
                    
            }

            if (!item1.Equals(item2))
            {
                return new(EnumerablesEqualsResultCode.Item, i);
            }
        }

        return new(EnumerablesEqualsResultCode.Equals);
    }

    public static EnumerablesEqualsResult EnumerablesAreEqual(IEnumerable expected, IEnumerable actual)
    {
        if (expected == actual)
            return new(EnumerablesEqualsResultCode.Equals);

        var enumerator1 = expected.GetEnumerator();
        var enumerator2 = actual.GetEnumerator();

        int count = 0;

        while (true)
        {
            var res1 = enumerator1.MoveNext();
            var res2 = enumerator2.MoveNext();

            if (res1 != res2)
                return new(EnumerablesEqualsResultCode.Count, count);

            if (!(res1 && res2))
                break;

            if (!enumerator1.Current.Equals(enumerator2.Current))
            {
                return new(EnumerablesEqualsResultCode.Item, count);
            }

            count += 1;
        }

        return new(EnumerablesEqualsResultCode.Equals);
    }
    
}
