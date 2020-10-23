using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using AForge.Video;

namespace Randcry
{
    class ImageBuffer
    {
        public int TotalFrames;
        public int BufferSize = 4;
        public List<Bitmap> Frames = new List<Bitmap>();
        public List<RawFrame> Buffer = new List<RawFrame>();
        public void NewImage(object sender, NewFrameEventArgs eventArgs)
        {
            var image = eventArgs.Frame;
            if (TotalFrames < 100)
            {
                TotalFrames++;
                return;
            }

            lock (Frames)
            {
                if (Frames.Count >= 2)
                {
                    var Frame = new PixelDiff().GetDifferenceImage(Frames[0], Frames[1]);
                    if (Frame._Valid)
                    {
                        lock (Buffer)
                        {
                            if (Buffer.Count >= BufferSize)
                            {
                                new Processor().ProcessBuffer(new List<RawFrame>(Buffer));
                                Buffer.Clear();
                            }
                            else
                            {
                                Buffer.Add(Frame);
                            }
                        }
                    }
                    Frames.Clear();
                }
                else
                {
                    Frames.Add(image.Clone(new Rectangle(0, 0, image.Width, image.Height), PixelFormat.Format24bppRgb));
                }
            }

        }
    }
}

