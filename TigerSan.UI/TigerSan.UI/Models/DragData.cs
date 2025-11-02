using System.Windows;
using System.Windows.Input;
using TigerSan.CsvLog;
using TigerSan.ScreenDetection;

namespace TigerSan.UI.Models
{
    #region 拖拽事件
    /// <summary>
    /// 拖拽事件
    /// </summary>
    /// <param name="sender">发送方</param>
    /// <param name="dragData">拖拽数据</param>
    public delegate void DragEvent(object sender, DragData dragData);

    /// <summary>
    /// 拖拽事件配置
    /// </summary>
    public class DragEvents
    {
        /// <summary>
        /// 鼠标拖拽事件
        /// </summary>
        public DragEvent? _onDrag;

        /// <summary>
        /// 鼠标按下事件
        /// </summary>
        public DragEvent? _onMouseDown;

        /// <summary>
        /// 鼠标抬起事件
        /// </summary>
        public DragEvent? _onMouseUp;

        /// <summary>
        /// 鼠标按下事件
        /// </summary>
        public DragEvent? _onMouseEnter;

        /// <summary>
        /// 鼠标抬起事件
        /// </summary>
        public DragEvent? _onMouseLeave;

        /// <summary>
        /// 鼠标双击事件
        /// </summary>
        public DragEvent? _onDoubleClicked;
    }
    #endregion

    #region 拖拽数据
    /// <summary>
    /// 拖拽数据
    /// </summary>
    public class DragData
    {
        #region 【Fields】
        /// <summary>
        /// 旧的“屏幕坐标”
        /// </summary>
        public Point? _oldScreenPosition;

        /// <summary>
        /// 鼠标事件参数
        /// </summary>
        public MouseEventArgs? e;

        /// <summary>
        /// 元素
        /// </summary>
        public FrameworkElement _element;

        /// <summary>
        /// 控件坐标（虚拟像素）
        /// </summary>
        public Point ControlPosition;

        /// <summary>
        /// 屏幕坐标（虚拟像素）
        /// </summary>
        public Point ScreenPosition;
        #endregion 【Fields】

        #region 【Properties】
        /// <summary>
        /// 缩放比例
        /// </summary>
        public double Scale { get => ScreenHelper.GetDpiScale(); }

        /// <summary>
        /// 移动距离（虚拟像素）
        /// </summary>
        public Point MovePosition { get => GetMovePosition(); }

        /// <summary>
        /// 中心距离（实际像素）
        /// </summary>
        public Point CentralDistance { get => GetCentralDistance(); }
        #endregion 【Properties】

        #region 【Ctor】
        public DragData(FrameworkElement element)
        {
            _element = element;
        }
        #endregion 【Ctor】

        #region 【Functions】
        #region 获取“中心距离”
        public Point GetCentralDistance()
        {
            return new Point()
            {
                X = ControlPosition.X * Scale - _element.Width / 2,
                Y = ControlPosition.Y * Scale - _element.Height / 2,
            };
        }
        #endregion

        #region 获取“移动距离”
        public Point GetMovePosition()
        {
            if (_oldScreenPosition == null)
            {
                LogHelper.Instance.IsNull(nameof(_oldScreenPosition));
                return new Point();
            }

            Point movePosition = new Point();
            movePosition.X = ScreenPosition.X - _oldScreenPosition.Value.X;
            movePosition.Y = ScreenPosition.Y - _oldScreenPosition.Value.Y;

            _oldScreenPosition = ScreenPosition;

            return movePosition;
        }
        #endregion
        #endregion 【Functions】
    }
    #endregion
}
