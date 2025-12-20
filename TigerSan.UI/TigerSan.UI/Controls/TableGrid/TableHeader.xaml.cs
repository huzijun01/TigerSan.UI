using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using TigerSan.CsvLog;
using TigerSan.UI.Models;
using TigerSan.UI.Helpers;
using TigerSan.TimerHelper.WPF;
using TigerSan.ScreenDetection.Models;

namespace TigerSan.UI.Controls
{
    /// <summary>
    /// 表头
    /// </summary>
    public partial class TableHeader : UserControl
    {
        #region 【Fields】
        #region [Static]
        /// <summary>
        /// 列宽手柄鼠标拖拽行为
        /// </summary>
        private double _oldWidth;
        public DragHelper? _dragHelper;
        private ClickCounter _clickCounter = new ClickCounter(500);
        #endregion [Static]
        #endregion 【Fields】

        #region 【Properties】
        /// <summary>
        /// 列宽手柄状态
        /// </summary>
        private HandelState HandelState
        {
            get { return _handelState; }
            set { SetHandelState(value); }
        }
        private HandelState _handelState = HandelState.Hidden;
        #endregion 【Properties】

        #region 【DependencyProperties】
        #region [Protected]
        #region 遮罩背景
        /// <summary>
        /// 遮罩背景
        /// </summary>
        public Brush MaskBackground
        {
            get { return (Brush)GetValue(MaskBackgroundProperty); }
            protected set { SetValue(MaskBackgroundProperty, value); }
        }
        public static readonly DependencyProperty MaskBackgroundProperty =
            DependencyProperty.Register(
                nameof(MaskBackground),
                typeof(Brush),
                typeof(TableHeader),
                new PropertyMetadata(new SolidColorBrush()));
        #endregion

        #region 标题
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            protected set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                nameof(Title),
                typeof(string),
                typeof(TableHeader),
                new PropertyMetadata("null", null));
        #endregion

        #region 排序模式
        /// <summary>
        /// 排序模式
        /// </summary>
        public SortMode SortMode
        {
            get { return (SortMode)GetValue(SortModeProperty); }
            protected set { SetValue(SortModeProperty, value); }
        }
        public static readonly DependencyProperty SortModeProperty =
            DependencyProperty.Register(
                nameof(SortMode),
                typeof(SortMode),
                typeof(TableHeader),
                new PropertyMetadata(SortMode.Normal, SortModeChanged));

        private static void SortModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tableHeader = (TableHeader)d;

            #region 修改排序文本
            switch ((SortMode)e.NewValue)
            {
                case SortMode.Increasing:
                    tableHeader.SortText = "▲";
                    break;
                case SortMode.Decreasing:
                    tableHeader.SortText = "▼";
                    break;
                default:
                    tableHeader.SortText = string.Empty;
                    break;
            }
            #endregion
        }
        #endregion

        #region 排序文本
        /// <summary>
        /// 排序文本
        /// </summary>
        public string SortText
        {
            get { return (string)GetValue(SortTextProperty); }
            protected set { SetValue(SortTextProperty, value); }
        }
        public static readonly DependencyProperty SortTextProperty =
            DependencyProperty.Register(
                nameof(SortText),
                typeof(string),
                typeof(TableHeader),
                new PropertyMetadata(string.Empty, null));
        #endregion

        #region 是否已筛选
        /// <summary>
        /// 是否已筛选
        /// </summary>
        public bool IsFiltered
        {
            get { return (bool)GetValue(IsFilteredProperty); }
            protected set { SetValue(IsFilteredProperty, value); }
        }
        public static readonly DependencyProperty IsFilteredProperty =
            DependencyProperty.Register(
                nameof(IsFiltered),
                typeof(bool),
                typeof(TableHeader),
                new PropertyMetadata(false, null));
        #endregion

        #region 是否允许排序
        /// <summary>
        /// 是否允许排序
        /// </summary>
        public bool IsAllowSort
        {
            get { return (bool)GetValue(IsAllowSortProperty); }
            protected set { SetValue(IsAllowSortProperty, value); }
        }
        public static readonly DependencyProperty IsAllowSortProperty =
            DependencyProperty.Register(
                nameof(IsAllowSort),
                typeof(bool),
                typeof(TableHeader),
                new PropertyMetadata(false, IsAllowSortChanged));

        private static void IsAllowSortChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tableHeader = (TableHeader)d;
            tableHeader.background.Cursor = tableHeader.header.Cursor = (bool)e.NewValue ? Cursors.Hand : Cursors.Arrow;
        }
        #endregion

        #region 手柄宽度
        /// <summary>
        /// 手柄宽度
        /// </summary>
        public double HandelWidth
        {
            get { return (double)GetValue(HandelWidthProperty); }
            protected set { SetValue(HandelWidthProperty, value); }
        }
        public static readonly DependencyProperty HandelWidthProperty =
            DependencyProperty.Register(
                nameof(HandelWidth),
                typeof(double),
                typeof(TableHeader),
                new PropertyMetadata(Generic.ColumnWidthHandleWidth));
        #endregion
        #endregion [Private]

        #region 表头模型
        /// <summary>
        /// 表头模型
        /// </summary>
        public HeaderModel? HeaderModel
        {
            get { return (HeaderModel)GetValue(HeaderModelProperty); }
            set { SetValue(HeaderModelProperty, value); }
        }
        public static readonly DependencyProperty HeaderModelProperty =
            DependencyProperty.Register(
                nameof(HeaderModel),
                typeof(HeaderModel),
                typeof(TableHeader),
                new PropertyMetadata(HeaderModelChanged));

        private static void HeaderModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var header = (TableHeader)d;

            header.DataBinding();
        }
        #endregion
        #endregion 【DependencyProperties】

        #region 【Ctor】
        public TableHeader()
        {
            InitializeComponent();
            Init();
        }

        public TableHeader(HeaderModel headerModel)
        {
            InitializeComponent();
            Init();
            HeaderModel = headerModel;
        }
        #endregion 【Ctor】

        #region 【Events】
        #region 鼠标进入背景
        private void Background_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            MaskBackground = Generic.BasicWhite;
            HandelState = HandelState.Normal;
        }
        #endregion

        #region 鼠标离开背景
        private void Background_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            MaskBackground = Generic.Transparent;
            HandelState = HandelState.Hidden;
        }
        #endregion

        #region 鼠标进入背景
        private void Handel_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            HandelState = HandelState.Hover;
        }
        #endregion

        #region 点击“排序标签”
        private void Sort_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ChangeSortMode();
        }
        #endregion

        #region “列宽手柄”加载完成
        private void handel_Loaded(object sender, RoutedEventArgs e)
        {
            if (_dragHelper != null) return;

            var dragDelegate = new DragDelegate()
            {
                _setDistance = (x, y) =>
                {
                    SetWidth(_oldWidth + x);
                },
                _mouseDown = (x, y) =>
                {
                    _oldWidth = header.ActualWidth - handel.Width;
                    DoubleClickDetect();
                },
            };

            _dragHelper = new DragHelper(
                handel,
                dragDelegate,
                GetOldPosition,
                GetPanelSize,
                GetMinPosition)
            { _cursor = Cursors.SizeWE };
        }
        #endregion
        #endregion 【Events】

        #region 【Commands】
        #endregion 【Commands】

        #region 【Functions】
        #region 初始化
        private void Init()
        {
            AddEvent();
            Loaded += (s, e) => { Style = Generic.TransparentUserControlStyle; };
        }
        #endregion

        #region 添加事件
        private void AddEvent()
        {
            #region 背景
            background.MouseEnter += Background_MouseEnter;
            background.MouseLeave += Background_MouseLeave;
            header.MouseEnter += Background_MouseEnter;
            header.MouseLeave += Background_MouseLeave;
            filter.MouseEnter += Background_MouseEnter;
            filter.MouseLeave += Background_MouseLeave;
            sort.MouseEnter += Background_MouseEnter;
            sort.MouseLeave += Background_MouseLeave;
            handel.MouseEnter += Background_MouseEnter;
            handel.MouseLeave += Background_MouseLeave;
            #endregion 背景

            #region 列宽手柄
            handel.Loaded += handel_Loaded;
            handel.MouseDown += (s, e) => { DoubleClickDetect(); };
            #endregion 列宽手柄

            #region 排序标签
            background.MouseLeftButtonUp += Sort_MouseLeftButtonUp;
            header.MouseLeftButtonUp += Sort_MouseLeftButtonUp;
            filter.MouseLeftButtonUp += Sort_MouseLeftButtonUp;
            sort.MouseLeftButtonUp += Sort_MouseLeftButtonUp;
            #endregion 排序标签
        }
        #endregion

        #region 数据绑定
        private void DataBinding()
        {
            if (HeaderModel == null)
            {
                LogHelper.Instance.IsNull(nameof(HeaderModel));
                return;
            }

            #region 绑定“Background”
            // 创建双向绑定对象：
            var bindingBackground = new Binding(nameof(HeaderModel.Background))
            {
                Source = HeaderModel,
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged // 实时更新
            };

            // 应用绑定到目标控件：
            SetBinding(BackgroundProperty, bindingBackground);
            #endregion 绑定“Background”

            #region 绑定“Title”
            // 创建双向绑定对象：
            var bindingTitle = new Binding(nameof(HeaderModel.Title))
            {
                Source = HeaderModel,
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged // 实时更新
            };

            // 应用绑定到目标控件：
            SetBinding(TitleProperty, bindingTitle);
            #endregion 绑定“Title”

            #region 绑定“SortMode”
            // 创建双向绑定对象：
            var bindingSortMode = new Binding(nameof(HeaderModel.SortMode))
            {
                Source = HeaderModel,
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged // 实时更新
            };

            // 应用绑定到目标控件：
            SetBinding(SortModeProperty, bindingSortMode);
            #endregion 绑定“SortMode”

            #region 绑定“IsAllowSort”
            // 创建双向绑定对象：
            var bindingIsAllowSort = new Binding(nameof(HeaderModel.IsAllowSort))
            {
                Source = HeaderModel,
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged // 实时更新
            };

            // 应用绑定到目标控件：
            SetBinding(IsAllowSortProperty, bindingIsAllowSort);
            #endregion 绑定“IsAllowSort”
        }
        #endregion

        #region 修改排序模式
        private void ChangeSortMode()
        {
            if (!IsAllowSort) return;

            switch (SortMode)
            {
                case SortMode.Normal:
                    SortMode = SortMode.Increasing;
                    break;
                case SortMode.Increasing:
                    SortMode = SortMode.Decreasing;
                    break;
                case SortMode.Decreasing:
                    SortMode = SortMode.Normal;
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region 设置列宽手柄状态
        private void SetHandelState(HandelState handelState)
        {
            if (HeaderModel == null)
            {
                _handelState = HandelState.Hidden;

                handel.Opacity = 0;
                handel.Visibility = Visibility.Collapsed;

                LogHelper.Instance.Warning($"The {nameof(HeaderModel)} is null!");
                return;
            }

            if (!HeaderModel.GetHeaderAttribute().IsAllowResize)
            {
                _handelState = HandelState.Hidden;

                handel.Opacity = 0;
                handel.Visibility = Visibility.Collapsed;

                return;
            }

            _handelState = handelState;

            double opacity;

            switch (HandelState)
            {
                case HandelState.Hidden:
                    opacity = 0;
                    handel.Visibility = Visibility.Collapsed;
                    break;
                case HandelState.Hover:
                    opacity = 1;
                    handel.Visibility = Visibility.Visible;
                    break;
                default:
                    opacity = 0.5;
                    handel.Visibility = Visibility.Visible;
                    break;
            }

            handel.Opacity = opacity;
        }
        #endregion

        #region 设置宽度
        public void SetWidth(double? width)
        {
            if (HeaderModel == null)
            {
                LogHelper.Instance.IsNull(nameof(HeaderModel));
                return;
            }

            HeaderModel.Width = width;
        }
        #endregion

        #region [列宽手柄]
        #region 获取“旧坐标”
        public Point2D GetOldPosition()
        {
            return new Point2D(_oldWidth, 0);
        }
        #endregion

        #region 获取“容器尺寸”
        public Point2D GetPanelSize()
        {
            return new Point2D(double.MaxValue, double.MaxValue);
        }
        #endregion

        #region 获取“最小位置”
        public Point2D GetMinPosition()
        {
            return new Point2D(double.MinValue, double.MinValue);
        }
        #endregion

        #region 双击检测
        public void DoubleClickDetect()
        {
            if (_clickCounter.IsStoped)
            {
                _clickCounter.Start();
            }

            ++_clickCounter._count;

            if (_clickCounter._count > 1)
            {
                if (HeaderModel == null)
                {
                    LogHelper.Instance.IsNull(nameof(HeaderModel));
                    return;
                }

                HeaderModel.Width = null;
            }
        }
        #endregion
        #endregion [列宽手柄]
        #endregion 【Functions】
    }

    #region 设计数据
    public class TableHeaderDesignData : TableHeader
    {
        public TableHeaderDesignData()
        {
            Title = "Header";
            IsFiltered = true;
            IsAllowSort = true;
            SortMode = SortMode.Increasing;
        }
    }
    #endregion
}
