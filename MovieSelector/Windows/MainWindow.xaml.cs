using System.ComponentModel;
using System.Windows;
using MovieSelector.ViewModels;

namespace MovieSelector.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly DirectoryWindow _dWindow;
        private MovieSelectorViewModel ViewModel { get; } 

        public MainWindow()
        {
            InitializeComponent();

            ViewModel = new MovieSelectorViewModel();
            DataContext = ViewModel;

            _dWindow = new DirectoryWindow();

            Closing += OnClosing;
        }

        private void DirectoryDialogShowClick(object sender, RoutedEventArgs e)
        {
            if (_dWindow.DataContext == null)
            {
                _dWindow.DataContext = DataContext;
            }
            _dWindow.Show();
        }

        public void OnClosing(object sender, CancelEventArgs e)
        {
            ViewModel.SavePreferences();
            _dWindow.Close();
        }
    }
}
