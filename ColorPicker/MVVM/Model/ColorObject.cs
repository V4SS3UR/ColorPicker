using ColorPicker.Core;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace ColorPicker.MVVM.Model
{
    public class ColorObject : ObservableObject
    {
        private static Random Randomizer;

        public delegate void ColorChangedHandler(object sender);

        public event ColorChangedHandler OnColorChanged;

        private void OnColorChangedCallBack()
        {
            if (OnColorChanged == null) return;
            OnColorChanged(this);
        }

        

        private SolidColorBrush _solidColorBrush; public SolidColorBrush SolidColorBrush
        {
            get { return _solidColorBrush; }
            set { _solidColorBrush = value; OnPropertyChanged(); }
        }

        public Color Color
        {
            get { return SolidColorBrush.Color; }
            set
            {
                this.SolidColorBrush = new SolidColorBrush(value);
                OnColorChangedCallBack();
                OnPropertyChanged();
            }
        }

        private string _hexValue; public string HexValue
        {
            get { return _hexValue; }
            set { _hexValue = value; OnPropertyChanged(); }
        }

        private string _rgbValue; public string RgbValue
        {
            get { return _rgbValue; }
            set { _rgbValue = value; OnPropertyChanged(); }
        }

        private string _hslValue; public string HslValue
        {
            get { return _hslValue; }
            set { _hslValue = value; OnPropertyChanged(); }
        }

        private string _name; public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        static ColorObject()
        {
            Randomizer = new Random();
        }

        public ColorObject(string name)
        {
            this.Name = name;
            SetRandomBrush();
        }

        public ColorObject(string name, Color color)
        {
            this.Name = name;
            this.Color = color;
        }

        public void SetBrush(SolidColorBrush solidColorBrush)
        {
            this.Color = solidColorBrush.Color;
        }

        public void SetRandomBrush()
        {
            this.Color = this.GetRandomBrush();
        }

        public void SetBrightness(int value)
        {
            this.Color = GetVariantColorBrightness(this.SolidColorBrush, (float)value / 100);
        }

        public ColorObject getCopy()
        {
            var obj = new ColorObject(this.Name)
            {
                HexValue = this.HexValue,
                RgbValue = this.RgbValue,
                HslValue = this.HslValue,
                Color = this.Color,
                Name = this.Name,
                SolidColorBrush = new SolidColorBrush(this.SolidColorBrush.Color),
            };
            return obj;
        }

        private Color GetVariantColorBrightness(SolidColorBrush solidColorBrush, float correctionFactor)
        {
            float red = (float)solidColorBrush.Color.R;
            float green = (float)solidColorBrush.Color.G;
            float blue = (float)solidColorBrush.Color.B;
            float alpha = (float)solidColorBrush.Color.A;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            return Color.FromArgb((byte)alpha, (byte)red, (byte)green, (byte)blue);
        }

        private Color GetRandomBrush()
        {
            var color = Color.FromRgb(
                (byte)Randomizer.Next(0, 255),
                (byte)Randomizer.Next(0, 255),
                (byte)Randomizer.Next(0, 255));

            return color;
        }
    }
}