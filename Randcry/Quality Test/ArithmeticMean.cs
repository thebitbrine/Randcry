using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Randcry.Quality_Test
{
    class ArithmeticMean
    {
        public double Calculate(byte[] Data)
        {
            return Data.Select(x => { return (int)x; }).Average();
        }
    }
}
