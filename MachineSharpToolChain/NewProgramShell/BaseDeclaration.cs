using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineSharpToolChain.NewProgramShell
{
    //The baseline declaration, contains children of the type using and namespaces
    internal class BaseDeclaration : StandardDeclaration
    {
        //A list of using statements
        public List<UsingDeclaration> UsingDeclarations;
        //A list of namespace declarations
        public List<NamespaceDeclaration> NamespaceDeclarations;

        public BaseDeclaration()
        {
            UsingDeclarations = new List<UsingDeclaration>();
            NamespaceDeclarations = new List<NamespaceDeclaration>();
        }

        public string ToNativeCode()
        {
            return string.Empty;
        }
    }
}
