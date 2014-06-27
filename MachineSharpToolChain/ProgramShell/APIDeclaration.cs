using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineSharpToolChain.ProgramShell
{
    internal class APIDeclaration : StandardDeclaration
    {
        public List<StandardDeclaration> apiChildren;

        public APIDeclaration(StandardDeclaration api)
        {
            apiChildren = new List<StandardDeclaration>();
            apiChildren.Add(api);
        }

        public APIDeclaration()
        {
            apiChildren = new List<StandardDeclaration>();
        }

        public string ToNativeCode()
        {
            return apiChildren[0].ToNativeCode() + ";";
        }
    }
}
