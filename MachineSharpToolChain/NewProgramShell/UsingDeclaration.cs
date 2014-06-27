using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineSharpToolChain.NewProgramShell
{
    internal class UsingDeclaration : StandardDeclaration
    {
        public List<string> NamespaceLayout;

        public UsingDeclaration()
        {
            NamespaceLayout = new List<string>();
        }

        public string ToNativeCode()
        {
            //Not directly converted to c++ code
            return string.Empty;
        }
    }
}
