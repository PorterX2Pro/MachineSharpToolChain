using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineSharpToolChain.ProgramShell
{
    internal class NamespaceDeclaration : StandardDeclaration
    {
        public List<StandardDeclaration> childrenDeclarations;

        public string NamespaceName = string.Empty;

        public NamespaceDeclaration()
        {
            childrenDeclarations = new List<StandardDeclaration>();
        }

        public string ToNativeCode()
        {
            throw new NotImplementedException();
        }
    }
}
