using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Grille.ConsoleTestLib.IO;

public class ConsoleWriter : IColoredTextWriter
{
    public ConsoleColor DefaultColor { get; set; } = ConsoleColor.Gray;
    public ConsoleColor TitleColor { get; set; } = ConsoleColor.Cyan;
    public ConsoleColor SuccesColor { get; set; } = ConsoleColor.Green;
    public ConsoleColor ErrorColor { get; set; } = ConsoleColor.Red;
    public ConsoleColor WarnColor { get; set; } = ConsoleColor.DarkYellow;
    public ConsoleColor FailedColor { get; set; } = ConsoleColor.Magenta;
    public ConsoleColor InvalidColor { get; set; } = ConsoleColor.Blue;

    public void Write(string msg)
    {
        var color = GetColor(PrintStyle.Default);
        Console.ForegroundColor = color;
        Console.Write(msg);
    }

    public void Write(string msg, PrintStyle style)
    {
        var color = GetColor(style);
        var bcolor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(msg);
        Console.ForegroundColor = bcolor;
    }

    public ConsoleColor GetColor(PrintStyle style) => style switch
    {
        PrintStyle.Succes => SuccesColor,
        PrintStyle.Warn => WarnColor,
        PrintStyle.Failed => FailedColor,
        PrintStyle.Error => ErrorColor,
        PrintStyle.Title => TitleColor,
        PrintStyle.Invalid => InvalidColor,

        PrintStyle.Gray => ConsoleColor.Gray,
        PrintStyle.Red => ConsoleColor.Red,
        PrintStyle.Green => ConsoleColor.Green,
        PrintStyle.Blue => ConsoleColor.Blue,
        PrintStyle.Yellow => ConsoleColor.DarkYellow,
        PrintStyle.Cyan => ConsoleColor.Cyan,
        PrintStyle.Magenta => ConsoleColor.Magenta,

        _ => DefaultColor,
    };
}
