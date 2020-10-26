using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using AForge.Video.DirectShow;
using Serilog;
using SharpHash.Base;
using SharpHash.Interfaces;

namespace Randcry
{
    class Processor
    {
        public void ProcessBuffer(List<byte> Bucket, ulong HashLength)
        {
            IHash hash = HashFactory.XOF.CreateShake_256(HashLength);
            hash.Initialize();
            hash.TransformBytes(Bucket.ToArray());
            var Output = hash.TransformFinal().GetBytes();
            var QT = new QualityTest(Output);
            if (QT.RunAllTests())
            {
                new Writer().Write(Output);
            }
            else
            {
                Log.Debug($"Trash random, throwing away.");
            }

        }
        public byte[] Crypt(byte[] data)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            var Keys = new byte[data.Length];
            provider.GetBytes(Keys);

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (byte)(data[i] ^ Keys[i]);
            }
            return data;
        }

        public void Shuffle<T>(IList<T> list)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[4];
                do provider.GetBytes(box);
                while (!(BitConverter.ToUInt32(box, 0) < n * (uint.MaxValue / n)));
                int k = (int)(BitConverter.ToUInt32(box, 0) % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

    }
}
