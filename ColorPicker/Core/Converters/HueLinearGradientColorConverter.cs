using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using static ColorPicker.Core.ColorMethods;

namespace ColorPicker.Core.Converters
{
    internal class HueLinearGradientColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush item = (SolidColorBrush)value;
            ColorMethods.RGB2HSL(new ColorMethods.ColorRGB(item.Color), out double h, out double s, out double l);

            LinearGradientBrush hueGradient = new LinearGradientBrush();
            hueGradient.StartPoint = new Point(0, 0);
            hueGradient.EndPoint = new Point(1, 1);

            for (double i = 0; i < 1; i += 0.01)
            {
                Color hueColor = HSL2RGB(i, s, l);
                hueColor.A = item.Color.A;

                GradientStop hueGradientStep = new GradientStop();
                hueGradientStep.Color = hueColor;
                hueGradientStep.Offset = i;
                hueGradient.GradientStops.Add(hueGradientStep);
            }

            return hueGradient;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}