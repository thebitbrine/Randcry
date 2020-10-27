using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AForge.Video.DirectShow;

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
        public string GetOutputFileName(VideoCaptureDevice Device)
        {
            //var OutputFile = Path.Combine(OutputDirectory, $"{Device.SourceObject.GetHashCode():X}-{DateTime.Now:yyyy-MM-dd-HH}.raw");
            //var OutputFile = Path.Combine(OutputDirectory, $"{Device.SourceObject.GetHashCode():X}-{DateTime.Now:yyyy-MM-dd-HH-mm}.raw");
            //var OutputFile = Path.Combine(OutputDirectory, $"{Device.SourceObject.GetHashCode():X}.raw");
            //var OutputFile = Path.Combine(OutputDirectory, $"{Device.SourceObject.GetHashCode():X}-{Guid.NewGuid()}{DateTime.Now:yyyy-MM-dd-HH-mm}.raw");
            //var OutputFile = Path.Combine(OutputDirectory, $"{DateTime.Now:yyyy-MM-dd-HH-mm}.raw");

            var OutputFile = Path.Combine(OutputDirectory, $"RandCry-{long.Parse(DateTime.Now.ToString("yyyyMMddHHmm") + ((int)Math.Round((double)DateTime.Now.Second/10)) * 10).GetHashCode():X}.bin");


            //if (!File.Exists(OutputFile))
            //    File.WriteAllText(OutputFile, "");

            return OutputFile;
        }
    }
}
