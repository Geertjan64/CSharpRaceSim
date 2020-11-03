using Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPFApp
{
    static class ContentLoader
    {
        private static  Dictionary<string, Bitmap> cache;

        public  static Bitmap GetBitmapFromCache ( string resourceString)
        {
            if ( cache == null)
            {
                cache = new Dictionary<string, Bitmap>();
            }


            if ( cache.ContainsKey (resourceString)) {
                return cache[resourceString];
            }else
            {
                if (File.Exists(resourceString) == false)
                {
                    System.Windows.MessageBox.Show("Could not load resource!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
                
                var grphc = new Bitmap(Image.FromFile(resourceString));
                cache.Add(resourceString, grphc );
                return grphc;
            }
        }

        public static Bitmap getTile (int width, int height)
        {
            string key = "empty";
            Bitmap b = ContentLoader.GetBitmapFromCache(key);
            if (b == null)
            {
                SolidBrush brush = new SolidBrush(System.Drawing.Color.Red);
                
                var map = new Bitmap(width, height);
                var g = Graphics.FromImage(map);
                g.FillRectangle(brush, 0, 0, width, height);
                g.Flush();
                cache.Add(key, map);

                return (Bitmap)map.Clone();
                

            }

            return (Bitmap)b.Clone();

        }

        public static void ClearCache()
        {
            cache.Clear();
        }

        public static BitmapSource CreateBitmapSourceFromGdiBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");
            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var bitmapData = bitmap.LockBits(
            rect,
            ImageLockMode.ReadWrite,
            System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            try
            {
                var size = (rect.Width * rect.Height) * 4;
                return BitmapSource.Create(
                bitmap.Width,
                bitmap.Height,
                bitmap.HorizontalResolution,
                bitmap.VerticalResolution,
                PixelFormats.Bgra32,
                null,
                bitmapData.Scan0,
                size,
                bitmapData.Stride);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }
    }
}
