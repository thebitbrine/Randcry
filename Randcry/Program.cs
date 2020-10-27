using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using SharpHash.Base;
using SharpHash.Interfaces;
using AForge.Video.DirectShow;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Randcry.Output;
using System.Threading;
using System.Threading.Tasks;

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


            var Cameras = Camera.GetCameras();
            var LiveCameras = new List<VideoCaptureDevice>();
            for (int i = 0; i < Cameras.Count; i++)
            {
                var CurrentCamera = Camera.OpenCamera(Cameras[i], i);
                if (CurrentCamera != null)
                {
                    LiveCameras.Add(CurrentCamera);
                    Log.Debug($"Spawned instance for [{Cameras[i].Name}] [{i}]");
                }
            }

            while (true)
            {
                var Key = Console.ReadKey(true).Key;
                switch (Key)
                {
                    case ConsoleKey.A:
                        List<string> AnalyzedFiles = new List<string>();
                        foreach (var VideoDevice in LiveCameras)
                        {
                            try
                            {
                                if (VideoDevice.VideoCapabilities.Any())
                                {
                                    var CurrentFile = new Configs().GetOutputFileName(VideoDevice);
                                    if (!AnalyzedFiles.Contains(CurrentFile))
                                    {
                                        var Analyzer = new Analyzer(CurrentFile);
                                        var AnalysisResults = Analyzer.AnalyzeSample(CurrentFile);
                                        Analyzer.PrintAnalysisReport(AnalysisResults);
                                        AnalyzedFiles.Add(CurrentFile);
                                    }
                                }
                            }
                            catch { }

                        }
                        break;

                    case ConsoleKey.C:
                        Console.Clear();
                        break;

                }
                Thread.Sleep(222);
            }
        }
    }
}
