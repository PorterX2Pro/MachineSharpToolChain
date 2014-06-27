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
                                

                                //Add the declaration to the base file
                                baseReturn.UsingDeclarations.Add(usingDec);
                            }
                        }
                    }
                    System.Diagnostics.Debug.WriteLine(childNode.Term);
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
