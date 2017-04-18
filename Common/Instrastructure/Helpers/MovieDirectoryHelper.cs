using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.Models;

namespace Common.Instrastructure.Helpers
{
    public static class MovieDirectoryHelper
    {
        public static List<MovieModel> GetMoviesFromFolders(ICollection<string> foldersList)
        {
            var res = new List<MovieModel>();

            foreach (var directory in foldersList.Where(Directory.Exists))
            {
                res.AddRange(Directory.GetFiles(directory, "*", SearchOption.AllDirectories).ToList()
                    .Where(x => ResourceHelper.Resources.ExtensionArray.Any(x.EndsWith))
                    .Select(x => new MovieModel(x)));
            }

            return res;
        }
    }
}