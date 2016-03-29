using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using MovieSelector.ViewModels;

namespace MovieSelector.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly DirectoryWindow _dWindow;

        public MainWindow()
        {
            InitializeComponent();
            _dWindow = new DirectoryWindow();
        }

        private void DirectoryDialogShowClick(object sender, RoutedEventArgs e)
        {
            if (_dWindow.DataContext == null)
            {
                _dWindow.DataContext = DataContext;
            }
            _dWindow.Show();
        }

        protected void MinimizeClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        protected void RestoreClick(object sender, RoutedEventArgs e)
        {
            WindowState = (WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
        }

        protected void CloseClick(object sender, RoutedEventArgs e)
        {
            ((MovieSelectorViewModel)DataContext).SavePreferences();
            Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _dWindow.Close();
        }
    }
}
