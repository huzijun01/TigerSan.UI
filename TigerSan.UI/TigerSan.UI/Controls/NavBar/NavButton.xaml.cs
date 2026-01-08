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
    public partial class NavButton : UserControl
    {
        #region 【Fields】
        /// <summary>
        /// 前景动画
        /// </summary>
        private BrushGradientAnimation _fgAnimation;

        /// <summary>
        /// 背景动画
        /// </summary>
        private BrushGradientAnimation _bgcAnimation;
        #endregion 【Fields】

        #region 【DependencyProperties】
        #region [OneWay]
        #region 背景
        /// <summary>
        /// 背景
        /// </summary>
        public new Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            private set { SetValue(BackgroundProperty, value); }
        }
        public static new readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register(
                nameof(Background),
                typeof(Brush),
                typeof(NavButton),
                new PropertyMetadata(Generic.Transparent));
        #endregion
        #endregion [OneWay]

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
                typeof(NavButton),
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
                typeof(NavButton),
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
                typeof(NavButton),
                new FrameworkPropertyMetadata(
                    false,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    IsSelectedChanged));
        private static void IsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = (NavButton)d;
            sender.UpdateState();
        }
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
                typeof(NavButton),
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
                typeof(NavButton),
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
                typeof(NavButton));

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
            typeof(NavButton));

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
                typeof(NavButton),
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
                typeof(NavButton),
                new PropertyMetadata(null));

        protected void RaiseCheckedCommand()
        {
            CheckedCommand?.Execute(ButtonModel);
        }
        #endregion
        #endregion 【CustomCommands】

        #region 【Ctor】
        public NavButton()
        {
            InitializeComponent();
            Foreground = Generic.PrimaryText;
            _bgcAnimation = new BrushGradientAnimation(SetBackground, Colors.Transparent);
            _fgAnimation = new BrushGradientAnimation(SetForeground, Generic.PrimaryText.Color);
            MouseLeftButtonDown += OnMouseLeftButtonDown;
            AddValueChanged();
        }
        #endregion 【Ctor】

        #region 【Events】
        #region 鼠标按下
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RaiseCommand();
            RaiseClickEvent();

            if (!ButtonModel.IsSelected)
            {
                RaiseCheckedEvent();
                RaiseCheckedCommand();
            }

            NavBarModel.SelectedButtonModel = ButtonModel;

            if (!NavBarModel.OpenedButtonModels.Contains(ButtonModel))
            {
                NavBarModel.OpenedButtonModels.Add(ButtonModel);
            }
        }
        #endregion
        #endregion 【Events】

        #region 【Functions】
        #region 添加“值改变”事件
        private void AddValueChanged()
        {
            BindingHelper.AddValueChanged(
                this,
                typeof(NavButton),
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
            #region 状态切换
            if (!IsEnabled)
            {
                FontWeight = FontWeights.Normal;
                _fgAnimation.SetColor(Generic.DisabledText.Color);
                _bgcAnimation.SetColor(Generic.BaseBorder.Color);
            }
            else if (IsSelected)
            {
                FontWeight = FontWeights.Bold;
                _fgAnimation.SetColor(Generic.Brand.Color);
                _bgcAnimation.SetColor(Generic.Brand_10pct.Color);
            }
            else if (IsMouseOver)
            {
                FontWeight = FontWeights.Normal;
                _fgAnimation.SetColor(Generic.Brand.Color);
                _bgcAnimation.SetColor(Colors.Transparent);
            }
            else
            {
                FontWeight = FontWeights.Normal;
                _fgAnimation.SetColor(Generic.PrimaryText.Color);
                _bgcAnimation.SetColor(Colors.Transparent);
            }
            #endregion 状态切换
        }
        #endregion

        #region 设置“前景”
        private void SetForeground(Brush brush)
        {
            Foreground = brush;
        }
        #endregion

        #region 设置“背景”
        private void SetBackground(Brush brush)
        {
            Background = brush;
        }
        #endregion
        #endregion 【Functions】
    }

    #region 设计数据
    public class NavButtonDesignData : UserControl
    {
        #region 【Fields】
        #endregion 【Fields】

        #region 【DependencyProperties】
        #region [OneWay]
        #endregion [OneWay]

        public string Icon { get; set; }
        public string Title { get; set; }
        public bool IsSelected { get; set; }
        #endregion 【DependencyProperties】

        #region 【Ctor】
        public NavButtonDesignData()
        {
            Icon = Icons.File_Linear;
            Title = "文件";
            Foreground = Generic.PrimaryText;
        }
        #endregion 【Ctor】
    }
    #endregion
}
