using System.Windows.Data;
using System.Globalization;
using TigerSan.CsvLog;

namespace TigerSan.UI.Converters
{
    [ValueConversion(typeof(double), typeof(string))]
    public class Double2StringConverter : IValueConverter
    {
        #region 【Props】
        public int? Digits { get; set; }
        #endregion 【Props】

        #region 【Ctor】
        public Double2StringConverter() { }
        public Double2StringConverter(int digits‌)
        {
            Digits = digits;
        }
        #endregion 【Ctor】

        #region 源到目标
        public object? Convert(object? value, Type? targetType = null, object? parameter = null, CultureInfo? culture = null)
        {
            if (value is double number)
            {
                int? digits = Digits;

                if (parameter != null)
                {
                    if (int.TryParse(parameter.ToString(), out int paramDigits))
                    {
                        digits = paramDigits;
                    }
                }

                return digits == null ? number.ToString() : number.ToString($"F{digits}", culture);
            }

            LogHelper.Instance.Warning($"Unable to convert the value! ({value})");
            return null;
        }
        #endregion

        #region 目标到源
        public object? ConvertBack(object? value, Type? targetType = null, object? parameter = null, CultureInfo? culture = null)
        {
            if (value is string str)
            {
                if (double.TryParse(str, NumberStyles.Any, culture, out double result))
                {
                    return result;
                }
            }

            LogHelper.Instance.Warning($"Unable to convert the value! ({value})");
            return null;
        }
        #endregion

        #region 【Functions】
        #region [Static]
        #region 获取“double”
        public static double? GetDouble(string str)
        {
            return new Double2StringConverter().ConvertBack(str) as double?;
        }
        #endregion
        #endregion [Static]
        #endregion 【Functions】
    }
}
