using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Randcry.Extensions
{
    public static class FileOperations
    {
        public static byte[] ReadBytes(this string SamplePath)
        {
            using var FS = new FileStream(SamplePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            var RandBytes = new byte[FS.Length];
            var numBytesToRead = (int)FS.Length;
            var numBytesRead = 0;
            while (numBytesToRead > 0)
            {
                var n = FS.Read(RandBytes, numBytesRead, numBytesToRead);

                if (n == 0)
                    break;

                numBytesRead += n;
                numBytesToRead -= n;
            }
            return RandBytes;
        }
    }
}
