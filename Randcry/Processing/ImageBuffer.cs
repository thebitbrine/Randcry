﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using AForge.Video;
using AForge.Video.DirectShow;
using Serilog;

namespace Randcry
{
    class ImageBuffer
    {
        public int TotalFrames;
        public int BufferSize = 2;
        public int ThrowAwayCount = 30;
        public List<Bitmap> Frames = new List<Bitmap>();
        public List<byte> Buffer = new List<byte>();
        public void NewImage(object sender, NewFrameEventArgs eventArgs)
        {
            var Device = sender as VideoCaptureDevice;
            var image = eventArgs.Frame;
            if (TotalFrames < 30)
            {
                Log.Debug($"Throwing away frame #{TotalFrames}, {ThrowAwayCount - TotalFrames} more frames to go.");
                TotalFrames++;
                return;
            }

            lock (Frames)
            {
                if (Frames.Count >= 2)
                {
                    var Frame = new PixelDiff().GetDifferenceImage(Frames[0], Frames[1]);
                    if (Frame != null && Frame.Valid)
                    {
                        lock (Buffer)
                        {
                            if (Buffer.Count >= BufferSize)
                            {
                                new Processor().ProcessBuffer(Buffer, (ulong)Frame.Data.Length, Device);
                                Buffer.Clear();
                            }
                            else
                            {
                                Buffer.AddRange(Frame.Data);
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

