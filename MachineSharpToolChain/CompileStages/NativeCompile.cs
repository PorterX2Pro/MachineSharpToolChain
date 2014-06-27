using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace MachineSharpToolChain.CompileStages
{
    internal class NativeCompile
    {
        public static void ToNativeExecutable(string ExecutableName)
        {
            CompileMainCpp(ExecutableName);
        }

        public static void GenerateMainCpp(string EntryPoint, StreamWriter fileWriter)
        {
            fileWriter.WriteLine("#include \"stdafx.h\"");
            fileWriter.WriteLine("#include \"base1.h\"");
            fileWriter.WriteLine("using namespace std;");
            fileWriter.WriteLine("int main(int argc, char* argv[]) {");
            fileWriter.WriteLine(EntryPoint);
            fileWriter.Write("}");
        }

        public static void PrepareBuildEnvironment(string ExeName)
        {
            //Modify Build Template for compile process
            string BuildTemplate = File.ReadAllText(Path.Combine(global.AppDir, "mstcinc", "buildtemp.win"));
            File.WriteAllText(Path.Combine(global.BuildDirectory, "build.win"), BuildTemplate.Replace("{!!addinc}", "-I\"" + Path.Combine(global.AppDir, "mstcinc") + "\"").Replace("{!!PROJECTNAME}", "Test Project").Replace("{!!EXENAME}", ExeName));
        }

        private static void CompileMainCpp(string ExeName)
        {
            Console.WriteLine("Compile Output:");
            Console.WriteLine("----------------");
            PrepareBuildEnvironment(ExeName);
            int Result = StartMakeWithArgs("-f \"" + Path.Combine(global.BuildDirectory, "build.win") + "\" all");
            Console.WriteLine("----------------");
            if (Result == 0)
            {
                ConsoleHelper.SuccessLine("Compile was successful.");
            }
            else
            {
                ConsoleHelper.ErrorLine("Compile error occured.");
            }
            ConsoleHelper.WriteLine(ConsoleColor.Cyan, "Cleaning up...");
            try
            {
                System.IO.File.Delete(Path.Combine(global.BuildDirectory, "build.win"));
            }
            catch
            {
                //Not There
            }
            try
            {
                //  System.IO.File.Delete(Path.Combine(global.BuildDirectory, "main.cpp"));
            }
            catch
            {
                //Not There
            }
            try
            {
                System.IO.File.Delete(Path.Combine(global.BuildDirectory, "main.o"));
            }
            catch
            {
                //Not There
            }
        }
        private static string Output = string.Empty;
        public static int StartMakeWithArgs(string Args)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "C:\\Program Files (x86)\\Dev-Cpp\\MinGW32\\bin\\mingw32-make.exe";
            startInfo.Arguments = Args;
            startInfo.UseShellExecute = false;
            startInfo.EnvironmentVariables["PATH"] = "C:\\Program Files (x86)\\Dev-Cpp\\MinGW32\\bin\\";
            startInfo.RedirectStandardOutput = true;
            startInfo.WorkingDirectory = global.BuildDirectory;
            Process proc = Process.Start(startInfo);
            proc.BeginOutputReadLine();
            proc.WaitForExit();
            return proc.ExitCode;
        }
    }
}
