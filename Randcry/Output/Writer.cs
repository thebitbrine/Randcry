using Randcry.Output;
using Serilog;
using System;
using System.IO;
using System.Text;
using System.Threading;
using AForge.Video.DirectShow;
using static Randcry.Extensions.ByteArray;

namespace Randcry
{
    class Writer
    {
        public void Write(byte[] Data, VideoCaptureDevice Device)
        {
            var OutputFile = new Configs().GetOutputFileName(Device);
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    using FileStream fsStream = new FileStream(OutputFile, FileMode.Append, FileAccess.Write);
                    using BinaryWriter BW = new BinaryWriter(fsStream, Encoding.UTF8);
                    BW.Write(Data);
                    BW.Flush();
                    BW.Close();
                    Log.Information($"Pushed out {Data.Length.GetSize()} into {OutputFile}");
                    break;
                }
                catch(Exception ex)
                {
                    Log.Error($"An error has occurred while writing bytes to: {OutputFile}", ex);
                    Thread.Sleep(4444);
                }
            }
            //new Analyzer(OutputFile).AnalyzeAndPrint();
        }
    }
}
