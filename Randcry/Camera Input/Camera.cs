﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using AForge.Video;
using AForge.Video.DirectShow;
using Serilog;

namespace Randcry
{
    public static class Camera
    {
        #region Open Scan Camera
        public static void OpenCamera(FilterInfo Camera, int Index)
        {
            try
            {
                var videoDevice = new VideoCaptureDevice(Camera.MonikerString);
                if (videoDevice.VideoCapabilities.Any())
                {
                    videoDevice.NewFrame += new ImageBuffer().NewImage;
                    videoDevice.Start();
                    Thread.Sleep(2222);
                    Log.Information($"Initiated {Camera.Name}");

                    var videoCap = videoDevice.VideoCapabilities
                    .OrderByDescending(x => x.BitCount)
                    .ThenByDescending(y => y.AverageFrameRate)
                    .ThenByDescending(z => z.FrameSize.Width * z.FrameSize.Height)
                    .First();

                    Log.Information($"Max FPS: {videoCap.MaximumFrameRate}");
                    Log.Information($"Avg FPS: {videoCap.AverageFrameRate}");
                    Log.Information($"Bit count: {videoCap.BitCount}");
                    Log.Information($"Frame size: {videoCap.FrameSize.Width}x{videoCap.FrameSize.Height}");
                }
                else
                {
                    Log.Debug($"Ignored {Camera.Name}, no video capabilities");
                }
            }
            catch (Exception err)
            {
                Log.Error(err, $"Failed to initiate {Camera.Name}");
            }

        }


        #endregion

        public static FilterInfoCollection GetCameras()
        {
            return new FilterInfoCollection(FilterCategory.VideoInputDevice);
        }
    }
}
