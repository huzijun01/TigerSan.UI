using System.Windows;
using System.Windows.Controls;

namespace TigerSan.UI.Panels
{
    public class VerticalAveragePanel : Panel
    {
        #region 【Properties】
        /// <summary>
        /// 垂直间距
        /// </summary>
        public double GapY
        {
            get { return (double)GetValue(GapYProperty); }
            set { SetValue(GapYProperty, value); }
        }
        public static readonly DependencyProperty GapYProperty =
            DependencyProperty.Register(
                nameof(GapY),
                typeof(double),
                typeof(VerticalAveragePanel),
                new PropertyMetadata(0.0));
        #endregion 【Properties】

        #region 测量
        protected override Size MeasureOverride(Size availableSize)
        {
            #region 防止尺寸为Infinity
            // 计算子元素的总尺寸需求
            Size totalChildDesiredSize = new Size();
            foreach (UIElement child in Children)
            {
                child.Measure(availableSize);
                totalChildDesiredSize.Width += child.DesiredSize.Width;
                totalChildDesiredSize.Height = Math.Max(totalChildDesiredSize.Height, child.DesiredSize.Height);
            }

            if (double.IsInfinity(availableSize.Width))
            {
                // 如果宽度为Infinity，根据子元素需求动态计算宽度
                availableSize.Width = totalChildDesiredSize.Width;
            }

            if (double.IsInfinity(availableSize.Height))
            {
                // 如果高度为Infinity，根据子元素需求动态计算高度
                availableSize.Height = totalChildDesiredSize.Height;
            }
            #endregion 防止尺寸为Infinity

            return availableSize;
        }
        #endregion

        #region 排列
        protected override Size ArrangeOverride(Size finalSize)
        {
            // 过滤不可见元素：
            var childs = new List<UIElement>();
            foreach (UIElement child in InternalChildren)
            {
                childs.Add(child);
            }
            var visibleChilds = childs.Where(child => child.Visibility != Visibility.Collapsed).ToList();

            // 计算尺寸：
            var childCount = visibleChilds.Count();
            var gapCount = childCount > 0 ? childCount - 1 : 0;

            var size = new Size()
            {
                Width = finalSize.Width,
                Height = (finalSize.Height - gapCount * GapY) / childCount
            };

            // 排布：
            var dY = 0.0;
            for (int index = 0; index < childCount; index++)
            {
                var rect = new Rect()
                {
                    X = 0,
                    Y = dY,
                    Width = size.Width,
                    Height = size.Height,
                };

                visibleChilds[index].Arrange(rect);

                dY += size.Height + GapY;
            }

            return finalSize;
        }
        #endregion
    }
}
