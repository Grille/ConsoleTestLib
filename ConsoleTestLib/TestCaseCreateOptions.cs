namespace Grille.ConsoleTestLib;

public record struct TestCaseCreateOptions(string Name, bool RethrowExeptions, bool ExecuteImmediately, ITestPrinter Printer) { }