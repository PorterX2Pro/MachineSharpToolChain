using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineSharpToolChain.CompileStages
{
    internal class ProgramShellHelp
    {
        public static ProgramShell.BaseDeclaration ToProgramShell(string Code)
        {
            ProgramShell.BaseDeclaration baseReturn = new ProgramShell.BaseDeclaration();
            List<ProgramShell.StandardDeclaration> codestack = new List<ProgramShell.StandardDeclaration>();
            string[] SplittedCode = Code.Split('\n');
            for (int i = 0; i < SplittedCode.Length; i++)
            {
                string CurrentLine = SplittedCode[i].Trim();
                string ScopeRemoveLine = StripScopeModifiers(CurrentLine);
                if (CurrentLine.StartsWith("namespace"))
                {
                    codestack.Add(new ProgramShell.NamespaceDeclaration() { NamespaceName = CurrentLine.Substring(10) });
                }
                else if (ScopeRemoveLine.StartsWith("class"))
                {
                    codestack.Add(new ProgramShell.ClassDeclaration { ClassName = ScopeRemoveLine.Substring(6) });
                }
                else if (ScopeRemoveLine.StartsWith("void"))
                {
                    if (CurrentLine.Contains("private"))
                    {
                        string NameAndArgs = ScopeRemoveLine.Substring(5);
                        if (NameAndArgs.Contains("()"))
                        {
                            //Purged
                            codestack.Add(new ProgramShell.FunctionDeclaration { FunctionName = ScopeRemoveLine.Substring(5).Replace("()", ""), isStatic = (CurrentLine.Contains("static")), ScopeModifier = ProgramShell.FunctionDeclaration.Modifiers.Private });
                        }
                        else
                        {
                            //Needs Purge
                            int Start = NameAndArgs.IndexOf("(") + 1;
                            int End = NameAndArgs.LastIndexOf(")") - 1;
                            codestack.Add(new ProgramShell.FunctionDeclaration { FunctionName = ScopeRemoveLine.Substring(5, Start - 1), isStatic = (CurrentLine.Contains("static")), ScopeModifier = ProgramShell.FunctionDeclaration.Modifiers.Private, FunctionArguments = NameAndArgs.Substring(Start, End - Start) });
                        }
                    }
                    else
                    {
                        string NameAndArgs = ScopeRemoveLine.Substring(5);
                        if (NameAndArgs.Contains("()"))
                        {
                            //Purged
                            codestack.Add(new ProgramShell.FunctionDeclaration { FunctionName = ScopeRemoveLine.Substring(5).Replace("()", ""), isStatic = (CurrentLine.Contains("static")), ScopeModifier = ProgramShell.FunctionDeclaration.Modifiers.Public });
                        }
                        else
                        {
                            //Needs Purge
                            int Start = NameAndArgs.IndexOf("(") + 1;
                            int End = NameAndArgs.LastIndexOf(")") - 1;
                            codestack.Add(new ProgramShell.FunctionDeclaration { FunctionName = ScopeRemoveLine.Substring(5, Start - 1), isStatic = (CurrentLine.Contains("static")), ScopeModifier = ProgramShell.FunctionDeclaration.Modifiers.Public, FunctionArguments = NameAndArgs.Substring(Start, End - Start) });
                        }
                    }
                }
                else if (ScopeRemoveLine == "}")
                {
                    System.Diagnostics.Debug.WriteLine(codestack.Last().GetType());
                    if (codestack.Last() is ProgramShell.FunctionDeclaration)
                    {
                        //Find last function declaration and add to the last class
                        //Work backwards
                        int IndexOfLastClass = codestack.FindLastIndex(delegate(ProgramShell.StandardDeclaration sd) { return (sd is ProgramShell.ClassDeclaration); });
                        List<ProgramShell.StandardDeclaration> cloneList = codestack.GetRange(IndexOfLastClass + 1, codestack.Count - (IndexOfLastClass + 1));
                        foreach (ProgramShell.StandardDeclaration sdec in cloneList)
                        {
                            ((ProgramShell.ClassDeclaration)codestack[IndexOfLastClass]).childrenDeclarations.Add(sdec);
                            codestack.Remove(sdec);
                        }
                    }
                    else if (codestack.Last() is ProgramShell.ClassDeclaration)
                    {
                        //Find last class declaration and add to the last namespace
                        //Work backwards
                        int IndexOfLastNamespace = codestack.FindLastIndex(delegate(ProgramShell.StandardDeclaration sd) { return (sd is ProgramShell.NamespaceDeclaration); });
                        List<ProgramShell.StandardDeclaration> cloneList = codestack.GetRange(IndexOfLastNamespace + 1, codestack.Count - (IndexOfLastNamespace + 1));
                        foreach (ProgramShell.StandardDeclaration sdec in cloneList)
                        {
                            ((ProgramShell.NamespaceDeclaration)codestack[IndexOfLastNamespace]).childrenDeclarations.Add(sdec);
                            codestack.Remove(sdec);
                        }
                    }
                    else if (codestack.Last() is ProgramShell.NamespaceDeclaration)
                    {
                        //Find last namespace declaration and add to the base declaration
                        //Work backwards
                        int IndexOfLastNamespace = codestack.FindLastIndex(delegate(ProgramShell.StandardDeclaration sd) { return (sd is ProgramShell.NamespaceDeclaration); });
                        if (IndexOfLastNamespace > 0)
                        {
                            //Nested Namespaces
                            //Find namespace before this one
                        }
                        else
                        {
                            //Top level Namespace
                            //Add to base
                            baseReturn.childrenDeclarations.Add(codestack[IndexOfLastNamespace]);
                            codestack.RemoveAt(IndexOfLastNamespace);
                        }
                    }
                    else if (codestack.Last() is ProgramShell.APIDeclaration || codestack.Last() is ProgramShell.VariableDeclaration)
                    {
                        //Find last var/api declaration and add to the last function
                        //Work backwards
                        int IndexOfLastNamespace = codestack.FindLastIndex(delegate(ProgramShell.StandardDeclaration sd) { return (sd is ProgramShell.FunctionDeclaration); });
                        List<ProgramShell.StandardDeclaration> cloneList = codestack.GetRange(IndexOfLastNamespace + 1, codestack.Count - (IndexOfLastNamespace + 1));
                        foreach (ProgramShell.StandardDeclaration sdec in cloneList)
                        {
                            ((ProgramShell.FunctionDeclaration)codestack[IndexOfLastNamespace]).childrenDeclarations.Add(sdec);
                            codestack.Remove(sdec);
                        }
                    }
                }
                else if (CurrentLine != "{")
                {
                    if (isVariableDeclaration(CurrentLine))
                    {
                        //Check the value of variable declaration... if the thing contains an = operater
                        string NextSpacedSep = GetNextSpaceSep(CurrentLine, 2);
                        if (NextSpacedSep == "=")
                        {
                            //Equal Operator, check for a following .Net Class

                            //Check for New Then a .Net Class

                            //No following .Net Class? must be a constant value such as strings ("") or bool or such

                        }
                        else if (NextSpacedSep == string.Empty || NextSpacedSep == null)
                        {
                            //Probably ends with a ; check to be sure
                            if (CurrentLine.EndsWith(";"))
                            {

                            }
                        }
                        codestack.Add(new ProgramShell.VariableDeclaration() { VariableString = CurrentLine });
                        //TODO:
                        //Modify a functions arguments to include variable changes
                        //All of this stuff
                        //Inject the (Managed) main to a (Native) main (int main = console, WinMain = winforms)
                        //Namespace stuff
                        //Last Lambada expressions
                    }
                    else
                    {
                        //API Call
                        if (CurrentLine.StartsWith("System.Console.") || CurrentLine.StartsWith("Console."))
                        {
                            int ConsoleEnd = CurrentLine.IndexOf("Console.") + 8;
                            int EndOf = 0;
                            if (CurrentLine.Contains("("))
                            {
                                EndOf = CurrentLine.IndexOf("(");
                            }
                            else
                            {
                                EndOf = CurrentLine.IndexOf(" ");
                            }
                            string NextWord = CurrentLine.Substring(ConsoleEnd, EndOf - ConsoleEnd);
                            int astart = CurrentLine.IndexOf("(") + 1;
                            int aend = CurrentLine.LastIndexOf(")");
                            string Arguments = CurrentLine.Substring(astart, aend - astart);
                            if (NextWord == "Write")
                            {
                                codestack.Add(new ProgramShell.APIDeclaration(new ProgramShell.API.Console() { consoleFunction = ProgramShell.API.Console.ConsoleFunction.Write, ManagedArguments = Arguments }));
                            }
                            else if (NextWord == "WriteLine")
                            {
                                codestack.Add(new ProgramShell.APIDeclaration(new ProgramShell.API.Console() { consoleFunction = ProgramShell.API.Console.ConsoleFunction.WriteLine, ManagedArguments = Arguments }));
                            }
                            else if (NextWord == "Read")
                            {
                                codestack.Add(new ProgramShell.APIDeclaration(new ProgramShell.API.Console() { consoleFunction = ProgramShell.API.Console.ConsoleFunction.Read }));
                            }
                            else if (NextWord == "ReadLine")
                            {
                                codestack.Add(new ProgramShell.APIDeclaration(new ProgramShell.API.Console() { consoleFunction = ProgramShell.API.Console.ConsoleFunction.ReadLine }));
                            }
                            else if (NextWord == "ReadKey")
                            {
                                codestack.Add(new ProgramShell.APIDeclaration(new ProgramShell.API.Console() { consoleFunction = ProgramShell.API.Console.ConsoleFunction.ReadKey }));
                            }
                        }
                        else if (CurrentLine.StartsWith("System.IO.File") || CurrentLine.StartsWith("IO.File.") || CurrentLine.StartsWith("File."))
                        {
                            int ConsoleEnd = CurrentLine.IndexOf("Console.") + 8;
                            int EndOf = 0;
                            if (CurrentLine.Contains("("))
                            {
                                EndOf = CurrentLine.IndexOf("(");
                            }
                            else
                            {
                                EndOf = CurrentLine.IndexOf(" ");
                            }
                            string NextWord = CurrentLine.Substring(ConsoleEnd, EndOf - ConsoleEnd);
                            int astart = CurrentLine.IndexOf("(") + 1;
                            int aend = CurrentLine.LastIndexOf(")");
                            string Arguments = CurrentLine.Substring(astart, aend - astart);
                            if (NextWord == "w")
                            {

                            }
                            else if (NextWord == "")
                            {

                            }
                        }
                    }
                }
                Console.WriteLine(CurrentLine);
            }
            if (codestack.Count >= 1)
            {
                foreach (ProgramShell.StandardDeclaration sdec in codestack)
                {
                    baseReturn.childrenDeclarations.Add(sdec);
                }
            }
            return baseReturn;
        }

        private static bool isVariableDeclaration(string line)
        {
            string lower = line.ToLower();
            if (lower.StartsWith("string") || lower.StartsWith("int") || lower.StartsWith("uint") || lower.StartsWith("bool") || lower.StartsWith("boolean") || lower.StartsWith("int32") || lower.StartsWith("int64") || lower.StartsWith("uint32"))
            {
                return true;
            }

            return false;
        }

        private static string GetNextSpaceSep(string line, int Position)
        {
            List<string> wordBuilder = new List<string>();
            StringBuilder currentBuild = null;
            foreach (char c in line.ToCharArray())
            {
                if (c != (" ").ToCharArray()[0])
                {
                    if (currentBuild != null)
                    {
                        currentBuild.Append(c);
                    }
                    else
                    {
                        currentBuild = new StringBuilder();
                        currentBuild.Append(c);
                    }
                }
                else
                {
                    if (currentBuild != null)
                    {
                        wordBuilder.Add(currentBuild.ToString());
                        currentBuild = null;
                    }
                }
            }
            if (currentBuild != null)
            {
                wordBuilder.Add(currentBuild.ToString());
                currentBuild = null;
            }
            if ((wordBuilder.Count - 1) >= Position)
            {
                return wordBuilder[Position];
            }
            return string.Empty;
        }

        private static string StripScopeModifiers(string line)
        {
            return line.Replace("static ", "").Replace("internal ", "").Replace("public ", "").Replace("protected ", "").Replace("virtual ", "").Replace("unsafe ", "");
        }
    }
}
