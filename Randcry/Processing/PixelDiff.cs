using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using Randcry;
using Serilog;

namespace Randcry
{
    class PixelDiff
    {
        public unsafe Channel GetDifferenceImage(Bitmap image1, Bitmap image2)
        {
            if (image1 == null | image2 == null)
                return null;

            if (image1.Height != image2.Height || image1.Width != image2.Width)
                return null;


            int height = image1.Height;
            int width = image1.Width;

            BitmapData data1 = image1.LockBits(new Rectangle(0, 0, width, height),
                                               ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData data2 = image2.LockBits(new Rectangle(0, 0, width, height),
                                               ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            byte* data1Ptr = (byte*)data1.Scan0;
            byte* data2Ptr = (byte*)data2.Scan0;


            int rowPadding = data1.Stride - (image1.Width * 3);

            var Channel = new Channel(width, height, true);

            int Counter = 0;
            var Bucket = new int[width * height * 3];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var DiffVal = data1Ptr[0] - data2Ptr[0];
                        Bucket[Counter] = DiffVal;
                        data1Ptr++;
                        data2Ptr++;
                        Counter++;
                    }
                }

                if (rowPadding > 0)
                {
                    data1Ptr += rowPadding;
                    data2Ptr += rowPadding;
                }
            }

            image1.UnlockBits(data1);
            image2.UnlockBits(data2);


            Channel.Mean = Bucket.Average();

            if (Channel.Mean > 0.1 || Channel.Mean < -0.1)
                Channel.Valid = false;

            if (!Channel.Valid)
            {
                Log.Debug($"Bad frame diff, mean: {Channel.Mean}");
                return null;
            }

            //Channel.Min = Channel.Data.Min();
            //Channel.Max = Channel.Data.Max();
            //Channel.Sum = Channel.Data.Sum();
            //Channel.Zeros = Channel.Data.Count(x => x == 0);

            //for (int i = 0; i < Channel.Data.Length; i++)
            //{
            //    Channel.Data[i] += (Channel.Min * -1);
            //}

            Channel.Data = Bucket.Select(x => { return (byte)x; }).ToArray();
            Log.Debug($"Generated diff from 2 frames, byte count: {Channel.Data.Length}, mean: {Channel.Mean}");
            return Channel;
        }

    }
}