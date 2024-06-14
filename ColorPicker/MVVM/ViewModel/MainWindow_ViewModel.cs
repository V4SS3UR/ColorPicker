using ColorPicker.Core;
using ColorPicker.MVVM.View;
using System.Globalization;
using System.Reflection;

namespace ColorPicker.MVVM.ViewModel
{
    internal class MainWindow_ViewModel : ObservableObject
    {
        public static MainWindow_ViewModel Instance { get; set; }

        private string _windowTitle; public string WindowTitle
        {
            get { return _windowTitle; }
            set { _windowTitle = value; OnPropertyChanged(); }
        }

        private string _windowTitleVersion; public string WindowTitleVersion
        {
            get { return _windowTitleVersion; }
            set { _windowTitleVersion = value; OnPropertyChanged(); }
        }

        private object _currentView; public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        public MainWindow_ViewModel()
        {
            Instance = this;

            WindowTitle = Assembly.GetExecutingAssembly().GetName().Name;

            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr-FR");
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR");

            string assemblyNameVersionMinor = Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString();
            string assemblyNameVersionMajor = Assembly.GetExecutingAssembly().GetName().Version.Major.ToString();
            WindowTitleVersion = $"v{assemblyNameVersionMajor}.{assemblyNameVersionMinor}";

            CurrentView = new ProjectNavigation_View();
        }
    }
}