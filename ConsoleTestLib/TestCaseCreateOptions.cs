using Grille.ConsoleTestLib.IO;

namespace Grille.ConsoleTestLib;

public record struct TestCaseCreateOptions(string Name, bool RethrowExeptions, bool RethrowFailed, bool ExecuteImmediately);