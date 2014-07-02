using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineSharpToolChain.NewProgramShell
{
    internal class ClassDeclaration : StandardDeclaration
    {
        public string ClassIdentifier = string.Empty;
        public List<FunctionDeclaration> FunctionDeclarations;
        public List<StandardDeclaration> VariableDeclarations;
        public ClassAccessModifiers AccessModifier;
        public ClassModifiers Modifier;

        public enum ClassAccessModifiers
        {
            Public,
            Private,
            Internal,
            Protected
        }

        public enum ClassModifiers
        {
            None,
            Abstract,
            Partial,
            Sealed,
            New
        }

        public ClassDeclaration()
        {
            AccessModifier = ClassAccessModifiers.Public;
            Modifier = ClassModifiers.None;
            FunctionDeclarations = new List<FunctionDeclaration>();
            VariableDeclarations = new List<StandardDeclaration>();
        }

        public string ToNativeCode()
        {
            return string.Empty;
        }
    }
}
