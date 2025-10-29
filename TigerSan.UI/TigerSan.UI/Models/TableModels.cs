using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using TigerSan.CsvLog;
using TigerSan.UI.Helpers;
using TigerSan.UI.Controls;

namespace TigerSan.UI.Models
{
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

    #region 项目基类
    /// <summary>
    /// 项目模型模型
    /// </summary>
    public abstract class ItemModelBase : BindableBase
    {
        #region 【Properties】
        #region [引用]
        /// <summary>
        /// 项目类型
        /// </summary>
        public ItemType ItemType { get; private set; }

        /// <summary>
        /// 行号
        /// </summary>
        public int RowIndex { get => GetRowIndex(); }

        /// <summary>
        /// 列号
        /// </summary>
        public int ColIndex { get => GetColIndex(); }
        #endregion [引用]

        /// <summary>
        /// 背景
        /// </summary>
        public Brush Background
        {
            get { return _background; }
            set { SetProperty(ref _background, value); }
        }
        private Brush _background = Generic.Transparent;
        #endregion 【Properties】

        #region 【Ctor】
        protected ItemModelBase(ItemType type)
        {
            ItemType = type;
        }
        #endregion 【Ctor】

        #region 【Functions】
        /// <summary>
        /// 获取行号
        /// </summary>
        public abstract int GetRowIndex();

        /// <summary>
        /// 获取列号
        /// </summary>
        public abstract int GetColIndex();
        #endregion 【Functions】
    }
    #endregion

    #region 表头模型
    /// <summary>
    /// 表头模型
    /// </summary>
    public class HeaderModel : ItemModelBase
    {
        #region 【Fields】
        /// <summary>
        /// 表格模型
        /// </summary>
        public TableModel _tableModel;
        #endregion【Fields】

        #region 【Properties】
        #region [引用]
        #region [尺寸]
        public double? MinWidth { get => GetHeaderAttribute().MinWidth; }
        public double? MaxWidth { get => GetHeaderAttribute().MaxWidth; }
        public GridLength WidthGridLength { get => GetWidthGridLength(); }
        #endregion [尺寸]

        /// <summary>
        /// 属性名
        /// </summary>
        public string PropName { get => GetPropName(); }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get => GetHeaderAttribute().Title; }

        /// <summary>
        /// 是否只读
        /// </summary>
        public bool IsReadOnly { get => GetHeaderAttribute().IsReadOnly; }

        /// <summary>
        /// 是否允许排序
        /// </summary>
        public bool IsAllowSort { get => GetHeaderAttribute().IsAllowSort; }

        /// <summary>
        /// 是否允许调整尺寸
        /// </summary>
        public bool IsAllowResize { get => GetHeaderAttribute().IsAllowResize; }

        /// <summary>
        /// 排序模式
        /// </summary>
        public SortMode SortMode { get => GetHeaderAttribute().SortMode; }

        /// <summary>
        /// 文本对齐方式
        /// </summary>
        public TextAlignment TextAlignment { get => GetHeaderAttribute().TextAlignment; }
        #endregion [引用]

        /// <summary>
        /// 宽度
        /// </summary>
        public double? Width
        {
            get { return _width; }
            set { SetWidth(value); }
        }
        private double? _width;

        /// <summary>
        /// 转换器
        /// </summary>
        public IValueConverter? Converter { get; set; }

        /// <summary>
        /// 校验方法
        /// </summary>
        public Verification? Verification { get; set; } = Verifications.IsNotNull;
        #endregion 【Properties】

        #region 【Ctor】
        public HeaderModel(TableModel table) : base(ItemType.Header)
        {
            _tableModel = table;
        }
        #endregion 【Ctor】

        #region 【Functions】
        #region 获取行号
        public override int GetRowIndex()
        {
            return 0;
        }

        #endregion

        #region 获取列号
        public override int GetColIndex()
        {
            var index = _tableModel.HeaderModels.IndexOf(this);
            if (index < 0)
            {
                LogHelper.Instance.IsNotContain(nameof(_tableModel.HeaderModels), nameof(HeaderModel));
                return 1;
            }
            return index + 1;
        }
        #endregion

        #region 获取属性
        public PropertyInfo? GetProp()
        {
            int index = _tableModel.HeaderModels.IndexOf(this);
            if (index == -1)
            {
                LogHelper.Instance.Warning($"There is no HeaderModel named \"{Title}\" in HeaderModels!");
                return null;
            }

            return TypeHelper.GetProp(_tableModel.DataType, index);
        }
        #endregion

        #region 获取网格宽度
        public GridLength GetWidthGridLength()
        {
            return Width != null ? new GridLength(Width.Value) : Generic.DefaultGridWidth;
        }
        #endregion

        #region 获取属性名
        public string GetPropName()
        {
            // 获取属性：
            var prop = GetProp();
            if (prop == null)
            {
                LogHelper.Instance.Warning($"The {nameof(prop)} is null!");
                return string.Empty;
            }

            return prop.Name;
        }
        #endregion

        #region 获取表头特性
        public TableHeaderAttribute GetHeaderAttribute()
        {
            // 获取属性：
            var prop = GetProp();
            if (prop == null)
            {
                LogHelper.Instance.Warning($"The {nameof(prop)} is null!");
                return new TableHeaderAttribute();
            }

            // 获取属性名：
            var attributes = prop.GetCustomAttributes(typeof(TableHeaderAttribute), true);
            if (attributes == null || attributes.Length < 1)
            {
                LogHelper.Instance.Warning($"The {nameof(attributes)} is null!");
                return new TableHeaderAttribute();
            }

            return (TableHeaderAttribute)attributes[0];
        }
        #endregion

        #region 设置宽度
        public void SetWidth(double? width)
        {
            // 修改宽度：
            if (width == null || width == TableHeaderAttribute._notSet)
            {
                _width = null;
            }
            else if (width == Width)
            {
                return;
            }
            else if (MinWidth != null && width < MinWidth)
            {
                _width = MinWidth;
            }
            else if (MaxWidth != null && width > MaxWidth)
            {
                _width = MaxWidth;
            }
            else
            {
                _width = width;
            }

            // 修改网格列宽：
            var index = _tableModel.HeaderModels.IndexOf(this) + 1;
            if (index < 0 || _tableModel._colDefs.Count <= index)
            {
                LogHelper.Instance.Warning($"The {nameof(index)} out of range!");
                return;
            }

            var col = _tableModel._colDefs[index];
            var floatCol = _tableModel._floatColDefs[index];
            col.Width = floatCol.Width = WidthGridLength;
        }
        #endregion
        #endregion 【Functions】
    }
    #endregion

    #region 项目模型
    /// <summary>
    /// 项目模型
    /// </summary>
    public class ItemModel : ItemModelBase
    {
        #region 【Fields】
        /// <summary>
        /// 行模型
        /// </summary>
        public RowModel _rowModel;

        /// <summary>
        /// 表头模型
        /// </summary>
        public HeaderModel _headerModel;
        #endregion【Fields】

        #region 【Properties】
        #region [引用]
        /// <summary>
        /// 是否验证无误
        /// </summary>
        public bool IsVerifyOk { get => GetIsVerifyOk(); }

        /// <summary>
        /// 行数据
        /// </summary>
        public object RowData { get => _rowModel._rowData; }

        /// <summary>
        /// 修改前的行数据
        /// </summary>
        public object? OldRowData { get => _rowModel._oldRowData; }
        #endregion [引用]

        #region [OneWay]
        /// <summary>
        /// 修改前的源数据
        /// </summary>
        public object? OldSource
        {
            get { return _OldSource; }
            private set
            {
                SetProperty(ref _OldSource, value);
                IsModified = GetIsModified();
            }
        }
        private object? _OldSource;

        /// <summary>
        /// 是否只读
        /// </summary>
        public bool IsReadOnly
        {
            get { return _IsReadOnly; }
            private set { SetProperty(ref _IsReadOnly, value); }
        }
        private bool _IsReadOnly;

        /// <summary>
        /// 文本对齐方式
        /// </summary>
        public TextAlignment TextAlignment
        {
            get { return _textAlignment; }
            private set { SetProperty(ref _textAlignment, value); }
        }
        private TextAlignment _textAlignment;

        /// <summary>
        /// 项目状态
        /// </summary>
        public ItemState ItemState
        {
            get { return _ItemState; }
            private set { SetProperty(ref _ItemState, value); }
        }
        private ItemState _ItemState = ItemState.Normal;

        /// <summary>
        /// 是否被修改了
        /// </summary>
        public bool IsModified
        {
            get { return _IsModified; }
            private set
            {
                SetProperty(ref _IsModified, value);
                UpdateItemState();
            }
        }
        private bool _IsModified = false;
        #endregion [OneWay]

        /// <summary>
        /// 源数据
        /// </summary>
        public object? Source
        {
            get { return _Source; }
            set
            {
                SetSource(value);
                Target = GetTarget();
            }
        }
        private object? _Source;

        /// <summary>
        /// 目标数据
        /// </summary>
        public string Target
        {
            get { return _Target; }
            set
            {
                SetProperty(ref _Target, value);
                Target2Source();
            }
        }
        private string _Target = string.Empty;

        /// <summary>
        /// 是否鼠标悬浮
        /// </summary>
        public bool IsHover
        {
            get { return _IsHover; }
            set
            {
                SetProperty(ref _IsHover, value);
                UpdateItemState();
            }
        }
        private bool _IsHover = false;
        #endregion 【Properties】

        #region 【Ctor】
        public ItemModel(RowModel rowModel, HeaderModel header) : base(ItemType.Item)
        {
            _rowModel = rowModel;
            _headerModel = header;
            Source = GetSource();
            OldSource = GetOldSource();
            IsReadOnly = header.IsReadOnly;
            TextAlignment = header.TextAlignment;
            UpdateItemState();
        }
        #endregion 【Ctor】

        #region 【Functions】
        #region [Private]
        #region 获取“源数据”
        private object? GetSource()
        {
            // 获取RowData的类型：
            Type type = RowData.GetType();

            // 获取指定名称的属性：
            var property = type.GetProperty(_headerModel.PropName);
            if (property == null)
            {
                LogHelper.Instance.Warning($"There is no property named {_headerModel.PropName} in {nameof(RowData)}!");
                return null;
            }

            // 获取属性值：
            var value = property.GetValue(RowData);
            if (value == null)
            {
                LogHelper.Instance.Warning($"The value of the property named {_headerModel.PropName} in {nameof(RowData)} is null!");
                return null;
            }

            return value;
        }
        #endregion

        #region 设置“源数据”
        private void SetSource(object? value)
        {
            if (value == null)
            {
                SetProperty(ref _Source, _Source);
                return;
            }
            else if (value is string)
            {
                var str = ((string)value).Trim();
                SetRowData(str);
                SetProperty(ref _Source, str);
            }
            else
            {
                SetRowData(value);
                SetProperty(ref _Source, value);
            }

            IsModified = GetIsModified();
        }
        #endregion

        #region 将“目标数据”设置到“源数据”
        private void Target2Source()
        {
            try
            {
                // 转为“源数据”：
                object source;

                if (_headerModel.Converter != null)
                {
                    source = _headerModel.Converter.ConvertBack(Target, null, null, null);
                }
                else
                {
                    source = Target.Trim();
                }

                // 若转换失败，则回退：
                if (source == null)
                {
                    Target = GetTarget();
                    return;
                }

                // 修改“源数据”：
                SetSource(source);

                // 保证“目标数据”格式：
                var target = GetTarget();
                if (Target != target)
                {
                    Target = target;
                }
            }
            catch (Exception e)
            {
                LogHelper.Instance.Error(e.Message);
                Target = GetTarget();
            }
        }
        #endregion

        #region 获取“目标数据”
        private string GetTarget()
        {
            try
            {
                if (_headerModel.Converter == null)
                {
                    if (Source == null)
                    {
                        return string.Empty;
                    }
                    var str = Source.ToString();
                    return str == null ? string.Empty : str;
                }

                return (string)_headerModel.Converter.Convert(Source, null, null, null);
            }
            catch (Exception e)
            {
                LogHelper.Instance.Error(e.Message);
                return "error";
            }
        }
        #endregion

        #region 设置“行数据”
        private void SetRowData(object? source)
        {
            // 获取RowData的类型：
            Type type = RowData.GetType();

            // 获取指定名称的属性：
            var property = type.GetProperty(_headerModel.PropName);
            if (property == null)
            {
                LogHelper.Instance.Warning($"There is no property named {_headerModel.PropName} in {nameof(RowData)}!");
                return;
            }

            try
            {
                // 修改属性值：
                property.SetValue(RowData, source);
            }
            catch (Exception e)
            {
                LogHelper.Instance.Error(e.Message);
            }
        }
        #endregion

        #region 获取“修改前”的“源数据”
        private object? GetOldSource()
        {
            if (OldRowData == null) return null;

            // 获取OldRowData的类型：
            Type type = OldRowData.GetType();

            // 获取指定名称的属性：
            var property = type.GetProperty(_headerModel.PropName);
            if (property == null)
            {
                LogHelper.Instance.Warning($"There is no property named {_headerModel.PropName} in {nameof(OldRowData)}!");
                return null;
            }

            // 获取属性值：
            var value = property.GetValue(OldRowData);
            if (value == null)
            {
                LogHelper.Instance.Warning($"The value of the property named {_headerModel.PropName} in {nameof(OldRowData)} is null!");
                return null;
            }

            return value;
        }
        #endregion

        #region 获取是否验证无误
        private bool GetIsVerifyOk()
        {
            var verify = _headerModel.Verification;
            if (verify == null) return true;

            return verify(Source);
        }
        #endregion

        #region 获取是否被修改了
        private bool GetIsModified()
        {
            if (Source == null)
            {
                return false;
            }

            var type = Source.GetType();
            if (type == null)
            {
                return false;
            }

            return !TypeHelper.IsEqual(Source, OldSource);
        }
        #endregion

        #region 更新项目状态
        private void UpdateItemState()
        {
            if (!IsVerifyOk)
            {
                ItemState = ItemState.Error;
            }
            else if (IsModified)
            {
                ItemState = ItemState.Modified;
            }
            else if (IsHover)
            {
                ItemState = ItemState.Hover;
            }
            else
            {
                ItemState = ItemState.Normal;
            }
        }
        #endregion
        #endregion [Private]

        #region 获取行号
        public override int GetRowIndex()
        {
            var index = _headerModel._tableModel.RowDatas.IndexOf(_rowModel._rowData);
            if (index < 0)
            {
                LogHelper.Instance.IsNotContain(nameof(_headerModel._tableModel.RowDatas), nameof(_rowModel._rowData));
                return 0;
            }
            return index + 1;
        }
        #endregion

        #region 获取列号
        public override int GetColIndex()
        {
            return _headerModel.ColIndex;
        }
        #endregion
        #endregion 【Functions】
    }
    #endregion

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

        /// <summary>
        /// 行数据
        /// </summary>
        public object _rowData;

        /// <summary>
        /// 修改前的行数据
        /// </summary>
        public object? _oldRowData { get; set; }
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
        #endregion 【Properties】

        #region 【Ctor】
        public RowModel(
            TableModel tableModel,
            object rowData,
            object? oldRowData)
        {
            _rowData = rowData;
            _oldRowData = oldRowData;
            _tableModel = tableModel;
        }
        #endregion 【Ctor】

        #region 【Functions】
        #region 获取行号
        private int GetRowIndex()
        {
            var index = _tableModel.RowDatas.IndexOf(_rowData);
            if (index < 0)
            {
                LogHelper.Instance.IsNotContain(nameof(_tableModel.RowDatas), nameof(_rowData));
                return 0;
            }
            return index + 1;
        }
        #endregion
        #endregion 【Functions】
    }
    #endregion

    #region 表格模型
    /// <summary>
    /// 表格模型
    /// </summary>
    public class TableModel : BindableBase
    {
        #region 【Fields】
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
        /// 修改前的行数据集合
        /// </summary>
        public List<object> _oldRowDatas = new List<object>();

        /// <summary>
        /// 被选中的行数据集合改变时
        /// </summary>
        public Action? _onSelectedRowDatasChanged;

        /// <summary>
        /// 行数据集合改变委托
        /// </summary>
        public NotifyCollectionChangedEventHandler? _rowDatasCollectionChanged;
        #endregion

        #region 【Properties】
        #region [引用]
        public double? Height { get => GetTableAttribute().Height; }
        public double? MinHeight { get => GetTableAttribute().MinHeight; }
        public double? MaxHeight { get => GetTableAttribute().MaxHeight; }
        public GridLength HeightGridLength { get => Height != null ? new GridLength(Height.Value) : Generic.DefaultGridHeight; }
        #endregion [引用]

        #region [OneWay]
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

            InitGrid();
            InitHeaderModels();
            InitItemModels();
        }
        #endregion 【Ctor】

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
                    Width = Generic.DefaultGridWidth,
                    MinWidth = attr.MinWidth,
                    MaxWidth = attr.MaxWidth
                };
                _colDefs.Add(col);

                var floatCol = new ColumnDefinition()
                {
                    Width = Generic.DefaultGridWidth,
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
        }
        #endregion

        #region 初始化项目模型集合
        private void InitItemModels()
        {
            RowModels.Clear();
            var rowCount = RowDatas.Count;
            var colCount = HeaderModels.Count;

            // 更新“修改前的行数据”：
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
                    rowModel.ItemModels.Add(headerModel, itemModel);
                }

                RowModels.Add(rowData, rowModel);
            }
        }
        #endregion

        #region 行数据集合改变
        private void RowDatas_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            InitGrid();
            InitItemModels();

            if (!_isAutoRefresh) return;

            _rowDatasCollectionChanged?.Invoke(sender, e);
        }
        #endregion
        #endregion [Private]

        #region 数据是否正确
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

        #region 刷新表格
        public void Refresh()
        {
            InitGrid();
            InitItemModels();
            IsSelectAll = false;
            _onSelectedRowDatasChanged?.Invoke();
            _rowDatasCollectionChanged?.Invoke(null, _args);
        }
        #endregion

        #region 获取表头模型
        /// <summary>
        /// 获取表头模型
        /// </summary>
        /// <param name="propName">属性名</param>
        /// <returns></returns>
        public HeaderModel? GetHeaderModel(string propName)
        {
            return HeaderModels.FirstOrDefault(header => string.Equals(header.PropName, propName));
        }
        #endregion

        #region 获取项目模型
        /// <summary>
        /// 获取项目模型
        /// </summary>
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

        #region 获取项目模型
        /// <summary>
        /// 获取项目模型
        /// </summary>
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

        #region 获取行模型
        /// <summary>
        /// 获取行模型
        /// </summary>
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

        #region 获取行模型
        /// <summary>
        /// 获取行模型
        /// </summary>
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

        #region 获取表格特性
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

        #region 获取被选中的行数据集合
        private List<object> GetSelectedRowDatas()
        {
            var selectedRowDatas = new List<object>();

            foreach (var row in RowModels)
            {
                if (row.Value.IsChecked)
                {
                    selectedRowDatas.Add(row.Value._rowData);
                }
            }

            return selectedRowDatas;
        }
        #endregion

        #region 更新修改前的行数据集合
        public void UpdateOldRowDatas()
        {
            // 更新“修改前的行数据”：
            var rowDatas = TypeHelper.DeepCopyList(RowDatas);
            if (rowDatas == null)
            {
                LogHelper.Instance.Warning($"The {nameof(rowDatas)} is null!");
                return;
            }
            _oldRowDatas = rowDatas;

            // 更新“项目模型”的“修改前的行数据”：
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

                    rowModel._oldRowData = oldRowData;
                }
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
