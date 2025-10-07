using System.Globalization;
using System.Windows.Data;
using TigerSan.CsvLog;

namespace TigerSan.UI.Converters
{
    #region 时间格式
    /// <summary>
    /// 时间格式
    /// </summary>
    public static class DateTimeFormate
    {
        public static string YearMonthDay = "yyyy-MM-dd";
        public static string HourMinuteSecond = "HH:mm:ss";
        public static string YearMonthDayHourMinuteSecond = "yyyy-MM-dd-HH:mm:ss";
    }
    #endregion

    [ValueConversion(typeof(DateTime), typeof(string))]
    public class DateTime2StringConverter : IValueConverter
    {
        #region 【Fields】
        public string _formate = DateTimeFormate.YearMonthDay;
        #endregion 【Fields】

        #region 源到目标
        public object? Convert(object? value, Type? targetType = null, object? parameter = null, CultureInfo? culture = null)
        {
            if (value == null)
            {
                LogHelper.Instance.IsNull(nameof(value));
                return null;
            }

            var source = (DateTime)value;

            return source.ToString(_formate);
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

            var source = new DateTime();

            if (!DateTime.TryParse(target, out source)) return null;

            return source;
        }
        #endregion
    }
}
