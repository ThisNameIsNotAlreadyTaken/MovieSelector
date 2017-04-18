using System;

namespace Common.Models
{
    public class MovieModel
    {
        public MovieModel(string path)
        {
            FullPath = path;
        }

        public string FullPath { get; }

        public string FileName => FullPath.Substring(FullPath.LastIndexOf("\\", StringComparison.Ordinal) + 1);

        public string FilePath => FullPath.Substring(0, FullPath.LastIndexOf("\\", StringComparison.Ordinal));

        public string FileNameWithoutExtension
            => FileName.Substring(0, FileName.LastIndexOf(".", StringComparison.Ordinal));

        public CommonInfoModel Info { get; set; }
    }
}