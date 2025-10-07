using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using TigerSan.CsvLog;
using TigerSan.UI.Animations;

namespace TigerSan.UI.Controls
{
    public partial class Switch : UserControl
    {
        #region 【Fields】
        private static GridLength _gridLengthZero = new GridLength(0, GridUnitType.Star);
        private static GridLength _gridLengthAverage = new GridLength(1, GridUnitType.Star);
        #endregion 【Fields】

        #region 【DependencyProperties】
        #region [Private]
        #region 圆形网格左列占比
        /// <summary>
        /// 圆形网格左列占比
        /// </summary>
        public double CircGridleLeftColumnProportion
        {
            get { return (double)GetValue(CircleGridLeftColumnProportionProperty); }
            private set { SetValue(CircleGridLeftColumnProportionProperty, value); }
        }
        public static readonly DependencyProperty CircleGridLeftColumnProportionProperty =
            DependencyProperty.Register(
                nameof(CircGridleLeftColumnProportion),
                typeof(double),
                typeof(Switch),
                new PropertyMetadata(0.0, CircGridleLeftColumnProportionChanged));

        private static void CircGridleLeftColumnProportionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Switch)d;
            control.CircleGridLeftColumnWidth = new GridLength(control.CircGridleLeftColumnProportion, GridUnitType.Star);
            control.CircleGridRightColumnWidth = new GridLength(1 - control.CircGridleLeftColumnProportion, GridUnitType.Star);
        }
        #endregion

        #region 圆形网格左宽度
        /// <summary>
        /// 圆形网格左宽度
        /// </summary>
        public GridLength CircleGridLeftColumnWidth
        {
            get { return (GridLength)GetValue(CircleGridLeftColumnWidthProperty); }
            private set { SetValue(CircleGridLeftColumnWidthProperty, value); }
        }
        public static readonly DependencyProperty CircleGridLeftColumnWidthProperty =
            DependencyProperty.Register(
                nameof(CircleGridLeftColumnWidth),
                typeof(GridLength),
                typeof(Switch),
                new PropertyMetadata(_gridLengthZero));
        #endregion

        #region 圆形网格右宽度
        /// <summary>
        /// 圆形网格右宽度
        /// </summary>
        public GridLength CircleGridRightColumnWidth
        {
            get { return (GridLength)GetValue(CircleGridRightColumnWidthProperty); }
            private set { SetValue(CircleGridRightColumnWidthProperty, value); }
        }
        public static readonly DependencyProperty CircleGridRightColumnWidthProperty =
            DependencyProperty.Register(
                nameof(CircleGridRightColumnWidth),
                typeof(GridLength),
                typeof(Switch),
                new PropertyMetadata(_gridLengthAverage));
        #endregion

        #region 文本
        /// <summary>
        /// 文本
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            private set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                nameof(Text),
                typeof(string),
                typeof(Switch),
                new PropertyMetadata(string.Empty));
        #endregion

        #region 使用中的开关背景
        /// <summary>
        /// 使用中的开关背景
        /// </summary>
        public Brush UsingSwitchBackground
        {
            get { return (Brush)GetValue(UsingSwitchBackgroundProperty); }
            private set { SetValue(UsingSwitchBackgroundProperty, value); }
        }
        public static readonly DependencyProperty UsingSwitchBackgroundProperty =
            DependencyProperty.Register(
                nameof(UsingSwitchBackground),
                typeof(Brush),
                typeof(Switch),
                new PropertyMetadata(Generic.Brand));
        #endregion

        #region 使用中的开关背景色
        /// <summary>
        /// 使用中的开关背景
        /// </summary>
        public Color UsingSwitchBackgroundColor
        {
            get { return (Color)GetValue(UsingSwitchBackgroundColorProperty); }
            private set { SetValue(UsingSwitchBackgroundColorProperty, value); }
        }
        public static readonly DependencyProperty UsingSwitchBackgroundColorProperty =
            DependencyProperty.Register(
                nameof(UsingSwitchBackgroundColor),
                typeof(Color),
                typeof(Switch),
                new PropertyMetadata(Generic.Brand.Color, UsingSwitchBackgroundColorChanged));

        private static void UsingSwitchBackgroundColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Switch)d;
            control.UsingSwitchBackground = new SolidColorBrush(control.UsingSwitchBackgroundColor);
        }
        #endregion
        #endregion [Private]

        #region 值
        /// <summary>
        /// 值
        /// </summary>
        public bool Value
        {
            get { return (bool)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                nameof(Value),
                typeof(bool),
                typeof(Switch),
                new FrameworkPropertyMetadata(
                    false,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnValueChanged));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Switch)d;
            control.Init();
            control.RaiseValueChangedEvent();
            control.RaiseValueChangedCommand();
        }
        #endregion

        #region 开文本
        /// <summary>
        /// 开文本
        /// </summary>
        public string OnText
        {
            get { return (string)GetValue(OnTextProperty); }
            set { SetValue(OnTextProperty, value); }
        }
        public static readonly DependencyProperty OnTextProperty =
            DependencyProperty.Register(
                nameof(OnText),
                typeof(string),
                typeof(Switch),
                new PropertyMetadata("ON", OnTextChanged));

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Switch)d;
            control.Init();
        }
        #endregion

        #region 关文本
        /// <summary>
        /// 关文本
        /// </summary>
        public string OffText
        {
            get { return (string)GetValue(OffTextProperty); }
            set { SetValue(OffTextProperty, value); }
        }
        public static readonly DependencyProperty OffTextProperty =
            DependencyProperty.Register(
                nameof(OffText),
                typeof(string),
                typeof(Switch),
                new PropertyMetadata("OFF", OffTextChanged));

        private static void OffTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Switch)d;
            control.Init();
        }
        #endregion

        #region 是否启用
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable
        {
            get { return (bool)GetValue(IsEnableProperty); }
            set { SetValue(IsEnableProperty, value); }
        }
        public static readonly DependencyProperty IsEnableProperty =
            DependencyProperty.Register(
                nameof(IsEnable),
                typeof(bool),
                typeof(Switch),
                new PropertyMetadata(true, OnIsEnableChanged));

        private static void OnIsEnableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Switch)d;
            control.Init();
        }
        #endregion

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
                typeof(Switch),
                new PropertyMetadata(new SolidColorBrush()));
        #endregion

        #region 开关背景
        /// <summary>
        /// 开关背景
        /// </summary>
        public SolidColorBrush SwitchBackground
        {
            get { return (SolidColorBrush)GetValue(SwitchBackgroundProperty); }
            set { SetValue(SwitchBackgroundProperty, value); }
        }
        public static readonly DependencyProperty SwitchBackgroundProperty =
            DependencyProperty.Register(
                nameof(SwitchBackground),
                typeof(SolidColorBrush),
                typeof(Switch),
                new PropertyMetadata(Generic.Brand, SwitchBackgroundChanged));

        private static void SwitchBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Switch)d;
            control.Init();
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
                typeof(Switch));

        // 触发事件的方法：
        protected void RaiseValueChangedEvent()
        {
            RaiseEvent(new RoutedEventArgs(ValueChangedEvent, this) { Source = Value });
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
                typeof(Switch),
                new PropertyMetadata(null));

        protected void RaiseValueChangedCommand()
        {
            ValueChangedCommand?.Execute(Value);
        }
        #endregion
        #endregion 【CustomCommands】

        #region 【Ctor】
        public Switch()
        {
            InitializeComponent();
            Init();
            // 事件：
            MouseDown -= OnMouseDown;
            MouseDown += OnMouseDown;
        }
        #endregion 【Ctor】

        #region 【Events】
        #region 鼠标按下
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsEnable) return;

            var control = (Switch)sender;

            control.Value = !control.Value;
        }
        #endregion
        #endregion 【Events】

        #region 【Functions】
        #region 初始化
        private void Init()
        {
            // 属性：
            Text = Value ? OnText : OffText;
            UsingSwitchBackground = Value ? SwitchBackground : Generic.DarkerBorder;
            Cursor = IsEnable ? Cursors.Hand : Cursors.Arrow;
            // 网格：
            UpdateCircGridleLeftColumnProportion();
            // 背景：
            UpdateUsingSwitchBackgroundColor();
        }
        #endregion

        #region 更新圆形网格左列占比
        /// <summary>
        /// 更新圆形网格左列占比
        /// </summary>
        private void UpdateCircGridleLeftColumnProportion()
        {
            Storyboard storyboard = new Storyboard();

            if (Value)
            {
                // 淡入动作：
                var fadeIn = DoubleAnimations.FadeIn(this, CircleGridLeftColumnProportionProperty, Generic.DurationTotalSeconds);
                storyboard.Children.Add(fadeIn);
            }
            else
            {
                // 淡出动作：
                var fadeOut = DoubleAnimations.FadeOut(this, CircleGridLeftColumnProportionProperty, Generic.DurationTotalSeconds);
                storyboard.Children.Add(fadeOut);
            }

            // 开始storyboard：
            storyboard.Begin();
        }
        #endregion

        #region 更新使用中的开关背景色
        /// <summary>
        /// 更新使用中的开关背景色
        /// </summary>
        private void UpdateUsingSwitchBackgroundColor()
        {
            var brush = SwitchBackground;
            if (brush == null)
            {
                LogHelper.Instance.IsNull(nameof(brush));
                return;
            }

            Storyboard storyboard = new Storyboard();

            if (Value)
            {
                var gradient = ColorAnimations.Gradient(
                    this,
                    UsingSwitchBackgroundColorProperty,
                    Generic.DurationTotalSeconds,
                    Generic.DarkerBorder.Color,
                    brush.Color
                    );
                storyboard.Children.Add(gradient);
            }
            else
            {
                var gradient = ColorAnimations.Gradient(
                    this,
                    UsingSwitchBackgroundColorProperty,
                    Generic.DurationTotalSeconds,
                    brush.Color,
                    Generic.DarkerBorder.Color
                    );
                storyboard.Children.Add(gradient);
            }

            // 开始storyboard：
            storyboard.Begin();
        }
        #endregion
        #endregion 【Functions】
    }

    #region 设计数据
    public class SwitchDesignData : Switch
    {
        public SwitchDesignData()
        {
            Value = true;
            //OnText = "男";
            //OffText = "女";
        }
    }
    #endregion
}