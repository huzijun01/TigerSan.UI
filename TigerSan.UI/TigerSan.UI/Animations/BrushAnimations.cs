using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace TigerSan.UI.Animations
{
    #region 【委托】
    /// <summary>
    /// 设置笔刷委托
    /// </summary>
    public delegate void SetBrush(Brush brush);
    #endregion 【委托】

    public class BrushGradientAnimation : DependencyObject
    {
        #region 【Fields】
        /// <summary>
        /// 设置笔刷委托
        /// </summary>
        private SetBrush _setBrush;

        /// <summary>
        /// 故事板
        /// </summary>
        Storyboard _storyboard = new Storyboard();
        #endregion 【Fields】

        #region  【DependencyProperties】
        #region 临时颜色
        /// <summary>
        /// 临时颜色
        /// </summary>
        public Color TempColor
        {
            get { return (Color)GetValue(TempColorProperty); }
            set { SetValue(TempColorProperty, value); }
        }
        public static readonly DependencyProperty TempColorProperty =
            DependencyProperty.Register(
                nameof(TempColor),
                typeof(Color),
                typeof(BrushGradientAnimation),
                new PropertyMetadata(Colors.Transparent, OnTempColorChanged));

        private static void OnTempColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = (BrushGradientAnimation)d;
            sender._setBrush.Invoke(new SolidColorBrush((Color)e.NewValue));
        }
        #endregion
        #endregion 【DependencyProperties】

        #region 【Ctor】
        public BrushGradientAnimation(SetBrush setBrush, Color defaultColor)
        {
            _setBrush = setBrush;
            TempColor = defaultColor;
        }
        #endregion 【Ctor】

        #region 【Functions】
        #region [Private]
        #region 获取“渐变动画”
        /// <summary>
        /// 获取“渐变动画”
        /// </summary>
        private ColorAnimation Gradient(double secDuration, Color to)
        {
            ColorAnimation animation = new ColorAnimation
            {
                From = TempColor,
                To = to,
                Duration = TimeSpan.FromSeconds(secDuration),
                AutoReverse = false,
            };

            Storyboard.SetTarget(animation, this);
            Storyboard.SetTargetProperty(animation, new PropertyPath(TempColorProperty));

            return animation;
        }
        #endregion
        #endregion [Private]

        #region 设置“颜色”
        public void SetColor(Color to)
        {
            if (Equals(TempColor, to)) return;

            // 开始storyboard：
            var gradient = Gradient(Generic.DurationTotalSeconds, to);
            _storyboard.Stop();
            _storyboard.Children.Clear();
            _storyboard.Children.Add(gradient);
            _storyboard.Begin();
        }
        #endregion
        #endregion 【Functions】
    }
}
