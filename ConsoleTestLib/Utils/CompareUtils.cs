using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grille.ConsoleTestLib.Utils;

public static class CompareUtils
{
    public static bool IsIListEqual<T>(IList<T> ilist1, IList<T> ilist2)
    {
        if (ilist1.Count != ilist2.Count)
            return false;
        for (int i = 0; i < ilist2.Count; i++)
            if (!ilist2[i].Equals(ilist1[i]))
                return false;
        return true;
    }
}
