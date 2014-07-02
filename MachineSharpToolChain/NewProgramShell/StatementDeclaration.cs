using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineSharpToolChain.NewProgramShell
{
    internal class StatementDeclaration : StandardDeclaration
    {
        public string StatementResolveName = string.Empty;
        public List<StandardDeclaration> Arguments;

        public StatementDeclaration()
        {
            Arguments = new List<StandardDeclaration>();
        }

        public string ToNativeCode()
        {
            return string.Empty;
        }
    }
}
