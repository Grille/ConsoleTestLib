using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grille.ConsoleTestLib;

public interface ITestPrinter
{
    public void PrintTest(TestCase test);

    public void PrintSectionBegin(Section section);

    public void PrintSectionEnd(Section section);

    public void Print(ICollection<Section> sections);

    public void PrintSumary(TestCounter counter);
}

public enum ITestPrinterHighlight
{
    Text,
    Title,
    Succes,
    Fail,
    Error,
}
