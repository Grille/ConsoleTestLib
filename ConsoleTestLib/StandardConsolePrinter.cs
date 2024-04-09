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
    public string FailPrefix { get; set; } = "FAIL";
    public string ErrorPrefix { get; set; } = "ERROR";

    public bool PrintFullExceptions { get; set; } = true;

    public ConsoleColor DefaultColor { get; set; } = ConsoleColor.Gray;
    public ConsoleColor SuccesColor { get; set; } = ConsoleColor.Green;
    public ConsoleColor FailColor { get; set; } = ConsoleColor.Magenta;
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

    public void WriteTitle(string msg)
    {
        Write(msg, TitleColor);
    }

    public void WriteSucces(string msg)
    {
        Write(msg, SuccesColor);
    }

    public void WriteFail(string msg)
    {
        Write(msg, FailColor);
    }

    public void WriteError(string msg)
    {
        Write(msg, ErrorColor);
    }

    private void WriteSeperator(string message)
    {
        if (string.IsNullOrEmpty(message))
            return;

        if (message.Contains('\n'))
        {
            Write("\n");
        }
        else
        {
            Write(" ");
        }
    }

    public virtual void PrintTest(TestCase test)
    {
        Write(test.Name);
        Write(": ");
        switch (test.Status)
        {
            case TestStatus.Success:
                WriteSucces(SuccesPrefix);
                WriteSeperator(test.Message);
                WriteSucces(test.Message);
                break;
            case TestStatus.Failure:
                WriteFail(FailPrefix);
                WriteSeperator(test.Message);
                WriteFail(test.Message);
                break;
            case TestStatus.Error:
                WriteError(ErrorPrefix);
                string message;
                if (PrintFullExceptions)
                {
                    message = test.Exception != null ? test.Exception.ToString() : "null";
                }
                else
                {
                    message = test.Message;
                }
                WriteSeperator(message);
                WriteError(message);
                break;
            default:
                WriteError(test.Status.ToString());
                WriteSeperator(test.Message);
                WriteError(test.Message);
                break;
        }
        Write("\n");
    }

    public virtual void PrintSectionBegin(Section section)
    {
        WriteTitle(section.Name);
        Write("\n");
    }

    public virtual void PrintSectionEnd(Section section)
    {
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

    public virtual void PrintSummary(TestCounter counter)
    {
        WriteTitle("Results:\n");

        Write($"Testcases: {counter.Total}\n");
        Write($"* Success: {counter.Success}\n");
        Write($"* failure: {counter.Failure+counter.Error}\n");
    }
}


