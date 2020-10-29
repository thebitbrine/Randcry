using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using Randcry.Extensions;
using Serilog;

namespace Randcry.Output
{
    class SendToRemoteServer
    {
        private string RemoteServerAddress = "http://192.168.88.88:6699/feed/RNG-01";
        public void Upload(byte[] Data, VideoCaptureDevice Device)
        {

            RemoteServerAddress += "-" + new Configs().GetOutputFileName(Device);
            for (int i = 0; i < 1; i++)
            {
                try
                {

                    WebRequest request = WebRequest.Create(RemoteServerAddress);
                    request.Method = "POST";
                    request.ContentType = "application/octet-stream";
                    request.ContentLength = Data.Length;

                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(Data, 0, Data.Length);
                    dataStream.Close();

                    WebResponse response = request.GetResponse();

                    dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    reader.ReadToEnd();

                    reader.Close();
                    dataStream.Close();
                    response.Close();

                    Log.Information($"Pushed out {Data.Length.GetSize()} to {RemoteServerAddress}");
                    return;
                }
                catch { }
                Thread.Sleep(4444);
            }

            Log.Error($"Failed to push bytes to: {RemoteServerAddress}");
        }
    }
}
