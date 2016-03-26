using System.Windows;
using MovieInfoParser.ViewModels;

namespace MovieInfoParser
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private void Application_StartUp(object sender, StartupEventArgs e)
        {
            new MainWindow { DataContext = new MovieInfoParserViewModel() }.Show();
        }
    }
}
