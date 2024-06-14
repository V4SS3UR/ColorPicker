using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ColorPicker.Core.Converters
{
    internal class SaturationLinearGradientColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush item = (SolidColorBrush)value;
            ColorMethods.RGB2HSL(new ColorMethods.ColorRGB(item.Color), out double h, out double s, out double l);

            var startColor = (Color)ColorMethods.HSL2RGB(h, 0, l);
            startColor.A = item.Color.A;
            var start = new GradientStop();
            start.Offset = 0;
            start.Color = startColor;

            var endColor = (Color)ColorMethods.HSL2RGB(h, 1, l);
            endColor.A = item.Color.A;
            var stop = new GradientStop();
            stop.Offset = 1;
            stop.Color = endColor;

            var result = new LinearGradientBrush();
            result.GradientStops = new GradientStopCollection();
            result.GradientStops.Add(start);
            result.GradientStops.Add(stop);

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}