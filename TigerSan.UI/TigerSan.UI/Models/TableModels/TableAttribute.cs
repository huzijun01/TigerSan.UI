using System.Windows;

namespace TigerSan.UI.Models
{
    #region 表格特性
    /// <summary>
    /// 表格特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        #region [尺寸]
        public double? Height { get; set; } = null;
        public double? MinHeight { get; set; } = null;
        public double? MaxHeight { get; set; } = null;
        #endregion [尺寸]

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
    #endregion

    #region 表头特性
    /// <summary>
    /// 表头特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class TableHeaderAttribute : Attribute
    {
        /// <summary>
        /// 未设置
        /// </summary>
        public static readonly double _notSet = -1;

        #region [尺寸]
        public double Width { get; set; } = _notSet;
        public double MinWidth { get; set; } = 50;
        public double MaxWidth { get; set; } = double.MaxValue;
        public bool IsWidthAvailable { get => Width >= MinWidth && Width <= MaxWidth; }
        public GridLength WidthLength { get => IsWidthAvailable ? new GridLength(Width) : Generic.DefaultGridWidth; }
        #endregion [尺寸]

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 是否只读
        /// </summary>
        public bool IsReadOnly { get; set; } = false;

        /// <summary>
        /// 是否允许排序
        /// </summary>
        public bool IsAllowSort { get; set; } = false;

        /// <summary>
        /// 是否允许调整尺寸
        /// </summary>
        public bool IsAllowResize { get; set; } = true;

        /// <summary>
        /// 排序模式
        /// </summary>
        public SortMode SortMode { get; set; } = SortMode.Normal;

        /// <summary>
        /// 文本对齐方式
        /// </summary>
        public TextAlignment TextAlignment { get; set; } = TextAlignment.Left;
    }
    #endregion
}
