using System.Windows;
using System.Windows.Controls;

namespace TigerSan.UI.Behaviors
{
    public class DynamicGridBehavior
    {
        #region 【Fields】
        private static readonly GridLength _defaultWidth = GridLength.Auto;
        private static readonly GridLength _defaultHeight = GridLength.Auto;
        #endregion 【Fields】

        #region 【AttachedProperties】
        #region 行数
        public static readonly DependencyProperty RowCountProperty =
            DependencyProperty.RegisterAttached(
                "RowCount",
                typeof(int),
                typeof(DynamicGridBehavior),
                new PropertyMetadata(0, RowCountChanged));

        private static void RowCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = d as Grid;
            if (grid == null) return;
            InitRowDefinitions(grid, (int)e.NewValue);
        }

        public static int GetRowCount(DependencyObject obj)
        {
            return (int)obj.GetValue(RowCountProperty);
        }

        public static void SetRowCount(DependencyObject obj, int value)
        {
            obj.SetValue(RowCountProperty, value);
        }
        #endregion

        #region 列数
        public static readonly DependencyProperty ColCountProperty =
            DependencyProperty.RegisterAttached(
                "ColCount",
                typeof(int),
                typeof(DynamicGridBehavior),
                new PropertyMetadata(0, ColCountChanged));

        private static void ColCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = d as Grid;
            if (grid == null) return;
            InitColumnDefinitions(grid, (int)e.NewValue);
        }

        public static int GetColCount(DependencyObject obj)
        {
            return (int)obj.GetValue(ColCountProperty);
        }

        public static void SetColCount(DependencyObject obj, int value)
        {
            obj.SetValue(ColCountProperty, value);
        }
        #endregion

        #region 宽度
        public static readonly DependencyProperty WidthProperty =
            DependencyProperty.RegisterAttached(
                "Width",
                typeof(GridLength),
                typeof(DynamicGridBehavior),
                new PropertyMetadata(_defaultWidth, WidthChanged));

        private static void WidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (Equals(e.NewValue, e.OldValue)) return;
            var grid = d as Grid;
            if (grid == null) return;
            InitWidth(grid, (GridLength)e.NewValue);
        }

        public static GridLength GetWidth(DependencyObject obj)
        {
            return (GridLength)obj.GetValue(WidthProperty);
        }

        public static void SetWidth(DependencyObject obj, GridLength value)
        {
            obj.SetValue(WidthProperty, value);
        }
        #endregion

        #region 高度
        public static readonly DependencyProperty HeightProperty =
            DependencyProperty.RegisterAttached(
                "Height",
                typeof(GridLength),
                typeof(DynamicGridBehavior),
                new PropertyMetadata(_defaultHeight, HeightChanged));

        private static void HeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (Equals(e.NewValue, e.OldValue)) return;
            var grid = d as Grid;
            if (grid == null) return;
            InitHeight(grid, (GridLength)e.NewValue);
        }

        public static GridLength GetHeight(DependencyObject obj)
        {
            return (GridLength)obj.GetValue(HeightProperty);
        }

        public static void SetHeight(DependencyObject obj, GridLength value)
        {
            obj.SetValue(HeightProperty, value);
        }
        #endregion
        #endregion 【AttachedProperties】

        #region 【Functions】
        #region 初始化“行定义”
        private static void InitRowDefinitions(Grid grid, int rowCount)
        {
            grid.RowDefinitions.Clear();

            for (int i = 0; i < rowCount; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = _defaultHeight });
            }
        }
        #endregion

        #region 初始化“列定义”
        private static void InitColumnDefinitions(Grid grid, int colCount)
        {
            grid.ColumnDefinitions.Clear();

            for (int i = 0; i < colCount; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = _defaultWidth });
            }
        }
        #endregion

        #region 初始化“宽度”
        private static void InitWidth(Grid grid, GridLength width)
        {
            foreach (var col in grid.ColumnDefinitions)
            {
                col.Width = width;
            }
        }
        #endregion

        #region 初始化“高度”
        private static void InitHeight(Grid grid, GridLength height)
        {
            foreach (var row in grid.RowDefinitions)
            {
                row.Height = height;
            }
        }
        #endregion
        #endregion 【Functions】
    }
}
