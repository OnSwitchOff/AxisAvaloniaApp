using System.Drawing;
using System.Drawing.Imaging;

namespace AxisAvaloniaApp.Helpers
{
    public static class ImageExtensions
    {
        /// <summary>
        /// Converts System.Drawing.Image to Avalonia.Media.Imaging.Bitmap.
        /// </summary>
        /// <param name="image">Image to convert.</param>
        /// <returns>Avalonia.Media.Imaging.Bitmap</returns>
        /// <date>24.06.2022.</date>
        public static Avalonia.Media.Imaging.Bitmap ConvertToAvaloniaBitmap(this Image image)
        {
            using (System.IO.Stream stream = new System.IO.MemoryStream())
            {
                image.Save(stream, ImageFormat.Png);
                stream.Position = 0;

                return new Avalonia.Media.Imaging.Bitmap(stream);
            }
        }
    }
}
