using Serilog;
using System;
using System.IO;
using System.Text;

namespace Randcry
{
    class Writer
    {
        public void Write(byte[] Data)
        {
            var CurrBin = Path.Combine("Bins", DateTime.Now.ToString("yyyy-MM-dd-HH") + ".raw");
            using FileStream fsStream = new FileStream(CurrBin, FileMode.Append);
            using BinaryWriter BW = new BinaryWriter(fsStream, Encoding.UTF8);
            BW.Write(Data);
            Log.Information($"Pushed out {Data.Length} hashed bytes into {CurrBin}");
        }
    }
}
