using System.Windows;
using System.Windows.Input;
using TigerSan.UI.Windows;
using TigerSan.ScreenDetection.Models;

namespace TigerSan.UI.Helpers
{
    #region 拖拽委托类
    public class DragDelegate
    {
        public SetValue2D? _setX;
        public SetValue2D? _setY;
        public SetPoint2D? _mouseUp;
        public SetPoint2D? _mouseDown;
        public SetPoint2D? _setDistance;
        public SetPoint2D? _setMousePosition;
    }
    #endregion 拖拽委托类

    public class DragHelper
    {
        #region 【Fields】
        double _oldX;
        double _oldY;
        public Cursor? _cursor;
        public DragWindow? _dragWindow;
        private FrameworkElement _element;

        #region [委托]
        SetValue2D? _setX;
        SetValue2D? _setY;
        SetPoint2D? _mouseUp;
        SetPoint2D? _mouseDown;
        SetPoint2D? _setDistance;
        SetPoint2D? _setMousePosition;
        GetPoint2D _getOldPosition;
        GetPoint2D _getPanelSize;
        GetPoint2D _getMinPosition;
        #endregion [委托]
        #endregion 【Fields】

        #region 【Properties】
        private double PanelWidth { get => _getPanelSize().X; }
        private double PanelHeight { get => _getPanelSize().Y; }
        private double MaxX { get => PanelWidth - _element.Width; }
        private double MaxY { get => PanelHeight - _element.Height; }
        private double MinX { get => _getMinPosition().X; }
        private double MinY { get => _getMinPosition().Y; }
        #endregion 【Properties】

        #region 【Ctor】
        public DragHelper(
            FrameworkElement element,
            DragDelegate dragDelegate,
            GetPoint2D getOldPosition,
            GetPoint2D getPanelSize,
            GetPoint2D getMinPosition)
        {
            _element = element;
            _setX = dragDelegate._setX;
            _setY = dragDelegate._setY;
            _mouseUp = dragDelegate._mouseUp;
            _mouseDown = dragDelegate._mouseDown;
            _setDistance = dragDelegate._setDistance;
            _setMousePosition = dragDelegate._setMousePosition;
            _getOldPosition = getOldPosition;
            _getPanelSize = getPanelSize;
            _getMinPosition = getMinPosition;
            _element.MouseEnter -= ShowDragWindow;
            _element.MouseEnter += ShowDragWindow;
            UpdateOldPosition();

        }
        #endregion 【Ctor】

        #region 【Functions】
        #region 显示“拖拽窗口”
        private void ShowDragWindow(object sender, MouseEventArgs e)
        {
            if (_dragWindow != null) return;

            _dragWindow = new DragWindow(_element);

            if (_cursor != null) { _dragWindow.Cursor = _cursor; }

            _dragWindow._mouseLeftButtonDown += (distX, distY) =>
            {
                UpdateOldPosition();
                _setMousePosition?.Invoke(_dragWindow.MousePositionX, _dragWindow.MousePositionY);
                _mouseDown?.Invoke(_dragWindow.MousePositionX, _dragWindow.MousePositionY);
            };

            _dragWindow._mouseLeftButtonUp += (distX, distY) =>
            {
                _setMousePosition?.Invoke(_dragWindow.MousePositionX, _dragWindow.MousePositionY);
                _mouseUp?.Invoke(_dragWindow.MousePositionX, _dragWindow.MousePositionY);
            };

            _dragWindow._locationChanged += (distX, distY) =>
            {
                _setMousePosition?.Invoke(_dragWindow.MousePositionX, _dragWindow.MousePositionY);
                _setDistance?.Invoke(distX, distY);
                SetControlPosition(distX, distY);
            };

            _dragWindow._closed += (distX, distY) =>
            {
                _setMousePosition?.Invoke(_dragWindow.MousePositionX, _dragWindow.MousePositionY);
            };

            _dragWindow._closedDelay += (distX, distY) =>
            {
                _dragWindow = null;
            };

            _dragWindow.Show();
        }
        #endregion

        #region 更新“旧坐标”
        private void UpdateOldPosition()
        {
            var p = _getOldPosition();
            _oldX = p.X;
            _oldY = p.Y;
        }
        #endregion

        #region 设置“控件位置”
        private void SetControlPosition(double distanceX, double distanceY)
        {
            var targetX = _oldX + distanceX;
            var targetY = _oldY + distanceY;
            if (targetX < MinX)
            {
                _setX?.Invoke(MinX);
            }
            else if (targetX > MaxX)
            {
                _setX?.Invoke(MaxX);
            }
            else
            {
                _setX?.Invoke(targetX);
            }

            if (targetY < MinY)
            {
                _setY?.Invoke(MinY);
            }
            else if (targetY > MaxY)
            {
                _setY?.Invoke(MaxY);
            }
            else
            {
                _setY?.Invoke(targetY);
            }
        }
        #endregion
        #endregion 【Functions】
    }
}
