using System.Collections.ObjectModel;
using System.Collections.Specialized;
using TigerSan.CsvLog;
using TigerSan.UI.Converters;

namespace TigerSan.UI.Models
{
    #region 按钮模型
    /// <summary>
    /// 按钮模型
    /// </summary>
    public class PaginationButtonModel : BindableBase
    {
        #region 【Fields】
        /// <summary>
        /// 默认文本
        /// </summary>
        public static readonly string _defaultText = "1";

        /// <summary>
        /// “点击后”委托
        /// </summary>
        public Action<PaginationButtonModel>? _onClicked { get; set; }

        /// <summary>
        /// “选中后”委托
        /// </summary>
        public Action<PaginationButtonModel>? _onChecked { get; set; }

        /// <summary>
        /// “选中后”委托
        /// （由“Pagination”传入）
        /// </summary>
        public Action<PaginationButtonModel>? _onCheckedInternal { get; set; }
        #endregion 【Fields】

        #region 【Properties】
        #region [OneWay]
        #endregion [OneWay]

        #region [引用]
        /// <summary>
        /// 数字
        /// </summary>
        public int Num
        {
            get => Int2StringConverter.GetInt(Text);
            set => Text = value.ToString();
        }
        #endregion [引用]

        #region 文本
        /// <summary>
        /// 文本
        /// </summary>
        public string Text
        {
            get { return _Text; }
            set { SetProperty(ref _Text, value); }
        }
        private string _Text = _defaultText;
        #endregion

        #region 悬浮文本
        /// <summary>
        /// 悬浮文本
        /// </summary>
        public string HoverText
        {
            get { return _HoverText; }
            set { SetProperty(ref _HoverText, value); }
        }
        private string _HoverText = string.Empty;
        #endregion

        #region 是否“被选中”
        /// <summary>
        /// 是否“被选中”
        /// </summary>
        public bool IsSelected
        {
            get { return _IsSelected; }
            set { SetProperty(ref _IsSelected, value); }
        }
        private bool _IsSelected = false;
        #endregion

        #region 是否“显示”
        /// <summary>
        /// 是否“显示”
        /// </summary>
        public bool IsShow
        {
            get { return _IsShow; }
            set { SetProperty(ref _IsShow, value); }
        }
        private bool _IsShow = true;
        #endregion

        #region 是否“启用”
        /// <summary>
        /// 是否“启用”
        /// </summary>
        public bool IsEnabled
        {
            get { return _IsEnabled; }
            set { SetProperty(ref _IsEnabled, value); }
        }
        private bool _IsEnabled = true;
        #endregion
        #endregion 【Properties】

        #region 【Functions】
        #region [Private]
        #endregion [Private]
        #endregion 【Functions】
    }
    #endregion

    /// <summary>
    /// 分页模型
    /// </summary>
    public class PaginationModel : BindableBase
    {
        #region 【Fields】
        /// <summary>
        /// 是否“初始化”
        /// </summary>
        private bool _isInit = true;

        /// <summary>
        /// 默认“页大小”
        /// </summary>
        private static readonly int Default_Page_Size = 10;
        #endregion 【Fields】

        #region 【Properties】
        #region [OneWay]
        #region “页大小模型”集合
        /// <summary>
        /// “页大小模型”集合
        /// （由“PaginationModel”维护）
        /// </summary>
        public ObservableCollection<MenuItemModel> PageSizeModels
        {
            get { return _PageSizeModels; }
            private set { SetProperty(ref _PageSizeModels, value); }
        }
        private ObservableCollection<MenuItemModel> _PageSizeModels = new ObservableCollection<MenuItemModel>();
        #endregion

        #region 上一页“按钮模型”
        /// <summary>
        /// 上一页“按钮模型”
        /// （由“PaginationModel”维护）
        /// </summary>
        public PaginationButtonModel PrePageButtonModel
        {
            get { return _PreButtonModel; }
            private set { SetProperty(ref _PreButtonModel, value); }
        }
        private PaginationButtonModel _PreButtonModel = new PaginationButtonModel();
        #endregion

        #region 下一页“按钮模型”
        /// <summary>
        /// 下一页“按钮模型”
        /// （由“PaginationModel”维护）
        /// </summary>
        public PaginationButtonModel NextPageButtonModel
        {
            get { return _NextButtonModel; }
            private set { SetProperty(ref _NextButtonModel, value); }
        }
        private PaginationButtonModel _NextButtonModel = new PaginationButtonModel();
        #endregion

        #region 最小值“按钮模型”
        /// <summary>
        /// 最小值“按钮模型”
        /// （由“PaginationModel”维护）
        /// </summary>
        public PaginationButtonModel MinButtonModel
        {
            get { return _MinButtonModel; }
            private set { SetProperty(ref _MinButtonModel, value); }
        }
        private PaginationButtonModel _MinButtonModel = new PaginationButtonModel();
        #endregion

        #region 最大值“按钮模型”
        /// <summary>
        /// 最大值“按钮模型”
        /// （由“PaginationModel”维护）
        /// </summary>
        public PaginationButtonModel MaxButtonModel
        {
            get { return _MaxButtonModel; }
            private set { SetProperty(ref _MaxButtonModel, value); }
        }
        private PaginationButtonModel _MaxButtonModel = new PaginationButtonModel();
        #endregion

        #region 上一行“按钮模型”
        /// <summary>
        /// 上一行“按钮模型”
        /// （由“PaginationModel”维护）
        /// </summary>
        public PaginationButtonModel PreRowButtonModel
        {
            get { return _PreRowButtonModel; }
            private set { SetProperty(ref _PreRowButtonModel, value); }
        }
        private PaginationButtonModel _PreRowButtonModel = new PaginationButtonModel();
        #endregion

        #region 下一行“按钮模型”
        /// <summary>
        /// 下一行“按钮模型”
        /// （由“PaginationModel”维护）
        /// </summary>
        public PaginationButtonModel NextRowButtonModel
        {
            get { return _NextRowButtonModel; }
            private set { SetProperty(ref _NextRowButtonModel, value); }
        }
        private PaginationButtonModel _NextRowButtonModel = new PaginationButtonModel();
        #endregion

        #region “按钮模型”集合
        /// <summary>
        /// “按钮模型”集合
        /// （由“PaginationModel”维护）
        /// </summary>
        public ObservableCollection<PaginationButtonModel> ButtonModels
        {
            get { return _ButtonModels; }
            private set { SetProperty(ref _ButtonModels, value); }
        }
        private ObservableCollection<PaginationButtonModel> _ButtonModels = new ObservableCollection<PaginationButtonModel>();
        #endregion
        #endregion [OneWay]

        #region [引用]
        /// <summary>
        /// 总函数
        /// </summary>
        public int RowCount { get => GetRowCount(); }

        /// <summary>
        /// 总函数
        /// </summary>
        public int PageCount { get => GetPageCount(); }

        /// <summary>
        /// 所选“按钮模型”
        /// </summary>
        public PaginationButtonModel SelectedButtonModel { get => ButtonModels.FirstOrDefault(b => Equals(b.Num, SelectedNum)) ?? new PaginationButtonModel(); }
        #endregion [引用]

        #region [初始化]
        #region 总数
        /// <summary>
        /// 总数（非负）
        /// </summary>
        public int Count
        {
            get { return _Count; }
            set
            {
                SetProperty(ref _Count, value);

                if (_isInit)
                {
                    Init();
                }
            }
        }
        private int _Count = 0;
        #endregion

        #region 页大小
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize
        {
            get { return _PageSize; }
            set
            {
                SetProperty(ref _PageSize, value);

                if (_isInit)
                {
                    Init();
                }

                RaiseOnChecked();
            }
        }
        private int _PageSize = Default_Page_Size;
        #endregion

        #region 所选数字
        /// <summary>
        /// 所选数字
        /// （1 ~ Count）
        /// </summary>
        public int SelectedNum
        {
            get { return _SelectedNum; }
            set
            {
                SetProperty(ref _SelectedNum, value);

                if (_isInit)
                {
                    Init();
                }

                PageText = value.ToString();

                var buttonModel = ButtonModels.FirstOrDefault(b => Equals(b.Num, value));
                OnChecked?.Invoke(buttonModel ?? new PaginationButtonModel());
            }
        }
        private int _SelectedNum = 1;
        #endregion

        #region 最大显示个数
        /// <summary>
        /// 最大显示个数
        /// （非负）
        /// </summary>
        public int MaxShowPageCount
        {
            get { return _MaxShowPageCount; }
            set
            {
                SetProperty(ref _MaxShowPageCount, value);

                if (_isInit)
                {
                    Init();
                }
            }
        }
        private int _MaxShowPageCount = 0;
        #endregion
        #endregion [初始化]

        #region 页文本
        /// <summary>
        /// 页文本
        /// </summary>
        public string PageText
        {
            get { return _PageText; }
            set { SetProperty(ref _PageText, value); }
        }
        private string _PageText = string.Empty;
        #endregion

        #region “页大小”集合
        /// <summary>
        /// “页大小”集合
        /// （不为空，大于0）
        /// </summary>
        public ObservableCollection<int> PageSizes
        {
            get { return _PageSizes; }
            set
            {
                SetProperty(ref _PageSizes, value);

                PageSizes_CollectionChanged();
                PageSizes.CollectionChanged -= PageSizes_CollectionChanged;
                PageSizes.CollectionChanged += PageSizes_CollectionChanged;
            }
        }
        private ObservableCollection<int> _PageSizes = new ObservableCollection<int>();
        #endregion

        #region “页大小”宽度
        /// <summary>
        /// “页大小”宽度
        /// </summary>
        public double PageSizeWidth
        {
            get { return _PageSizeWidth; }
            set { SetProperty(ref _PageSizeWidth, value); }
        }
        private double _PageSizeWidth = 100;
        #endregion

        #region “点击后”委托
        /// <summary>
        /// “点击后”委托
        /// </summary>
        public Action<PaginationButtonModel>? OnClicked
        {
            get { return _OnClicked; }
            set
            {
                SetProperty(ref _OnClicked, value);
                InitEvents();
            }
        }
        private Action<PaginationButtonModel>? _OnClicked;
        #endregion

        #region “选中后”委托
        /// <summary>
        /// “选中后”委托
        /// </summary>
        public Action<PaginationButtonModel>? OnChecked
        {
            get { return _OnChecked; }
            set
            {
                SetProperty(ref _OnChecked, value);
                InitEvents();
            }
        }
        private Action<PaginationButtonModel>? _OnChecked;
        #endregion

        #region 是否显示“总数”
        /// <summary>
        /// 是否显示“总数”
        /// </summary>
        public bool IsShowCount
        {
            get { return _IsShowCount; }
            set { SetProperty(ref _IsShowCount, value); }
        }
        private bool _IsShowCount = true;
        #endregion

        #region 是否显示“页大小”
        /// <summary>
        /// 是否显示“页大小”
        /// </summary>
        public bool IsShowPageSize
        {
            get { return _IsShowPageSize; }
            set { SetProperty(ref _IsShowPageSize, value); }
        }
        private bool _IsShowPageSize = true;
        #endregion

        #region 是否显示“页文本框”
        /// <summary>
        /// 是否显示“页文本框”
        /// </summary>
        public bool IsShowPageTextBox
        {
            get { return _IsShowPageTextBox; }
            set { SetProperty(ref _IsShowPageTextBox, value); }
        }
        private bool _IsShowPageTextBox = true;
        #endregion
        #endregion 【Properties】

        #region 【Ctor】
        public PaginationModel()
        {
            PageSizes.CollectionChanged += PageSizes_CollectionChanged;
            Init();
            InitStableButtons();
        }
        #endregion 【Ctor】

        #region 【Events】
        #region “普通按钮”被选中
        private void NormalButton_OnCheckedInternal(PaginationButtonModel model)
        {
            SelectedNum = model.Num;
        }
        #endregion

        #region “上一页按钮”被选中
        private void PrePageButton_OnChecked(PaginationButtonModel model)
        {
            --SelectedNum;
        }
        #endregion

        #region “下一页按钮”被选中
        private void NextPageButton_OnChecked(PaginationButtonModel model)
        {
            ++SelectedNum;
        }
        #endregion

        #region “下一行按钮”被选中
        private void PreRowButton_OnChecked(PaginationButtonModel model)
        {
            var currentRow = GetCurrentRow();
            --currentRow;
            // 起始、结束：
            int start = 0;
            int end = 0;
            GetStartAndEnd(currentRow, ref start, ref end);
            SelectedNum = end;
        }
        #endregion

        #region “下一行按钮”被选中
        private void NextRowButton_OnChecked(PaginationButtonModel model)
        {
            var currentRow = GetCurrentRow();
            ++currentRow;
            // 起始、结束：
            int start = 0;
            int end = 0;
            GetStartAndEnd(currentRow, ref start, ref end);
            SelectedNum = start;
        }
        #endregion

        #region “页大小集合”改变后
        private void PageSizes_CollectionChanged(object? sender = null, NotifyCollectionChangedEventArgs? e = null)
        {
            PageSizeModels.Clear();

            foreach (var size in PageSizes)
            {
                PageSizeModels.Add(new MenuItemModel() { Source = size });
            }
        }
        #endregion
        #endregion 【Events】

        #region 【Functions】
        #region [Private]
        #region 初始化
        private void Init()
        {
            CorrectMistakes();

            // 行数：
            var rowCount = GetRowCount();
            var pageCount = GetPageCount();

            // 当前行号：
            var currentRow = GetCurrentRow();

            // 起始、结束：
            int start = 0;
            int end = 0;
            GetStartAndEnd(currentRow, ref start, ref end);

            // 页按钮：
            PrePageButtonModel.IsEnabled = SelectedNum > 1;
            NextPageButtonModel.IsEnabled = SelectedNum < pageCount && pageCount > 0;

            // 行按钮：
            PreRowButtonModel.IsShow = currentRow > 1;
            NextRowButtonModel.IsShow = currentRow < rowCount;

            // 最值按钮：
            MinButtonModel.IsShow = PreRowButtonModel.IsShow;
            MaxButtonModel.IsShow = NextRowButtonModel.IsShow;
            MaxButtonModel.Num = pageCount;

            InitButtonModels(start, end);
            UpdateIsSelected();
        }
        #endregion

        #region 初始化“固定按钮”
        private void InitStableButtons()
        {
            // 最值按钮：
            MinButtonModel.Num = 1;
            MinButtonModel._onChecked = NormalButton_OnCheckedInternal;

            MaxButtonModel._onChecked = NormalButton_OnCheckedInternal;

            // 行按钮：
            PreRowButtonModel.Text = "···";
            PreRowButtonModel.HoverText = "◁";
            PreRowButtonModel._onChecked = PreRowButton_OnChecked;

            NextRowButtonModel.Text = "···";
            NextRowButtonModel.HoverText = "▷";
            NextRowButtonModel._onChecked = NextRowButton_OnChecked;

            // 页按钮：
            PrePageButtonModel.Text = "<";
            PrePageButtonModel._onChecked = PrePageButton_OnChecked;

            NextPageButtonModel.Text = ">";
            NextPageButtonModel._onChecked = NextPageButton_OnChecked;
        }
        #endregion

        #region 初始化“按钮模型”
        private void InitButtonModels(int start, int end)
        {
            ButtonModels.Clear();

            if (start < 0 || end < 0 || start > end)
            {
                LogHelper.Instance.IsOutOfRange($"{nameof(start)} or {nameof(end)}");
                return;
            }

            ButtonModels.Add(PrePageButtonModel);
            ButtonModels.Add(MinButtonModel);
            ButtonModels.Add(PreRowButtonModel);

            for (int index = start; index <= end; index++)
            {
                ButtonModels.Add(new PaginationButtonModel()
                {
                    Num = index,
                    _onClicked = OnClicked,
                    _onCheckedInternal = NormalButton_OnCheckedInternal
                });
            }

            ButtonModels.Add(NextRowButtonModel);
            ButtonModels.Add(MaxButtonModel);
            ButtonModels.Add(NextPageButtonModel);
        }
        #endregion

        #region 初始化“事件”
        private void InitEvents()
        {
            foreach (var buttonModel in ButtonModels)
            {
                if (buttonModel.Num < 1) continue;
                buttonModel._onClicked = OnClicked;
            }
        }
        #endregion

        #region 纠正错误
        private void CorrectMistakes()
        {
            _isInit = false;

            if (Count < 1) // 非负
            {
                Count = 0;
            }

            if (SelectedNum < 1 || SelectedNum > PageCount) // 1 ~ Count
            {
                SelectedNum = 1;
            }

            if (MaxShowPageCount < 1) // 非负
            {
                MaxShowPageCount = 7;
            }

            if (PageSizes.Count < 1 || PageSizes.Any(size => size < 1)) // 不为空，大于0
            {
                PageSizes.Clear();
                PageSizes.AddRange([10, 20, 30, 50, 100]);
            }

            if (PageSize < 1) // 非负
            {
                PageSize = GetPageSize(PageSizes[0]);
            }

            _isInit = true;
        }
        #endregion

        #region 更新“是否被选中”
        private void UpdateIsSelected()
        {
            _isInit = false;

            foreach (var buttonModel in ButtonModels)
            {
                if (Equals(buttonModel.Num, SelectedNum))
                {
                    if (!Equals(buttonModel.IsSelected, true))
                    {
                        buttonModel.IsSelected = true;
                    }
                }
                else
                {
                    if (!Equals(buttonModel.IsSelected, false))
                    {
                        buttonModel.IsSelected = false;
                    }
                }
            }

            _isInit = true;
        }
        #endregion

        #region 获取“页大小”
        private int GetPageSize(object? size)
        {
            return size == null ? Default_Page_Size : (int)size;
        }
        #endregion

        #region 获取“当前行号”
        private int GetCurrentRow()
        {
            var currentRow = SelectedNum / MaxShowPageCount;
            if (SelectedNum % MaxShowPageCount != 0)
            {
                ++currentRow;
            }
            return currentRow;
        }
        #endregion

        #region 获取“起始”和“结束”值
        private void GetStartAndEnd(int row, ref int start, ref int end)
        {
            // 行数：
            var rowCount = GetRowCount();
            if (rowCount < 1)
            {
                start = end = 1;
                return;
            }

            // 起始：
            var offset = (row - 1) * MaxShowPageCount;
            start = offset + 1;

            // 剩下的页数：
            var remainingPageCount = PageCount - start + 1;

            // 结束：
            if (remainingPageCount / MaxShowPageCount > 0)
            {
                end = start + MaxShowPageCount - 1;
            }
            else
            {
                end = PageCount;
            }
        }
        #endregion
        #endregion [Private]

        #region 获取“行数”
        public int GetRowCount()
        {
            var rowCount = PageCount / MaxShowPageCount;
            if (PageCount % MaxShowPageCount != 0)
            {
                ++rowCount;
            }
            return rowCount;
        }
        #endregion

        #region 获取“页数”
        public int GetPageCount()
        {
            var rowCount = Count / PageSize;
            if (Count % PageSize != 0)
            {
                ++rowCount;
            }
            return rowCount;
        }
        #endregion

        #region 获取“页数据”集合
        public IEnumerable<TSource> GetPageDatas<TSource>(IEnumerable<TSource> datas, int page)
        {
            return datas
                .Skip((page - 1) * PageSize)
                .Take(PageSize);
        }
        #endregion

        #region 执行“选中后”委托
        public void RaiseOnChecked()
        {
            OnChecked?.Invoke(SelectedButtonModel);
        }
        #endregion

        #region 跳转到“指定页”
        public void GoToPage()
        {
            if (int.TryParse(PageText, out int pageNum))
            {
                if (pageNum < 1)
                {
                    pageNum = 1;
                }
                else if (pageNum > PageCount)
                {
                    pageNum = PageCount;
                }
                SelectedNum = pageNum;
            }

            PageText = SelectedNum.ToString();
        }
        #endregion
        #endregion 【Functions】
    }
}
