using System.Globalization;
using System.Windows;
using System.Windows.Data;
using TigerSan.CsvLog;

namespace TigerSan.UI.Converters
{
    public class Bool2ResizeModeConverter : IValueConverter
    {
        #region 源到目标
        public object? Convert(object? value, Type? targetType = null, object? parameter = null, CultureInfo? culture = null)
        {
            if (value == null)
            {
                LogHelper.Instance.IsNull(nameof(value));
                return null;
            }

            return (bool)value ? ResizeMode.CanResizeWithGrip : ResizeMode.NoResize;
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

            return (ResizeMode)value == ResizeMode.CanResizeWithGrip ? true : false;
        }
        #endregion
    }
}
