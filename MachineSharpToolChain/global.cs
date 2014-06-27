using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineSharpToolChain
{
    internal class global
    {
        public static string AppDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public static string BuildDirectory = string.Empty;

        public static List<string> EntryPoints = new List<string>();
    }
}
