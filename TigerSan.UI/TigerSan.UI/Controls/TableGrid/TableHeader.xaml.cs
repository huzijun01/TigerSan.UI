﻿using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using TigerSan.CsvLog;
using TigerSan.UI.Models;
using TigerSan.UI.Behaviors;

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
        private static MouseDragBehavior? _handelMouseDrag;
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

            var headerModel = header.HeaderModel;
            if (headerModel == null)
            {
                LogHelper.Instance.Warning($"The {nameof(headerModel)} is null!");
                return;
            }

            header.Title = headerModel.Title;
            header.SortMode = headerModel.SortMode;
            header.IsAllowSort = headerModel.IsAllowSort;
        }
        #endregion
        #endregion 【DependencyProperties】

        #region 【Ctor】
        public TableHeader()
        {
            InitializeComponent();
            Loaded += TableHeader_Loaded;
        }

        public TableHeader(HeaderModel headerModel)
        {
            InitializeComponent();
            HeaderModel = headerModel;
            Loaded += TableHeader_Loaded;
        }
        #endregion 【Ctor】

        #region 【Events】
        #region 初始化完成
        private void TableHeader_Loaded(object sender, RoutedEventArgs e)
        {
            AddEvent();
            DataBinding();
            Style = Generic.TransparentUserControlStyle;
        }
        #endregion

        #region 数据绑定
        private void DataBinding()
        {
            #region 绑定“Background”
            // 创建双向绑定对象：
            var bindingBackground = new Binding(nameof(ItemModel.Background))
            {
                Source = HeaderModel,
                Mode = BindingMode.OneWay, // 启用双向绑定
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged // 实时更新
            };

            // 应用绑定到目标控件：
            SetBinding(BackgroundProperty, bindingBackground);
            #endregion 绑定“Background”
        }
        #endregion

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

        #region 点击排序标签
        private void Sort_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ChangeSortMode();
        }
        #endregion

        #region 列宽手柄鼠标按下
        private static void handel_MouseDown(object sender, DragData dragData)
        {
            var header = (TableHeader)sender;

            var headerModel = header.HeaderModel;
            if (headerModel == null)
            {
                LogHelper.Instance.Warning($"The {nameof(headerModel)} is null!");
                return;
            }

            headerModel.Width = header.ActualWidth + dragData.CentralDistance.X;
        }
        #endregion

        #region 列宽手柄鼠标拖拽
        private static void handel_MouseDrag(object sender, DragData dragData)
        {
            var header = (TableHeader)sender;

            var headerModel = header.HeaderModel;
            if (headerModel == null)
            {
                LogHelper.Instance.Warning($"The {nameof(headerModel)} is null!");
                return;
            }

            headerModel.Width = header.ActualWidth + dragData.MovePosition.X;
        }
        #endregion

        #region 列宽手柄鼠标双击
        private static void handel_DoubleClicked(object sender, DragData dragData)
        {
            var header = (TableHeader)sender;

            var headerModel = header.HeaderModel;
            if (headerModel == null)
            {
                LogHelper.Instance.Warning($"The {nameof(headerModel)} is null!");
                return;
            }

            headerModel.Width = headerModel.GetHeaderAttribute().Width;
        }
        #endregion
        #endregion 【Events】

        #region 【Commands】
        #endregion 【Commands】

        #region 【Functions】
        #region 添加事件
        private void AddEvent()
        {
            #region [减]
            #region 背景
            background.MouseEnter -= Background_MouseEnter;
            background.MouseLeave -= Background_MouseLeave;
            header.MouseEnter -= Background_MouseEnter;
            header.MouseLeave -= Background_MouseLeave;
            filter.MouseEnter -= Background_MouseEnter;
            filter.MouseLeave -= Background_MouseLeave;
            sort.MouseEnter -= Background_MouseEnter;
            sort.MouseLeave -= Background_MouseLeave;
            handel.MouseEnter -= Background_MouseEnter;
            handel.MouseLeave -= Background_MouseLeave;
            #endregion

            #region 列宽手柄
            handel.MouseEnter -= Handel_MouseEnter;

            _handelMouseDrag = new MouseDragBehavior(
                handel,
                this,
                handel_MouseDrag,
                handel_MouseDown,
                null,
                handel_DoubleClicked);
            #endregion

            #region 排序标签
            background.MouseLeftButtonUp -= Sort_MouseLeftButtonUp;
            header.MouseLeftButtonUp -= Sort_MouseLeftButtonUp;
            filter.MouseLeftButtonUp -= Sort_MouseLeftButtonUp;
            sort.MouseLeftButtonUp -= Sort_MouseLeftButtonUp;
            #endregion
            #endregion [减]

            #region [加]
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
            #endregion

            #region 列宽手柄
            handel.MouseEnter += Handel_MouseEnter;

            _handelMouseDrag = new MouseDragBehavior(
                handel,
                this,
                handel_MouseDrag,
                handel_MouseDown,
                null,
                handel_DoubleClicked);
            #endregion

            #region 排序标签
            background.MouseLeftButtonUp += Sort_MouseLeftButtonUp;
            header.MouseLeftButtonUp += Sort_MouseLeftButtonUp;
            filter.MouseLeftButtonUp += Sort_MouseLeftButtonUp;
            sort.MouseLeftButtonUp += Sort_MouseLeftButtonUp;
            #endregion
            #endregion [加]
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
