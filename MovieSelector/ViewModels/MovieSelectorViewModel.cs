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

namespace MovieSelector.ViewModels
{
    public class MovieSelectorViewModel : ObservableObject
    {
        private List<Movie> _movieList = new List<Movie>();

        private readonly Random _rnd = new Random();

        private int _counter;

        private Movie _selectedMovie;

        public ObservableCollection<string> Directories { get; set;}

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
            //TODO: if on start directories aren't empty, fill movieList
            Directories = new ObservableCollection<string>();
            Directories.CollectionChanged += OnDirectoriesCollectionChanged;
        }
        
        private void OnDirectoriesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _movieList = MovieDirectoryHelper.GetMoviesFromFolders(Directories);
        }


        private void SelectMovie()
        {
            if (!_movieList.Any()) return;

            var value = _rnd.Next(_movieList.Count());

            SelectedMovie = _movieList[value];

            Counter++;
        }

        public ICommand SelectMovieCommand => new DelegateCommand(SelectMovie);

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
    }
}
