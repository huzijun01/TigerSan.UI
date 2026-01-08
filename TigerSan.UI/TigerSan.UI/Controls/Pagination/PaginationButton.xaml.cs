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
    public partial class PaginationButton : UserControl
    {
        #region 【Fields】
        /// <summary>
        /// “前景”动画
        /// </summary>
        private BrushGradientAnimation _foregroundAnimation;

        /// <summary>
        /// “背景”动画
        /// </summary>
        private BrushGradientAnimation _backgroundAnimation;
        #endregion 【Fields】

        #region 【DependencyProperties】
        #region [OneWay]
        #region 内容
        /// <summary>
        /// 内容
        /// </summary>
        public new object Content
        {
            get { return (object)GetValue(ContentProperty); }
            private set { SetValue(ContentProperty, value); }
        }
        public new static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(
                nameof(Content),
                typeof(object),
                typeof(PaginationButton),
                new PropertyMetadata());
        #endregion

        #region 显示文本
        /// <summary>
        /// 显示文本
        /// </summary>
        public string ShowText
        {
            get { return (string)GetValue(ShowTextProperty); }
            private set { SetValue(ShowTextProperty, value); }
        }
        public static readonly DependencyProperty ShowTextProperty =
            DependencyProperty.Register(
                nameof(ShowText),
                typeof(string),
                typeof(PaginationButton),
                new PropertyMetadata(PaginationButtonModel._defaultText));
        #endregion
        #endregion [OneWay]

        #region 按钮模型
        /// <summary>
        /// 按钮模型
        /// </summary>
        public PaginationButtonModel ButtonModel
        {
            get { return (PaginationButtonModel)GetValue(ButtonModelProperty); }
            set { SetValue(ButtonModelProperty, value); }
        }
        public static readonly DependencyProperty ButtonModelProperty =
            DependencyProperty.Register(
                nameof(ButtonModel),
                typeof(PaginationButtonModel),
                typeof(PaginationButton),
                new PropertyMetadata(new PaginationButtonModel()));
        #endregion

        #region 文本
        /// <summary>
        /// 文本
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                nameof(Text),
                typeof(string),
                typeof(PaginationButton),
                new PropertyMetadata(PaginationButtonModel._defaultText));
        #endregion

        #region 悬浮文本
        /// <summary>
        /// 悬浮文本
        /// </summary>
        public string HoverText
        {
            get { return (string)GetValue(HoverTextProperty); }
            set { SetValue(HoverTextProperty, value); }
        }
        public static readonly DependencyProperty HoverTextProperty =
            DependencyProperty.Register(
                nameof(HoverText),
                typeof(string),
                typeof(PaginationButton),
                new PropertyMetadata(string.Empty));
        #endregion

        #region 是否“显示”
        /// <summary>
        /// 是否“显示”
        /// </summary>
        public bool IsShow
        {
            get { return (bool)GetValue(IsShowProperty); }
            set { SetValue(IsShowProperty, value); }
        }
        public static readonly DependencyProperty IsShowProperty =
            DependencyProperty.Register(
                nameof(IsShow),
                typeof(bool),
                typeof(PaginationButton),
                new FrameworkPropertyMetadata(
                    true,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    IsShowChanged));
        private static void IsShowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = (PaginationButton)d;
            sender.UpdateState();
        }
        #endregion

        #region 是否“被选中”
        /// <summary>
        /// 是否“被选中”
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
                typeof(PaginationButton),
                new FrameworkPropertyMetadata(
                    false,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
                typeof(PaginationButton));

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
            typeof(PaginationButton));

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
                typeof(PaginationButton),
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
                typeof(PaginationButton),
                new PropertyMetadata(null));

        protected void RaiseCheckedCommand()
        {
            CheckedCommand?.Execute(ButtonModel);
        }
        #endregion
        #endregion 【CustomCommands】

        #region 【Ctor】
        public PaginationButton()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            MouseLeftButtonDown += OnMouseLeftButtonDown;
            _foregroundAnimation = new BrushGradientAnimation(SetForeground_bdrSub, Generic.PrimaryText.Color);
            _backgroundAnimation = new BrushGradientAnimation(SetBackground, Colors.Transparent);
            AddValueChanged();
        }
        #endregion 【Ctor】

        #region 【Events】
        #region 加载完成
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateState();
        }
        #endregion

        #region 鼠标按下
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RaiseCommand();
            RaiseClickEvent();
            ButtonModel._onClicked?.Invoke(ButtonModel);

            IsSelected = true;
        }
        #endregion
        #endregion 【Events】

        #region 【Functions】
        #region 添加“值改变”事件
        private void AddValueChanged()
        {
            #region IsMouseOver
            BindingHelper.AddValueChanged(
                this,
                typeof(PaginationButton),
                IsMouseOverProperty,
                (s, e) =>
                {
                    UpdateState();
                    new ActionTimer(100, false, UpdateState).Start();
                });
            #endregion IsMouseOver

            #region IsSelected
            BindingHelper.AddValueChanged(
                this,
                typeof(PaginationButton),
                IsSelectedProperty,
                (s, e) =>
                {
                    UpdateState();
                    new ActionTimer(100, false, UpdateState).Start();

                    if (IsSelected && IsLoaded)
                    {
                        RaiseCheckedEvent();
                        RaiseCheckedCommand();
                        ButtonModel._onChecked?.Invoke(ButtonModel);
                        ButtonModel._onCheckedInternal?.Invoke(ButtonModel);
                    }
                });
            #endregion IsSelected

            #region IsEnabled
            BindingHelper.AddValueChanged(
                this,
                typeof(PaginationButton),
                IsEnabledProperty,
                (s, e) =>
                {
                    UpdateState();
                    new ActionTimer(100, false, UpdateState).Start();

                    if (IsEnabled)
                    {
                        MouseLeftButtonDown += OnMouseLeftButtonDown;
                    }
                    else
                    {
                        MouseLeftButtonDown -= OnMouseLeftButtonDown;
                        MouseLeftButtonDown += OnMouseLeftButtonDown;
                    }
                });
            #endregion IsEnabled
        }
        #endregion

        #region 更新状态
        public void UpdateState()
        {
            if (IsShow)
            {
                Visibility = Visibility.Visible;
            }
            else
            {
                Visibility = Visibility.Collapsed;
                return;
            }

            if (!IsEnabled)
            {
                ShowText = Text;
                Cursor = Cursors.Arrow;
                FontWeight = FontWeights.Normal;
                _foregroundAnimation.SetColor(Generic.PlaceholderText.Color);
            }
            else if (IsSelected)
            {
                ShowText = Text;
                Cursor = Cursors.Hand;
                FontWeight = FontWeights.Bold;
                _foregroundAnimation.SetColor(Generic.Brand.Color);
            }
            else if (IsMouseOver)
            {
                if (!string.IsNullOrEmpty(HoverText))
                {
                    ShowText = HoverText;
                }
                Cursor = Cursors.Hand;
                FontWeight = FontWeights.Normal;
                _foregroundAnimation.SetColor(Generic.Brand.Color);
            }
            else
            {
                ShowText = Text;
                Cursor = Cursors.Hand;
                FontWeight = FontWeights.Normal;
                _foregroundAnimation.SetColor(Generic.PrimaryText.Color);
            }
        }
        #endregion

        #region 设置“前景”
        private void SetForeground_bdrSub(Brush brush)
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
    public class PaginationButtonDesignData : UserControl
    {
        #region 【Properties】
        public string Text { get; set; } = PaginationButtonModel._defaultText;
        public string ShowText { get; set; } = PaginationButtonModel._defaultText;
        public string HoverText { get; set; } = string.Empty;
        public bool IsSelect { get; set; } = false;
        public bool IsShow { get; set; } = true;
        #endregion 【Properties】

        #region 【Ctor】
        public PaginationButtonDesignData()
        {
            Foreground = new SolidColorBrush(Generic.PrimaryText.Color);
        }
        #endregion 【Ctor】
    }
    #endregion
}
