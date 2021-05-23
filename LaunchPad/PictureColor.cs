using System.Drawing;

namespace LaunchPad
{
    class PictureColor
    {
        public static Color ComputeBitmapColor(Bitmap src)
        {
            double bright = GetBrightness(src);
            return bright >= 0.5 ? Color.Black : Color.White;
        }
        public static double GetBrightness(Bitmap src)
        {
            if (src == null)
            {
                return 151;
            }
            int width = src.Width, height = src.Height;
            Color pixel;
            double pixelCount = width * height, bright = 0;
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    pixelCount++;
                    pixel = src.GetPixel(x, y);
                    bright += pixel.GetBrightness();
                }
            }
            return (bright / pixelCount);
        }
    }
}
