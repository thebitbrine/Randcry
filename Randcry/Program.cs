using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SharpHash.Base;
using SharpHash.Interfaces;
using AForge.Video.DirectShow;

namespace Randcry
{
    class Program
    {
        static void Main(string[] args)
        {
            Directory.CreateDirectory("Bins");

            //var rnd = new Random();
            //var List = new byte[1000000];
            //rnd.NextBytes(List);
            //File.WriteAllBytes("test", List);

            var Cameras = Camera.GetCameras();
            foreach (FilterInfo Camera in Cameras)
            {
                Randcry.Camera.OpenCamera(Camera);
                Console.WriteLine($"Using {Camera.Name}");
            }

        }
    }
}
