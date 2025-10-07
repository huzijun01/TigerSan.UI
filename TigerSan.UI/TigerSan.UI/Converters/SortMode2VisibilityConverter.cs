using System.Windows;
using System.Windows.Data;
using System.Globalization;
using TigerSan.CsvLog;
using TigerSan.UI.Models;

namespace TigerSan.UI.Converters
{
    public class SortMode2VisibilityConverter : IValueConverter
    {
        #region 源到目标
        public object? Convert(object? value, Type? targetType = null, object? parameter = null, CultureInfo? culture = null)
        {
            if (value is not SortMode)
            {
                LogHelper.Instance.Warning($"The value is not {typeof(SortMode)}!");
                return Visibility.Collapsed;
            }

            return (SortMode)value == SortMode.Normal ? Visibility.Collapsed : Visibility.Visible;
        }
        #endregion

        #region 目标到源
        public object? ConvertBack(object? value, Type? targetType = null, object? parameter = null, CultureInfo? culture = null)
        {
            throw new NotSupportedException("This converter is one-way only.");
        }
        #endregion
    }
}
