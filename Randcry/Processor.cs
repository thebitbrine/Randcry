using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using SharpHash.Base;
using SharpHash.Interfaces;

namespace Randcry
{
    class Processor
    {
        public void ProcessBuffer(List<RawFrame> Buffer)
        {
            Shuffle(Buffer);

            var Bucket = new List<byte>();
            for (int i = 0; i < Buffer.Count; i++)
            {

                //Shuffle(Buffer[i].Red.Data);
                //Shuffle(Buffer[i].Green.Data);
                //Shuffle(Buffer[i].Blue.Data);

                for (int j = 0; j < Buffer[i].Red.Data.Count(); j++)
                {
                    if (Buffer[i].Red.Data[j] != null)
                    {
                        Bucket.Add((byte)Buffer[i].Red.Data[j]);
                    }
                    if (Buffer[i].Green.Data[j] != null)
                    {
                        Bucket.Add((byte)Buffer[i].Green.Data[j]);
                    }
                    if (Buffer[i].Blue.Data[j] != null)
                    {
                        Bucket.Add((byte)Buffer[i].Blue.Data[j]);
                    }
                }
            }

            using (FileStream fsStream = new FileStream(Path.Combine("Y:\\Bins", DateTime.Now.ToString("yyyy-MM-dd-HH") + ".raw"), FileMode.Append))
            {
                using (BinaryWriter BW = new BinaryWriter(fsStream, Encoding.UTF8))
                {
                    IHash hash = HashFactory.XOF.CreateShake_128(32 + 4096);
                    hash.Initialize();
                    hash.TransformBytes(Bucket.ToArray());
                    var Output = hash.TransformFinal();
                    BW.Write(Output.GetBytes());
                    BW.Close();
                }
            }
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
