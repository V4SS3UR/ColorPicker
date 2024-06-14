using System.Runtime.InteropServices;
using System.Windows.Media;

namespace ColorPicker.Core
{
    internal class ColorGetter
    {
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(ref System.Drawing.Point lpPoint);

        private static ColorPicker.MVVM.View.Picker _window { get; set; }

        private static Color _pixelColor;

        public static Color CreateWindow(Color color)
        {
            _window = new ColorPicker.MVVM.View.Picker()
            {
                PreviousColor = new SolidColorBrush(color),
            };
            _window.Closing += _window_Closing;

            _window.ShowDialog();

            return _pixelColor;
        }

        private static void _window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _pixelColor = _window.PixelColor.Color;
        }
    }
}