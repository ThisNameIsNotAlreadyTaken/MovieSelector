using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Common.Models
{
    public class Movie
    {
        private BitmapImage _image;

        public int Year { get; set; }

        public Movie(string path)
        {
            FullPath = path;
        }

        public string FullPath { get; }

        public string FileName => FullPath.Substring(FullPath.LastIndexOf("\\", StringComparison.Ordinal) + 1);

        public string FilePath => FullPath.Substring(0, FullPath.LastIndexOf("\\", StringComparison.Ordinal));

        public BitmapImage Image
        {
            get { return _image ?? (BitmapImage)Application.Current.Resources["NoImage"]; }
            set { _image = value; }
        }

        public string Actors => "Unknown";
    }
}
