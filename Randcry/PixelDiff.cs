using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using Randcry;

namespace Randcry
{
    class PixelDiff
    {
        public unsafe RawFrame GetDifferenceImage(Bitmap image1, Bitmap image2)
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

            var Frame = new RawFrame(width, height);

            int Counter = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Frame.Red.Data[Counter] = (data1Ptr[0] - data2Ptr[0]);
                    data1Ptr++;
                    data2Ptr++;

                    Frame.Green.Data[Counter] = (data1Ptr[0] - data2Ptr[0]);
                    data1Ptr++;
                    data2Ptr++;

                    Frame.Blue.Data[Counter] = (data1Ptr[0] - data2Ptr[0]);
                    data1Ptr++;
                    data2Ptr++;
                    Counter++;
                }

                if (rowPadding > 0)
                {
                    data1Ptr += rowPadding;
                    data2Ptr += rowPadding;
                }
            }


            Frame.Red.Min = Frame.Red.Data.Min();
            Frame.Red.Max = Frame.Red.Data.Max();
            Frame.Red.Sum = Frame.Red.Data.Sum();
            Frame.Red.Mean = Frame.Red.Data.Average();
            Frame.Red.Zeros = Frame.Red.Data.Count(x => x == 0);

            if (Frame.Red.Mean > 0.1 /*|| Frame.Red.Mean < -0.1*/)
                Frame._Valid = false;

            Frame.Green.Min = Frame.Green.Data.Min();
            Frame.Green.Max = Frame.Green.Data.Max();
            Frame.Green.Sum = Frame.Green.Data.Sum();
            Frame.Green.Mean = Frame.Green.Data.Average();
            Frame.Green.Zeros = Frame.Green.Data.Count(x => x == 0);

            if (Frame.Green.Mean > 0.1 /*|| Frame.Green.Mean < -0.1*/)
                Frame._Valid = false;

            Frame.Blue.Min = Frame.Blue.Data.Min();
            Frame.Blue.Max = Frame.Blue.Data.Max();
            Frame.Blue.Sum = Frame.Blue.Data.Sum();
            Frame.Blue.Mean = Frame.Blue.Data.Average();
            Frame.Blue.Zeros = Frame.Blue.Data.Count(x => x == 0);

            if (Frame.Blue.Mean > 0.1 /*|| Frame.Blue.Mean < -0.1*/)
                Frame._Valid = false;

            Frame._Mean = (Frame.Red.Mean + Frame.Green.Mean + Frame.Blue.Mean) / 3;

            image1.UnlockBits(data1);
            image2.UnlockBits(data2);

            if (Frame._Mean > 0.1 /*|| Frame._Mean < -0.1*/)
                Frame._Valid = false;

            return Frame;
        }

    }
}