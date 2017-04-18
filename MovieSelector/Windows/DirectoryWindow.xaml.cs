using System.ComponentModel;
using System.Windows;
using Gat.Controls;
using MovieSelector.ViewModels;

namespace MovieSelector.Windows
{
    /// <summary>
    /// Interaction logic for DirectoryWindow.xaml
    /// </summary>
    public partial class DirectoryWindow
    {
        private MovieSelectorViewModel ViewModel { get; }

        public DirectoryWindow(MovieSelectorViewModel viewModel)
        {
            InitializeComponent();

            DataContext = ViewModel = viewModel;

            Closing += OnClosing;
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

        private void BrowseFileClick(object sender, RoutedEventArgs e)
        {
            var vm = (OpenDialogViewModel)new OpenDialogView().DataContext;

            vm.FileFilterExtensions.Clear();
            vm.FileFilterExtensions.Add(".json");
            vm.StartupLocation = WindowStartupLocation.CenterScreen;

            if (vm.Show() != true) return;

            ViewModel.AddInfoFile(vm.SelectedFilePath);
        }

        public void OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
