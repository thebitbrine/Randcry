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
        public void ProcessBuffer(List<Channel> Buffer)
        {
            //Shuffle(Buffer);

            var Bucket = new List<byte>();
            for (int i = 0; i < Buffer.Count; i++)
            {
                //Shuffle(Buffer[i].Data);

                for (int j = 0; j < Buffer[i].Data.Count(); j++)
                {
                    if (Buffer[i].Data[j] != null)
                    {
                        Bucket.Add((byte)Buffer[i].Data[j]);
                    }
                }
            }

            using (FileStream fsStream = new FileStream(Path.Combine("Bins", DateTime.Now.ToString("yyyy-MM-dd-HH") + ".raw"), FileMode.Append))
            {
                using (BinaryWriter BW = new BinaryWriter(fsStream, Encoding.UTF8))
                {
                    IHash hash = HashFactory.XOF.CreateShake_256((ulong)Buffer.First().Data.Length /*/ 75*/ * 1);
                    hash.Initialize();
                    hash.TransformBytes(Bucket.ToArray());
                    var Output = hash.TransformFinal();
                    BW.Write(Output.GetBytes());
                    BW.Close();

                    Console.WriteLine("Written bytes.");
                }
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
