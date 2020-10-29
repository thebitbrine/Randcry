using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AForge.Video.DirectShow;
using Randcry.Output;
using Serilog;
using SharpHash.Base;
using SharpHash.Interfaces;

namespace Randcry
{
    class Processor
    {
        public void ProcessBuffer(List<byte> Bucket, ulong HashLength, VideoCaptureDevice Device)
        {
            var hash = HashFactory.XOF.CreateShake_256(HashLength);
            hash.Initialize();
            hash.TransformBytes(Bucket.ToArray());
            var Output = hash.TransformFinal().GetBytes();
            var QT = new QualityTest(Output, new Configs().GetOutputFilePath(Device));
            if (QT.RunAllTests())
            {
                new Writer().Write(Output, Device);
                new Thread(() => new SendToRemoteServer().Upload(Output, Device)).Start();
            }
            else
            {
                Log.Debug("Trash random, throwing away.");
            }

        }
        public byte[] Crypt(byte[] data)
        {
            var provider = new RNGCryptoServiceProvider();
            var Keys = new byte[data.Length];
            provider.GetBytes(Keys);

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (byte)(data[i] ^ Keys[i]);
            }
            return data;
        }

       

    }
}
