using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;
using AForge.Video;
using AForge.Video.DirectShow;

namespace Randcry
{
    public static class Camera
    {
        #region Open Scan Camera
        public static void OpenCamera(FilterInfo Camera)
        {
            try
            {
                var videoDevice = new VideoCaptureDevice(Camera.MonikerString);
                videoDevice.NewFrame += new ImageBuffer().NewImage;
                videoDevice.Start();
            }
            catch (Exception err)
            {
            }

        }


        #endregion

        public static FilterInfoCollection GetCameras()
        {
            return new FilterInfoCollection(FilterCategory.VideoInputDevice);
        }

    }
}
