using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows.Controls;
using TigerSan.UI.Models;
using TigerSan.UI.Helpers;
using TigerSan.UI.Animations;
using TigerSan.TimerHelper.WPF;

namespace TigerSan.UI.Controls
{
    public partial class PageButton : UserControl
    {
        #region 【Fields】
        /// <summary>
        /// “前景”动画
        /// </summary>
        private BrushGradientAnimation _foregroundAnimation;

        /// <summary>
        /// “主边框”的“背景”动画
        /// </summary>
        private BrushGradientAnimation _bdrMain_BackgroundAnimation;

        /// <summary>
        /// “子边框”的“背景”动画
        /// </summary>
        private BrushGradientAnimation _bdrSub_BackgroundAnimation;

        /// <summary>
        /// “分隔线”的“背景”动画
        /// </summary>
        private BrushGradientAnimation _line_StrokeAnimation;
        #endregion 【Fields】

        #region 【DependencyProperties】
        #region 图标
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(
                nameof(Icon),
                typeof(string),
                typeof(PageButton),
                new PropertyMetadata(Icons.File_Linear));
        #endregion

        #region 标题
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                nameof(Title),
                typeof(string),
                typeof(PageButton),
                new PropertyMetadata(Generic.Null));
        #endregion

        #region 是否选中
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register(
                nameof(IsSelected),
                typeof(bool),
                typeof(PageButton),
                new FrameworkPropertyMetadata(
                    false,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    IsSelectedChanged));
        private static void IsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = (PageButton)d;
            sender.UpdateState();
        }
        #endregion

        #region 是否显示“关闭按钮”
        /// <summary>
        /// 是否显示“关闭按钮”
        /// </summary>
        public bool IsShowCloseButton
        {
            get { return (bool)GetValue(IsShowCloseButtonProperty); }
            set { SetValue(IsShowCloseButtonProperty, value); }
        }
        public static readonly DependencyProperty IsShowCloseButtonProperty =
            DependencyProperty.Register(
                nameof(IsShowCloseButton),
                typeof(bool),
                typeof(PageButton),
                new PropertyMetadata(true));
        #endregion

        #region 按钮模型
        /// <summary>
        /// 按钮模型
        /// </summary>
        public NavButtonModel ButtonModel
        {
            get { return (NavButtonModel)GetValue(ButtonModelProperty); }
            set { SetValue(ButtonModelProperty, value); }
        }
        public static readonly DependencyProperty ButtonModelProperty =
            DependencyProperty.Register(
                nameof(ButtonModel),
                typeof(NavButtonModel),
                typeof(PageButton),
                new PropertyMetadata(NavBarModel.GetDefaultButtonModel()));
        #endregion

        #region 导航栏模型
        /// <summary>
        /// 导航栏模型
        /// </summary>
        public NavBarModel NavBarModel
        {
            get { return (NavBarModel)GetValue(NavBarModelProperty); }
            set { SetValue(NavBarModelProperty, value); }
        }
        public static readonly DependencyProperty NavBarModelProperty =
            DependencyProperty.Register(
                nameof(NavBarModel),
                typeof(NavBarModel),
                typeof(PageButton),
                new PropertyMetadata(new NavBarModel()));
        #endregion
        #endregion 【DependencyProperties】

        #region 【CustomEvents】
        #region 点击
        // CLR事件包装器：
        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        // 声明事件：
        public static readonly RoutedEvent ClickEvent =
            EventManager.RegisterRoutedEvent(
                nameof(Click),
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(PageButton));

        // 触发事件的方法：
        protected void RaiseClickEvent()
        {
            RaiseEvent(new RoutedEventArgs(ClickEvent, this) { Source = ButtonModel });
        }
        #endregion

        #region 选中
        public event RoutedEventHandler Checked
        {
            add { AddHandler(CheckedEvent, value); }
            remove { RemoveHandler(CheckedEvent, value); }
        }
        [Category("Behavior")]
        public static readonly RoutedEvent CheckedEvent =
        EventManager.RegisterRoutedEvent(
            nameof(Checked),
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(PageButton));

        protected virtual void RaiseCheckedEvent()
        {
            RaiseEvent(new RoutedEventArgs(CheckedEvent, this) { Source = ButtonModel });
        }
        #endregion
        #endregion 【CustomEvents】

        #region 【CustomCommands】
        #region 命令
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                nameof(Command),
                typeof(ICommand),
                typeof(PageButton),
                new PropertyMetadata(null));

        protected void RaiseCommand()
        {
            Command?.Execute(ButtonModel);
        }
        #endregion

        #region 选中
        public ICommand CheckedCommand
        {
            get => (ICommand)GetValue(CheckedCommandProperty);
            set => SetValue(CheckedCommandProperty, value);
        }
        public static readonly DependencyProperty CheckedCommandProperty =
            DependencyProperty.Register(
                nameof(CheckedCommand),
                typeof(ICommand),
                typeof(PageButton),
                new PropertyMetadata(null));

        protected void RaiseCheckedCommand()
        {
            CheckedCommand?.Execute(ButtonModel);
        }
        #endregion
        #endregion 【CustomCommands】

        #region 【Ctor】
        public PageButton()
        {
            InitializeComponent();
            btnClose.Click += BtnClose_Click;
            MouseLeftButtonDown += OnMouseLeftButtonDown;
            _foregroundAnimation = new BrushGradientAnimation(SetForeground_bdrSub, Generic.PrimaryText.Color);
            _bdrSub_BackgroundAnimation = new BrushGradientAnimation(SetBackground_bdrSub, Colors.Transparent);
            _bdrMain_BackgroundAnimation = new BrushGradientAnimation(SetBackground_bdrMain, Colors.Transparent);
            _line_StrokeAnimation = new BrushGradientAnimation(SetLineStroke, Generic.White_25pct.Color);
            AddValueChanged();
        }
        #endregion 【Ctor】

        #region 【Events】
        #region 鼠标按下
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RaiseCommand();
            RaiseClickEvent();

            if (!IsSelected)
            {
                RaiseCheckedEvent();
                RaiseCheckedCommand();
            }

            NavBarModel.SelectedButtonModel = ButtonModel;
        }
        #endregion

        #region 点击“关闭”按钮
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            ButtonModel.IsSelected = false;
            NavBarModel.OpenedButtonModels.Remove(ButtonModel);

            NavBarModel.SelectedButtonModel = NavBarModel.OpenedButtonModels.FirstOrDefault();
        }
        #endregion
        #endregion 【Events】

        #region 【Functions】
        #region 添加“值改变”事件
        private void AddValueChanged()
        {
            BindingHelper.AddValueChanged(
                this,
                typeof(PageButton),
                IsMouseOverProperty,
                (s, e) =>
                {
                    UpdateState();
                    new ActionTimer(100, false, UpdateState).Start();
                });
        }
        #endregion

        #region 更新状态
        public void UpdateState()
        {
            if (IsSelected)
            {
                FontWeight = FontWeights.Bold;
                _foregroundAnimation.SetColor(Generic.Brand.Color);
                _bdrMain_BackgroundAnimation.SetColor(Generic.Brand_25pct.Color);
                _bdrSub_BackgroundAnimation.SetColor(Colors.Transparent);
                _line_StrokeAnimation.SetColor(Colors.Transparent);
            }
            else if (IsMouseOver)
            {
                FontWeight = FontWeights.Normal;
                _foregroundAnimation.SetColor(Generic.PrimaryText.Color);
                _bdrMain_BackgroundAnimation.SetColor(Colors.Transparent);
                _bdrSub_BackgroundAnimation.SetColor(Generic.White_10pct.Color);
                _line_StrokeAnimation.SetColor(Colors.Transparent);
            }
            else
            {
                FontWeight = FontWeights.Normal;
                _foregroundAnimation.SetColor(Generic.PrimaryText.Color);
                _bdrMain_BackgroundAnimation.SetColor(Colors.Transparent);
                _bdrSub_BackgroundAnimation.SetColor(Colors.Transparent);
                _line_StrokeAnimation.SetColor(Generic.White_25pct.Color);
            }
        }
        #endregion

        #region 设置“前景”
        private void SetForeground_bdrSub(Brush brush)
        {
            Foreground = brush;
        }
        #endregion

        #region 设置“主边框”的“背景”
        private void SetBackground_bdrMain(Brush brush)
        {
            bdrMain.Background = brush;
        }
        #endregion

        #region 设置“子边框”的“背景”
        private void SetBackground_bdrSub(Brush brush)
        {
            bdrSub.Background = brush;
        }
        #endregion

        #region 设置“子边框”的“背景”
        private void SetLineStroke(Brush brush)
        {
            line.Stroke = brush;
        }
        #endregion
        #endregion 【Functions】
    }

    #region 设计数据
    public class PageButtonDesignData : UserControl
    {
        #region 【Properties】
        public string Icon { get; set; } = Icons.File_Linear;
        public string Title { get; set; } = Generic.Null;
        public bool IsSelect { get; set; } = false;
        public bool IsShowCloseButton { get; set; } = true;
        #endregion 【Properties】

        #region 【Ctor】
        public PageButtonDesignData()
        {
            Foreground = new SolidColorBrush(Generic.PrimaryText.Color);
        }
        #endregion 【Ctor】
    }
    #endregion
}
