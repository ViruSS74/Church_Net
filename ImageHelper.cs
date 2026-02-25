using System.Drawing;

namespace ChurchBudget
{
    public static class ImageHelper
    {
        public static Image Resize(Image image, int size)
        {
            if (image == null) return null;

            var newImage = new Bitmap(size, size);
            using (var g = Graphics.FromImage(newImage))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, 0, 0, size, size);
            }
            return newImage;
        }
    }
}
