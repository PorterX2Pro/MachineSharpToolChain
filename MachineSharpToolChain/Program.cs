using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace MachineSharpToolChain
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("MSTC Command Line Builder");
            //Arguments, args[0] = input file
            if (args.Length >= 1)
            {
                if (args[0].EndsWith(".cs"))
                {
                    Stopwatch totalTime = new Stopwatch();
                    totalTime.Start();
                    //Output input file
                    Console.WriteLine("Input: " + args[0]);
                    //Single C# file
                    global.BuildDirectory = Path.Combine(Path.GetDirectoryName(args[0]), "mcsbuildenv");
                    //Output build environment
                    Console.WriteLine("Build Env: " + global.BuildDirectory);
                    //Invoke compiler for a single .cs file
                    CompileStages.Compiler.CompileSingleFile(args[0]);
                    totalTime.Stop();
                    ConsoleHelper.InfoLine("[INFO] Compiler completed in " + totalTime.Elapsed.ToString());
                }
                else if (args[0].EndsWith(".csproj"))
                {
                    //C# Project file
                }
                Console.ReadKey();
            }
            else
            {
                ConsoleHelper.InfoLine("[INFO] No input file. Use /help for a list of commands");
                Console.Write("Press any key to continue...");
                Console.ReadKey();
            }
        }
    }
}
