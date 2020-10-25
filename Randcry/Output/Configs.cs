using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Randcry.Output
{
    class Configs
    {
        private readonly string OutputDirectory = "Bins";
        public Configs()
        {
            if (!Directory.Exists(OutputDirectory))
            {
                Directory.CreateDirectory(OutputDirectory);
                Log.Information($"Created {OutputDirectory} directory");
            }
        }
        public string GetOutputFileName()
        {
            var OutputFile = Path.Combine(OutputDirectory, DateTime.Now.ToString("yyyy-MM-dd-HH") + ".raw");
            if (!File.Exists(OutputFile))
                File.WriteAllText(OutputFile, "");
            return OutputFile;
        }
    }
}
