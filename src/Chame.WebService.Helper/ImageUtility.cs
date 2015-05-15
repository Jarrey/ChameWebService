using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chame.WebService.Helper
{
    public class ImageUtility
    {
        public static Image Base64ToImage(string imageBase64)
        {
            byte[] imageData = Convert.FromBase64String(imageBase64);
            return Image.FromStream(new MemoryStream(imageData)) as Bitmap;
        }
        
        public static byte[] ImageToByteArray(Image image, ImageFormat format = null)
        {
            if (format == null) format = ImageFormat.Png;
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);
                return ms.ToArray();
            }
        }

        public static string GetMimeType(ImageFormat format)
        {
            if (format == ImageFormat.Png)
                return "image/png";
            if (format == ImageFormat.Jpeg)
                return "image/jpg";
            if (format == ImageFormat.Gif)
                return "image/gif";
            if (format == ImageFormat.Bmp)
                return "image/bmp";
            if (format == ImageFormat.Tiff)
                return "image/tiff";

            return "application/octet-stream"; // for unknown mime type
        }

        public static Image GetFitImage(Image image, SizeF screenSize)
        {
            // Get ratio of uniform to fill
            float bitmapAspectRatio = (float)(image.Width) / (float)(image.Height);
            float screenAspectRatio = screenSize.Width / screenSize.Height;
            float ratio = bitmapAspectRatio < screenAspectRatio ? (float)(screenSize.Width / image.Width) : (float)(screenSize.Height / image.Height);

            Size newSize = bitmapAspectRatio < screenAspectRatio ? new Size((int)screenSize.Width, (int)(image.Height * ratio)) : new Size((int)(image.Width * ratio), (int)screenSize.Height);
            using (var tempBitmap = ResizeImage(image, newSize))
            {
                float offsetY = bitmapAspectRatio < screenAspectRatio ? 0.5F * (tempBitmap.Height - screenSize.Height) : 0.0F;
                float offsetX = bitmapAspectRatio < screenAspectRatio ? 0.0F : 0.5F * (tempBitmap.Width - screenSize.Width);

                return CropImage(tempBitmap, new Rectangle((int)offsetX, (int)offsetY, (int)screenSize.Width, (int)screenSize.Height));
            }
        }

        private static Image CropImage(Image img, Rectangle cropArea)
        {
            using (Bitmap bmpImage = new Bitmap(img))
            {
                return (Image)bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            }
        }

        private static Image ResizeImage(Image imgToResize, Size size)
        {
            var b = new Bitmap(size.Width, size.Height);
            using (var g = Graphics.FromImage((Image)b))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(imgToResize, 0, 0, size.Width, size.Height);
            }

            return (Image)b;
        }
    }
}
