using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Tools
{
    public static class Images
    {
        public static Image Base64ToImage(string base64String)
        {
            // Convert base 64 string to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            // Convert byte[] to Image
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                Image image = Image.FromStream(ms, true);
                return image;
            }
        }

        public static string ImageToBase64(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to base 64 string
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        /// <summary>
        /// Изменение размера изображения, с сохранением пропорций. Если ширина была
        /// меньше width, она не изменится
        /// </summary>
        /// <param name="width">Ширина в пикселях, до которой сожмём изображение </param>
        /// <param name="imageInBytes">Картинка в байтовом виде</param>
        public static byte[] Resize(int width, byte[] imageInBytes)
        {
            MemoryStream ms = new MemoryStream(imageInBytes, 0, imageInBytes.Length);
            ms.Write(imageInBytes, 0, imageInBytes.Length);
            var image = Image.FromStream(ms, true);
            image = ScaleImage(image, width, width);
            return ImageToByteArray(image);
        }

        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            // Если обе стороны вписались 
            if ((image.Height < maxHeight) && (image.Width < maxWidth))
            {
                return image;
            }

            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }

        public static byte[] ImageToByteArray(Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }
}
