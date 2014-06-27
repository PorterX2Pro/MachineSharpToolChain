using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MachineSharpToolChain.CompileStages
{
    internal class NativeProgramShell
    {
        public static void ToNativeClass(ProgramShell.ClassDeclaration managedClass, StreamWriter fileWriter)
        {
            fileWriter.WriteLine("class " + managedClass.ClassName + " {");
            //Handle Private Scope
            foreach (ProgramShell.StandardDeclaration sdec in managedClass.childrenDeclarations)
            {
                if (sdec is ProgramShell.FunctionDeclaration)
                {
                    ProgramShell.FunctionDeclaration func = (ProgramShell.FunctionDeclaration)sdec;
                    if (func.ScopeModifier == ProgramShell.FunctionDeclaration.Modifiers.Private)
                    {
                        ToNativeFunction(func, fileWriter);
                        fileWriter.WriteLine(";");
                    }
                }
            }
            fileWriter.WriteLine("public:");
            //Handle Public Scope
            foreach (ProgramShell.StandardDeclaration sdec in managedClass.childrenDeclarations)
            {
                if (sdec is ProgramShell.FunctionDeclaration)
                {
                    ProgramShell.FunctionDeclaration func = (ProgramShell.FunctionDeclaration)sdec;
                    if (func.ScopeModifier == ProgramShell.FunctionDeclaration.Modifiers.Public)
                    {
                        ToNativeFunction(func, fileWriter);
                        fileWriter.WriteLine(";");
                    }
                }
            }
            fileWriter.WriteLine("};");
        }

        public static void ToNativeNamespace(ProgramShell.NamespaceDeclaration managedNamespace, StreamWriter fileWriter)
        {
            fileWriter.WriteLine("namespace " + managedNamespace.NamespaceName + " {");
            foreach (ProgramShell.StandardDeclaration sdec in managedNamespace.childrenDeclarations)
            {
                if (sdec is ProgramShell.ClassDeclaration)
                {
                    ToNativeClass((ProgramShell.ClassDeclaration)sdec, fileWriter);
                }
            }
            fileWriter.Write("}");
        }

        public static void ToNativeFunction(ProgramShell.FunctionDeclaration managedFunction, StreamWriter fileWriter)
        {
            if (managedFunction.isStatic)
            {
                fileWriter.WriteLine("static void " + managedFunction.FunctionName + "(" + ToUtilities.ToStdType(ToUtilities.ConvertArgumentDeclarations(managedFunction.FunctionArguments)) + ")" + " {");
            }
            else
            {
                fileWriter.WriteLine("void " + managedFunction.FunctionName + "(" + ToUtilities.ToStdType(ToUtilities.ConvertArgumentDeclarations(managedFunction.FunctionArguments)) + ")" + " {");
            }
            foreach (ProgramShell.StandardDeclaration sdec in managedFunction.childrenDeclarations)
            {
                if (sdec is ProgramShell.VariableDeclaration)
                {
                    fileWriter.WriteLine(((ProgramShell.VariableDeclaration)sdec).ToNativeCode());
                }
                else
                {
                    //Must be an api call of some sort
                    fileWriter.WriteLine(((ProgramShell.APIDeclaration)sdec).ToNativeCode());
                }
            }
            fileWriter.Write("}");
        }
    }
}
