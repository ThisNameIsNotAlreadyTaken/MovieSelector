using System.Windows;
using MovieSelector.ViewModels;
using MovieSelector.Windows;

namespace MovieSelector
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private void Application_StartUp(object sender, StartupEventArgs e)
        {
           new MainWindow {DataContext = new MovieSelectorViewModel()}.Show();
        }
    }
}
