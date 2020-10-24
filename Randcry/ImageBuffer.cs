using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using AForge.Video;

namespace Randcry
{
    class ImageBuffer
    {
        public int TotalFrames;
        public int BufferSize = 8;
        public List<Bitmap> Frames = new List<Bitmap>();
        public List<Channel> Buffer = new List<Channel>();
        public void NewImage(object sender, NewFrameEventArgs eventArgs)
        {
            var image = eventArgs.Frame;
            if (TotalFrames < 30)
            {
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
                                var BufferClone = CloneList(Buffer);
                                new Thread(() => new Processor().ProcessBuffer(BufferClone)).Start();
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
        public static List<T> CloneList<T>(List<T> oldList)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, oldList);
            stream.Position = 0;
            return (List<T>)formatter.Deserialize(stream);
        }
    }
}

