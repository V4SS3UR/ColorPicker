using ColorPicker.MVVM.ViewModel;
using System.Linq;
using System.Windows;

namespace ColorPicker
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App() : base()
        {
            this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;        
            this.Exit += App_Exit;
        }

        void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            ProjectNavigation_ViewModel.SaveJson(ProjectNavigation_ViewModel.Instance.ProjectCollection.ToArray());
            MessageBox.Show("Unhandled exception occurred: \n" + e.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            ProjectNavigation_ViewModel.SaveJson(ProjectNavigation_ViewModel.Instance.ProjectCollection.ToArray());
        }
    }
}