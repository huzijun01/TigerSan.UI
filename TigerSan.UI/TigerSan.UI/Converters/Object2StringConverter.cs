using System;
using System.Globalization;
using System.Windows.Data;

namespace TigerSan.UI.Converters
{
    [ValueConversion(typeof(object), typeof(string))]
    public class Object2StringConverter : IValueConverter
    {
        #region 源到目标
        public object? Convert(object? value, Type? targetType = null, object? parameter = null, CultureInfo? culture = null)
        {
            if (value == null)
            {
                return string.Empty;
            }

            return value.ToString() ?? string.Empty;
        }

        public string Convert(object? value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            return value.ToString() ?? string.Empty;
        }
        #endregion

        #region 目标到源
        public object? ConvertBack(object? value, Type? targetType = null, object? parameter = null, CultureInfo? culture = null)
        {
            return value;
        }
        #endregion
    }
}
