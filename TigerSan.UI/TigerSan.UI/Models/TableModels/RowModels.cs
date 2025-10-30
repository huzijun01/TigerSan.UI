using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using TigerSan.CsvLog;
using TigerSan.UI.Controls;

namespace TigerSan.UI.Models
{
    #region 行模型
    /// <summary>
    /// 行模型
    /// </summary>
    public class RowModel : BindableBase
    {
        #region 【Fields】
        /// <summary>
        /// 行数据
        /// </summary>
        public TableModel _tableModel;
        #endregion【Fields】

        #region 【Properties】
        #region [引用]
        /// <summary>
        /// 行号
        /// </summary>
        public int RowIndex { get => GetRowIndex(); }
        #endregion [引用]

        #region [OneWay]
        /// <summary>
        /// 背景
        /// </summary>
        public Brush Background
        {
            get { return _background; }
            private set { SetProperty(ref _background, value); }
        }
        private Brush _background = Generic.Transparent;
        #endregion [OneWay]

        #region [更新状态]
        /// <summary>
        /// 行数据
        /// </summary>
        public object RowData
        {
            get { return _RowData; }
            set
            {
                _RowData = value;
                UpdateSources();
                UpdateItemStates();
            }
        }
        private object _RowData;

        /// <summary>
        /// 旧行数据
        /// </summary>
        public object? OldRowData
        {
            get { return _OldRowData; }
            set
            {
                _OldRowData = value;
                UpdateOldSources();
                UpdateItemStates();
            }
        }
        private object? _OldRowData;
        #endregion [更新状态]

        #region [Others]
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                SetProperty(ref _isChecked, value);
                Background = value ? Generic.Brand : Generic.Transparent;
            }
        }
        private bool _isChecked = false;

        /// <summary>
        /// 项目模型集合
        /// </summary>
        public Dictionary<HeaderModel, ItemModel> ItemModels { get; private set; } = new Dictionary<HeaderModel, ItemModel>();
        #endregion [Others]
        #endregion 【Properties】

        #region 【Ctor】
        public RowModel(
            TableModel tableModel,
            object rowData,
            object? oldRowData)
        {
            _RowData = rowData;
            _OldRowData = oldRowData;
            _tableModel = tableModel;
        }
        #endregion 【Ctor】

        #region 【Functions】
        #region 获取行号
        private int GetRowIndex()
        {
            var index = _tableModel.RowDatas.IndexOf(RowData);
            if (index < 0)
            {
                LogHelper.Instance.IsNotContain(nameof(_tableModel.RowDatas), nameof(RowData));
                return 0;
            }
            return index + 1;
        }
        #endregion

        #region 更新“源数据”集合
        private void UpdateSources()
        {
            foreach (var itemModel in ItemModels)
            {
                itemModel.Value.LoadSource(true);
            }
        }
        #endregion

        #region 更新“旧源数据”集合
        private void UpdateOldSources()
        {
            foreach (var itemModel in ItemModels)
            {
                itemModel.Value.UpdateOldSource();
            }
        }
        #endregion

        #region 更新“项目状态”集合
        private void UpdateItemStates()
        {
            foreach (var itemModel in ItemModels)
            {
                itemModel.Value.UpdateItemState();
            }
        }
        #endregion
        #endregion 【Functions】
    }
    #endregion

    #region 表头行UI元素
    /// <summary>
    /// 表头行UI元素
    /// </summary>
    public class HeaderRowUIElement
    {
        /// <summary>
        /// 复选框
        /// </summary>
        public CheckBox CheckBox = new CheckBox();

        /// <summary>
        /// 水平分割线
        /// </summary>
        public Line HorizontalDividingLine = new Line();

        /// <summary>
        /// 表头集合
        /// </summary>
        public Dictionary<HeaderModel, TableHeader> TableHeaders = new Dictionary<HeaderModel, TableHeader>();
    }
    #endregion

    #region 项目行UI元素
    /// <summary>
    /// 项目行UI元素
    /// </summary>
    public class ItemRowUIElement
    {
        /// <summary>
        /// 背景
        /// </summary>
        public Border Background = new Border();

        /// <summary>
        /// 复选框
        /// </summary>
        public CheckBox CheckBox = new CheckBox();

        /// <summary>
        /// 水平分割线
        /// </summary>
        public Line HorizontalDividingLine = new Line();

        /// <summary>
        /// 表格项目集合
        /// </summary>
        public Dictionary<ItemModel, TableItem> TableItems = new Dictionary<ItemModel, TableItem>();
    }
    #endregion
}
