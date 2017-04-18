using System.ComponentModel;
using System.Windows;
using Gat.Controls;
using MovieInfoParser.ViewModels;

namespace MovieInfoParser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private MovieInfoParserViewModel ViewModel { get; }

        public MainWindow()
        {
            InitializeComponent();

            ViewModel = (MovieInfoParserViewModel)DataContext;
        }


        private void AddDirectoryClick(object sender, RoutedEventArgs e)
        {
            var vm = (OpenDialogViewModel)new OpenDialogView().DataContext;

            vm.IsDirectoryChooser = true;
            vm.StartupLocation = WindowStartupLocation.CenterScreen;

            if (vm.Show() != true) return;

            ViewModel.AddDirectory(vm.SelectedFolder.Path);
        }

        private void RemoveDirectoryClick(object sender, RoutedEventArgs e)
        {
            if (LbDirectories.SelectedItem != null)
            {
                ViewModel.RemoveDirectory(LbDirectories.SelectedItem as string);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            ViewModel.SavePreferences();
        }
    }
}
