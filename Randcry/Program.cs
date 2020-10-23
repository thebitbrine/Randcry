using System;
using System.IO;
using System.Text;
using SharpHash.Base;
using SharpHash.Interfaces;

namespace Randcry
{
    class Program
    {
        static void Main(string[] args)
        {
            Directory.CreateDirectory("Bins");

            var MainCamera = Camera.GetCameras()[1];
            Camera.OpenCamera(MainCamera);
            Console.WriteLine($"Using {MainCamera.Name}");

        }
    }
}
