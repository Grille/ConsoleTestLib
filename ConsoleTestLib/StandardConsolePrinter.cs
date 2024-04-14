using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Grille.ConsoleTestLib;

public class StandardConsolePrinter : ITestPrinter
{
    public string SuccesPrefix { get; set; } = "OK";
    public string WarnPrefix { get; set; } = "WARN";
    public string FailedPrefix { get; set; } = "FAIL";
    public string ErrorPrefix { get; set; } = "ERROR";

    public string Seperator { get; set; } = ":";

    public bool PrintFailAsException { get; set; } = false;
    public bool PrintFullExceptions { get; set; } = true;

    public bool ColoredNames { get; set; } = false;
    public bool ColoredMessages { get; set; } = true;
    public bool ColoredPrefixes { get; set; } = true;

    public ConsoleColor DefaultColor { get; set; } = ConsoleColor.Gray;
    public ConsoleColor SuccesColor { get; set; } = ConsoleColor.Green;
    public ConsoleColor WarnColor { get; set; } = ConsoleColor.DarkYellow;
    public ConsoleColor FailedColor { get; set; } = ConsoleColor.Magenta;
    public ConsoleColor ErrorColor { get; set; } = ConsoleColor.Red;
    public ConsoleColor TitleColor { get; set; } = ConsoleColor.Cyan;

    public void Write(string msg)
    {
        Console.ForegroundColor = DefaultColor;
        Console.Write(msg);
    }

    public void Write(string msg, ConsoleColor color)
    {
        var bcolor = Console.ForegroundColor;

        Console.ForegroundColor = color;
        Console.Write(msg);
        Console.ForegroundColor = bcolor;
    }

    public void Write(string msg, ConsoleColor color, bool useColor)
    {
        if (useColor)
            Write(msg, color);
        else
            Write(msg);
    }

    private void WriteSeperator(string message)
    {
        if (string.IsNullOrEmpty(message))
            return;

        if (message.Contains('\n'))
            Write("\n");
        else
            Write(" ");
    }

    public virtual void PrintTest(TestCase test)
    {
        var color = test.Status switch
        {
            TestStatus.Success => SuccesColor,
            TestStatus.Error => ErrorColor,
            TestStatus.Warning => WarnColor,
            TestStatus.Failure => FailedColor,
            _ => ErrorColor,
        };

        var prefix = test.Status switch
        {
            TestStatus.Success => SuccesPrefix,
            TestStatus.Error => ErrorPrefix,
            TestStatus.Warning => WarnPrefix,
            TestStatus.Failure => FailedPrefix,
            _ => string.Empty,
        };

        Write(test.Name, color, ColoredNames);
        Write(Seperator, color, ColoredNames);

        if (!string.IsNullOrEmpty(prefix))
        {
            Write(" ");
            Write(prefix, color, ColoredPrefixes);
        }

        string message;
        if (test.Status == TestStatus.Failure && PrintFailAsException)
            message = test.Exception != null ? test.Exception.ToString() : test.Message;
        else if (test.Status == TestStatus.Error && PrintFullExceptions)
            message = test.Exception != null ? test.Exception.ToString() : "null";
        else
            message = test.Message;

        WriteSeperator(message);
        Write(message, color, ColoredMessages);

        Write("\n");
    }

    public virtual void PrintSectionBegin(Section section)
    {
        if (section.TestCases.Count == 0)
            return;

        Write(section.Name, TitleColor);
        Write("\n");
    }

    public virtual void PrintSectionEnd(Section section)
    {
        if (section.TestCases.Count == 0)
            return;

        Write("\n");
    }

    public void PrintSection(Section section)
    {
        PrintSectionBegin(section);

        foreach (var test in section.TestCases)
        {
            test.Wait();
            PrintTest(test);
        }

        PrintSectionEnd(section);
    }

    public void PrintSections(IEnumerable<Section> sections)
    {
        foreach (Section section in sections)
        {
            PrintSection(section);
        }
    }

    public virtual void PrintSummary(TestSummary counter)
    {
        Write("Results:\n", TitleColor);

        Write($"Testcases: {counter.Total}\n");
        Write($"* Success: {counter.Success + counter.Warning}\n");
        Write($"* failure: {counter.Failure + counter.Error}\n");
    }
}


