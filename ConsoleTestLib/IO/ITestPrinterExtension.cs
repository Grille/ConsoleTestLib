using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grille.ConsoleTestLib.IO;

public static class ITestPrinterExtension
{
    public static void PrintTestCases(this ITestPrinter printer, IEnumerable<TestCase> testcases)
    {
        foreach (var test in testcases)
        {
            test.Wait();
            printer.PrintTest(test);
        }
    }

    public static void PrintSection(this ITestPrinter printer, Section section)
    {
        printer.PrintSectionBegin(section);
        printer.PrintTestCases(section.TestCases);
        printer.PrintSectionEnd(section);
    }

    public static void PrintSections(this ITestPrinter printer, IEnumerable<Section> sections)
    {
        foreach (var section in sections)
        {
            if (section.TestCases.Count == 0)
                continue;

            printer.PrintSection(section);
        }
    }

    public static Task PrintSectionsAsync(this ITestPrinter printer, IEnumerable<Section> sections)
    {
        return Task.Run(() => PrintSections(printer, sections));
    }
}
