using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grille.ConsoleTestLib.IO;

internal class TextWriterWrapper : IColoredTextWriter
{
    TextWriter _writer;
    public TextWriterWrapper(TextWriter writer)
    {
        _writer = writer;
    }

    public void Write(string msg) => _writer.Write(msg);

    public void Write(string msg, PrintStyle style) => _writer.Write(msg);
}
