using System.Windows;
using System.Windows.Data;
using System.Globalization;
using TigerSan.CsvLog;

namespace TigerSan.UI.Converters
{
    public class Bool2VisibilityConverter : IValueConverter
    {
        #region 【Fields】
        public Visibility _visibilityTrue = Visibility.Visible;
        public Visibility _visibilityFalse = Visibility.Collapsed;
        public Visibility _visibilityDefault = Visibility.Collapsed;
        public bool _boolDefault = false;
        #endregion 【Fields】

        #region 源到目标
        public object? Convert(object? value, Type? targetType = null, object? parameter = null, CultureInfo? culture = null)
        {
            if (value is not bool)
            {
                LogHelper.Instance.Warning($"The value is not {typeof(bool)}!");
                return _visibilityDefault;
            }

            return (bool)value ? _visibilityTrue : _visibilityFalse;
        }
        #endregion

        #region 目标到源
        public object? ConvertBack(object? value, Type? targetType = null, object? parameter = null, CultureInfo? culture = null)
        {
            if (value is not Visibility)
            {
                LogHelper.Instance.Warning($"The value is not {typeof(Visibility)}!");
                return _boolDefault;
            }

            return (Visibility)value == _visibilityTrue;
        }
        #endregion
    }
}
