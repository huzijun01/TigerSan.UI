using System.Windows;
using System.Windows.Input;
using TigerSan.TimerHelper.WPF;
using TigerSan.ScreenDetection;
using TigerSan.UI.Models;

namespace TigerSan.UI.Behaviors
{
    public class MouseDragBehavior
    {
        #region 【Fields】
        #region [Private]
        /// <summary>
        /// 是否悬浮
        /// </summary>
        private bool _isHover = false;

        /// <summary>
        /// 是否按下
        /// </summary>
        private bool _isPressed = false;

        /// <summary>
        /// 拖拽数据
        /// </summary>
        private DragData _dragData;

        /// <summary>
        /// 点击计数器
        /// </summary>
        private ClickCounter _clickCounter = new ClickCounter(200);
        #endregion [Private]

        /// <summary>
        /// 元素
        /// </summary>
        public FrameworkElement _element;

        /// <summary>
        /// 发送方
        /// </summary>
        public object _sender;

        /// <summary>
        /// 鼠标拖拽事件
        /// </summary>
        public event DragEvent? _onDrag;

        /// <summary>
        /// 鼠标按下事件
        /// </summary>
        public event DragEvent? _onMouseDown;

        /// <summary>
        /// 鼠标抬起事件
        /// </summary>
        public event DragEvent? _onMouseUp;

        /// <summary>
        /// 鼠标按下事件
        /// </summary>
        public event DragEvent? _onMouseEnter;

        /// <summary>
        /// 鼠标抬起事件
        /// </summary>
        public event DragEvent? _onMouseLeave;

        /// <summary>
        /// 鼠标双击事件
        /// </summary>
        public event DragEvent? _onDoubleClicked;
        #endregion 【Fields】

        #region 【Properties】
        /// <summary>
        /// 是否正在拖动
        /// </summary>
        private bool IsDragging { get { return _isHover && _isPressed; } }
        #endregion 【Properties】

        #region 【Ctor】
        public MouseDragBehavior(
            FrameworkElement element,
            object? sender,
            DragEvents? dragEvents = null)
        {
            // Fields:
            _element = element;
            _sender = sender ?? element;
            if (dragEvents != null)
            {
                _onDrag = dragEvents._onDrag;
                _onMouseDown = dragEvents._onMouseDown;
                _onMouseUp = dragEvents._onMouseUp;
                _onMouseEnter = dragEvents._onMouseEnter;
                _onMouseLeave = dragEvents._onMouseLeave;
                _onDoubleClicked = dragEvents._onDoubleClicked;
            }
            // Events:
            _element.MouseDown += Element_MouseDown;
            _element.MouseUp += Element_MouseUp;
            _element.MouseEnter += Element_MouseEnter;
            _element.MouseLeave += Element_MouseLeave;
            _element.MouseMove += Element_MouseMove;
            // DragData:
            _dragData = new DragData(element);
        }
        #endregion 【Ctor】

        #region 【Events】
        #region 鼠标进入
        private void Element_MouseEnter(object sender, MouseEventArgs e)
        {
            _isHover = true;

            _onMouseEnter?.Invoke(_sender, GetDragData(sender, e));
        }
        #endregion

        #region 鼠标离开
        private void Element_MouseLeave(object sender, MouseEventArgs e)
        {
            _isHover = false;
            _isPressed = false;

            _onMouseLeave?.Invoke(_sender, GetDragData(sender, e));
        }
        #endregion

        #region 鼠标按下
        private void Element_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _isPressed = true;

            _dragData._oldScreenPosition = GetScreenPosition(e);

            _onMouseDown?.Invoke(_sender, GetDragData(sender, e));

            #region 双击
            if (_clickCounter.IsStoped)
            {
                _clickCounter.Start();
            }

            ++_clickCounter._count;

            if (_clickCounter._count >= 2)
            {
                _onDoubleClicked?.Invoke(_sender, GetDragData(sender, e));
            }
            #endregion 双击
        }
        #endregion

        #region 鼠标抬起
        private void Element_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _isPressed = false;

            _onMouseUp?.Invoke(_sender, GetDragData(sender, e));
        }
        #endregion

        #region 鼠标移动
        private void Element_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsDragging) return;

            _onDrag?.Invoke(_sender, GetDragData(sender, e));
        }
        #endregion
        #endregion 【Events】

        #region 【Functions】
        #region 获取拖拽数据
        private DragData GetDragData(object element, MouseEventArgs e)
        {
            _dragData.ControlPosition = GetControlPosition(e);
            _dragData.ScreenPosition = GetScreenPosition(e);
            return _dragData;
        }
        #endregion

        #region 获取控件坐标
        public Point GetControlPosition(MouseEventArgs e)
        {
            // 控件坐标：
            var position = e.GetPosition(_element);

            // 缩放：
            var scale = ScreenHelper.GetDpiScale();
            position.X /= scale;
            position.Y /= scale;

            return position;
        }
        #endregion

        #region 获取屏幕坐标
        public Point GetScreenPosition(MouseEventArgs e)
        {
            // 控件坐标：
            Point relativePosition = e.GetPosition(_element);

            // 屏幕坐标：
            var position = _element.PointToScreen(relativePosition); ;

            // 缩放：
            var scale = ScreenHelper.GetDpiScale();
            position.X /= scale;
            position.Y /= scale;

            return position;
        }
        #endregion
        #endregion 【Functions】
    }
}
