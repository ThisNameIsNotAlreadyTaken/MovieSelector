using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Common.Instrastructure;

namespace MovieInfoParser.ViewModels
{
    public class MovieInfoParserViewModel : ObservableObject
    {
        public ObservableCollection<string> Directories { get; set; }

        public MovieInfoParserViewModel()
        {
            //TODO: if on start directories aren't empty, fill movieList
            Directories = new ObservableCollection<string>();
        }

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

        private void ScanForInfo()
        {
        }

        public ICommand ScanForInfoCommand => new DelegateCommand(ScanForInfo);
    }
}
