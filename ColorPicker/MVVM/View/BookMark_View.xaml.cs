using ColorPicker.MVVM.Model;
using ColorPicker.MVVM.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace ColorPicker.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour BookMark_View.xaml
    /// </summary>
    public partial class BookMark_View : UserControl
    {

        public BookMark_View()
        {
            InitializeComponent();
        }


        private void ApplyColorButton_Click(object sender, RoutedEventArgs e)
        {
            ColorObject colorObject = ((Button)sender).DataContext as ColorObject;
            var vm = (Project_ViewModel)this.DataContext;
            vm.Project.CurrentColor = colorObject.getCopy();
        }

        private void RemovePinnedColorButton_Click(object sender, RoutedEventArgs e)
        {
            ColorObject colorObject = ((Button)sender).DataContext as ColorObject;
            var vm = (Project_ViewModel)this.DataContext;
            vm.Project.ColorPinned.Remove(colorObject);
        }

        private void AddSampleButton_Click(object sender, RoutedEventArgs e)
        {
            ColorObject colorObject = ((Button)sender).DataContext as ColorObject;
            var vm = (Project_ViewModel)this.DataContext;
            vm.OpenSampleWindowCommandFunction(colorObject);
        }

        private void CopyColorButton_Click(object sender, RoutedEventArgs e)
        {
            ColorObject colorObject = ((Button)sender).DataContext as ColorObject;
            Clipboard.SetText(colorObject.Color.ToString());
        }
    }
}
