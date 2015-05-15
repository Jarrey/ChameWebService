using Chame.WebService.Helper.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chame.WebService.Helper
{
    public class ImageEncryptor
    {
        /// <summary>
        ///     Dencryt image and return image code
        /// </summary>
        /// <param name="image"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static ImageAppCode DencrytImage(Bitmap image, int offset)
        {
            if (image == null) throw new ArgumentNullException("image");
            if (offset < 0 || offset > Math.Min(image.Height, image.Width))
                throw new ArgumentException("Argument is out of range", "offset");
            
            Color color1 = image.GetPixel(offset, 0);
            Color color2 = image.GetPixel(image.Width - 1, offset);
            Color color3 = image.GetPixel(image.Width - 1 - offset, image.Height - 1);
            Color color4 = image.GetPixel(0, image.Height - 1 - offset);

            return new ImageAppCode(color1.R, color2.G, color3.B, color4.R, offset);
        }

        /// <summary>
        ///     Encryt image with image code
        /// </summary>
        /// <param name="image"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Bitmap EncrytImage(Bitmap image, ImageAppCode code)
        {
            if (image == null) throw new ArgumentNullException("image");

            int offset = code.CodePositionOffset;
            image.SetPixel(offset, 0, Color.FromArgb(0xff, code.Code1, 0, 0));
            image.SetPixel(image.Width - 1, offset, Color.FromArgb(0xff, 0, code.Code2, 0));
            image.SetPixel(image.Width - 1 - offset, image.Height - 1, Color.FromArgb(0xff, 0, 0, code.Code3));
            image.SetPixel(0, image.Height - 1 - offset, Color.FromArgb(0xff, code.Code4, 0, 0));

            return image;
        }
    }
}
