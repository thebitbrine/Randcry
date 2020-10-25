using System;
using System.Collections.Generic;
using System.Text;

namespace Randcry.Quality_Test
{
    class ShannonEntropy
    {
        public double Calculate(byte[] Data)
        {
            var map = new Dictionary<byte, int>();
            foreach (byte c in Data)
            {
                if (!map.ContainsKey(c))
                    map.Add(c, 1);
                else
                    map[c] += 1;
            }

            double result = 0.0;
            int len = Data.Length;
            foreach (var item in map)
            {
                var frequency = (double)item.Value / len;
                result -= frequency * (Math.Log(frequency) / Math.Log(2));
            }

            return result;
        }
    }
}
