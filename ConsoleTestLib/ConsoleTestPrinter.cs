using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Grille.ConsoleTestLib;

internal class ConsoleTestPrinter : ITestPrinter
{
    public static void Write(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write(msg);
    }

    public static void Write(string msg, ConsoleColor color)
    {
        var bcolor = Console.ForegroundColor;

        Console.ForegroundColor = color;
        Console.Write(msg);

        Console.ForegroundColor = bcolor;
    }

    public static void WriteTitle(string msg)
    {
        Write(msg, ConsoleColor.Cyan);
    }

    public static void WriteSucces(string msg)
    {
        Write(msg, ConsoleColor.Green);
    }

    public static void WriteFail(string msg)
    {
        Write(msg, ConsoleColor.Magenta);
    }

    public static void WriteError(string msg)
    {
        Write(msg, ConsoleColor.Red);
    }

    public virtual void PrintTest(TestCase test)
    {
        Write(test.Name);
        Write(": ");
        switch (test.Result)
        {
            case TestResult.Success:
                WriteSucces("OK ");
                WriteSucces(test.Message);
                break;
            case TestResult.Failure:
                WriteFail("FAIL ");
                WriteFail(test.Message);
                break;
            case TestResult.Error:
                WriteError("ERROR\n");
                WriteError(test.Message);
                break;
            default:
                WriteError("NONE ");
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

    public virtual void Print(ICollection<Section> sections)
    {
        foreach (Section section in sections)
        {
            PrintSectionBegin(section);
        }
    }

    public virtual void PrintSumary(TestCounter counter)
    {
        WriteTitle("Results:\n");

        Write($"Testcases: {counter.Total}\n");
        Write($"* Success: {counter.Success}\n");
        Write($"* failure: {counter.Failure+counter.Error}\n");
    }
}


