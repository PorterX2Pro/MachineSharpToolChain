using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineSharpToolChain.ProgramShell
{
    internal class VariableDeclaration : StandardDeclaration
    {
        public string VariableString = string.Empty;

        public VariableDeclaration()
        {
        }

        public string ToNativeCode()
        {
            return ToUtilities.ToStdType(VariableString);
        }
    }
}
