using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MachineSharpToolChain.NewCompileStages
{
    internal class SharpLexer
    {
        public static NewProgramShell.BaseDeclaration ToProgramShell(string Code)
        {
            //Create a base return for the base file
            NewProgramShell.BaseDeclaration baseReturn = new NewProgramShell.BaseDeclaration();
            //Initialize language data using grammer from Irony
            LanguageData language = new LanguageData(new CSharpGrammar());
            //Initialize the parser using the language
            Parser parser = new Parser(language);
            //Parse the code into a tree
            ParseTree parseTree = parser.Parse(Code);
            //Make sure root node is not null
            if (parseTree.Root != null)
            {
                foreach (ParseTreeNode childNode in parseTree.Root.ChildNodes)
                {
                    //Using Declarations
                    if (childNode.Term.Name == "using_directives_opt")
                    {
                        //Loop through each declaration (childNode.ChildNodes[0] = "using_directives")
                        foreach (ParseTreeNode usingChildNode in childNode.ChildNodes[0].ChildNodes)
                        {
                            //Using namespace declaration, two child nodes using (Keyword), qual_name_with_targs
                            if (usingChildNode.ChildNodes[0].Term.Name == "using_ns_directive")
                            {
                                //Get the using namespace from qual_name_with_targs
                                //Create a new declaration object to hold the info
                                NewProgramShell.UsingDeclaration usingDec = new NewProgramShell.UsingDeclaration();
                                if (usingChildNode.ChildNodes[0].ChildNodes[1].ChildNodes[1].ChildNodes.Count > 0)
                                {
                                    //Nested Namespaces (Separated by a .)
                                    usingDec.NamespaceLayout.Add(usingChildNode.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].Token.Text);
                                    //take child node[1] token as namespace
                                    foreach (ParseTreeNode nestedNamepspaceNode in usingChildNode.ChildNodes[0].ChildNodes[1].ChildNodes[1].ChildNodes)
                                    {
                                        //Add nested namespace
                                        usingDec.NamespaceLayout.Add(nestedNamepspaceNode.ChildNodes[1].Token.Text);
                                    }
                                }
                                else
                                {
                                    //One Namespace
                                    usingDec.NamespaceLayout.Add(usingChildNode.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].Token.Text);
                                }
                                //Add the declaration to the base file
                                baseReturn.UsingDeclarations.Add(usingDec);
                            }
                        }
                    }
                    //Namespace Declarations
                    else if (childNode.Term.Name == "namespace_declarations_opt")
                    {
                        //Loop through each namespace declaration
                        foreach (ParseTreeNode namespaceDec in childNode.ChildNodes)
                        {
                            //Found namespace declaration (just used to verify) childNodes[0] namespace(key), [1] qualifiedID, [2] namespace body, [3] semi????
                            if (namespaceDec.Term.Name == "namespace_declaration")
                            {
                                //Create new namespace decalration instance to hold information
                                NewProgramShell.NamespaceDeclaration namespaceDeclaration = new NewProgramShell.NamespaceDeclaration();
                                //Get the namespace name including nested ones separated by a .
                                foreach (ParseTreeNode childNamespaceName in namespaceDec.ChildNodes[1].ChildNodes)
                                {
                                    namespaceDeclaration.NamespaceLayout.Add(childNamespaceName.Token.Text);
                                }
                                //Handle In Namespace Using Declarations
                                #region namespaceIncludedUsingDeclarations
                                //Loop through each declaration (namespaceDec.ChildNodes[2].ChildNodes[1] = "using_directives_opts")
                                if (namespaceDec.ChildNodes[2].ChildNodes[1].ChildNodes.Count >= 1)
                                {
                                    foreach (ParseTreeNode usingChildNode in namespaceDec.ChildNodes[2].ChildNodes[1].ChildNodes[0].ChildNodes)
                                    {
                                        //Using namespace declaration, two child nodes using (Keyword), qual_name_with_targs
                                        if (usingChildNode.ChildNodes[0].Term.Name == "using_ns_directive")
                                        {
                                            //Get the using namespace from qual_name_with_targs
                                            //Create a new declaration object to hold the info
                                            NewProgramShell.UsingDeclaration usingDec = new NewProgramShell.UsingDeclaration();
                                            if (usingChildNode.ChildNodes[0].ChildNodes[1].ChildNodes[1].ChildNodes.Count > 0)
                                            {
                                                //Nested Namespaces (Separated by a .)
                                                usingDec.NamespaceLayout.Add(usingChildNode.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].Token.Text);
                                                //take child node[1] token as namespace
                                                foreach (ParseTreeNode nestedNamepspaceNode in usingChildNode.ChildNodes[0].ChildNodes[1].ChildNodes[1].ChildNodes)
                                                {
                                                    //Add nested namespace
                                                    usingDec.NamespaceLayout.Add(nestedNamepspaceNode.ChildNodes[1].Token.Text);
                                                }
                                            }
                                            else
                                            {
                                                //One Namespace
                                                usingDec.NamespaceLayout.Add(usingChildNode.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].Token.Text);
                                            }
                                            //Add the declaration to the namespace declaration
                                            namespaceDeclaration.UsingDeclarations.Add(usingDec);
                                        }
                                    }
                                }
                                #endregion
                                //Loop through namespace members
                                foreach (ParseTreeNode namespaceMember in namespaceDec.ChildNodes[2].ChildNodes[2].ChildNodes)
                                {
                                    //Class Declarations
                                    if (namespaceMember.Term.Name == "class_declaration")
                                    {
                                        //TODO: WORK WORK WORK
                                        //Create a new class declaration instance to hold information
                                        NewProgramShell.ClassDeclaration classDec = new NewProgramShell.ClassDeclaration();
                                        //Set the class identifier childnodes[2] = id
                                        classDec.ClassIdentifier = namespaceMember.ChildNodes[2].Token.Text;
                                        //Handle class modifiers
                                        foreach (ParseTreeNode classMod in namespaceMember.ChildNodes[0].ChildNodes[1].ChildNodes)
                                        {
                                            switch (classMod.ChildNodes[0].Token.Text)
                                            {
                                                case "public":
                                                    classDec.AccessModifier = NewProgramShell.ClassDeclaration.ClassAccessModifiers.Public;
                                                    break;
                                                case "private":
                                                    classDec.AccessModifier = NewProgramShell.ClassDeclaration.ClassAccessModifiers.Private;
                                                    break;
                                                case "internal":
                                                    classDec.AccessModifier = NewProgramShell.ClassDeclaration.ClassAccessModifiers.Internal;
                                                    break;
                                                case "protected":
                                                    classDec.AccessModifier = NewProgramShell.ClassDeclaration.ClassAccessModifiers.Protected;
                                                    break;
                                                case "sealed":
                                                    classDec.Modifier = NewProgramShell.ClassDeclaration.ClassModifiers.Sealed;
                                                    break;
                                                case "new":
                                                    classDec.Modifier = NewProgramShell.ClassDeclaration.ClassModifiers.New;
                                                    break;
                                                case "abstract":
                                                    classDec.Modifier = NewProgramShell.ClassDeclaration.ClassModifiers.Abstract;
                                                    break;
                                                case "partial":
                                                    classDec.Modifier = NewProgramShell.ClassDeclaration.ClassModifiers.Partial;
                                                    break;
                                            }
                                        }
                                        //Handle Function declarations
                                        foreach (ParseTreeNode funcDec in namespaceMember.ChildNodes[6].ChildNodes[0].ChildNodes)
                                        {
                                            //Function declarations
                                            if (funcDec.Term.Name == "method_declaration")
                                            {
                                                //Create new instance of function declaration to hold information
                                                NewProgramShell.FunctionDeclaration funcDeclaration = new NewProgramShell.FunctionDeclaration();
                                                //Get the function name from childnode[2] qual_name_with_targs, [0] id_bulitin, [0] id
                                                funcDeclaration.FunctionName = funcDec.ChildNodes[2].ChildNodes[0].ChildNodes[0].Token.Text;
                                                //Handle (Actions) {variable declarations and api calls and stuff}
                                                //A little error checking to make sure we actually have actions inside the function
                                                if (funcDec.ChildNodes[5].ChildNodes.Count >= 1 && funcDec.ChildNodes[5].ChildNodes[0].ChildNodes.Count >= 1 && funcDec.ChildNodes[5].ChildNodes[0].ChildNodes[0].ChildNodes.Count >= 1)
                                                {
                                                    //Loop through each action
                                                    foreach (ParseTreeNode actDec in funcDec.ChildNodes[5].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes)
                                                    {
                                                        //Handle variable declarations
                                                        if (actDec.Term.Name == "declaration_statement")
                                                        {

                                                        }
                                                        //Handle api calls and statements
                                                        else if (actDec.Term.Name == "statement_expression")
                                                        {
                                                            NewProgramShell.StatementDeclaration statDec = new NewProgramShell.StatementDeclaration();
                                                            //First part of the resolve at actDec.childnodes[0].childnodes[0][0][0].Token.Text
                                                            //Initialize a string to append parts to
                                                            string buildResolve = actDec.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].Token.Text;
                                                            //Loop through other resolve parts
                                                            foreach (ParseTreeNode resolveParts in actDec.ChildNodes[0].ChildNodes[1].ChildNodes)
                                                            {
                                                                //Append the part with a . in front
                                                                if (resolveParts.ChildNodes.Count == 2)
                                                                {
                                                                    buildResolve += "." + resolveParts.ChildNodes[1].Token.Text;
                                                                }
                                                                else
                                                                {
                                                                    //resolveParts = Argument List
                                                                    System.Diagnostics.Debugger.Break();
                                                                }
                                                            }
                                                            //Set the complete resolve name
                                                            statDec.StatementResolveName = buildResolve.ToString();
                                                            //Add the statement declaration to the function declaration
                                                            funcDeclaration.FunctionCodeBlock.Add(statDec);
                                                        }
                                                        //Handle (While, For, Foreach, Do While)
                                                        else if (actDec.Term.Name == "iteration_statement")
                                                        {

                                                        }
                                                    }
                                                }
                                            }
                                            //Variable declaration
                                            else if (funcDec.Term.Name == "field_declaration")
                                            {

                                            }
                                        }
                                        namespaceDeclaration.ClassDeclarations.Add(classDec);
                                    }
                                    //Interface Declarations
                                    else if (namespaceMember.Term.Name == "interface")
                                    {

                                    }
                                }
                                //Add the declaration to the base file
                                baseReturn.NamespaceDeclarations.Add(namespaceDeclaration);
                            }
                        }
                    }
                }
            }  


            //Dump Commands
            //using (StreamWriter writeDump = new StreamWriter("C:\\temp\\dump.log"))
            //{
            //    writeDump.WriteLine(DebugTools.var_dumpold(parseTree));
            //    dump(parseTree.Root, writeDump);
            //}
            return baseReturn;
        }

        public static void dump(ParseTreeNode node, StreamWriter writeDump)
        {
            writeDump.WriteLine(DebugTools.var_dumpold(node));
            foreach (ParseTreeNode nc in node.ChildNodes)
            {
                dump(nc, writeDump);
            }
        }
    }
}
