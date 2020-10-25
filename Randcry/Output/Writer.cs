using Randcry.Output;
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
            var OutputFile = new Configs().GetOutputFileName();
            using FileStream fsStream = new FileStream(OutputFile, FileMode.Append);
            using BinaryWriter BW = new BinaryWriter(fsStream, Encoding.UTF8);
            BW.Write(Data);
            BW.Flush();
            BW.Close();
            Log.Information($"Pushed out {Data.Length} hashed bytes into {OutputFile}");
            new Analyzer().AnalyzeAndPrint();
        }
    }
}
