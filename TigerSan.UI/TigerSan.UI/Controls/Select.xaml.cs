using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using TigerSan.CsvLog;
using TigerSan.TimerHelper.WPF;
using TigerSan.UI.Animations;
using TigerSan.UI.Converters;
using TigerSan.UI.Helpers;
using TigerSan.UI.Models;
using TigerSan.UI.Windows;

namespace TigerSan.UI.Controls
{
    public partial class Select : UserControl
    {
        #region 【Fields】
        /// <summary>
        /// 打开时的箭头角度
        /// </summary>
        private static double _arrowAngleOpen = -90.0;

        /// <summary>
        /// 关闭时的箭头角度
        /// </summary>
        private static double _arrowAngleClose = 90.0;

        /// <summary>
        /// 菜单窗口
        /// </summary>
        private MenuWindow? _menuWindow;

        /// <summary>
        /// 菜单显示延时定时器
        /// </summary>
        private ActionTimer _timerMenuShowDelay = new ActionTimer(50, false);

        /// <summary>
        /// 边框动画
        /// </summary>
        private BrushGradientAnimation _borderAnimation;
        #endregion 【Fields】

        #region 【DependencyProperties】
        #region [OneWay]
        #region 文本
        /// <summary>
        /// 文本
        /// </summary>
        public string? Text
        {
            get { return (string?)GetValue(TextProperty); }
            private set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                nameof(Text),
                typeof(string),
                typeof(Select),
                new PropertyMetadata(string.Empty));
        #endregion

        #region 边框笔刷
        /// <summary>
        /// 边框笔刷
        /// </summary>
        public new Brush BorderBrush
        {
            get { return (Brush)GetValue(BorderBrushProperty); }
            private set { SetValue(BorderBrushProperty, value); }
        }
        public static new readonly DependencyProperty BorderBrushProperty =
            DependencyProperty.Register(
                nameof(BorderBrush),
                typeof(Brush),
                typeof(Select),
                new PropertyMetadata(new SolidColorBrush()));
        #endregion

        #region 箭头角度
        /// <summary>
        /// 箭头角度
        /// </summary>
        public double ArrowAngle
        {
            get { return (double)GetValue(ArrowAngleProperty); }
            private set { SetValue(ArrowAngleProperty, value); }
        }
        public static readonly DependencyProperty ArrowAngleProperty =
            DependencyProperty.Register(
                nameof(ArrowAngle),
                typeof(double),
                typeof(Select),
                new PropertyMetadata(_arrowAngleClose));
        #endregion

        #region 占位文本可见性
        /// <summary>
        /// 占位文本可见性
        /// </summary>
        public Visibility PlaceholderVisibility
        {
            get { return (Visibility)GetValue(PlaceholderVisibilityProperty); }
            private set { SetValue(PlaceholderVisibilityProperty, value); }
        }
        public static readonly DependencyProperty PlaceholderVisibilityProperty =
            DependencyProperty.Register(
                nameof(PlaceholderVisibility),
                typeof(Visibility),
                typeof(Select),
                new PropertyMetadata(Visibility.Visible));
        #endregion
        #endregion [OneWay]

        #region 值
        /// <summary>
        /// 值
        /// </summary>
        public object? Value
        {
            get { return (object?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                nameof(Value),
                typeof(object),
                typeof(Select),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnValueChanged));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var select = (Select)d;
            select.UpdateText();
            select.RaiseValueChangedEvent();
            select.RaiseValueChangedCommand();
        }
        #endregion

        #region 占位文本
        /// <summary>
        /// 占位文本
        /// </summary>
        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register(
                nameof(Placeholder),
                typeof(string),
                typeof(Select),
                new PropertyMetadata("Please select."));
        #endregion

        #region 转换器
        /// <summary>
        /// 转换器
        /// </summary>
        public IValueConverter Converter
        {
            get { return (IValueConverter)GetValue(ConverterProperty); }
            set { SetValue(ConverterProperty, value); }
        }
        public static readonly DependencyProperty ConverterProperty =
            DependencyProperty.Register(
                nameof(Converter),
                typeof(IValueConverter),
                typeof(Select),
                new PropertyMetadata(new Object2StringConverter(), ConverterChanged));

        private static void ConverterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var select = (Select)d;
            select.UpdateText();
        }
        #endregion

        #region 项目集合
        /// <summary>
        /// 项目集合
        /// </summary>
        public ObservableCollection<MenuItemModel> Items
        {
            get { return (ObservableCollection<MenuItemModel>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register(
                nameof(Items),
                typeof(ObservableCollection<MenuItemModel>),
                typeof(Select),
                new PropertyMetadata(new ObservableCollection<MenuItemModel>(), ItemsChanged));

        private static void ItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var select = (Select)d;
            if (select._menuWindow == null) return;
            select._menuWindow.InitItems(select.Items);
        }
        #endregion

        #region 是否启用
        /// <summary>
        /// 是否启用
        /// </summary>
        public new bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }
        public static new readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.Register(
                nameof(IsEnabled),
                typeof(bool),
                typeof(Select),
                new PropertyMetadata(true, IsEnabledChanged));
        private static new void IsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = (Select)d;
            sender.UpdateState();
        }
        #endregion

        #region 是否打开
        /// <summary>
        /// 是否打开
        /// </summary>
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register(
                nameof(IsOpen),
                typeof(bool),
                typeof(Select),
                new PropertyMetadata(false, IsOpenChanged));
        private static void IsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = (Select)d;
            sender.UpdateState();
            sender.UpdateArrowAngle();
        }
        #endregion

        #region 菜单最大高度
        /// <summary>
        /// 菜单最大高度
        /// </summary>
        public double MenuMaxHeight
        {
            get { return (double)GetValue(MenuMaxHeightProperty); }
            set { SetValue(MenuMaxHeightProperty, value); }
        }
        public static readonly DependencyProperty MenuMaxHeightProperty =
            DependencyProperty.Register(
                nameof(MenuMaxHeight),
                typeof(double),
                typeof(Select),
                new PropertyMetadata(400.0, MenuMaxHeightChanged));
        private static void MenuMaxHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = (Select)d;
            if (sender._menuWindow == null) return;
            sender._menuWindow.MaxHeight = sender._menuWindow.MaxHeight;
        }
        #endregion
        #endregion 【DependencyProperties】

        #region 【CustomEvents】
        #region 值改变
        // CLR事件包装器：
        public event RoutedEventHandler ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        // 声明事件：
        public static readonly RoutedEvent ValueChangedEvent =
            EventManager.RegisterRoutedEvent(
                nameof(ValueChanged),
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(Select));

        // 触发事件的方法：
        protected void RaiseValueChangedEvent()
        {
            RaiseEvent(new RoutedEventArgs(ValueChangedEvent, this) { Source = Value });
        }
        #endregion

        #region 打开后
        // CLR事件包装器：
        public event RoutedEventHandler Opened
        {
            add { AddHandler(OpenedEvent, value); }
            remove { RemoveHandler(OpenedEvent, value); }
        }

        // 声明事件：
        public static readonly RoutedEvent OpenedEvent =
            EventManager.RegisterRoutedEvent(
                nameof(Opened),
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(Select));

        // 触发事件的方法：
        protected void RaiseOpenedEvent()
        {
            RaiseEvent(new RoutedEventArgs(OpenedEvent, this));
        }
        #endregion

        #region 关闭后
        // CLR事件包装器：
        public event RoutedEventHandler Closed
        {
            add { AddHandler(ClosedEvent, value); }
            remove { RemoveHandler(ClosedEvent, value); }
        }

        // 声明事件：
        public static readonly RoutedEvent ClosedEvent =
            EventManager.RegisterRoutedEvent(
                nameof(Closed),
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(Select));

        // 触发事件的方法：
        protected void RaiseClosedEvent()
        {
            RaiseEvent(new RoutedEventArgs(ClosedEvent, this));
        }
        #endregion
        #endregion 【CustomEvents】

        #region 【CustomCommands】
        #region 值改变
        public ICommand ValueChangedCommand
        {
            get => (ICommand)GetValue(ValueChangedCommandProperty);
            set => SetValue(ValueChangedCommandProperty, value);
        }
        public static readonly DependencyProperty ValueChangedCommandProperty =
            DependencyProperty.Register(
                nameof(ValueChangedCommand),
                typeof(ICommand),
                typeof(Select),
                new PropertyMetadata(null));

        protected void RaiseValueChangedCommand()
        {
            ValueChangedCommand?.Execute(Value);
        }
        #endregion

        #region 打开后
        public ICommand OpenedCommand
        {
            get => (ICommand)GetValue(OpenedCommandProperty);
            set => SetValue(OpenedCommandProperty, value);
        }
        public static readonly DependencyProperty OpenedCommandProperty =
            DependencyProperty.Register(
                nameof(OpenedCommand),
                typeof(ICommand),
                typeof(Select),
                new PropertyMetadata(null));

        protected void RaiseOpenedCommand()
        {
            OpenedCommand?.Execute(null);
        }
        #endregion

        #region 关闭后
        public ICommand ClosedCommand
        {
            get => (ICommand)GetValue(ClosedCommandProperty);
            set => SetValue(ClosedCommandProperty, value);
        }
        public static readonly DependencyProperty ClosedCommandProperty =
            DependencyProperty.Register(
                nameof(ClosedCommand),
                typeof(ICommand),
                typeof(Select),
                new PropertyMetadata(null));

        protected void RaiseClosedCommand()
        {
            ClosedCommand?.Execute(null);
        }
        #endregion
        #endregion 【CustomCommands】

        #region 【Ctor】
        public Select()
        {
            InitializeComponent();
            _borderAnimation = new BrushGradientAnimation(SetBorderBrush, Colors.Transparent);
            UpdateState();
            AddValueChanged();
            Loaded += OnLoaded;
            MouseLeftButtonDown += OnMouseLeftButtonDown;
            _timerMenuShowDelay._action = ShowMenu;
        }
        #endregion 【Ctor】

        #region 【Events】
        #region 加载完成
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //InitRotationCenter();
        }
        #endregion

        #region “菜单窗口”关闭
        private void MenuWindow_Closed()
        {
            IsOpen = false;
        }
        #endregion

        #region “菜单窗口”项目被点击
        private void MenuWindow_ItemClicked(MenuItemModel model)
        {
            Value = model.Source;
        }
        #endregion

        #region 鼠标按下
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsOpen = !IsOpen;

            if (IsOpen)
            {
                OpenMenu();
            }
            else
            {
                CloseMenu();
            }
        }
        #endregion
        #endregion 【Events】

        #region 【Functions】
        #region 添加“值改变事件”
        private void AddValueChanged()
        {
            BindingHelper.AddValueChanged(
                this,
                typeof(Select),
                IsMouseOverProperty,
                (s, e) =>
                {
                    UpdateState();
                });
        }
        #endregion

        #region 更新“文本”
        public void UpdateText()
        {
            Text = GetText(Value);

            PlaceholderVisibility = string.IsNullOrEmpty(Text) ? Visibility.Visible : Visibility.Collapsed;
        }
        #endregion

        #region 更新“状态”
        public void UpdateState()
        {
            #region 边框色
            if (!IsEnabled)
            {
                _borderAnimation.SetColor(Generic.BaseBorder.Color);
            }
            else if (IsOpen)
            {
                _borderAnimation.SetColor(Generic.Brand.Color);
            }
            else if (IsMouseOver)
            {
                _borderAnimation.SetColor(Generic.DisabledText.Color);
            }
            else
            {
                _borderAnimation.SetColor(Generic.DarkBorder.Color);
            }
            #endregion 边框色

            #region 可见性
            foreach (var item in Items)
            {
                item.Visibility = Visibility.Visible;
            }

            if (Value != null)
            {
                var item = Items.FirstOrDefault(item => Equals(item.Source, Value));
                if (item != null)
                {
                    item.Visibility = Visibility.Collapsed;
                }
            }
            #endregion

            #region 其它
            // 光标：
            Cursor = IsEnabled ? Cursors.Hand : Cursors.Arrow;
            #endregion 其它
        }
        #endregion

        #region 初始化“旋转中心”
        private void InitRotationCenter()
        {
            var transform = Arrow.RenderTransform as RotateTransform;
            if (transform == null)
            {
                LogHelper.Instance.IsNull(nameof(transform));
                return;
            }

            transform.CenterX = Arrow.ActualWidth / 2;
            transform.CenterY = Arrow.ActualHeight / 2;
        }
        #endregion

        #region 更新“箭头角度”
        private void UpdateArrowAngle()
        {
            Storyboard storyboard = new Storyboard();

            var from = IsOpen ? _arrowAngleClose : _arrowAngleOpen;
            var to = IsOpen ? _arrowAngleOpen : _arrowAngleClose;

            // 渐变动画：
            var gradient = DoubleAnimations.Gradient(
                this,
                ArrowAngleProperty,
                Generic.DurationTotalSeconds,
                from,
                to);
            storyboard.Children.Add(gradient);

            // 开始storyboard：
            storyboard.Begin();
        }
        #endregion

        #region 设置“边框笔刷”
        private void SetBorderBrush(Brush brush)
        {
            BorderBrush = brush;
        }
        #endregion

        #region 获取“文本”
        public string GetText(object? source)
        {
            string? text = string.Empty;

            if (Converter == null)
            {
                text = new Object2StringConverter().Convert(source) as string;
            }
            else
            {
                text = Converter.Convert(source, null, null, null) as string;
            }

            return text ?? string.Empty;
        }
        #endregion

        #region 打开“菜单”
        private void OpenMenu()
        {
            RaiseOpenedEvent();
            RaiseOpenedCommand();
            _menuWindow = new MenuWindow(this, Items)
            {
                Width = Width,
                MaxHeight = MenuMaxHeight,
                Converter = Converter,
                _closed = MenuWindow_Closed,
                _itemClicked = MenuWindow_ItemClicked
            };
            _timerMenuShowDelay.Start();
        }
        #endregion

        #region 显示“菜单”
        private void ShowMenu()
        {
            _menuWindow?.Show();
        }
        #endregion

        #region 关闭“菜单”
        private void CloseMenu()
        {
            RaiseClosedEvent();
            RaiseClosedCommand();
            _menuWindow?.SafeClose();
            _menuWindow = null;
        }
        #endregion
        #endregion 【Functions】
    }

    #region 设计数据
    public class SelectDesignData : BindableBase
    {
        #region 【Fields】
        //private static double _arrowAngleOpen = -90.0;
        private static double _arrowAngleClose = 90.0;
        #endregion 【Fields】

        #region 【Properties】
        #region [OneWay]
        /// <summary>
        /// 边框色
        /// </summary>
        public Color BorderColor { get; private set; } = Colors.Transparent;

        /// <summary>
        /// 边框笔刷
        /// </summary>
        public Brush BorderBrush { get; private set; } = new SolidColorBrush();

        /// <summary>
        /// 箭头角度
        /// </summary>
        public double ArrowAngle { get; private set; } = _arrowAngleClose;

        /// <summary>
        /// 占位文本可见性
        /// </summary>
        public Visibility PlaceholderVisibility { get; private set; } = Visibility.Visible;
        #endregion [OneWay]

        /// <summary>
        /// 值
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// 值
        /// </summary>
        public string Placeholder { get; set; } = "Please select.";

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// 是否打开
        /// </summary>
        public bool IsOpen { get; set; } = false;

        /// <summary>
        /// 可见性
        /// </summary>
        public Visibility Visibility { get; set; } = Visibility.Visible;
        #endregion 【Properties】

        #region 【Ctor】
        public SelectDesignData()
        {
            //Text = "Hello World";
            PlaceholderVisibility = Visibility.Visible;
        }
        #endregion 【Ctor】
    }
    #endregion
}
