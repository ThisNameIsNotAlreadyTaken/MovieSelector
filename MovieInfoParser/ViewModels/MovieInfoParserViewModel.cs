using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Common.Instrastructure;
using Common.Instrastructure.Helpers;
using Common.Instrastructure.SearchEngines;
using Common.Models;
using Newtonsoft.Json;

namespace MovieInfoParser.ViewModels
{
    public class MovieInfoParserViewModel : ObservableObject
    {
        public ObservableCollection<string> Directories { get; set; }

        public EngineType EngineTypeValue { get; set; }

        public MovieInfoParserViewModel()
        {
            Directories = new ObservableCollection<string>();

            var storedDirectories = ResourceHelper.Resources.Directories;

            storedDirectories?.ForEach(x =>
            {
                if (Directory.Exists(x)) Directories.Add(x);
            });
        }

        public StringBuilder Log { get; set; } = new StringBuilder();

        public bool NotProcessing { get; set; } = true;
        public bool IsSaveLogToFileEnabled { get; set; } = false;

        /* counters */

        public int MovieListCount { get; set; }
        public int LeftToAnalyze { get; set; }
        public int Found { get; set; }
        public int NotFound { get; set; }

        /* methods */

        public void AddDirectory(string item)
        {
            if (item == null || Directories.FirstOrDefault(x => x == item) != null) return;
            Directories.Add(item);
            NotifyPropertyChanged("Directories");
        }

        public void RemoveDirectory(string item)
        {
            if (item == null) return;
            Directories.Remove(item);
            NotifyPropertyChanged("Directories");
        }

        public void PrepareToAnalyze(List<MovieModel> movieList)
        {
            Log.Clear();

            NotifyPropertyChanged("Log");

            NotProcessing = false;
            NotifyPropertyChanged("NotProcessing");

            LeftToAnalyze = movieList.Count;
            MovieListCount = movieList.Count;
            Found = 0;
            NotFound = 0;
            NotifyPropertyChanged("LeftToAnalyze");
            NotifyPropertyChanged("MovieListCount");
            NotifyPropertyChanged("Found");
            NotifyPropertyChanged("NotFound");
        }

        public void AnalyzeResults(MovieModel movie, List<CommonInfoModel> infoList, bool result)
        {
            Log.Append(result
                        ? $"Info found: {movie.FileName}{Environment.NewLine}"
                        : $"Info not found: {movie.FileName}{Environment.NewLine}");

            NotifyPropertyChanged("Log");

            LeftToAnalyze--;
            NotifyPropertyChanged("LeftToAnalyze");

            if (result)
            {
                Found++;
                NotifyPropertyChanged("Found");
            }
            else
            {
                NotFound++;
                NotifyPropertyChanged("NotFound");
            }

            if (LeftToAnalyze != 0) return;

            SaveInfoListToFile(infoList);

            infoList.Clear();

            NotProcessing = true;
            NotifyPropertyChanged("NotProcessing");
        }

        private void ScanForInfo()
        {
            var movieList = MovieDirectoryHelper.GetMoviesFromFolders(Directories);
            var infoList = new List<CommonInfoModel> ();

            PrepareToAnalyze(movieList);

            if (WebHelper.IsInternetConnectionAvailable)
            {
                if (movieList.Any())
                {
                    var searchEngine = EngineFactory.GetEngine(EngineTypeValue);

                    movieList.ForEach(async m =>
                    {
                        var info = await searchEngine.GetMovieInfo(m);

                        if (info != null)
                        {
                            info.RelatedFileName = m.FileNameWithoutExtension;
                            infoList.Add(info);
                        }

                        AnalyzeResults(m, infoList, info != null);
                    });
                }
            }
            else
            {
                Log.Append("Can't connect to the Internet, check your connection");
                NotifyPropertyChanged("Log");
            }
        }

        private void SaveInfoListToFile(IEnumerable<CommonInfoModel> infoList)
        {
            var ticks = DateTime.Now.Ticks;

            var fileName = "infos_" + ticks;
            var filePath = Path.Combine(Environment.CurrentDirectory, fileName + ".json");

            using (var file = File.CreateText(filePath)) {
                var serializer = new JsonSerializer();
                serializer.Serialize(file, infoList);
            }

            if (!IsSaveLogToFileEnabled) return;

            var logFilePath = Path.Combine(Environment.CurrentDirectory, "log_" + ticks + ".txt");
            using (var writer = new StreamWriter(logFilePath))
            {
                writer.WriteLine(Log);
            }
        }

        public ICommand ScanForInfoCommand => new DelegateCommand(ScanForInfo);

        public void SavePreferences()
        {
            if (Directories != null && Directories.Any())
            {
                ResourceHelper.Resources.Directories = Directories.ToList();
            }

            ResourceHelper.SavePreferences();
        }
    }
}
