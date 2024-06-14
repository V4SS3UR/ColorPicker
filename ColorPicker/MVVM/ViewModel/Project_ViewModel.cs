using ColorPicker.Core;
using ColorPicker.MVVM.Model;
using ColorPicker.MVVM.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ColorPicker.MVVM.ViewModel
{
    internal class Project_ViewModel : ObservableObject
    {
        public static Project_ViewModel Instance;

        public RelayCommand PickScreenColorCommand { get; set; }
        public RelayCommand PinColorCommand { get; set; }
        public RelayCommand OpenSampleWindowCommand { get; set; }
        public RelayCommand CloseAllSampleWindowCommand { get; set; }
        public RelayCommand DeleteProjectCommand { get; set; }
        public RelayCommand ResetSaturationCommand { get; set; }
        public RelayCommand ResetLightnessCommand { get; set; }
        public RelayCommand ClearPickedColorHistoryCommand { get; set; }
        public RelayCommand ClearPinnedColorHistoryCommand { get; set; }
        public RelayCommand ColorSortPinnedColorHistoryCommand { get; set; }
        public RelayCommand AlphaSortPinnedColorHistoryCommand { get; set; }
        public RelayCommand CopyToClipboardCommand { get; set; }
        public RelayCommand RandomizedColorCommand { get; set; }

        public List<SampleWindow> SampleWindows { get; set; }

        private Project _project; public Project Project
        {
            get { return _project; }
            set { _project = value; OnPropertyChanged(); }
        }


        public Project_ViewModel()
        {
            Instance = this;

            this.SampleWindows = new List<SampleWindow>();
            this.Project = new Project();

            this.PickScreenColorCommand = new RelayCommand(
                command => PickScreenColorCommandFunction());
            this.PinColorCommand = new RelayCommand(
                command => this.Project.ColorPinned.Insert(0,this.Project.CurrentColor.getCopy()));
            this.RandomizedColorCommand = new RelayCommand(
                command => this.Project.CurrentColor.SetRandomBrush());

            this.OpenSampleWindowCommand = new RelayCommand(
                command => OpenSampleWindowCommandFunction(this.Project.CurrentColor));

            this.CloseAllSampleWindowCommand = new RelayCommand(
                command => CloseAllSampleWindowCommandFunction(),
                Condition => SampleWindows.Any());            

            this.DeleteProjectCommand = new RelayCommand(
                command => ProjectNavigation_ViewModel.Instance.DeleteProject(this.Project));

            this.ResetLightnessCommand = new RelayCommand(
                command => this.Project.LightnessSliderObject.Value = 0.5);
            this.ResetSaturationCommand = new RelayCommand(
                command => this.Project.SaturationSliderObject.Value = 1);

            this.ClearPickedColorHistoryCommand = new RelayCommand(
                command => this.Project.ColorHistory.Clear());
            this.ClearPinnedColorHistoryCommand = new RelayCommand(
                command => this.Project.ColorPinned.Clear());

            this.CopyToClipboardCommand = new RelayCommand(
                command => Clipboard.SetText(this.Project.CurrentColor.HexValue));

            this.ColorSortPinnedColorHistoryCommand = new RelayCommand(
                command => ColorSortPinnedColorHistoryCommandFunction());
            this.AlphaSortPinnedColorHistoryCommand = new RelayCommand(
                command => AlphaSortPinnedColorHistoryCommandFunction());

            
        }



        public void PickScreenColorCommandFunction()
        {
            var color = ColorGetter.CreateWindow(this.Project.CurrentColor.SolidColorBrush.Color);
            this.Project.SetCurrentColorBrush(new SolidColorBrush(color));
            this.Project.ColorHistory.Insert(0,new ColorObject(color.ToString())
            {
                Color = color
            });
        }
        public void OpenSampleWindowCommandFunction(ColorObject colorObject)
        {
            SampleWindow sample = new SampleWindow(this.Project, colorObject.getCopy());
            sample.Show();

            SampleWindows.Add(sample);

            if (SampleWindows.Count > 1)
                sample.Owner = SampleWindows[SampleWindows.IndexOf(sample) - 1];
        }
        public void CloseAllSampleWindowCommandFunction()
        {
            SampleWindow[] samples = SampleWindows.ToArray();
            foreach (var sample in samples)
            {
                SampleWindows.Remove(sample);

                try
                {
                    sample.Close();
                }
                catch (System.Exception)
                {
                }
            }
        }
        public void ColorSortPinnedColorHistoryCommandFunction()
        {
            

            Comparison<ColorObject> ColorComparison = (colorObjectA, colorObjectB) =>
            {
                var thisH = colorObjectA.Color.Get_H();
                var otherH = colorObjectB.Color.Get_H();

                if (thisH < otherH)
                    return -1;
                if (thisH > otherH)
                    return 1;

                return 0;
            };
            Comparer<ColorObject> ColorComparer = Comparer<ColorObject>.Create(ColorComparison);

            var sortedObj = this.Project.ColorPinned.OrderBy(x => x, ColorComparer).ToArray();

            this.Project.ColorPinned.Clear();
            foreach (var item in sortedObj)
            {
                this.Project.ColorPinned.Add(item);
            }
        }
        public void AlphaSortPinnedColorHistoryCommandFunction()
        {
            var sortedObj = this.Project.ColorPinned.OrderBy(x => x.Name).ToArray();

            this.Project.ColorPinned.Clear();
            foreach (var item in sortedObj)
            {
                this.Project.ColorPinned.Add(item);
            }
        }
    }
}