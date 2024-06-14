using ColorPicker.Core;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ColorPicker.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour Picker.xaml
    /// </summary>
    public partial class Picker : Window, INotifyPropertyChanged
    {
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(ref System.Drawing.Point lpPoint);

        private SolidColorBrush _pixelColor; public SolidColorBrush PixelColor
        {
            get { return _pixelColor; }
            set { _pixelColor = value; OnPropertyChanged(); }
        }

        private SolidColorBrush _previousColor; public SolidColorBrush PreviousColor
        {
            get { return _previousColor; }
            set { _previousColor = value; OnPropertyChanged(); }
        }

        private BitmapSource _screenSample; public BitmapSource ScreenSample
        {
            get { return _screenSample; }
            set { _screenSample = value; OnPropertyChanged(); }
        }

        public Picker()
        {
            InitializeComponent();
            this.Loaded += Picker_Loaded;
            this.Closed += Picker_Closed;

            MouseHook.Start();
            MouseHook.MouseMove += MouseHook_MouseMove;
            MouseHook.MouseDown += MouseHook_MouseDown; ;
        }

        private void Picker_Loaded(object sender, RoutedEventArgs e)
        {
            PixelColor = new SolidColorBrush(Colors.White);

            System.Drawing.Point cursor = new System.Drawing.Point();
            GetCursorPos(ref cursor);

            this.Left = cursor.X - this.ActualWidth / 2;
            this.Top = cursor.Y - this.ActualHeight / 2;
        }

        private void Picker_Closed(object sender, EventArgs e)
        {
            MouseHook.MouseMove -= MouseHook_MouseMove;
            MouseHook.MouseDown -= MouseHook_MouseDown;
            MouseHook.stop();
        }

        private void MouseHook_MouseDown(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MouseHook_MouseMove(object sender, EventArgs e)
        {
            System.Drawing.Point cursor = new System.Drawing.Point();
            GetCursorPos(ref cursor);

            this.Left = cursor.X - this.ActualWidth / 2;
            this.Top = cursor.Y - this.ActualHeight / 2;

            this.PixelColor = new SolidColorBrush(GetColorAt(cursor.X, cursor.Y));
            //this.ScreenSample = this.GetScreenAt(cursor.X, cursor.Y, 5, 100);
        }

        public System.Windows.Media.Color GetColorAt(int x, int y)
        {
            Bitmap bmp = new Bitmap(1, 1);
            System.Drawing.Rectangle bounds = new System.Drawing.Rectangle(x, y, 1, 1);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(bounds.Location, System.Drawing.Point.Empty, bounds.Size);
            }
            var color = bmp.GetPixel(0, 0);
            return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public BitmapSource GetScreenAt(int x, int y, int scale, int zoomFactor)
        {
            Bitmap bmp = new Bitmap(scale, scale);
            System.Drawing.Rectangle bounds = new System.Drawing.Rectangle(x - scale / 2, y - scale / 2, scale, scale);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(bounds.Location, System.Drawing.Point.Empty, bounds.Size);
            }

            System.Drawing.Size newSize = new System.Drawing.Size((int)(bmp.Width * zoomFactor), (int)(bmp.Height * zoomFactor));
            bmp = new Bitmap(bmp, newSize);

            var screenCapture = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bmp.GetHbitmap(),
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(newSize.Width, newSize.Height));

            return screenCapture;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}