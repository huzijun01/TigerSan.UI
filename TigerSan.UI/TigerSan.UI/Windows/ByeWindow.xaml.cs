using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using TigerSan.UI.Animations;

namespace TigerSan.UI.Windows
{
    public partial class ByeWindow : Window
    {
        #region 【Fields】
        /// <summary>
        /// 淡入时间
        /// </summary>
        public double secFadeIn = 0.3;
        /// <summary>
        /// 淡出时间
        /// </summary>
        public double secFadeOut = 0.5;
        /// <summary>
        /// 等待时间
        /// </summary>
        public double secWait = 0.3;
        /// <summary>
        /// 窗口关闭函数
        /// </summary>
        public Action? fnClosed;
        #endregion 【Fields】

        #region 【DependencyProperties】
        #region 图片
        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register(
                nameof(Image),
                typeof(ImageSource),
                typeof(ByeWindow),
                new PropertyMetadata(Generic.Bye));
        #endregion
        #endregion 【DependencyProperties】

        #region 【Ctor】
        public ByeWindow()
        {
            InitializeComponent();
        }
        #endregion 【Ctor】

        #region 【Events】
        #region 加载完成
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Storyboard storyboard = new Storyboard();

            // 淡入动作：
            var fadeIn = DoubleAnimations.FadeIn(this, OpacityProperty, secFadeIn);
            storyboard.Children.Add(fadeIn);

            // 淡出动作：
            var fadeOut = DoubleAnimations.FadeOut(this, OpacityProperty, secFadeOut);
            fadeOut.BeginTime = TimeSpan.FromSeconds(secFadeIn + secWait); // 在淡入后开始
            storyboard.Children.Add(fadeOut);

            // 处理整个故事板的Completed事件：
            storyboard.Completed += (s, args) => Close();

            // 开始storyboard：
            storyboard.Begin();
        }
        #endregion

        #region 窗口关闭
        private void Window_Closed(object sender, EventArgs e)
        {
            fnClosed?.Invoke();
        }
        #endregion
        #endregion 【Events】
    }
}
