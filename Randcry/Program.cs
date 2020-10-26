using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SharpHash.Base;
using SharpHash.Interfaces;
using AForge.Video.DirectShow;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Randcry.Output;
using System.Threading;

namespace Randcry
{
    class Program
    {
        static void Main()
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console(theme: AnsiConsoleTheme.Code)
            .CreateLogger();

            //var rnd  = new Random();
            //var Bucket = new byte[2048];
            //for (int i = 0; i < int.MaxValue; i++)
            //{
            //    rnd.NextBytes(Bucket);
            //    new Processor().ProcessBuffer(Bucket.ToList(), (ulong)2048);
            //    Thread.Sleep(3);
            //}

            var Cameras = Camera.GetCameras();

            for (int i = 0; i < Cameras.Count; i++)
            {
                Camera.OpenCamera(Cameras[i], i);
                Log.Debug($"Spawned instance for [{Cameras[i].Name}] [{i}]");
            }

            bool Exit = false;
            do
            {
                Console.ReadKey();
            }
            while (!Exit);

        }
    }
}
