using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.ComponentModel;
using TigerSan.CsvLog;
using TigerSan.UI.Converters;

namespace TigerSan.UI.Controls
{
    public partial class NumBox : UserControl
    {
        #region 【DependencyProperties】
        #region [OneWay]
        #region 是否获得焦点
        /// <summary>
        /// 是否获得焦点
        /// </summary>
        public new bool IsFocused
        {
            get { return (bool)GetValue(IsFocusedProperty); }
            private set { SetValue(IsFocusedProperty, value); }
        }
        public new static readonly DependencyProperty IsFocusedProperty =
            DependencyProperty.Register(
                nameof(IsFocused),
                typeof(bool),
                typeof(NumBox),
                new PropertyMetadata(false));
        #endregion
        #endregion [OneWay]

        #region 值
        /// <summary>
        /// 值
        /// </summary>
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set
            {
                double value1 = Math.Round(value, DecimalPlaces);

                if (value1 > MaxValue)
                {
                    value1 = MaxValue;
                }
                else if (value1 < MinValue)
                {
                    value1 = MinValue;
                }

                SetValue(ValueProperty, value1);
            }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                nameof(Value),
                typeof(double),
                typeof(NumBox),
                new FrameworkPropertyMetadata(
                    0.0,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnValueChanged));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = (NumBox)d;

            sender.SetValueToText();
            sender.RaiseValueChangedEvent();
            sender.RaiseValueChangedCommand();
        }
        #endregion

        #region 步长
        /// <summary>
        /// 步长
        /// </summary>
        public double Step
        {
            get { return (double)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }
        public static readonly DependencyProperty StepProperty =
            DependencyProperty.Register(
                nameof(Step),
                typeof(double),
                typeof(NumBox),
                new PropertyMetadata(1.0));
        #endregion

        #region 最大值
        /// <summary>
        /// 最大值
        /// </summary>
        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register(
                nameof(MaxValue),
                typeof(double),
                typeof(NumBox),
                new PropertyMetadata(double.MaxValue));
        #endregion

        #region 最小值
        /// <summary>
        /// 最小值
        /// </summary>
        public double MinValue
        {
            get { return (double)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register(
                nameof(MinValue),
                typeof(double),
                typeof(NumBox),
                new PropertyMetadata(double.MinValue));
        #endregion

        #region 小数位数
        /// <summary>
        /// 小数位数
        /// </summary>
        public int DecimalPlaces
        {
            get { return (int)GetValue(DecimalPlacesProperty); }
            set { SetValue(DecimalPlacesProperty, value); }
        }
        public static readonly DependencyProperty DecimalPlacesProperty =
            DependencyProperty.Register(
                nameof(DecimalPlaces),
                typeof(int),
                typeof(NumBox),
                new PropertyMetadata(2));
        #endregion

        #region 背景
        /// <summary>
        /// 背景
        /// </summary>
        public new Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }
        public new static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register(
                nameof(Background),
                typeof(Brush),
                typeof(NumBox),
                new PropertyMetadata(Generic.BasicWhite));
        #endregion

        #region 步长
        /// <summary>
        /// 步长
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
                typeof(NumBox),
                new PropertyMetadata(true));
        #endregion
        #endregion 【DependencyProperties】

        #region 【CustomEvents】
        #region 值改变
        [Category("Behavior")]
        public static readonly RoutedEvent ValueChangedEvent =
        EventManager.RegisterRoutedEvent(
            nameof(ValueChanged),
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(NumBox));

        public event RoutedEventHandler ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        protected virtual void RaiseValueChangedEvent()
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
                typeof(NumBox),
                new PropertyMetadata(null));

        protected void RaiseValueChangedCommand()
        {
            ValueChangedCommand?.Execute(Value);
        }
        #endregion
        #endregion 【CustomCommands】

        #region 【Ctor】
        public NumBox()
        {
            InitializeComponent();
            Init();
        }
        #endregion 【Ctor】

        #region 【Events】
        #region 加载完成
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var error = "NumBox initialization failed: ";

            if (DecimalPlaces < 0)
            {
                var msg = $"{error}The DecimalPlaces cannot be negative!";
                LogHelper.Instance.Error(msg);
                throw new Exception(msg);
            }

            if (MinValue > MaxValue)
            {
                var msg = $"{error}The MinValue cannot be greater than MaxValue!";
                LogHelper.Instance.Error(msg);
                throw new Exception(msg);
            }

            SetValueToText();
        }
        #endregion

        #region “内容”文本改变
        private void Content_TextChanged(object sender, RoutedEventArgs e)
        {
            SetTextToValue();
        }
        #endregion

        #region “内容”鼠标滚动
        private void Content_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!IsEnable) return;

            if (e.Delta > 0)
            {
                btnUp_Click();
            }
            else if (e.Delta < 0)
            {
                btnDown_Click();
            }
        }
        #endregion
        #endregion 【Events】

        #region 【Commands】
        #region 点击“上”按钮
        public ICommand btnUp_ClickCommand { get => new DelegateCommand(btnUp_Click); }
        private void btnUp_Click()
        {
            if (Value > MaxValue - Step) return;

            Value += Step;
            SetValueToText();
        }
        #endregion

        #region 点击“下”按钮
        public ICommand btnDown_ClickCommand { get => new DelegateCommand(btnDown_Click); }
        private void btnDown_Click()
        {
            if (Value < MinValue + Step) return;

            Value -= Step;
            SetValueToText();
        }
        #endregion
        #endregion 【Commands】

        #region 【Functions】
        #region 初始化
        private void Init()
        {
            AddEvent();
            DataBinding();
        }
        #endregion

        #region 添加事件
        private void AddEvent()
        {
            Loaded += OnLoaded;
            content.TextChanged += Content_TextChanged;
            content.MouseWheel += Content_MouseWheel;
        }
        #endregion

        #region 数据绑定
        private void DataBinding()
        {
            #region 绑定“IsFocused”
            // 创建双向绑定对象：
            var bindingIsFocused = new Binding(nameof(content.IsFocused))
            {
                Source = content,
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged // 实时更新
            };

            // 应用绑定到目标控件：
            SetBinding(IsFocusedProperty, bindingIsFocused);
            #endregion 绑定“IsFocused”
        }
        #endregion

        #region 将“文本”赋值给“值”
        private void SetTextToValue()
        {
            var value = new Double2StringConverter().ConvertBack(content.Text);
            if (value == null)
            {
                SetValueToText();
                LogHelper.Instance.IsNull(nameof(value));
                return;
            }

            Value = (double)value;
            SetValueToText();
        }
        #endregion

        #region 将“值”赋值给“文本”
        private void SetValueToText()
        {
            var text = DecimalPlaces > 0 ? Value.ToString($"F{DecimalPlaces}") : Value.ToString();

            if (string.Equals(text, content.Text)) return;

            content.Text = text;
        }
        #endregion
        #endregion 【Functions】
    }

    #region 设计数据
    public class NumBoxDesignData : UserControl
    {
        #region 【Properties】
        /// <summary>
        /// 文本
        /// </summary>
        public string Text { get; set; } = "123";

        /// <summary>
        /// 值
        /// </summary>
        public int Value { get; set; } = 0;

        /// <summary>
        /// 占位文本
        /// </summary>
        public string Placeholder { get; set; } = "Please enter.";

        /// <summary>
        /// "占位文本"可见性
        /// </summary>
        public Visibility PlaceholderVisibility { get; set; } = Visibility.Visible;
        #endregion 【Properties】

        #region 【Ctor】
        public NumBoxDesignData()
        {
            Foreground = Generic.PrimaryText;
            Background = Generic.DarkFill;
        }
        #endregion 【Ctor】
    }
    #endregion
}
