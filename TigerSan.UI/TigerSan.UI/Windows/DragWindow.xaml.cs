using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using TigerSan.CsvLog;
using TigerSan.TimerHelper.WPF;
using TigerSan.ScreenDetection;

namespace TigerSan.UI.Windows
{
    #region 【委托】
    /// <summary>
    /// 拖拽行为委托
    /// </summary>
    public delegate void DragAction(double distanceX, double distanceY);
    #endregion 【委托】

    public partial class DragWindow : Window
    {
        #region 【Fields】
        #region [Private]
        private bool _isClosed = false;
        private bool _isPressed = false;
        private double _mouseDownPositionX;
        private double _mouseDownPositionY;
        private double _oldLeft;
        private double _oldTop;

        /// <summary>
        /// 控件
        /// </summary>
        private Control _control;

        /// <summary>
        /// 关闭后延时
        /// </summary>
        private double _delay = 100;

        /// <summary>
        /// 重叠容差
        /// </summary>
        private double _overlapOffset = 2;
        #endregion [Private]

        #region [委托]
        /// <summary>
        /// 位置改变后
        /// </summary>
        public DragAction? _locationChanged;

        /// <summary>
        /// 鼠标左键抬起
        /// </summary>
        public DragAction? _mouseLeftButtonUp;

        /// <summary>
        /// 鼠标左键按下
        /// </summary>
        public DragAction? _mouseLeftButtonDown;

        /// <summary>
        /// 关闭后
        /// </summary>
        public DragAction? _closed;

        /// <summary>
        /// 关闭后延迟
        /// </summary>
        public DragAction? _closedDelay;
        #endregion [委托]
        #endregion 【Fields】

        #region 【Properties】
        /// <summary>
        /// 移动距离X
        /// </summary>
        public double DistanceX { get; private set; }

        /// <summary>
        /// 移动距离Y
        /// </summary>
        public double DistanceY { get; private set; }

        /// <summary>
        /// 鼠标位置X
        /// </summary>
        public double MousePositionX { get; private set; }

        /// <summary>
        /// 鼠标位置Y
        /// </summary>
        public double MousePositionY { get; private set; }
        #endregion 【Properties】

        #region 【Ctor】
        public DragWindow(Control control)
        {
            InitializeComponent();
            _control = control;
            Init(control);
        }
        #endregion 【Ctor】

        #region 【Events】
        #region 鼠标左键按下
        public void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isPressed = true;
            UpdateMousePosition(e.GetPosition(this).X, e.GetPosition(this).Y);
            UpdateWindowPosition();
            _mouseLeftButtonDown?.Invoke(DistanceX, DistanceY);

            #region 拖拽移动
            if (e.ChangedButton == MouseButton.Left)
            {
                try
                {
                    DragMove();
                }
                catch (Exception ex)
                {
                    LogHelper.Instance.Error(ex.Message);
                }
            }
            #endregion 拖拽移动
        }
        #endregion

        #region 鼠标左键抬起
        public void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isPressed = false;
            UpdateMousePosition();
            _mouseLeftButtonUp?.Invoke(DistanceX, DistanceY);

            if (!IsMouseOver || !IsOverlap())
            {
                SafeClose();
            }

            UpdateWindowPosition();
        }
        #endregion

        #region 鼠标离开
        public void OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (_isPressed) return;
            SafeClose();
        }
        #endregion

        #region 位置改变后
        public void OnLocationChanged(object? sender, EventArgs e)
        {
            if (!_isPressed) return;
            UpdateMousePosition();
            _locationChanged?.Invoke(DistanceX, DistanceY);
        }
        #endregion

        #region 关闭后
        public void OnClosed(object? sender, EventArgs e)
        {
            UpdateMousePosition();
            _closed?.Invoke(DistanceX, DistanceY);
            new ActionTimer(_delay, false, () =>
            {
                _closedDelay?.Invoke(DistanceX, DistanceY);
            }).Start();
        }
        #endregion
        #endregion 【Events】

        #region 【Functions】
        #region [Private]
        #region 初始化
        private void Init(Control control)
        {
            MouseLeftButtonDown += OnMouseLeftButtonDown;
            MouseLeftButtonUp += OnMouseLeftButtonUp;
            MouseLeave += OnMouseLeave;
            LocationChanged += OnLocationChanged;
            Closed += OnClosed;
            UpdateWindowPosition();
        }
        #endregion

        #region 更新“窗口位置”
        private void UpdateWindowPosition()
        {
            var positon = ScreenHelper.GetScreenPosition(_control);
            if (positon == null) return;

            Left = positon.X;
            Top = positon.Y;
            Width = _control.ActualWidth;
            Height = _control.ActualHeight;
        }
        #endregion

        #region 更新“鼠标位置”
        private void UpdateMousePosition(double? mouseDownPositionX = null, double? mouseDownPositionY = null)
        {
            if (mouseDownPositionX != null && mouseDownPositionY != null)
            {
                _oldLeft = Left;
                _oldTop = Top;
                _mouseDownPositionX = mouseDownPositionX.Value;
                _mouseDownPositionY = mouseDownPositionY.Value;
            }

            DistanceX = Left - _oldLeft;
            DistanceY = Top - _oldTop;
            MousePositionX = _mouseDownPositionX + Left;
            MousePositionY = _mouseDownPositionY + Top;
        }
        #endregion

        #region 判断“是否重叠”
        private bool IsOverlap()
        {
            var positon = ScreenHelper.GetScreenPosition(_control);
            if (positon == null) return false;

            return Math.Abs(Left - positon.X) < _overlapOffset && Math.Abs(Top - positon.Y) < _overlapOffset;
        }
        #endregion

        #region 安全关闭
        private void SafeClose()
        {
            if (_isClosed) return;
            _isClosed = true;
            Close();
        }
        #endregion
        #endregion [Private]
        #endregion 【Functions】
    }
}
