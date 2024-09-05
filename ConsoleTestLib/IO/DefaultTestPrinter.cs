using System;
using System.IO;

namespace Grille.ConsoleTestLib.IO;

public class DefaultTestPrinter : ITestPrinter
{
    readonly IColoredTextWriter _writer;

    public string SuccesPrefix { get; set; } = "OK";
    public string WarnPrefix { get; set; } = "WARN";
    public string FailedPrefix { get; set; } = "FAIL";
    public string ErrorPrefix { get; set; } = "ERROR";

    public string Seperator { get; set; } = ": ";

    public bool PrintFailAsException { get; set; } = false;
    public bool PrintFullExceptions { get; set; } = false;
    public bool PrintDates { get; set; } = false;

    public bool ColoredNames { get; set; } = false;
    public bool ColoredMessages { get; set; } = true;
    public bool ColoredPrefixes { get; set; } = true;

    public Func<Exception, string>? ExceptionFormatter { get; set; }

    public Func<TestCaseDates, string>? DatesFormatter { get; set; }

    public DefaultTestPrinter()
    {
        _writer = new ConsoleWriter();
    }

    public DefaultTestPrinter(IColoredTextWriter writer)
    {
        _writer = writer;
    }

    public DefaultTestPrinter(TextWriter writer)
    {
        _writer = new TextWriterWrapper(writer);
    }

    protected void Write(string msg)
    {
        _writer.Write(msg);
    }

    protected void Write(string msg, PrintStyle color)
    {
        _writer.Write(msg, color);
    }

    protected void Write(string msg, PrintStyle color, bool useColor)
    {
        if (useColor)
            Write(msg, color);
        else
            Write(msg);
    }

    protected virtual void WriteSeperator(string message)
    {
        if (string.IsNullOrEmpty(message))
            return;

        if (message.Contains("\n"))
            Write("\n");
        else
            Write(" ");
    }

    protected virtual void WriteDates(TestCaseDates dates)
    {
        if (!PrintDates)
            return;

        if (DatesFormatter != null)
        {
            Write(DatesFormatter(dates));
        }
        else
        {
            Write("<");
            Write(dates.Ended.ToLongTimeString());
            Write("> ");
        }
    }

    protected virtual string ExceptionToString(Exception? exception, string onNull)
    {
        if (exception == null)
            return onNull;

        if (ExceptionFormatter == null)
            return exception.ToString();

        return ExceptionFormatter(exception);
    }

    public virtual void PrintTest(TestCase test)
    {
        var color = test.Status switch
        {
            TestStatus.Success => PrintStyle.Succes,
            TestStatus.Error => PrintStyle.Error,
            TestStatus.Warning => PrintStyle.Warn,
            TestStatus.Failure => PrintStyle.Failed,
            _ => PrintStyle.Invalid,
        };

        var prefix = test.Status switch
        {
            TestStatus.Success => SuccesPrefix,
            TestStatus.Error => ErrorPrefix,
            TestStatus.Warning => WarnPrefix,
            TestStatus.Failure => FailedPrefix,
            _ => $"STATUS_{test.Status}",
        };

        WriteDates(test.Dates);
        Write(test.Name, color, ColoredNames);
        Write(Seperator, color, ColoredNames);

        if (!string.IsNullOrEmpty(prefix))
        {
            Write(prefix, color, ColoredPrefixes);
        }

        string message;
        if (test.Status == TestStatus.Failure && PrintFailAsException)
            message = ExceptionToString(test.Exception, test.Message);
        else if (test.Status == TestStatus.Error && PrintFullExceptions)
            message = ExceptionToString(test.Exception, "null");
        else
            message = test.Message;

        WriteSeperator(message);
        Write(message, color, ColoredMessages);

        Write("\n");
    }

    public virtual void PrintSectionBegin(Section section)
    {
        Write(section.Name, PrintStyle.Title);
        Write("\n");
    }

    public virtual void PrintSectionEnd(Section section)
    {
        Write("\n");
    }

    private void PrintSummary(string name1, int value1, PrintStyle style1, string name2, int value2, PrintStyle style2, PrintStyle style0)
    {
        int sum = value1 + value2;
        Write(name1);
        Write(Seperator);
        if (sum == 0)
        {
            Write("0", style0);
        }
        else
        {
            Write(value1.ToString(), style1);
            if (value2 > 0)
            {
                Write(" (");
                Write(name2);
                Write(Seperator);
                Write(value2.ToString(), style2);
                Write(") -> ");
                Write(sum.ToString(), style2);
            }
        }
        Write("\n");
    }

    public virtual void PrintSummary(TestSummary counter)
    {
        Write("Results:\n", PrintStyle.Title);

        PrintSummary("Testcases", counter.Executed, PrintStyle.Title, "Unexecuted", counter.Unexecuted, PrintStyle.Invalid, PrintStyle.Title);
        PrintSummary("* Success", counter.Success, PrintStyle.Succes, "Warning", counter.Warning, PrintStyle.Warn, PrintStyle.Succes);
        PrintSummary("* Failure", counter.Failure, PrintStyle.Failed, "Error", counter.Error, PrintStyle.Error, PrintStyle.Succes);

        Write("\n");
    }
}


