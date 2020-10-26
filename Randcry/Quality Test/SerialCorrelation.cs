using System;
using System.Collections.Generic;
using System.Text;

namespace Randcry.Quality_Test
{
    class SerialCorrelation
    {
        public double Calculate(byte[] Data)
        {
            double sccu0 = 0, scclast = 0, scct2, scct3;

            var scct1 = scct2 = scct3 = 0.0;

            for (int i = 0; i < Data.Length; i++)
            {
                double sccun = Data[i];
                if (i == 0)
                {
                    sccu0 = sccun;
                }
                else
                {
                    scct1 = scct1 + scclast * sccun;
                }
                scct2 = scct2 + sccun;
                scct3 = scct3 + (sccun * sccun);
                scclast = sccun;
            }

            scct1 += scclast * sccu0;
            scct2 *= scct2;
            var scc = Data.Length * scct3 - scct2;
            if (scc == 0.0)
            {
                scc = -100000;
            }
            else
            {
                scc = (Data.Length * scct1 - scct2) / scc;
            }
            return scc;

        }
    }
}
