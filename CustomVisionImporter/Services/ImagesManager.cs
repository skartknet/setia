using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace CustomVisionImporter.Services
{
    internal static class ImagesManager
    {
        /// <summary>
        /// It resizes an image if the file size is above or equals the threshold
        /// </summary>
        /// <param name="img"></param>
        /// <param name="sizeThresholdBytes"></param>
        /// <returns></returns>
        public static byte[] ResizeImage(byte[] img, int sizeThresholdBytes = 3000000) //3Mb
        {
            if (img.Length == 0) throw new System.Exception("No image provided");

            if (img.Length >= sizeThresholdBytes)
            {
                using (var image = Image.Load(img))
                {
                    image.Mutate(x => x.Resize(new ResizeOptions()
                    {
                        Mode = ResizeMode.Max,
                        Size = new SixLabors.Primitives.Size(1500, 1500)
                    }));

                    using (var newImage = new MemoryStream())
                    {
                        image.SaveAsJpeg(newImage);

                        return newImage.ToArray();
                    }
                }
            }

            return img;
        }
    }
}
