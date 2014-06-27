using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineSharpToolChain
{
    internal class ToUtilities
    {
        public static string ConvertArgumentDeclarations(string args)
        {
            if (args.Contains("[") && args.Contains("]"))
            {
                if (args.Contains(","))
                {
                    //Multiple args or an event modifier
                }
                else
                {
                    //A single arg or an event modifier
                    if (!args.Contains("<") && !args.Contains(">"))
                    {
                        //A single argument with array
                        if (args.Contains("[]"))
                        {
                            //Nothing in braces
                            //Get the type
                            int End = args.IndexOf("[");
                            string type = args.Substring(0, End);
                            //Get variable name (find space after the ])
                            int EndBrace = args.LastIndexOf("]") + 1;
                            string varname = args.Substring(EndBrace).Trim();
                            return type + " " + varname + "[]";
                        }
                        else
                        {
                            //Get the type
                            int End = args.IndexOf("[");
                            string type = args.Substring(0, End);
                        }
                    }
                    else
                    {
                        //Event modifier ex. Dictionary<string,bool>
                    }
                }
            }
            return args;
        }

        public static string ToStdType(string arg)
        {
            return arg.Replace("string", "std::string");
        }
    }
}
