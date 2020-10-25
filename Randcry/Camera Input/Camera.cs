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
                var Instance = new Thread(() =>
                {
                    var videoDevice = new VideoCaptureDevice(Camera.MonikerString);
                    videoDevice.NewFrame += new ImageBuffer().NewImage;
                    videoDevice.Start();
                    Thread.Sleep(2222);
                    Log.Information($"Initiated {Camera.Name}");
                    if (videoDevice.VideoCapabilities.Any())
                    {
                        var videoCap = videoDevice.VideoCapabilities[0];
                        Log.Information($"Max FPS: {videoCap.MaximumFrameRate}");
                        Log.Information($"Avg FPS: {videoCap.AverageFrameRate}");
                        Log.Information($"Bit count: {videoCap.BitCount}");
                        Log.Information($"Frame size: {videoCap.FrameSize.Width}x{videoCap.FrameSize.Height}");
                    }
                });
                Instance.Name = $"[{Camera.Name}] [{Index}]";
                Instance.Start();
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