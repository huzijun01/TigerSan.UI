using System.Windows;
using System.Windows.Controls;

namespace TigerSan.UI.Helpers
{
    public static class GridHelper
    {
        #region 设置列空间
        /// <summary>
        /// 设置列空间
        /// </summary>
        public static void SetColumnSpan(UIElement element, int value)
        {
            int value1 = value > 0 ? value : 1;
            Grid.SetColumnSpan(element, value1);
        }
        #endregion

        #region 设置行列
        /// <summary>
        /// 设置行列
        /// </summary>
        public static void SetRowColumn(UIElement element, int row, int col)
        {
            Grid.SetRow(element, row);
            Grid.SetColumn(element, col);
        }
        #endregion
    }
}
