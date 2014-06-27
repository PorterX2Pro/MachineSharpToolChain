using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineSharpToolChain.ProgramShell.API
{
    /// <summary>
    /// Handles all console operations read write to std and such...
    /// </summary>
    internal class Console : StandardDeclaration
    {
        public string ManagedArguments = string.Empty;
        public ConsoleFunction consoleFunction = ConsoleFunction.Unset;

        public Console()
        {
        }

        public enum ConsoleFunction
        {
            Write,
            WriteLine,
            Read,
            ReadLine,
            ReadKey,
            Unset
        }

        public string ToNativeCode()
        {
            switch (consoleFunction)
            {
                case ConsoleFunction.Write:
                    return "System::Console::Write(" + ManagedArguments + ")";
                case ConsoleFunction.WriteLine:
                    return "System::Console::WriteLine(" + ManagedArguments + ")";
                case ConsoleFunction.ReadLine:
                    return "System::Console::ReadLine()";
                case ConsoleFunction.Read:
                    return "System::Console::Read()";
                case ConsoleFunction.ReadKey:
                    return "System::Console::ReadKey()";
            }
            return string.Empty;
        }
    }
}
