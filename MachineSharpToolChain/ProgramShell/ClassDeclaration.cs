using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineSharpToolChain.ProgramShell
{
    internal class ClassDeclaration : StandardDeclaration
    {
        public List<StandardDeclaration> childrenDeclarations;

        public string ClassName = string.Empty;

        public ClassDeclaration()
        {
            childrenDeclarations = new List<StandardDeclaration>();
        }

        public string ToNativeCode()
        {
            throw new NotImplementedException();
        }
    }
}
