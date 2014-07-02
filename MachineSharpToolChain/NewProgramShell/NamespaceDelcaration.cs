using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineSharpToolChain.NewProgramShell
{
    internal class NamespaceDeclaration : StandardDeclaration
    {
        public List<string> NamespaceLayout;
        public List<ClassDeclaration> ClassDeclarations;
        public List<UsingDeclaration> UsingDeclarations;

        public NamespaceDeclaration()
        {
            NamespaceLayout = new List<string>();
            ClassDeclarations = new List<ClassDeclaration>();
            UsingDeclarations = new List<UsingDeclaration>();
        }

        public string ToNativeCode()
        {
            //Not directly converted to c++ code
            return string.Empty;
        }
    }
}
