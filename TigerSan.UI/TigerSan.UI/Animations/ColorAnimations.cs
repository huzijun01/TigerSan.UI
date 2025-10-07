using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace TigerSan.UI.Animations
{
    public static class ColorAnimations
    {
        #region 渐变
        /// <summary>
        /// 渐变
        /// </summary>
        public static ColorAnimation Gradient(
            DependencyObject target,
            DependencyProperty dp,
            double secDuration,
            Color from,
            Color to)
        {
            ColorAnimation animation = new ColorAnimation
            {
                From = from,
                To = to,
                Duration = TimeSpan.FromSeconds(secDuration),
                AutoReverse = false
            };
            Storyboard.SetTarget(animation, target);
            Storyboard.SetTargetProperty(animation, new PropertyPath(dp));
            return animation;
        }
        #endregion
    }
}
