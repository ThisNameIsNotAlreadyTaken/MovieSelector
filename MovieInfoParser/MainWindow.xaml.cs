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
        public MainWindow()
        {
            InitializeComponent();
        }


        private void AddDirectory_OnClick(object sender, RoutedEventArgs e)
        {
            var vm = (OpenDialogViewModel)new OpenDialogView().DataContext;

            vm.IsDirectoryChooser = true;
            vm.StartupLocation = WindowStartupLocation.CenterScreen;

            if (vm.Show() != true) return;

            ((MovieInfoParserViewModel)DataContext).AddDirectory(vm.SelectedFolder.Path);
        }

        private void RemoveDirectory_OnClick(object sender, RoutedEventArgs e)
        {
            if (LbDirectories.SelectedItem != null)
            {
                ((MovieInfoParserViewModel)DataContext).RemoveDirectory(LbDirectories.SelectedItem as string);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            ((MovieInfoParserViewModel)DataContext).SavePreferences();
        }
    }
}
