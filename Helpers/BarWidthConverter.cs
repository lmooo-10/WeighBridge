using System;
using System.Globalization;
using System.Windows.Data;

namespace WeighBridge.Helpers
{
    /// <summary>
    /// MultiValueConverter that calculates bar width in pixels.
    /// Bindings: [0] = current value, [1] = max value
    /// ConverterParameter = available pixel width (default 300)
    /// </summary>
    public class BarWidthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2) return 0.0;

            double value = System.Convert.ToDouble(values[0]);
            double max = System.Convert.ToDouble(values[1]);

            if (max <= 0) return 0.0;

            double availableWidth = 300.0;
            if (parameter is string s && double.TryParse(s, out double pw))
                availableWidth = pw;

            double width = value / max * availableWidth;
            return Math.Max(0, Math.Min(availableWidth, width));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}