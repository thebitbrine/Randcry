using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SharpHash.Base;
using SharpHash.Interfaces;
using AForge.Video.DirectShow;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Randcry
{
    class Program
    {
        static void Main()
        {
            Log.Logger = new LoggerConfiguration()
            //.MinimumLevel.Debug()
            .WriteTo.Console(theme: AnsiConsoleTheme.Code)
            .CreateLogger();

            if (!Directory.Exists("Bins"))
            {
                Directory.CreateDirectory("Bins");
                Log.Information("Created Bins directory");
            }

            var Cameras = Camera.GetCameras();

            for (int i = 0; i < Cameras.Count; i++)
            {
                Randcry.Camera.OpenCamera(Cameras[i]);
                Log.Information($"Spawned instance for {Cameras[i].Name}-{i}");
            }

        }
    }
}
