using System.Windows;
using System.Reflection;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using TigerSan.CsvLog;
using TigerSan.UI.Helpers;
using TigerSan.UI.Controls;

namespace TigerSan.UI.Models
{
    /// <summary>
    /// 表格模型
    /// </summary>
    public class TableModel : BindableBase
    {
        #region 【Fields】
        /// <summary>
        /// 表格控件
        /// </summary>
        private TableGrid? _tableGrid;

        /// <summary>
        /// 列定义集合
        /// </summary>
        public List<ColumnDefinition> _colDefs = new List<ColumnDefinition>();

        /// <summary>
        /// 浮动列定义集合
        /// </summary>
        public List<ColumnDefinition> _floatColDefs = new List<ColumnDefinition>();

        /// <summary>
        /// 是否自动刷新
        /// </summary>
        public bool _isAutoRefresh = true;

        /// <summary>
        /// 集合改变事件参数
        /// </summary>
        private NotifyCollectionChangedEventArgs _args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);

        /// <summary>
        /// 旧行数据集合
        /// </summary>
        public List<object> _oldRowDatas = new List<object>();

        /// <summary>
        /// “加载完成”委托
        /// </summary>
        public Action? _onLoaded;

        /// <summary>
        /// “选中行数据集合改变”委托
        /// </summary>
        public Action? _onSelectedRowDatasChanged;

        /// <summary>
        /// “表头初始化”委托
        /// </summary>
        public HeaderInitHandler? _onHeaderInit;

        /// <summary>
        /// “项目初始化”委托
        /// </summary>
        public ItemInitHandler? _onItemInit;

        /// <summary>
        /// “项目源数据改变”委托
        /// </summary>
        public ItemSourceChangedHandler? _onItemSourceChanged;

        /// <summary>
        /// “行数据集合改变”委托
        /// </summary>
        public NotifyCollectionChangedEventHandler? _onRowDatasCollectionChanged;
        #endregion 【Fields】

        #region 【Properties】
        #region [引用]
        public double? Height { get => GetTableAttribute().Height; }
        public double? MinHeight { get => GetTableAttribute().MinHeight; }
        public double? MaxHeight { get => GetTableAttribute().MaxHeight; }
        public GridLength HeightGridLength { get => Height != null ? new GridLength(Height.Value) : Generic.DefaultGridHeight; }
        #endregion [引用]

        #region [OneWay]
        /// <summary>
        /// 是否触发“项目源数据改变”委托
        /// </summary>
        public bool IsTriggerItemSourceChanged { get; private set; } = true;

        /// <summary>
        /// 数据类型
        /// </summary>
        public Type DataType { get; private set; }

        /// <summary>
        /// 表头模型集合
        /// </summary>
        public List<HeaderModel> HeaderModels { get; private set; } = new List<HeaderModel>();

        /// <summary>
        /// 项目模型集合
        /// </summary>
        public Dictionary<object, RowModel> RowModels { get; private set; } = new Dictionary<object, RowModel>();

        /// <summary>
        /// 复选框可见性
        /// </summary>
        public Visibility CheckBoxVisibility
        {
            get { return _checkBoxVisibility; }
            private set { SetProperty(ref _checkBoxVisibility, value); }
        }
        private Visibility _checkBoxVisibility = Visibility.Visible;
        #endregion [OneWay]

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
        private string _name = string.Empty;

        /// <summary>
        /// 是否显示复选框
        /// </summary>
        public bool IsShowCheckBox
        {
            get { return CheckBoxVisibility == Visibility.Visible; }
            set { CheckBoxVisibility = value ? Visibility.Visible : Visibility.Collapsed; }
        }

        /// <summary>
        /// 是否全选
        /// </summary>
        public bool IsSelectAll
        {
            get { return _isSelectAll; }
            set { SetProperty(ref _isSelectAll, value); }
        }
        private bool _isSelectAll = false;

        /// <summary>
        /// 行数据集合
        /// </summary>
        public ObservableCollection<object> RowDatas
        {
            get { return _rowDatas; }
            set
            {
                _rowDatas = value;
                RowDatas.CollectionChanged -= RowDatas_CollectionChanged;
                RowDatas.CollectionChanged += RowDatas_CollectionChanged;
                RowDatas_CollectionChanged(this, _args);
            }
        }
        private ObservableCollection<object> _rowDatas = new ObservableCollection<object>();

        /// <summary>
        /// 被选中的行数据集合
        /// </summary>
        public List<object> SelectedRowDatas { get => GetSelectedRowDatas(); }

        /// <summary>
        /// 行数
        /// </summary>
        public int RowCount { get => RowModels.Count; }

        /// <summary>
        /// 被选中的行数
        /// </summary>
        public int SelectedRowCount { get => RowModels.Where(row => row.Value.IsChecked).Count(); }
        #endregion 【Properties】

        #region 【Ctor】
        public TableModel(Type dataType)
        {
            DataType = dataType;
            if (string.IsNullOrEmpty(Name)) Name = DataType.Name;
            Refresh();
        }
        #endregion 【Ctor】

        #region 【Events】
        #region 行数据集合改变
        private void RowDatas_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (!_isAutoRefresh) return;

            Refresh();

            _onRowDatasCollectionChanged?.Invoke(sender, e);
        }
        #endregion
        #endregion 【Events】

        #region 【Functions】
        #region [Private]
        #region 初始化网格
        private void InitGrid()
        {
            _colDefs.Clear();
            _floatColDefs.Clear();

            var props = DataType.GetProperties();
            if (props == null)
            {
                LogHelper.Instance.IsNull(nameof(props));
                return;
            }

            // 复选框列：
            _colDefs.Add(new ColumnDefinition() { Width = Generic.DefaultGridWidth });
            _floatColDefs.Add(new ColumnDefinition() { Width = Generic.DefaultGridWidth });

            // 内容框列：
            foreach (var prop in props)
            {
                var attr = prop.GetCustomAttribute(typeof(TableHeaderAttribute)) as TableHeaderAttribute;
                if (attr == null)
                {
                    LogHelper.Instance.IsNull(nameof(attr));
                    return;
                }

                var col = new ColumnDefinition()
                {
                    Width = attr.WidthLength,
                    MinWidth = attr.MinWidth,
                    MaxWidth = attr.MaxWidth
                };
                _colDefs.Add(col);

                var floatCol = new ColumnDefinition()
                {
                    Width = attr.WidthLength,
                    MinWidth = attr.MinWidth,
                    MaxWidth = attr.MaxWidth
                };
                _floatColDefs.Add(floatCol);
            }
        }
        #endregion

        #region 初始化表头模型集合
        private void InitHeaderModels()
        {
            // 清空表头：
            HeaderModels.Clear();

            // 获取属性集合：
            var props = DataType.GetProperties();
            if (props == null)
            {
                LogHelper.Instance.Warning($"The {nameof(props)} is null!");
                return;
            }

            // 添加表头：
            var count = props.Length;
            for (int index = 0; index < count; index++)
            {
                var headerModel = new HeaderModel(this);
                HeaderModels.Add(headerModel);
                headerModel.SetWidth(null);
            }

            // 执行“初始化”委托：
            foreach (var headerModel in HeaderModels)
            {
                _onHeaderInit?.Invoke(headerModel);
            }
        }
        #endregion

        #region 初始化项目模型集合
        private void InitItemModels()
        {
            RowModels.Clear();
            var rowCount = RowDatas.Count;
            var colCount = HeaderModels.Count;

            // 更新“旧行数据”：
            var rowDatas = TypeHelper.DeepCopyList(RowDatas);
            if (rowDatas == null)
            {
                LogHelper.Instance.Warning($"The {nameof(rowDatas)} is null!");
                return;
            }
            _oldRowDatas = rowDatas;

            // 添加“项目模型”：
            for (var iRow = 0; iRow < rowCount; ++iRow)
            {
                var rowData = RowDatas[iRow];
                var oldRowData = _oldRowDatas.Count > iRow ? _oldRowDatas[iRow] : null;
                var rowModel = new RowModel(this, rowData, oldRowData);

                for (var iCol = 0; iCol < colCount; ++iCol)
                {
                    var headerModel = HeaderModels[iCol];

                    var itemModel = new ItemModel(rowModel, headerModel);
                    _onItemInit?.Invoke(itemModel); // 执行“初始化”委托
                    rowModel.ItemModels.Add(headerModel, itemModel);
                }

                RowModels.Add(rowData, rowModel);
            }
        }
        #endregion
        #endregion [Private]

        #region [获取模型]
        #region 获取“表头模型”
        public HeaderModel? GetHeaderModel(string propName)
        {
            return HeaderModels.FirstOrDefault(header => string.Equals(header.PropName, propName));
        }
        #endregion

        #region 获取“项目模型”
        public ItemModel? GetItemModel(object rowData, string propName)
        {
            var header = HeaderModels.FirstOrDefault(h => string.Equals(h.PropName, propName));
            if (header == null)
            {
                LogHelper.Instance.IsNull(nameof(header));
                return null;
            }

            var rowModel = RowModels[rowData];
            if (rowModel == null)
            {
                LogHelper.Instance.IsNull(nameof(rowModel));
                return null;
            }

            var itemModel = rowModel.ItemModels[header];
            if (itemModel == null)
            {
                LogHelper.Instance.IsNull(nameof(itemModel));
                return null;
            }

            return itemModel;
        }
        #endregion

        #region 获取“项目模型”
        public ItemModel? GetItemModel(int iRow, string propName)
        {
            if (iRow < 0 || iRow >= RowModels.Count() || iRow >= RowDatas.Count())
            {
                LogHelper.Instance.IsOutOfRange(nameof(iRow));
                return null;
            }

            var rowData = RowDatas[iRow];

            return GetItemModel(rowData, propName);
        }
        #endregion

        #region 获取“行模型”
        public RowModel? GetRowModel(object rowData)
        {
            var rowModel = RowModels[rowData];
            if (rowModel == null)
            {
                LogHelper.Instance.IsNull(nameof(rowModel));
                return null;
            }

            return rowModel;
        }
        #endregion

        #region 获取“行模型”
        public RowModel? GetRowModel(int iRow)
        {
            if (iRow < 0 || iRow >= RowModels.Count() || iRow >= RowDatas.Count())
            {
                LogHelper.Instance.IsOutOfRange(nameof(iRow));
                return null;
            }

            var rowData = RowDatas[iRow];

            return GetRowModel(rowData);
        }
        #endregion
        #endregion [获取模型]

        #region [初始化]
        #region 初始化“列宽”
        /// <summary>
        /// 初始化“列宽”
        /// </summary>
        public void InitColumnsWidth()
        {
            if (_tableGrid == null)
            {
                LogHelper.Instance.IsEmpty(nameof(_tableGrid));
                return;
            }

            _tableGrid.InitColumnsWidth();
        }
        #endregion

        #region 初始化“表格模型”
        public void InitTableModel(TableGrid? tableGrid = null)
        {
            if (tableGrid != null)
            {
                _tableGrid = tableGrid;
            }

            IsTriggerItemSourceChanged = false;

            InitGrid();
            InitHeaderModels();
            InitItemModels();

            IsSelectAll = false;

            _onSelectedRowDatasChanged?.Invoke();
            _onRowDatasCollectionChanged?.Invoke(null, _args);

            IsTriggerItemSourceChanged = true;
        }
        #endregion

        #region 初始化“UI元素”
        public void InitUIElements()
        {
            if (_tableGrid == null)
            {
                LogHelper.Instance.IsNull(nameof(_tableGrid));
                return;
            }
            _tableGrid.InitUIElements();
        }
        #endregion
        #endregion [初始化]

        #region 刷新
        /// <summary>
        /// 刷新
        /// （初始化“模型”和“UI元素”）
        /// </summary>
        public void Refresh()
        {
            InitTableModel(null);
            InitUIElements();
        }
        #endregion

        #region 更新“旧行数据”集合
        public void UpdateOldRowDatas()
        {
            // 更新“旧行数据”：
            var rowDatas = TypeHelper.DeepCopyList(RowDatas);
            if (rowDatas == null)
            {
                LogHelper.Instance.Warning($"The {nameof(rowDatas)} is null!");
                return;
            }
            _oldRowDatas = rowDatas;

            // 更新“项目模型”的“旧行数据”：
            var rowCount = RowDatas.Count;
            var colCount = HeaderModels.Count;

            for (var iRow = 0; iRow < rowCount; ++iRow)
            {
                var oldRowData = _oldRowDatas.Count > iRow ? _oldRowDatas[iRow] : null;

                for (var iCol = 0; iCol < colCount; ++iCol)
                {
                    var header = HeaderModels[iCol];

                    var rowModel = GetRowModel(iRow);
                    if (rowModel == null)
                    {
                        LogHelper.Instance.IsNull(nameof(rowModel));
                        continue;
                    }

                    rowModel.OldRowData = oldRowData;
                }
            }
        }
        #endregion

        #region 获取“表格特性”
        public TableAttribute GetTableAttribute()
        {
            // 获取属性名：
            var attributes = DataType.GetCustomAttributes(typeof(TableAttribute), true);
            if (attributes == null || attributes.Length < 1)
            {
                return new TableAttribute() { Name = Name };
            }

            return (TableAttribute)attributes[0];
        }
        #endregion

        #region 获取“被选中的行数据”集合
        private List<object> GetSelectedRowDatas()
        {
            var selectedRowDatas = new List<object>();

            foreach (var row in RowModels)
            {
                if (row.Value.IsChecked)
                {
                    selectedRowDatas.Add(row.Value.RowData);
                }
            }

            return selectedRowDatas;
        }
        #endregion

        #region 判断数据是否正确
        public bool IsVerifyOK()
        {
            foreach (var rowModel in RowModels)
            {
                foreach (var itemModel in rowModel.Value.ItemModels)
                {
                    if (!itemModel.Value.IsVerifyOk)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        #endregion
        #endregion 【Functions】
    }
}
