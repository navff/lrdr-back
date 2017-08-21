using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.HelpClasses
{
    public static class FileHelpers
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="base64String"></param>
        /// <param name="filePathAndName"></param>
        /// <param name="fileExtension">Расширение прилетевшего файла</param>
        /// <param name="width">Если картинка, — жать по ширине до этого размера</param>
        public static void Base64ToFile(string base64String, string filePathAndName, string fileExtension, int width)
        {
            var bytes = Convert.FromBase64String(base64String);
            if (IsImage(fileExtension)  && IsThumbnail(fileExtension))
            {
                bytes = Resize(width, bytes);
            }
            
            using (var fileStream = new FileStream(filePathAndName, FileMode.Create))
            {
                fileStream.Write(bytes, 0, bytes.Length);
                fileStream.Flush();
            }
        }

        public static System.Drawing.Image Base64ToImage(string base64String, int width)
        {
            // Convert base 64 string to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            imageBytes = Resize(width, imageBytes);
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

            using (var b = new Bitmap(image.Width, image.Height))
            {
                b.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                using (var g = Graphics.FromImage(b))
                {
                    g.Clear(Color.White);
                    g.DrawImageUnscaled(image, 0, 0);
                }

                return ImageToByteArray(b);
            }

            
 
            
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
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        public static bool IsImage(string extension)
        {
            extension = extension.ToLower();
            return (extension.Contains("jpg") ) 
                || (extension.Contains("jpeg")) 
                || (extension.Contains("png")) 
                || (extension.Contains("gif"))
                || (extension.Contains("bmp"));

        }

        public static bool IsThumbnail(string extension)
        {
            return (extension.ToLower().Contains("thumb"));

        }
    }
}
