using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Common.Instrastructure;
using Common.Models;
using MovieSelector.Infrascturcture;
using Newtonsoft.Json;
using WpfControls;

namespace MovieSelector.ViewModels
{
    public class MovieSelectorViewModel : ObservableObject
    {
        private List<Movie> _movieList = new List<Movie>();

        private readonly Random _rnd = new Random();

        private List<KinopoiskInfo> LocalInfoList { get; set; }

        public string LocalInfoFileName { get; set; }

        private int _counter;

        private Movie _selectedMovie;

        public ObservableCollection<string> Directories { get; set;}

        public bool Processing { get; set; }

        public Movie SelectedMovie
        {
            get { return _selectedMovie; }
            set
            {
                _selectedMovie = value;
                NotifyPropertyChanged("SelectedMovie");
            }
        }

        public int Counter
        {
            get { return _counter; }
            set
            {
                _counter = value;
                NotifyPropertyChanged("Counter");
            }
        }

        public MovieSelectorViewModel()
        {
            Directories = new ObservableCollection<string>();
            Directories.CollectionChanged += OnDirectoriesCollectionChanged;

            var storedDirectories = ResourceHelper.Resources.Directories;

            storedDirectories?.ForEach(x =>
            {
                if (Directory.Exists(x)) Directories.Add(x);
            });

            var storedInfoFile = ResourceHelper.Resources.LocalFile;

            if (!string.IsNullOrEmpty(storedInfoFile) && File.Exists(storedInfoFile))
            {
                AddInfoFile(storedInfoFile);
            }
        }
        
        private void OnDirectoriesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _movieList = MovieDirectoryHelper.GetMoviesFromFolders(Directories);
        }

        private async void SelectMovie(Movie movie)
        {
            if (!_movieList.Any()) return;

            Processing = true;
            NotifyPropertyChanged("Processing");

            var selectedMovie = movie ?? _movieList[_rnd.Next(_movieList.Count)];

            if (selectedMovie.KinopoiskInfo?.Id == null)
            {
                if (WebHelper.IsInternetConnectionAvailable)
                {
                    selectedMovie.KinopoiskInfo = await WebHelper.GetMovieInfo(selectedMovie, 1, 1);
                }

                if (selectedMovie.KinopoiskInfo?.Id == null && LocalInfoList != null)
                {
                    selectedMovie.KinopoiskInfo =
                        LocalInfoList.FirstOrDefault(x => x.RelatedFileName == selectedMovie.FileNameWithoutExtension);
                }

                if (selectedMovie.KinopoiskInfo == null)
                {
                    selectedMovie.KinopoiskInfo = new KinopoiskInfo();
                }
            }

            SelectedMovie = selectedMovie;

            if (movie == null)
            {
                Counter++;
            }

            Processing = false;
            NotifyPropertyChanged("Processing");
        }

        public ICommand SelectMovieCommand => new DelegateParameterCommand<Movie>(SelectMovie);

        private void ResetCounter()
        {
            Counter = 0;
        }

        public ICommand ResetCounterCommand => new DelegateCommand(ResetCounter);

        private void PlayMovie()
        {
            if (File.Exists(SelectedMovie.FullPath))
            {
                Process.Start(SelectedMovie.FullPath);
            }
        }

        public ICommand PlayMovieCommand => new DelegateCommand(PlayMovie);

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
            if (!Directories.Any())
            {
                SelectedMovie = null;
            }
            NotifyPropertyChanged("Directories");
        }

        public void AddInfoFile(string path)
        {
            LocalInfoList = null;
            LocalInfoFileName = null;

            try
            {
                if (!File.Exists(path)) return;

                using (var file = File.OpenText(path))
                {
                    using (var jReader = new JsonTextReader(file))
                    {
                        var serializer = new JsonSerializer();
                        var list = serializer.Deserialize<List<KinopoiskInfo>>(jReader);

                        if (!list.Any()) return;

                        LocalInfoFileName = path;
                        LocalInfoList = list;
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
            finally
            {
                NotifyPropertyChanged("LocalInfoList");
                NotifyPropertyChanged("LocalInfoFileName");
            }
        }

        public void SavePreferences()
        {
            if (Directories != null && Directories.Any())
            {
                ResourceHelper.Resources.Directories = Directories.ToList();
            }

            if (LocalInfoFileName != null)
            {
                ResourceHelper.Resources.LocalFile = LocalInfoFileName;
            }

            ResourceHelper.SavePreferences();
        }

        private int? _searchBoxWidth;
        public int? SearchBoxWidth
        {
            get { return _searchBoxWidth; }
            set
            {
                _searchBoxWidth = value;
                NotifyPropertyChanged("SearchBoxWidth");
            }
        }

        private Movie _searchBoxResult;
        public Movie SearchBoxResult
        {
            get { return _searchBoxResult; }
            set
            {
                _searchBoxResult = value;
                NotifyPropertyChanged("SearchBoxResult");

                if (value != null)
                {
                    SelectMovie(value);
                }
            }
        }

        public SuggestionProvider SProvider
        {
            get
            {
                return new SuggestionProvider(filter =>
                {
                    var valuesToTake = 15;

                    if (filter == null) return null;

                    var result = new List<Movie>();

                    var translitFilter = Transliter.TranslitRuToEn(filter);

                    result.AddRange(_movieList.Where(
                        x =>
                        {
                            var stringToCompare = x.FileNameWithoutExtension.Trim();

                            return stringToCompare.StartsWith(filter, StringComparison.OrdinalIgnoreCase) ||
                                   stringToCompare.StartsWith(translitFilter, StringComparison.OrdinalIgnoreCase);
                        }).ToList());

                    if (result.Count < valuesToTake)
                    {
                        var keyboardRuFilter = Transliter.KeyboardEnToRu(filter);
                        var keyboardEnFilter = Transliter.KeyboardRuToEn(filter);

                        result.AddRange(_movieList.Where(
                        x =>
                        {
                            var stringToCompare = x.FileNameWithoutExtension.Trim();

                            return !result.Contains(x) && 
                                   (stringToCompare.StartsWith(keyboardRuFilter, StringComparison.OrdinalIgnoreCase) ||
                                   stringToCompare.StartsWith(keyboardEnFilter, StringComparison.OrdinalIgnoreCase));
                        }).ToList());

                        if (result.Count < valuesToTake)
                        {
                            result.AddRange(_movieList.Where(
                                x =>
                                {
                                    var stringToCompare = x.FileNameWithoutExtension.Trim();

                                    return !result.Contains(x) && 
                                           (stringToCompare.IndexOf(filter,  StringComparison.OrdinalIgnoreCase) >= 0 ||
                                           stringToCompare.IndexOf(translitFilter, StringComparison.OrdinalIgnoreCase) >= 0);
                                }).ToList());

                            if (result.Count < valuesToTake)
                            {
                                result.AddRange(_movieList.Where(
                               x =>
                               {
                                   var stringToCompare = x.FileNameWithoutExtension.Trim();

                                   return !result.Contains(x) && 
                                           (stringToCompare.IndexOf(keyboardRuFilter, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                          stringToCompare.IndexOf(keyboardEnFilter, StringComparison.OrdinalIgnoreCase) >= 0);
                               }).ToList());
                            }
                        }
                    }

                    if (result.Any())
                    {
                        SearchBoxWidth = result.Max(x => x.FileNameWithoutExtension.Length)*7;
                    }
                    else
                    {
                        SearchBoxWidth = null;
                    }

                    return result.Take(valuesToTake);
                });
            }
        }
    }
}
