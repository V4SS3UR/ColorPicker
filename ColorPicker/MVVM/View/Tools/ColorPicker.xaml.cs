using ColorPicker.Core;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ColorPicker.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour ColorPicker.xaml
    /// </summary>
    public partial class ColorPickerControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty AlphaProperty =
            DependencyProperty.Register("Alpha", typeof(byte), typeof(ColorPickerControl),
                new PropertyMetadata((byte)0, OnBytesChanged));

        public static readonly DependencyProperty RedProperty =
            DependencyProperty.Register("Red", typeof(byte), typeof(ColorPickerControl),
                new PropertyMetadata((byte)0, OnBytesChanged));

        public static readonly DependencyProperty GreenProperty =
            DependencyProperty.Register("Green", typeof(byte), typeof(ColorPickerControl),
                new PropertyMetadata((byte)0, OnBytesChanged));

        public static readonly DependencyProperty BlueProperty =
            DependencyProperty.Register("Blue", typeof(byte), typeof(ColorPickerControl),
                new PropertyMetadata((byte)0, OnBytesChanged));

        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Color), typeof(ColorPickerControl),
                new PropertyMetadata(Colors.White, OnColorChanged));

        public static readonly DependencyProperty HexValueProperty =
            DependencyProperty.Register("HexValue", typeof(string), typeof(ColorPickerControl),
                new PropertyMetadata("#FFFF", OnHexValueChanged));

        public static readonly DependencyProperty RgbValueProperty =
            DependencyProperty.Register("RgbValue", typeof(string), typeof(ColorPickerControl),
                new PropertyMetadata("rgb(255,255,255)", OnRgbValueChanged));

        public static readonly DependencyProperty HslValueProperty =
            DependencyProperty.Register("HslValue", typeof(string), typeof(ColorPickerControl),
                new PropertyMetadata("hsl(0,100%,100%)", OnHslValueChanged));

        public byte Alpha
        {
            get { return (byte)GetValue(AlphaProperty); }
            set { SetValue(AlphaProperty, value); }
        }

        public byte Red
        {
            get { return (byte)GetValue(RedProperty); }
            set { SetValue(RedProperty, value); }
        }

        public byte Green
        {
            get { return (byte)GetValue(GreenProperty); }
            set { SetValue(GreenProperty, value); }
        }

        public byte Blue
        {
            get { return (byte)GetValue(BlueProperty); }
            set { SetValue(BlueProperty, value); }
        }

        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public string HexValue
        {
            get { return (string)GetValue(HexValueProperty); }
            set { SetValue(HexValueProperty, value); }
        }
        public string RgbValue
        {
            get { return (string)GetValue(RgbValueProperty); }
            set { SetValue(RgbValueProperty, value); }
        }
        public string HslValue
        {
            get { return (string)GetValue(HslValueProperty); }
            set { SetValue(HslValueProperty, value); }
        }

        private static bool _freezeComponents;

        public ColorPickerControl()
        {
            InitializeComponent();
        }

        
        private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var colorPicker = (ColorPickerControl)d;

            _freezeComponents = true;

            colorPicker.UpdateBytesFromColor((Color)e.NewValue);
            colorPicker.UpdateHexFromColor((Color)e.NewValue);
            colorPicker.UpdateRgbFromColor((Color)e.NewValue);
            colorPicker.UpdateHslFromColor((Color)e.NewValue);

            _freezeComponents = false;
        }
        private static void OnBytesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var colorPicker = (ColorPickerControl)d;

            if (!_freezeComponents)
                colorPicker.Color = Color.FromArgb(colorPicker.Alpha, colorPicker.Red, colorPicker.Green, colorPicker.Blue);
        }
        private static void OnHexValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var colorPicker = (ColorPickerControl)d;

            if(!_freezeComponents)
                colorPicker.UpdateColorFromHex((string)e.NewValue);
        }
        private static void OnRgbValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var colorPicker = (ColorPickerControl)d;

            if (!_freezeComponents)
                colorPicker.UpdateColorFromRgb((string)e.NewValue);
        }
        private static void OnHslValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var colorPicker = (ColorPickerControl)d;

            if (!_freezeComponents)
                colorPicker.UpdateColorFromHsl((string)e.NewValue);
        }



        private void UpdateBytesFromColor(Color color)
        {
            this.Alpha = color.A;
            this.Red = color.R;
            this.Green = color.G;
            this.Blue = color.B;
        }
        private void UpdateHexFromColor(Color color)
        {
            this.HexValue = color.ToString();
        }
        private void UpdateRgbFromColor(Color color)
        {
            this.RgbValue = $"rgb({color.R}, {color.G}, {color.B})";
        }
        private void UpdateHslFromColor(Color color)
        {
            var h = Math.Round(color.Get_H() * 360);
            var s = Math.Round(color.Get_S() * 100);
            var l = Math.Round(color.Get_L() * 100);
            this.HslValue = $"hsl({h}, {s}%, {l}%)";
        }



        private void UpdateColorFromHex(string hexValue)
        {
            if (hexValue == null)
                return;

            try
            {
                this.Color = (Color)ColorConverter.ConvertFromString(hexValue);
            }
            catch (FormatException)
            {
                // Ignore invalid format
            }
        }
        private void UpdateColorFromRgb(string rgbValue)
        {
            if (rgbValue == null)
                return;

            try
            {
                int firstIndex = rgbValue.IndexOf("(")+1;
                int lastIndex = rgbValue.IndexOf(")");
                var str = rgbValue.Substring(firstIndex, lastIndex- firstIndex).Replace(" ","");
                var values = str.Split(',');
                var r = byte.Parse(values[0]);
                var g = byte.Parse(values[1]);
               var b = byte.Parse(values[2]);
                this.Color = Color.FromRgb(r, g, b);
            }
            catch (FormatException)
            {
                // Ignore invalid format
            }
        }
        private void UpdateColorFromHsl(string hslValue)
        {
            if (hslValue == null)
                return;

            try
            {
                int firstIndex = hslValue.IndexOf("(") + 1;
                int lastIndex = hslValue.IndexOf(")") - 1;
                var str = hslValue.Substring(firstIndex, lastIndex - firstIndex).Replace(" ", "");
                var values = str.Split(',');
                var h = double.Parse(values[0]);
                var s = double.Parse(values[1].Replace("%", ""));
                var l = double.Parse(values[2].Replace("%", ""));
                this.Color = ColorMethods.HSL2RGB(h / 360, s / 100, l / 100);
            }
            catch (FormatException)
            {
                // Ignore invalid format
            }
        }





        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}