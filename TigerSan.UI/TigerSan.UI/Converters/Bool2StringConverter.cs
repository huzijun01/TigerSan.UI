using System.Globalization;
using System.Windows.Data;
using TigerSan.CsvLog;

namespace TigerSan.UI.Converters
{
    [ValueConversion(typeof(bool), typeof(string))]
    public class Bool2StringConverter : IValueConverter
    {
        #region 【Fields】
        public string _strTrue = "True";
        public string _strFalse = "False";
        #endregion 【Fields】

        #region 【Ctor】
        public Bool2StringConverter() { }
        public Bool2StringConverter(string strTrue, string strFalse)
        {
            _strTrue = strTrue;
            _strFalse = strFalse;
        }
        #endregion 【Ctor】

        #region 源到目标
        public object? Convert(object? value, Type? targetType = null, object? parameter = null, CultureInfo? culture = null)
        {
            if (value == null)
            {
                LogHelper.Instance.IsNull(nameof(value));
                return null;
            }

            var source = (bool)value;

            return source ? _strTrue : _strFalse;
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

            if (!string.Equals(target, _strTrue) && !string.Equals(target, _strFalse)) return null;

            return string.Equals(target, _strTrue);
        }
        #endregion
    }
}
