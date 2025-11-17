using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TigerSan.UI.Temp.Controls
{
    public partial class NumBox : UserControl
    {
        #region 【DependencyProperties】
        #region [OneWay]
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
                typeof(NumBox),
                new FrameworkPropertyMetadata(
                    string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion
        #endregion [OneWay]

        #region 值
        /// <summary>
        /// 值
        /// </summary>
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                nameof(Value),
                typeof(int),
                typeof(NumBox),
                new FrameworkPropertyMetadata(
                    0,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    ValueChanged));

        private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = (NumBox)d;
            sender.SetValueToText();
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
                typeof(NumBox),
                new PropertyMetadata("Please select."));
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
        #endregion 【DependencyProperties】

        #region 【Ctor】
        public NumBox()
        {
            InitializeComponent();
        }
        #endregion 【Ctor】

        #region 【Commands】
        #region 点击“上”按钮
        public ICommand btnUp_ClickCommand { get => new DelegateCommand(btnUp_Click); }
        private void btnUp_Click()
        {
            ++Value;
            SetValueToText();
        }
        #endregion

        #region 点击“下”按钮
        public ICommand btnDown_ClickCommand { get => new DelegateCommand(btnDown_Click); }
        private void btnDown_Click()
        {
            --Value;
            SetValueToText();
        }
        #endregion
        #endregion 【Commands】

        #region 【Functions】
        #region 将“值”赋值给“文本”
        private void SetValueToText()
        {
            Text = Value.ToString();
        }
        #endregion
        #endregion 【Functions】
    }

    #region 设计数据
    public class NumBoxDesignData : BindableBase
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
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// 是否启用
        /// </summary>
        public Brush Background { get; set; } = Generic.BasicWhite;
        #endregion 【Properties】

        #region 【Ctor】
        public NumBoxDesignData()
        {
        }
        #endregion 【Ctor】
    }
    #endregion
}
