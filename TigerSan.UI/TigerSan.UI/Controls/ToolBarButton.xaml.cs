using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TigerSan.UI.Controls
{
    public partial class ToolBarButton : UserControl
    {
        #region 【Fields】
        #region [Static]
        /// <summary>
        /// 白色
        /// </summary>
        private static Brush _white = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
        #endregion [Static]
        #endregion 【Fields】

        #region 【DependencyProperties】
        #region 文本
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                nameof(Text),
                typeof(string),
                typeof(ToolBarButton),
                new PropertyMetadata("Button"));
        #endregion

        #region 命令
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                nameof(Command),
                typeof(ICommand),
                typeof(ToolBarButton),
                new PropertyMetadata(null));
        #endregion

        #region 鼠标悬停时的背景颜色
        public Brush MouseOverBackground
        {
            get { return (Brush)GetValue(MouseOverBackgroundProperty); }
            set { SetValue(MouseOverBackgroundProperty, value); }
        }
        public static readonly DependencyProperty MouseOverBackgroundProperty =
            DependencyProperty.Register(
                nameof(MouseOverBackground),
                typeof(Brush),
                typeof(ToolBarButton),
                new PropertyMetadata(_white));
        #endregion

        #region 正在使用的背景透明度
        public double UsingBackgroundOpacity
        {
            get { return (double)GetValue(UsingBackgroundOpacityProperty); }
            set { SetValue(UsingBackgroundOpacityProperty, value); }
        }
        public static readonly DependencyProperty UsingBackgroundOpacityProperty =
            DependencyProperty.Register(
                nameof(UsingBackgroundOpacity),
                typeof(double),
                typeof(ToolBarButton),
                new PropertyMetadata(0.0));
        #endregion

        #region 鼠标悬停时的背景透明度
        public double MouseOverBackgroundOpacity
        {
            get { return (double)GetValue(MouseOverBackgroundOpacityProperty); }
            set { SetValue(MouseOverBackgroundOpacityProperty, value); }
        }
        public static readonly DependencyProperty MouseOverBackgroundOpacityProperty =
            DependencyProperty.Register(
                nameof(MouseOverBackgroundOpacity),
                typeof(double),
                typeof(ToolBarButton),
                new PropertyMetadata(0.15));
        #endregion
        #endregion 【DependencyProperties】

        #region 【Ctor】
        public ToolBarButton()
        {
            InitializeComponent();
            Width = 45;
            FontSize = 12;
            // 鼠标进入：
            content.MouseEnter += ToolBarButton_MouseEnter;
            background.MouseEnter += ToolBarButton_MouseEnter;
            // 鼠标离开：
            content.MouseLeave += ToolBarButton_MouseLeave;
            background.MouseLeave += ToolBarButton_MouseLeave;
            // 鼠标左键按下：
            content.MouseLeftButtonDown += ToolBarButton_MouseLeftButtonDown;
            background.MouseLeftButtonDown += ToolBarButton_MouseLeftButtonDown;
        }
        #endregion 【Ctor】

        #region 【Events】
        #region 鼠标进入
        private void ToolBarButton_MouseEnter(object sender, MouseEventArgs e)
        {
            UsingBackgroundOpacity = MouseOverBackgroundOpacity;
        }
        #endregion

        #region 鼠标离开
        private void ToolBarButton_MouseLeave(object sender, MouseEventArgs e)
        {
            UsingBackgroundOpacity = 0;
        }
        #endregion

        #region 鼠标左键按下
        private void ToolBarButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Command?.Execute(e);
        }
        #endregion
        #endregion 【Events】
    }
}
