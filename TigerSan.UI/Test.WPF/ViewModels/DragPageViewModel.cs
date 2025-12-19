using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using TigerSan.UI.Helpers;
using TigerSan.ScreenDetection.Models;

namespace Test.WPF.ViewModels
{
    public class DragPageViewModel : BindableBase
    {
        #region 【Fields】
        public DragHelper? _dragHelper;
        #endregion 【Fields】

        #region 【Properties】
        /// <summary>
        /// 画布宽度
        /// </summary>
        public double CanvasWidth
        {
            get { return _CanvasWidth; }
            set { SetProperty(ref _CanvasWidth, value); }
        }
        private double _CanvasWidth = 500;

        /// <summary>
        /// 画布高度
        /// </summary>
        public double CanvasHeight
        {
            get { return _CanvasHeight; }
            set { SetProperty(ref _CanvasHeight, value); }
        }
        private double _CanvasHeight = 400;

        /// <summary>
        /// 按钮宽度
        /// </summary>
        public double ButtonWidth
        {
            get { return _ButtonWidth; }
            set { SetProperty(ref _ButtonWidth, value); }
        }
        private double _ButtonWidth = 100;

        /// <summary>
        /// 按钮高度
        /// </summary>
        public double ButtonHeight
        {
            get { return _ButtonHeight; }
            set { SetProperty(ref _ButtonHeight, value); }
        }
        private double _ButtonHeight = 50;

        /// <summary>
        /// 按钮位置X
        /// </summary>
        public double ButtonX
        {
            get { return _ButtonX; }
            set { SetProperty(ref _ButtonX, value); }
        }
        private double _ButtonX;

        /// <summary>
        /// 按钮位置Y
        /// </summary>
        public double ButtonY
        {
            get { return _ButtonY; }
            set { SetProperty(ref _ButtonY, value); }
        }
        private double _ButtonY;

        /// <summary>
        /// 移动距离X
        /// </summary>
        public double DistanceX
        {
            get { return _DistanceX; }
            set { SetProperty(ref _DistanceX, value); }
        }
        private double _DistanceX;

        /// <summary>
        /// 移动距离Y
        /// </summary>
        public double DistanceY
        {
            get { return _DistanceY; }
            set { SetProperty(ref _DistanceY, value); }
        }
        private double _DistanceY;

        /// <summary>
        /// 鼠标位置X
        /// </summary>
        public double MousePositionX
        {
            get { return _MousePositionX; }
            set { SetProperty(ref _MousePositionX, value); }
        }
        private double _MousePositionX;

        /// <summary>
        /// 鼠标位置Y
        /// </summary>
        public double MousePositionY
        {
            get { return _MousePositionY; }
            set { SetProperty(ref _MousePositionY, value); }
        }
        private double _MousePositionY;

        /// <summary>
        /// 鼠标按下位置X
        /// </summary>
        public double MouseDownOrUpPositionX
        {
            get { return _MouseDownPositionX; }
            set { SetProperty(ref _MouseDownPositionX, value); }
        }
        private double _MouseDownPositionX;

        /// <summary>
        /// 鼠标按下位置Y
        /// </summary>
        public double MouseDownOrUpPositionY
        {
            get { return _MouseDownPositionY; }
            set { SetProperty(ref _MouseDownPositionY, value); }
        }
        private double _MouseDownPositionY;
        #endregion 【Properties】

        #region 【Ctor】
        public DragPageViewModel()
        {
        }
        #endregion 【Ctor】

        #region 【Commands】
        #region “按钮”加载完成
        public ICommand btn_LoadedCommand { get => new DelegateCommand<RoutedEventArgs>(btn_Loaded); }
        private void btn_Loaded(RoutedEventArgs args)
        {
            var control = args.Source as Control;
            if (control == null) return;
            if (_dragHelper != null) return;

            var dragDelegate = new DragDelegate()
            {
                _setDistance = (x, y) => { DistanceX = x; DistanceY = y; },
                _setX = (x) => { ButtonX = x; },
                _setY = (y) => { ButtonY = y; },
                _mouseUp = (x, y) => { MouseDownOrUpPositionX = x; MouseDownOrUpPositionY = y; },
                _mouseDown = (x, y) => { MouseDownOrUpPositionX = x; MouseDownOrUpPositionY = y; },
                _setMousePosition = (x, y) => { MousePositionX = x; MousePositionY = y; }
            };

            _dragHelper = new DragHelper(
                control,
                dragDelegate,
                GetOldPosition,
                GetPanelSize,
                GetMinPosition);
        }
        #endregion
        #endregion 【Commands】

        #region 【Functions】
        #region 获取“旧坐标”
        public Point2D GetOldPosition()
        {
            return new Point2D(ButtonX, ButtonY);
        }
        #endregion

        #region 获取“容器尺寸”
        public Point2D GetPanelSize()
        {
            return new Point2D(CanvasWidth, CanvasHeight);
        }
        #endregion

        #region 获取“最小位置”
        public Point2D GetMinPosition()
        {
            return new Point2D(0, 0);
        }
        #endregion
        #endregion 【Functions】
    }
}
