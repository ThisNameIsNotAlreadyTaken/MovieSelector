using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.Models;

namespace Common.Instrastructure
{
    public static class MovieDirectoryHelper
    {
        public static List<Movie> GetMoviesFromFolders(ICollection<string> foldersList)
        {
            var res = new List<Movie>();

            foreach (var directory in foldersList.Where(Directory.Exists))
            {
                res.AddRange(Directory.GetFiles(directory, "*", SearchOption.AllDirectories).ToList()
                    .Where(x => CommonHelper.ExtensionArray.Any(x.EndsWith))
                    .Select(x => new Movie(x)));
            }

            return res;
        }
    }
}
