using System.Drawing;
using System.Drawing.Drawing2D;
using System;
using System.Drawing.Imaging;
using System.IO;
using ImageResizer;

namespace Zit.Utils
{
    public static class ImageHelper
    {
        public static Image Resize(this Image image, int width, int height, bool keepSizeRatio = true)
        {
            if (width == image.Width && height > image.Height)
                return image;
            if (width > image.Width && height == image.Height)
                return image;
            if (width == image.Width && height == image.Height)
                return image;

            ResizeSettings resizeSettings;
            if (keepSizeRatio == true)
            {
                resizeSettings = new ResizeSettings(width, height, FitMode.Max, null);
            }
            else
            {
                resizeSettings = new ResizeSettings(width, height, FitMode.Carve, null);
            }

            MemoryStream dStream = new MemoryStream();
            ImageResizer.ImageBuilder.Current.Build(image, dStream, resizeSettings, false);

            return Image.FromStream(dStream);
        }
    }
}