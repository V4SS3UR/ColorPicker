using ColorPicker.MVVM.Model;
using ColorPicker.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Xml;

namespace ColorPicker.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour SampleWindow.xaml
    /// </summary>
    public partial class SampleWindow : Window, INotifyPropertyChanged
    {
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(ref System.Drawing.Point lpPoint);

        private ColorObject _colorObject; public ColorObject ColorObject
        {
            get { return _colorObject; }
            set { _colorObject = value; OnPropertyChanged(); }
        }

        private Project _project; public Project Project
        {
            get { return _project; }
            set { _project = value; OnPropertyChanged(); }
        }


        public SampleWindow(Project project, ColorObject colorObject)
        {
            InitializeComponent();

            this.Project = project;
            this.ColorObject = colorObject;

            this.DataContext = this;

            this.Loaded += SampleWindow_Loaded;
            this.Closed += SampleWindow_Closed;
            this.MouseDown += SampleWindow_MouseDown;
        }

        private void SampleWindow_Closed(object sender, EventArgs e)
        {
            try
            {
                Project_ViewModel.Instance.SampleWindows.Remove(this);
            }
            catch (Exception)
            {
            }
        }

        private void SampleWindow_Loaded(object sender, RoutedEventArgs e)
        {
            System.Drawing.Point cursor = new System.Drawing.Point();
            GetCursorPos(ref cursor);

            this.Left = cursor.X - this.ActualWidth / 2;
            this.Top = cursor.Y - this.ActualHeight / 2;
        }

        private void SampleWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Project.CurrentColor != this.ColorObject)
            {
                this.Project.CurrentColor = this.ColorObject;
            }
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            List<SampleWindow> sampleWindows = Project_ViewModel.Instance.SampleWindows;
            int index = sampleWindows.IndexOf(this);

            if (index < sampleWindows.Count)
            {
                Project_ViewModel.Instance.SampleWindows.Remove(this);
                Project_ViewModel.Instance.SampleWindows.Insert(Math.Min(index + 1, sampleWindows.Count), this);
            }

            foreach (var sample in sampleWindows)
            {
                sample.Owner = null;
            }

            for (int i = 0; i < sampleWindows.Count; i++)
            {
                if (i > 0)
                    sampleWindows[i].Owner = sampleWindows[i - 1];

                sampleWindows[i].Activate();
            }
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            List<SampleWindow> sampleWindows = Project_ViewModel.Instance.SampleWindows;
            int index = sampleWindows.IndexOf(this);

            if (index > 0)
            {
                Project_ViewModel.Instance.SampleWindows.Remove(this);
                Project_ViewModel.Instance.SampleWindows.Insert(Math.Max(index - 1, 0), this);
            }

            foreach (var sample in sampleWindows)
            {
                sample.Owner = null;
            }

            for (int i = 1; i < sampleWindows.Count; i++)
            {
                if (i > 0)
                    sampleWindows[i].Owner = sampleWindows[i - 1];

                sampleWindows[i].Activate();
            }
        }
    }
}