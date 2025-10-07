using System.Windows;
using System.Windows.Input;

namespace TigerSan.UI.Models
{
    #region 拖拽事件
    /// <summary>
    /// 拖拽事件
    /// </summary>
    /// <param name="sender">发送方</param>
    /// <param name="dragData">拖拽数据</param>
    public delegate void DragEvent(object sender, DragData dragData);
    #endregion

    #region 拖拽数据
    /// <summary>
    /// 拖拽数据
    /// </summary>
    public class DragData
    {
        /// <summary>
        /// 鼠标事件参数
        /// </summary>
        public MouseEventArgs e;

        /// <summary>
        /// 元素
        /// </summary>
        public FrameworkElement Element;

        /// <summary>
        /// 控件坐标（虚拟像素）
        /// </summary>
        public Point ControlPosition;

        /// <summary>
        /// 屏幕坐标（虚拟像素）
        /// </summary>
        public Point ScreenPosition;

        /// <summary>
        /// 移动距离（虚拟像素）
        /// </summary>
        public Point MovePosition;

        /// <summary>
        /// 中心距离（实际像素）
        /// </summary>
        public Point CentralDistance;

        /// <summary>
        /// 缩放比例
        /// </summary>
        public double Scale = 1.0;

        #region 【Ctor】
        public DragData(FrameworkElement element, MouseEventArgs e)
        {
            Element = element;
            this.e = e;
        }
        #endregion 【Ctor】
    }
    #endregion
}
