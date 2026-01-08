using System.Windows.Input;
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
        #region [Private]
        /// <summary>
        /// 默认特性
        /// </summary>
        private TableAttribute _defaultAttribute = new TableAttribute();

        /// <summary>
        /// 集合改变事件参数
        /// </summary>
        private NotifyCollectionChangedEventArgs _args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
        #endregion [Private]

        /// <summary>
        /// 表格实例
        /// （由TableGrid传入，用于刷新）
        /// </summary>
        public TableGrid? _tableGrid;

        /// <summary>
        /// 是否自动刷新
        /// </summary>
        public bool _isAutoRefresh = true;

        /// <summary>
        /// 是否更新“行”的“是否选中”
        /// （由RowModel维护）
        /// </summary>
        public bool _isUpdateRowIsChecked = true;

        /// <summary>
        /// “旧行数据”集合
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
        public double Height { get => GetTableAttribute().Height; }
        public double MinHeight { get => GetTableAttribute().MinHeight; }
        public double MaxHeight { get => GetTableAttribute().MaxHeight; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get => GetTableAttribute().Name; }

        /// <summary>
        /// 接受换行
        /// </summary>
        public bool AcceptsReturn { get => GetTableAttribute().AcceptsReturn; }

        /// <summary>
        /// “被选中行”总数
        /// </summary>
        public int SelectedRowsCount { get => RowModels.Where(row => row.Value.IsChecked).Count(); }

        /// <summary>
        /// “被选中”的“行数据”集合
        /// </summary>
        public List<object> SelectedRowDatas { get => GetSelectedRowDatas(); }

        /// <summary>
        /// “被选中”的“行模型”集合
        /// </summary>
        public List<RowModel> SelectedRowModels { get => GetSelectedRowModels(); }
        #endregion [引用]

        #region [OneWay]
        /// <summary>
        /// 数据类型
        /// </summary>
        public Type DataType { get; private set; }

        /// <summary>
        /// 行数
        /// </summary>
        public int RowCount
        {
            get { return _RowCount; }
            private set { SetProperty(ref _RowCount, value); }
        }
        private int _RowCount;

        /// <summary>
        /// 列数
        /// </summary>
        public int ColCount
        {
            get { return _ColCount; }
            private set { SetProperty(ref _ColCount, value); }
        }
        private int _ColCount;

        /// <summary>
        /// 是否触发“项目源数据改变”委托
        /// </summary>
        public bool IsTriggerItemSourceChanged { get; private set; } = true;

        /// <summary>
        /// 是否更新“是否全选”
        /// </summary>
        public bool IsUpdateIsSelectAll { get; private set; } = true;
        #endregion [OneWay]

        #region [Other]
        #region “行数据”集合
        /// <summary>
        /// “行数据”集合
        /// （触发刷新）
        /// </summary>
        public ObservableCollection<object> RowDatas
        {
            get { return _RowDatas; }
            set
            {
                _RowDatas = value;
                _RowDatas.CollectionChanged -= RowDatas_CollectionChanged;
                _RowDatas.CollectionChanged += RowDatas_CollectionChanged;
                RowDatas_CollectionChanged(this, _args);
            }
        }
        private ObservableCollection<object> _RowDatas = new ObservableCollection<object>();
        #endregion

        #region “列头模型”集合
        /// <summary>
        /// “列头模型”集合
        /// （触发网格初始化）
        /// </summary>
        public ObservableCollection<HeaderModel> HeaderModels
        {
            get { return _HeaderModels; }
            set
            {
                _HeaderModels = value;
                _RowDatas.CollectionChanged += RowDatas_CollectionChanged;
                _HeaderModels.CollectionChanged += HeaderModels_CollectionChanged;
                HeaderModels_CollectionChanged(this, _args);
            }
        }
        private ObservableCollection<HeaderModel> _HeaderModels = new ObservableCollection<HeaderModel>();
        #endregion

        #region “行模型”集合
        /// <summary>
        /// “行模型”集合
        /// </summary>
        public Dictionary<object, RowModel> RowModels
        {
            get { return _RowModels; }
            set { _RowModels = value; }
        }
        private Dictionary<object, RowModel> _RowModels = new Dictionary<object, RowModel>();
        #endregion

        #region 是否“全选”
        /// <summary>
        /// 是否“全选”
        /// </summary>
        public bool IsSelectAll
        {
            get { return _IsSelectAll; }
            set { SetProperty(ref _IsSelectAll, value); }
        }
        private bool _IsSelectAll = false;
        #endregion

        #region 是否“显示复选框”
        /// <summary>
        /// 是否“显示复选框”
        /// </summary>
        public bool IsShowCheckBox
        {
            get { return _IsShowCheckBox; }
            set { SetProperty(ref _IsShowCheckBox, value); }
        }
        private bool _IsShowCheckBox = true;
        #endregion
        #endregion [Other]
        #endregion 【Properties】

        #region 【Ctor】
        public TableModel(Type dataType)
        {
            DataType = dataType;
            _RowDatas.CollectionChanged += RowDatas_CollectionChanged;
            _HeaderModels.CollectionChanged += HeaderModels_CollectionChanged;
        }
        #endregion 【Ctor】

        #region 【Events】
        #region [Private]
        #region “行数据”集合改变后
        private void RowDatas_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (!_isAutoRefresh) return;

            Refresh();

            _onRowDatasCollectionChanged?.Invoke(sender, e);
        }
        #endregion

        #region “列头模型”集合改变后
        private void HeaderModels_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            InitGrid();
        }
        #endregion
        #endregion [Private]
        #endregion 【Events】

        #region 【Commands】
        #region 选中
        public ICommand CheckedCommand { get => new DelegateCommand(Checked); }
        private void Checked()
        {
            UpdateItemIsChecked();
        }
        #endregion

        #region 未选中
        public ICommand UncheckedCommand { get => new DelegateCommand(Unchecked); }
        private void Unchecked()
        {
            UpdateItemIsChecked();
        }
        #endregion
        #endregion 【Commands】

        #region 【Functions】
        #region [Private]
        #region 初始化“网格”
        private void InitGrid()
        {
            RowCount = RowModels.Count + 1;
            ColCount = HeaderModels.Count + 1;
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

            InitHeaderModels();
            InitItemModels(HeaderModels);

            IsSelectAll = false;

            _onSelectedRowDatasChanged?.Invoke();
            _onRowDatasCollectionChanged?.Invoke(null, _args);

            IsTriggerItemSourceChanged = true;
        }
        #endregion

        #region 初始化“表头模型”集合
        private void InitHeaderModels()
        {
            if (HeaderModels.Count > 0) return;

            // 清空“表头集合”：
            HeaderModels.Clear();

            // 获取“属性集合”：
            var props = DataType.GetProperties();
            if (props == null)
            {
                LogHelper.Instance.Warning($"The {nameof(props)} is null!");
                return;
            }

            // 添加“表头”：
            var count = props.Length;
            for (int iCol = 0; iCol < count; iCol++)
            {
                var headerModel = new HeaderModel(this) { ColIndex = iCol + 1 };
                HeaderModels.Add(headerModel);
            }

            // 初始化“表头集合”：
            foreach (var headerModel in HeaderModels)
            {
                headerModel.InitDefaultConverter(this); // 初始化“默认转换器”
                headerModel.InitDefaultAttribute(this); // 初始化“默认特性”
                _onHeaderInit?.Invoke(headerModel); // 执行“初始化”委托
            }
        }
        #endregion

        #region 初始化“项目模型”集合
        private void InitItemModels(ObservableCollection<HeaderModel> headerModels)
        {
            RowModels.Clear();
            var rowCount = RowDatas.Count;
            var colCount = headerModels.Count;

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
                    var headerModel = headerModels[iCol];

                    var itemModel = new ItemModel(rowModel, headerModel) { RowIndex = iRow + 1, ColIndex = iCol + 1 };
                    _onItemInit?.Invoke(itemModel); // 执行“初始化”委托
                    rowModel.ItemModels.Add(headerModel, itemModel);
                }

                RowModels.Add(rowData, rowModel);
            }

            // 初始化“网格”:
            InitGrid();
        }
        #endregion

        #region 更新“项目是否选中”
        private void UpdateItemIsChecked()
        {
            if (!_isUpdateRowIsChecked) return;

            IsUpdateIsSelectAll = false;

            foreach (var rowModel in RowModels)
            {
                rowModel.Value.IsChecked = IsSelectAll;
            }

            _onSelectedRowDatasChanged?.Invoke();

            IsUpdateIsSelectAll = true;
        }
        #endregion
        #endregion [Private]

        #region [获取模型]
        #region 获取“表头模型”（属性名）
        public HeaderModel? GetHeaderModel(string propName)
        {
            return HeaderModels.FirstOrDefault(header => string.Equals(header.PropName, propName));
        }
        #endregion

        #region 获取“项目模型”（源数据，属性名）
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

        #region 获取“项目模型”（行号，属性名）
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

        #region 获取“行模型”（源数据）
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

        #region 获取“行模型”（行号）
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

        #region 刷新
        /// <summary>
        /// 刷新
        /// </summary>
        public void Refresh()
        {
            InitGrid();
            InitTableModel();
            _tableGrid?.Refresh();
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

                var rowModel = GetRowModel(iRow);
                if (rowModel == null)
                {
                    LogHelper.Instance.IsNull(nameof(rowModel));
                    continue;
                }

                rowModel.OldRowData = oldRowData;
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
                return _defaultAttribute;
            }

            return (TableAttribute)attributes[0];
        }
        #endregion

        #region 获取“被选中”的“行数据”集合
        private List<object> GetSelectedRowDatas()
        {
            var list = new List<object>();

            foreach (var row in RowModels)
            {
                if (row.Value.IsChecked)
                {
                    list.Add(row.Key);
                }
            }

            return list;
        }
        #endregion

        #region 获取“被选中”的“行模型”集合
        private List<RowModel> GetSelectedRowModels()
        {
            var list = new List<RowModel>();

            foreach (var row in RowModels)
            {
                if (row.Value.IsChecked)
                {
                    list.Add(row.Value);
                }
            }

            return list;
        }
        #endregion

        #region 判断“数据是否正确”
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
