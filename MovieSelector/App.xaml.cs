using System.Windows;
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
           new MainWindow().Show();
        }
    }
}
