using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineSharpToolChain.NewProgramShell
{
    internal class FunctionDeclaration : StandardDeclaration
    {
        public string FunctionName = string.Empty;
        public List<StandardDeclaration> FunctionCodeBlock;

        public FunctionDeclaration()
        {
            FunctionCodeBlock = new List<StandardDeclaration>();
        }

        public string ToNativeCode()
        {
            return string.Empty;
        }
    }
}
