using System.IO;
using System.Windows.Media;
using TigerSan.CsvLog;
using TigerSan.ImageOperation;

namespace TigerSan.UI.Helpers
{
    public static class WindowHelper
    {
        #region 【Fields】
        /// <summary>
        /// 应用启动路径
        /// </summary>
        private static readonly string? _appStartupPath = Environment.ProcessPath;
        #endregion 【Fields】

        #region 【Functions】
        #region 获取“图标”
        public static ImageSource GetIcon()
        {
            if (!File.Exists(_appStartupPath))
            {
                LogHelper.Instance.Warning($"The file does not exist!{Environment.NewLine}{_appStartupPath}");
                return Generic.logo_32;
            }

            return IconHelper.GetBitmapSource(_appStartupPath) ?? Generic.logo_32;
        }
        #endregion
        #endregion 【Functions】
    }
}
