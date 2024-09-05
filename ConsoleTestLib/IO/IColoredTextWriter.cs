using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grille.ConsoleTestLib.IO;

public interface IColoredTextWriter
{
    public void Write(string msg);

    public void Write(string msg, PrintStyle level);
}
