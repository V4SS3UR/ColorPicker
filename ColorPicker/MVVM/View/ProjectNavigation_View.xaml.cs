using ColorPicker.MVVM.Model;
using ColorPicker.MVVM.ViewModel;
using System.Windows;
using System.Windows.Controls;
using VsrFade.Controls;

namespace ColorPicker.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour ProjectNavigation_View.xaml
    /// </summary>
    public partial class ProjectNavigation_View : UserControl
    {
        public ProjectNavigation_View()
        {
            InitializeComponent();
        }

        private void ProjectRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            FadeRadioButton fadeRadioButton = (FadeRadioButton)sender;
            Project project = fadeRadioButton.DataContext as Project;
            ProjectNavigation_ViewModel.Instance.SelectedProject = project;
        }

        private void DeleteContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            Project project = menuItem.DataContext as Project;
            ProjectNavigation_ViewModel.Instance.DeleteProject(project);
        }
    }
}