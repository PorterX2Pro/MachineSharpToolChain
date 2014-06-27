using MachineSharpToolChain.NewCompileStages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MachineSharpToolChain.CompileStages
{
    internal class Compiler
    {
        public static void CompileSingleFile(string InputFile)
        {
            //A variable to store the last error that occured.
            int LastError = 0;
            //Create the build folder if it doesnt exist. Clear the directory if it does.
            try
            {
                if (System.IO.Directory.Exists(global.BuildDirectory))
                {
                    //Directory exists, delete and remake
                    LastError = 1;
                    ConsoleHelper.InfoLine("[INFO] Deleting existing build environment...");
                    System.IO.Directory.Delete(global.BuildDirectory, true);
                    ConsoleHelper.SuccessLine("Build environment was cleared.");
                    System.Threading.Thread.Sleep(1000);
                    ConsoleHelper.InfoLine("[INFO] Creating new build environment...");
                    System.IO.Directory.CreateDirectory(global.BuildDirectory);
                    ConsoleHelper.SuccessLine("Build environment was created.");
                }
                else
                {
                    LastError = 2;
                    ConsoleHelper.InfoLine("[INFO] Creating new build environment...");
                    System.IO.Directory.CreateDirectory(global.BuildDirectory);
                    ConsoleHelper.SuccessLine("Build environment was created.");
                }
            }
            catch
            {
                ConsoleHelper.ErrorLine("An error occured while creating the build directory. Error {0}", LastError);
                return;
            }
            SharpLexer.ToProgramShell(File.ReadAllText(InputFile));
            //Read the single file
            string SingleFileCode = System.IO.File.ReadAllText(InputFile);
            //Start the first stage of the compiler by converting the file to a base file
            ProgramShell.BaseDeclaration SingleBaseFile = ProgramShellHelp.ToProgramShell(SingleFileCode);
            //Spawn base1.h
            using (StreamWriter writeBase = new StreamWriter(Path.Combine(global.BuildDirectory, "base1.h")))
            {
                //Loop through namespaces and preform stage 2 of the compile process
                foreach (ProgramShell.StandardDeclaration namespac in SingleBaseFile.childrenDeclarations)
                {
                    if (namespac is ProgramShell.NamespaceDeclaration)
                    {
                        //second stage of compile process
                        CompileStages.NativeProgramShell.ToNativeNamespace((ProgramShell.NamespaceDeclaration)namespac, writeBase);
                        //Leave a space for debug readability
                        writeBase.WriteLine("");
                    }
                }
            }
            //create program entry point in main.cpp
            using (StreamWriter writeMainCpp = new StreamWriter(Path.Combine(global.BuildDirectory, "main.cpp")))
            {
                //generate the main cpp file with specified entry point
                NativeCompile.GenerateMainCpp("Test::Program::Main(ToStringArgs(argv, argc));", writeMainCpp);
            }
            //setup the final stage of the compiler by invoking the c++ build chain
            //Use the name of the input file if no other name was specified with /outputfile
            NativeCompile.ToNativeExecutable(Path.GetFileNameWithoutExtension(InputFile) + ".exe");
        }
    }
}
