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

        #region 清除非首行元素
        public static void RemoveNotFirstRowElements(Grid grid)
        {
            // 遍历Grid子元素并移除非第一行元素
            for (int index = grid.Children.Count - 1; index >= 0; index--) // 倒序遍历避免索引错乱
            {
                UIElement child = grid.Children[index];
                int row = Grid.GetRow(child); // 获取子元素所在行号

                if (row != 0) // 不是第一行则移除
                {
                    grid.Children.Remove(child);
                }
            }
        }
        #endregion
    }
}
