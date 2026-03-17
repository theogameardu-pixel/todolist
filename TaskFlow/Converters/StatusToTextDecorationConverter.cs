using System.Globalization;
using System.Windows;
using System.Windows.Data;
using TaskFlow.Models;

namespace TaskFlow.Converters;

public class StatusToTextDecorationConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is TaskStatus status && status == TaskStatus.Completed)
        {
            return TextDecorations.Strikethrough;
        }

        return null!;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
