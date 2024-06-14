using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ColorPicker.Core.Converters
{
    public class LightnessLinearGradientColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush item = (SolidColorBrush)value;

            var blackColor = Brushes.Black.Color;
            blackColor.A = item.Color.A;
            var start = new GradientStop();
            start.Offset = 0;
            start.Color = blackColor;

            var middle = new GradientStop();
            middle.Offset = 0.5;
            middle.Color = item.Color;

            var whiteColor = Brushes.White.Color;
            blackColor.A = item.Color.A;
            var stop = new GradientStop();
            stop.Offset = 1;
            stop.Color = whiteColor;

            var result = new LinearGradientBrush();
            result.GradientStops = new GradientStopCollection();
            result.GradientStops.Add(start);
            result.GradientStops.Add(middle);
            result.GradientStops.Add(stop);

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}