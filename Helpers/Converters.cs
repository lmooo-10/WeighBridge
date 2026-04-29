using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WeighBridge.Helpers
{
    // ── Converts an Enum value to bool for ToggleButton IsChecked binding ──
    // Usage: ConverterParameter=EnumMemberName
    public class EnumToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string paramStr && value != null)
                return value.ToString() == paramStr;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is true && parameter is string paramStr)
                return Enum.Parse(targetType, paramStr);
            return Binding.DoNothing;
        }
    }

    // ── Converts null → Collapsed, non-null → Visible ──
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is null || (value is string s && string.IsNullOrEmpty(s))
               ? Visibility.Collapsed
               : Visibility.Visible;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    // ── Converts bool → Visibility ──
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is true ? Visibility.Visible : Visibility.Collapsed;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value is Visibility.Visible;
    }

    // ── Converts bool → Inverted Visibility (true → Collapsed) ──
    public class InverseBoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is true ? Visibility.Collapsed : Visibility.Visible;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value is Visibility.Collapsed;
    }
}
