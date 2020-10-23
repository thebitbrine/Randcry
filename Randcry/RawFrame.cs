using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Randcry
{
    public class RawFrame
    {
        public RawFrame(int Width, int Height)
        {
            Red = new Channel { Data = new int?[Width * Height] };
            Green = new Channel { Data = new int?[Width * Height] };
            Blue = new Channel { Data = new int?[Width * Height] };
        }
        public Channel Red;
        public Channel Green;
        public Channel Blue;
        public double? _Mean;
        public bool _Valid = true;

    }
    public class Channel
    {
        public int?[] Data;
        public int? Min;
        public int? Max;
        public int? Sum;
        public double? Mean;
        public int? Zeros;
    }
}
