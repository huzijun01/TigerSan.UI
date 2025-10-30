namespace TigerSan.UI.Models
{
    #region 委托
    /// <summary>
    /// “项目源数据改变”委托
    /// </summary>
    public delegate void ItemSourceChangedHandler(ItemModel itemModel);
    #endregion

    #region 列宽手柄状态
    /// <summary>
    /// 列宽手柄状态
    /// </summary>
    public enum HandelState
    {
        Hidden,
        Normal,
        Hover
    }
    #endregion

    #region 项目状态
    /// <summary>
    /// 项目状态
    /// </summary>
    public enum ItemState
    {
        Normal,
        Hover,
        Modified,
        Error
    }
    #endregion

    #region 项目类型
    /// <summary>
    /// 项目类型
    /// </summary>
    public enum ItemType
    {
        Header,
        Item,
    }
    #endregion

    #region 排序模式
    /// <summary>
    /// 排序模式
    /// </summary>
    public enum SortMode
    {
        Normal,
        Increasing,
        Decreasing,
    }
    #endregion
}
