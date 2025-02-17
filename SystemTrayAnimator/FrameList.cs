using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using SystemTrayAnimator.Native;

namespace SystemTrayAnimator
{
    class FrameList : List<Frame>
    {
        public static FrameList ParseFiles(params string[] fileNames)
        {
            var frameList = new FrameList();
            foreach (var fileName in fileNames)
            {
                var fileExtension = Path.GetExtension(fileName);
                if (fileExtension.ToLower() == ".gif")
                {
                    var iconHandle = IntPtr.Zero;
                    try
                    {
                        using var image = Image.FromFile(fileName);
                        var dimension = new FrameDimension(image.FrameDimensionsList[0]);
                        int frameCount = image.GetFrameCount(dimension);
                        for (var frameIndex = 0; frameIndex < frameCount; frameIndex++)
                        {
                            image.SelectActiveFrame(dimension, frameIndex);
                            using var bitmap = new Bitmap(image);
                            iconHandle = bitmap.GetHicon();
                            var icon = Icon.FromHandle(iconHandle);
                            var frame = new Frame
                            {
                                FileName = fileName,
                                Icon = icon,
                                IconHandle = iconHandle
                            };
                            frameList.Add(frame);
                        }
                    }
                    catch
                    {
                        if (iconHandle != IntPtr.Zero)
                        {
                            User32.DestroyIcon(iconHandle);
                            iconHandle = IntPtr.Zero;
                        }
                    }
                }
                else
                {
                    var frame = Frame.Parse(fileName);
                    if (frame != null)
                    {
                        frameList.Add(frame);
                    }
                }
            }
            return frameList;
        }

        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }

            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            if (GetType() != other.GetType())
            {
                return false;
            }

            return Equals((FrameList)other);
        }

        public bool Equals(FrameList other)
        {
            if (other == null)
            {
                return false;
            }

            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            if (GetType() != other.GetType())
            {
                return false;
            }

            if (Count != other.Count)
            {
                return false;
            }

            for (var i = 0; i < Count; i++)
            {
                if (!this[i].Equals(other[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            var hashCode = 0;
            foreach (var item in this)
            {
                hashCode ^= item.GetHashCode();
            }
            return hashCode;
        }
    }
}
