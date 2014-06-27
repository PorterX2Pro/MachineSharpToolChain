using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineSharpToolChain.ProgramShell
{
    internal class BaseDeclaration : StandardDeclaration
    {
        public List<StandardDeclaration> childrenDeclarations;

        public BaseDeclaration()
        {
            childrenDeclarations = new List<StandardDeclaration>();
        }

        public string ToNativeCode()
        {
            throw new NotImplementedException();
        }
    }
}
