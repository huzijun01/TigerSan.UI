using System.Windows;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Media;
using TigerSan.CsvLog;
using TigerSan.UI.Helpers;

namespace TigerSan.UI.Models
{
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

        /// <summary>
        /// 菜单宽度
        /// </summary>
        public double? _menuWidth { get; set; }

        /// <summary>
        /// “菜单数据”集合
        /// </summary>
        public List<object>? _menuDatas { get; set; }

        /// <summary>
        /// 默认转换器
        /// </summary>
        public IValueConverter? _defaultConverter { get; set; }

        /// <summary>
        /// 默认特性
        /// </summary>
        public TableHeaderAttribute _defaultAttribute { get; set; } = new TableHeaderAttribute();
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
            get { return _Width; }
            set { SetWidth(value); }
        }
        private double? _Width;

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
                LogHelper.Instance.Warning($"There is no HeaderModel in HeaderModels!");
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
                return _defaultAttribute;
            }

            // 获取属性名：
            var attributes = prop.GetCustomAttributes(typeof(TableHeaderAttribute), true);
            if (attributes == null || attributes.Length < 1)
            {
                return _defaultAttribute;
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
                _Width = null;
            }
            else if (width == Width)
            {
                return;
            }
            else if (MinWidth != null && width < MinWidth)
            {
                _Width = MinWidth;
            }
            else if (MaxWidth != null && width > MaxWidth)
            {
                _Width = MaxWidth;
            }
            else
            {
                _Width = width;
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

        #region 初始化“默认转换器”
        public void InitDefaultConverter(TableModel table)
        {
            var prop = GetProp();
            if (prop == null)
            {
                LogHelper.Instance.IsNull(nameof(prop));
                return;
            }

            if (prop.PropertyType == typeof(bool))
            {
                _defaultConverter = new Converters.Bool2StringConverter();
            }
            else if (prop.PropertyType == typeof(DateTime))
            {
                _defaultConverter = new Converters.DateTime2StringConverter();
            }
            else if (prop.PropertyType == typeof(double))
            {
                _defaultConverter = new Converters.Double2StringConverter();
            }
            else if (prop.PropertyType == typeof(int))
            {
                _defaultConverter = new Converters.Int2StringConverter();
            }
            else if (prop.PropertyType == typeof(object))
            {
                _defaultConverter = new Converters.Object2StringConverter();
            }
            else
            {
                _defaultConverter = null;
            }
        }
        #endregion

        #region 初始化“默认特性”
        public void InitDefaultAttribute(TableModel table)
        {
            var prop = GetProp();
            if (prop == null)
            {
                LogHelper.Instance.IsNull(nameof(prop));
                return;
            }

            _defaultAttribute.Title = prop.Name;
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
        /// 是否触发“项目源数据改变”委托
        /// </summary>
        private bool _isTriggerItemSourceChanged = true;

        /// <summary>
        /// 行模型
        /// </summary>
        public RowModel _rowModel;

        /// <summary>
        /// 表头模型
        /// </summary>
        public HeaderModel _headerModel;

        /// <summary>
        /// “目标数据”改变委托（控件内使用）
        /// </summary>
        public Action? _onTargetChanged;
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
        public object RowData { get => _rowModel.RowData; }

        /// <summary>
        /// 旧行数据
        /// </summary>
        public object? OldRowData { get => _rowModel.OldRowData; }
        #endregion [引用]

        #region [OneWay]
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
            private set { SetProperty(ref _IsModified, value); }
        }
        private bool _IsModified = false;
        #endregion [OneWay]

        #region [更新状态]
        /// <summary>
        /// 旧源数据
        /// </summary>
        public object? OldSource
        {
            get { return _OldSource; }
            private set
            {
                SetProperty(ref _OldSource, value);
                UpdateItemState();
            }
        }
        private object? _OldSource;

        /// <summary>
        /// 源数据
        /// </summary>
        public object? Source
        {
            get { return _Source; }
            set
            {
                SetSource(value);
                UpdateItemState();
                RaiseTargetChanged();
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
                SetTarget(value);
                UpdateItemState();
                RaiseTargetChanged();
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
        #endregion [更新状态]
        #endregion 【Properties】

        #region 【Ctor】
        public ItemModel(RowModel rowModel, HeaderModel header) : base(ItemType.Item)
        {
            _isTriggerItemSourceChanged = false;

            _rowModel = rowModel;
            _headerModel = header;
            IsReadOnly = header.IsReadOnly;
            TextAlignment = header.TextAlignment;
            LoadSource(true);
            UpdateItemState();
            UpdateOldSource();

            _isTriggerItemSourceChanged = true;
        }
        #endregion 【Ctor】

        #region 【Functions】
        #region [Private]
        #region 设置“源数据”
        private void SetSource(object? value)
        {
            if (value == null)
            {
                SetProperty(ref _Source, _Source);
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

            _Target = GetTarget();
            SetProperty(ref _Target, _Target);
            RaiseTargetChanged();

            if (_isTriggerItemSourceChanged
                && _headerModel._tableModel.IsTriggerItemSourceChanged)
            {
                _headerModel._tableModel._onItemSourceChanged?.Invoke(this);
            }
        }
        #endregion

        #region 设置“目标数据”
        private void SetTarget(string value)
        {
            if (string.Equals(_Target, value)) return;

            SetProperty(ref _Target, value);

            try
            {
                // 转为“源数据”：
                object source;

                if (_headerModel.Converter != null)
                {
                    source = _headerModel.Converter.ConvertBack(Target, null, null, null);
                }
                if (_headerModel._defaultConverter != null)
                {
                    source = _headerModel._defaultConverter.ConvertBack(Target, null, null, null);
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
                if (_headerModel.Converter != null)
                {
                    return (string)_headerModel.Converter.Convert(Source, null, null, null);
                }
                if (_headerModel._defaultConverter != null)
                {
                    return (string)_headerModel._defaultConverter.Convert(Source, null, null, null);
                }
                else
                {
                    if (Source == null)
                    {
                        return string.Empty;
                    }
                    var str = Source.ToString();
                    return str == null ? string.Empty : str;
                }
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

        #region 获取“是否验证无误”
        private bool GetIsVerifyOk()
        {
            var verify = _headerModel.Verification;
            if (verify == null) return true;

            return verify(Source);
        }
        #endregion

        #region 更新“是否被修改”
        private void UpdateIsModified()
        {
            if (Source == null)
            {
                IsModified = false;
                return;
            }

            var type = Source.GetType();
            if (type == null)
            {
                IsModified = false;
                return;
            }

            IsModified = !TypeHelper.IsEqual(Source, OldSource);
        }
        #endregion

        #region 触发“目标数据”改变
        protected void RaiseTargetChanged()
        {
            _onTargetChanged?.Invoke();
        }
        #endregion
        #endregion [Private]

        #region 获取“行号”
        public override int GetRowIndex()
        {
            var index = _headerModel._tableModel.RowDatas.IndexOf(_rowModel.RowData);
            if (index < 0)
            {
                LogHelper.Instance.IsNotContain(nameof(_headerModel._tableModel.RowDatas), nameof(_rowModel.RowData));
                return 0;
            }
            return index + 1;
        }
        #endregion

        #region 获取“列号”
        public override int GetColIndex()
        {
            return _headerModel.ColIndex;
        }
        #endregion

        #region 加载“源数据”
        public void LoadSource(bool updateUI)
        {
            // 获取RowData的类型：
            Type type = RowData.GetType();

            // 获取指定名称的属性：
            var property = type.GetProperty(_headerModel.PropName);
            if (property == null)
            {
                LogHelper.Instance.Warning($"There is no property named {_headerModel.PropName} in {nameof(RowData)}!");
                _Source = null;
            }
            else
            {
                // 获取属性值：
                var value = property.GetValue(RowData);
                if (value == null)
                {
                    LogHelper.Instance.Warning($"The value of the property named {_headerModel.PropName} in {nameof(RowData)} is null!");
                }
                _Source = value;
            }

            if (updateUI)
            {
                Source = _Source;
            }
        }
        #endregion

        #region 更新“旧源数据”
        public void UpdateOldSource()
        {
            if (OldRowData == null)
            {
                OldSource = null;
                return;
            }

            // 获取OldRowData的类型：
            Type type = OldRowData.GetType();

            // 获取指定名称的属性：
            var property = type.GetProperty(_headerModel.PropName);
            if (property == null)
            {
                LogHelper.Instance.Warning($"There is no property named {_headerModel.PropName} in {nameof(OldRowData)}!");
                OldSource = null;
                return;
            }

            // 获取属性值：
            var value = property.GetValue(OldRowData);
            if (value == null)
            {
                LogHelper.Instance.Warning($"The value of the property named {_headerModel.PropName} in {nameof(OldRowData)} is null!");
            }

            OldSource = value;
        }
        #endregion

        #region 更新“项目状态”
        public void UpdateItemState()
        {
            LoadSource(false);
            UpdateIsModified();

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
        #endregion 【Functions】
    }
    #endregion
}
