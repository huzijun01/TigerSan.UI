using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using TigerSan.CsvLog;

namespace TigerSan.UI.Converters
{
    [ValueConversion(typeof(Stretch), typeof(string))]
    public class Stretch2StringConverter : IValueConverter
    {
        #region 源到目标
        public object? Convert(object? value, Type? targetType = null, object? parameter = null, CultureInfo? culture = null)
        {
            if (value == null)
            {
                LogHelper.Instance.IsNull(nameof(value));
                return null;
            }

            var source = (Stretch)value;

            switch (source)
            {
                case Stretch.None:
                    return nameof(Stretch.None);
                case Stretch.Fill:
                    return nameof(Stretch.Fill);
                case Stretch.Uniform:
                    return nameof(Stretch.Uniform);
                case Stretch.UniformToFill:
                    return nameof(Stretch.UniformToFill);
                default:
                    return string.Empty;
            }
        }
        #endregion

        #region 目标到源
        public object? ConvertBack(object? value, Type? targetType = null, object? parameter = null, CultureInfo? culture = null)
        {
            throw new NotSupportedException($"OneWay conversion only!");
        }
        #endregion
    }
}
