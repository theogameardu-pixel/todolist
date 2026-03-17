using System.Globalization;
using System.Windows.Data;

namespace TaskFlow.Converters;

public class ProgressToPercentageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double progress)
        {
            return $"{progress:0}%";
        }

        return "0%";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
