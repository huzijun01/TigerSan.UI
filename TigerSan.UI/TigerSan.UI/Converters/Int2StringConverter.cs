using System.Globalization;
using System.Windows.Data;
using TigerSan.CsvLog;

namespace TigerSan.UI.Converters
{
    [ValueConversion(typeof(int), typeof(string))]
    public class Int2StringConverter : IValueConverter
    {
        #region 源到目标
        public object? Convert(object? value, Type? targetType = null, object? parameter = null, CultureInfo? culture = null)
        {
            if (value == null)
            {
                LogHelper.Instance.IsNull(nameof(value));
                return null;
            }

            var source = (int)value;

            return source.ToString();
        }
        #endregion

        #region 目标到源
        public object? ConvertBack(object? value, Type? targetType = null, object? parameter = null, CultureInfo? culture = null)
        {
            if (value == null)
            {
                LogHelper.Instance.IsNull(nameof(value));
                return null;
            }

            var target = (string)value;

            int source;

            if (!int.TryParse(target, out source)) return null;

            return source;
        }
        #endregion
    }
}
