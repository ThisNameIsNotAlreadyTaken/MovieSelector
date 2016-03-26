﻿using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Windows.Media.Imaging;

namespace Common.Instrastructure
{
    public static class CommonHelper
    {
        static CommonHelper()
        {
            ExtensionArray = RmManager.GetString("Extensions")?.Split(',').ToList();
        }

        public static ResourceManager RmManager = new ResourceManager("Common.Resource", Assembly.GetExecutingAssembly());

        public static readonly List<string> ExtensionArray;

        public static BitmapImage ToBitmapImage(this Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                var bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
    }
}
