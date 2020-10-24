using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Randcry
{
    [Serializable]
    public class RawFrame
    {
        public RawFrame(int Width, int Height)
        {
            Red = new Channel(Width, Height);
            Green = new Channel(Width, Height);
            Blue = new Channel(Width, Height);
        }
        public Channel Red;
        public Channel Green;
        public Channel Blue;
        public double? _Mean;
        public bool _Valid = true;

    }
    [Serializable]
    public class Channel
    {
        public Channel(int Width, int Height)
        {
            Data = new int?[Width * Height];
        }
        public int?[] Data;
        public int? Min;
        public int? Max;
        public int? Sum;
        public double? Mean;
        public int? Zeros;
        public bool Valid = true;
    }
}
