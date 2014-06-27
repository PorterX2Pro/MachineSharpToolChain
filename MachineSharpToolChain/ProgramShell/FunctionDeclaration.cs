using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineSharpToolChain.ProgramShell
{
    internal class FunctionDeclaration : StandardDeclaration
    {
        public List<StandardDeclaration> childrenDeclarations;
        public Modifiers ScopeModifier;
        public bool isStatic = false;

        public string FunctionName = string.Empty;
        public string FunctionArguments = string.Empty;

        public FunctionDeclaration()
        {
            childrenDeclarations = new List<StandardDeclaration>();
        }

        public enum Modifiers
        {
            Public,
            Private
        }

        public string ToNativeCode()
        {
            throw new NotImplementedException();
        }
    }
}
