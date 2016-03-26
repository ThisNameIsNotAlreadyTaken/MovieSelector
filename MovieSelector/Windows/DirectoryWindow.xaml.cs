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
        public DirectoryWindow()
        {
            InitializeComponent();
        }

        private void AddDirectory_OnClick(object sender, RoutedEventArgs e)
        {
            var vm = (OpenDialogViewModel)new OpenDialogView().DataContext;

            vm.IsDirectoryChooser = true;
            vm.StartupLocation = WindowStartupLocation.CenterScreen;

            if (vm.Show() != true) return;

            ((MovieSelectorViewModel)DataContext).AddDirectory(vm.SelectedFolder.Path);
        }

        private void RemoveDirectory_OnClick(object sender, RoutedEventArgs e)
        {
            if (LbDirectories.SelectedItem != null)
            {
                ((MovieSelectorViewModel)DataContext).RemoveDirectory(LbDirectories.SelectedItem as string);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
