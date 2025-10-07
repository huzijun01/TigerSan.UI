using System.Windows;
using System.Windows.Media.Animation;

namespace TigerSan.UI.Animations
{
    public static class DoubleAnimations
    {
        #region 淡入
        /// <summary>
        /// 淡入
        /// </summary>
        public static DoubleAnimation FadeIn(
            DependencyObject target,
            DependencyProperty dp,
            double secDuration)
        {
            DoubleAnimation animation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(secDuration),
                AutoReverse = false
            };
            Storyboard.SetTarget(animation, target);
            Storyboard.SetTargetProperty(animation, new PropertyPath(dp));
            return animation;
        }
        #endregion

        #region 淡出
        /// <summary>
        /// 淡出
        /// </summary>
        public static DoubleAnimation FadeOut(
            DependencyObject target,
            DependencyProperty dp,
            double secDuration)
        {
            DoubleAnimation animation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(secDuration),
                AutoReverse = false
            };
            Storyboard.SetTarget(animation, target);
            Storyboard.SetTargetProperty(animation, new PropertyPath(dp));
            return animation;
        }
        #endregion

        #region 旋转
        /// <summary>
        /// 旋转
        /// </summary>
        public static DoubleAnimationUsingKeyFrames Rotate(
            DependencyObject target,
            DependencyProperty dp,
            double secBeginTime,
            double secDuration)
        {
            // 创建Frames：
            var animation = new DoubleAnimationUsingKeyFrames
            {
                BeginTime = TimeSpan.FromSeconds(secBeginTime)
            };
            // 添加Frame：
            animation.KeyFrames.Add(new SplineDoubleKeyFrame
            {
                KeyTime = TimeSpan.FromSeconds(0),
                Value = 0
            });
            animation.KeyFrames.Add(new SplineDoubleKeyFrame
            {
                KeySpline = new KeySpline(0.4, 0, 0.6, 1), // 贝塞尔曲线
                KeyTime = TimeSpan.FromSeconds(secDuration),
                Value = 360
            });
            Storyboard.SetTarget(animation, target);
            Storyboard.SetTargetProperty(animation, new PropertyPath(dp));
            return animation;
        }
        #endregion

        #region 渐变
        /// <summary>
        /// 渐变
        /// </summary>
        public static DoubleAnimation Gradient(
            DependencyObject target,
            DependencyProperty dp,
            double secDuration,
            double from,
            double to)
        {
            DoubleAnimation animation = new DoubleAnimation
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
