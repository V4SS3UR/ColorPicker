using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;

namespace ColorPicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance;

        public MainWindow()
        {
            InitializeComponent();
            this.StateChanged += MainWindow_View_StateChanged;

            Instance = this;

            Screen screen = Screen.FromHandle(new WindowInteropHelper(this).Handle);
            if (this.MaxHeight != screen.WorkingArea.Height)
                this.MaxHeight = screen.WorkingArea.Height + 2 * 6 + 2;
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);

            Screen screen = Screen.FromHandle(new WindowInteropHelper(this).Handle);
            if (this.MaxHeight != screen.WorkingArea.Height)
                this.MaxHeight = screen.WorkingArea.Height + 2 * 6 + 2;
        }

        private void MainWindow_View_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.BorderThickness = new System.Windows.Thickness(6);
                this.EdgeBorder.CornerRadius = new CornerRadius(0);
            }
            else
            {
                //this.BorderThickness = new System.Windows.Thickness(0);
                this.EdgeBorder.CornerRadius = new CornerRadius(10, 10, 0, 0);
            }

            ////Recentre le carousel
            //var tempWeek = DailyReportEntry_ViewModel.Instance.SelectedWeekTile;
            //DailyReportEntry_ViewModel.Instance.SelectedWeekTile = null;
            //DailyReportEntry_ViewModel.Instance.SelectedWeekTile = tempWeek;
        }

        private void TitleBarBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (this.WindowState == WindowState.Maximized)
                {
                    var point = PointToScreen(e.MouseDevice.GetPosition(this));

                    Left = point.X - (RestoreBounds.Width * 0.5);
                    Top = point.Y;

                    WindowState = WindowState.Normal;
                }
                DragMove();
            }
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MinimizeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void DockBtn_Click(object sender, RoutedEventArgs e)
        {
            switch (this.WindowState)
            {
                case WindowState.Normal:
                    this.WindowState = WindowState.Maximized;
                    break;

                case WindowState.Maximized:
                    this.WindowState = WindowState.Normal;
                    break;
            }
        }
    }
}