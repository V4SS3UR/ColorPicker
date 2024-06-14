using ColorPicker.MVVM.Model;
using ColorPicker.MVVM.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace ColorPicker.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour Project_View.xaml
    /// </summary>
    public partial class Project_View : UserControl
    {
        public Project_View()
        {
            InitializeComponent();
        }          

        private void CopyColorButton_Click(object sender, RoutedEventArgs e)
        {
            ColorObject colorObject = ((Button)sender).DataContext as ColorObject;
            Clipboard.SetText(colorObject.Color.ToString());
        }
    }
}