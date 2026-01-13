using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Collections.ObjectModel;
using TigerSan.CsvLog;
using TigerSan.UI.Models;
using TigerSan.UI.Animations;
using TigerSan.UI.Converters;
using TigerSan.TimerHelper.WPF;
using TigerSan.ScreenDetection;
using TigerSan.ScreenDetection.Models;

namespace TigerSan.UI.Windows
{
    #region 方向
    /// <summary>
    /// 方向
    /// </summary>
    public enum Direction
    {
        Top,
        Bottom,
        Left,
        Right
    }
    #endregion

    public partial class MenuWindow : Window
    {
        #region 【Fields】
        /// <summary>
        /// 已打开的“菜单窗口”
        /// </summary>
        private static MenuWindow? _openedMenuWindow;

        /// <summary>
        /// 选择器
        /// </summary>
        private Control _control;

        /// <summary>
        /// 关闭委托
        /// </summary>
        public Action? _closed;

        /// <summary>
        /// 项目点击委托
        /// </summary>
        public Action<MenuItemModel>? _itemClicked;

        /// <summary>
        /// 打开方向
        /// </summary>
        private Direction _openDirection = Direction.Bottom;

        /// <summary>
        /// 控件矩形
        /// </summary>
        private Rectangle2D _rectControl = new Rectangle2D();

        /// <summary>
        /// 菜单矩形
        /// </summary>
        private Rectangle2D _rectMenu = new Rectangle2D();
        #endregion 【Fields】

        #region 【DependencyProperties】
        #region [Private]
        #region 垂直滚动条可见性
        /// <summary>
        /// 垂直滚动条可见性
        /// </summary>
        public ScrollBarVisibility VerticalScrollBarVisibility
        {
            get { return (ScrollBarVisibility)GetValue(VerticalScrollBarVisibilityProperty); }
            private set { SetValue(VerticalScrollBarVisibilityProperty, value); }
        }
        public static readonly DependencyProperty VerticalScrollBarVisibilityProperty =
            DependencyProperty.Register(
                nameof(VerticalScrollBarVisibility),
                typeof(ScrollBarVisibility),
                typeof(MenuWindow),
                new PropertyMetadata(ScrollBarVisibility.Auto));
        #endregion
        #endregion [Private]

        #region [OneWay]
        /// <summary>
        /// 是否已关闭
        /// </summary>
        public bool IsClosed { get; private set; } = false;
        #endregion [OneWay]

        #region 项目集合
        /// <summary>
        /// 项目集合
        /// </summary>
        public ObservableCollection<MenuItemModel> Items
        {
            get { return (ObservableCollection<MenuItemModel>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register(
                nameof(Items),
                typeof(ObservableCollection<MenuItemModel>),
                typeof(MenuWindow),
                new PropertyMetadata(new ObservableCollection<MenuItemModel>()));
        #endregion

        #region 转换器
        /// <summary>
        /// 转换器
        /// </summary>
        public IValueConverter Converter
        {
            get { return (IValueConverter)GetValue(ConverterProperty); }
            set { SetValue(ConverterProperty, value); }
        }
        public static readonly DependencyProperty ConverterProperty =
            DependencyProperty.Register(
                nameof(Converter),
                typeof(IValueConverter),
                typeof(MenuWindow),
                new PropertyMetadata(new Object2StringConverter()));
        #endregion
        #endregion 【DependencyProperties】

        #region 【Ctor】
        public MenuWindow(Control control, ObservableCollection<MenuItemModel>? items)
        {
            InitializeComponent();
            _control = control;
            Opacity = 0;
            Loaded += OnLoaded;
            Closed += OnClosed;
            Deactivated += OnDeactivated;
            AddEvents();
            InitItems(items);
            InitControlPosition();
        }
        #endregion 【Ctor】

        #region 【Events】
        #region 加载完成
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            InitWindow();
        }
        #endregion

        #region 关闭后
        private void OnClosed(object? sender, EventArgs e)
        {
            IsClosed = true;
        }
        #endregion

        #region 失活后
        private void OnDeactivated(object? sender, EventArgs e)
        {
            SetHeightAndClose();
        }
        #endregion

        #region 项目被点击
        private void OnItemClicked(MenuItemModel itemModel)
        {
            SetHeightAndClose();
            _itemClicked?.Invoke(itemModel);
        }
        #endregion
        #endregion 【Events】

        #region 【Functions】
        #region [Static]
        #region 判断“是否已显示弹窗”
        public static bool IsShowed(Control control)
        {
            return _openedMenuWindow != null && Equals(control, _openedMenuWindow._control);
        }
        #endregion

        #region 判断“是否已全部关闭”
        public static bool IsAllClosed(Control control)
        {
            return _openedMenuWindow == null || Equals(control, _openedMenuWindow._control) && _openedMenuWindow.IsClosed;
        }
        #endregion
        #endregion [Static]

        #region [Private]
        #region 初始化“窗口”
        private void InitWindow()
        {
            InitItems();
            InitMenuSize();

            switch (_openDirection)
            {
                case Direction.Top:
                    // 位置：
                    Left = _rectMenu.Left;
                    SetTop(_rectControl.Top, _rectMenu.Top);
                    // 尺寸：
                    SetHeight(0, _rectMenu.Height);
                    break;
                case Direction.Bottom:
                    // 位置：
                    Left = _rectMenu.Left;
                    Top = _rectMenu.Top;
                    // 尺寸：
                    SetHeight(0, _rectMenu.Height);
                    break;
                case Direction.Left:
                    // 位置：
                    SetLeft(_rectControl.Left, _rectMenu.Left);
                    Top = _rectMenu.Top;
                    // 尺寸：
                    SetWidth(0, _rectMenu.Width);
                    break;
                case Direction.Right:
                    // 位置：
                    Left = _rectMenu.Left;
                    Top = _rectMenu.Top;
                    // 尺寸：
                    SetWidth(0, _rectMenu.Width);
                    break;
                default:
                    break;
            }

            Opacity = 1;
        }
        #endregion

        #region 初始化“控件边框”
        private void InitControlPosition()
        {
            var position = ScreenHelper.GetScreenPosition(_control);
            if (position == null)
            {
                LogHelper.Instance.IsNull(nameof(position));
                return;
            }

            _rectControl = new Rectangle2D(position, _control.ActualWidth, _control.ActualHeight);
        }
        #endregion

        #region 初始化“项目集合”
        public void InitItems(ObservableCollection<MenuItemModel>? items = null)
        {
            if (items != null)
            {
                Items = items;
            }
            else if (Items == null)
            {
                Items = new ObservableCollection<MenuItemModel>();
            }

            foreach (var item in Items)
            {
                item.Converter = Converter;
                item._internalClicked = OnItemClicked;
            }
        }
        #endregion

        #region 初始化“菜单尺寸”
        private void InitMenuSize()
        {
            #region 使用“项目集合”的“实际高度”

            if (itemsControl.ActualHeight > MinHeight && itemsControl.ActualHeight < MaxHeight)
            {
                Height = itemsControl.ActualHeight;
            }
            #endregion 使用“项目集合”的“实际高度”

            #region 获取“所在屏幕信息”
            var screenInfos = ScreenHelper.GetScreenInfos();
            if (screenInfos == null)
            {
                LogHelper.Instance.IsNull(nameof(screenInfos));
                return;
            }

            var screenIndex = ScreenHelper.GetIndexOfScreen(_rectControl);

            if (screenIndex < 0 || screenIndex >= screenInfos.Count)
            {
                LogHelper.Instance.IsOutOfRange(nameof(screenIndex));
                return;
            }

            var screenInfo = screenInfos[screenIndex];
            #endregion 获取“所在屏幕信息”

            _rectMenu = new Rectangle2D(new Point2D(), Width, Height);

            #region 选择“打开方向”
            // 下：
            _rectMenu.ToBottom(_rectControl);
            if (screenInfo.VirtualWorkingAreaRect.IsIn(_rectMenu))
            {
                _openDirection = Direction.Bottom;
                return;
            }

            // 上：
            _rectMenu.ToTop(_rectControl);
            if (screenInfo.VirtualWorkingAreaRect.IsIn(_rectMenu))
            {
                _openDirection = Direction.Top;
                return;
            }

            // 右：
            _rectMenu.ToRight(_rectControl);
            if (screenInfo.VirtualWorkingAreaRect.IsIn(_rectMenu))
            {
                _openDirection = Direction.Right;
                return;
            }

            // 左：
            _rectMenu.ToLeft(_rectControl);
            if (screenInfo.VirtualWorkingAreaRect.IsIn(_rectMenu))
            {
                _openDirection = Direction.Left;
                return;
            }

            // 下：
            _rectMenu.ToBottom(_rectControl);
            _openDirection = Direction.Bottom;
            #endregion 选择“打开方向”
        }
        #endregion

        #region 设置“上”
        /// <summary>
        /// 设置“上”
        /// </summary>
        private void SetTop(double from, double to, Action? completed = null)
        {
            Storyboard storyboard = new Storyboard();

            // 渐变动画：
            var gradient = DoubleAnimations.Gradient(
                this,
                TopProperty,
                Generic.DurationTotalSeconds,
                from,
                to);
            storyboard.Children.Add(gradient);

            // 开始storyboard：
            storyboard.Begin();
        }
        #endregion

        #region 设置“左”
        /// <summary>
        /// 设置“左”
        /// </summary>
        private void SetLeft(double from, double to, Action? completed = null)
        {
            Storyboard storyboard = new Storyboard();

            // 渐变动画：
            var gradient = DoubleAnimations.Gradient(
                this,
                LeftProperty,
                Generic.DurationTotalSeconds,
                from,
                to);
            storyboard.Children.Add(gradient);

            // 开始storyboard：
            storyboard.Begin();
        }
        #endregion

        #region 设置“宽度”
        /// <summary>
        /// 设置“宽度”
        /// </summary>
        private void SetWidth(double from, double to, Action? completed = null)
        {
            Storyboard storyboard = new Storyboard();

            // 渐变动画：
            var gradient = DoubleAnimations.Gradient(
                this,
                WidthProperty,
                Generic.DurationTotalSeconds,
                from,
                to);
            storyboard.Children.Add(gradient);

            VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;

            // 完成后：
            storyboard.Completed += (s, args) =>
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                completed?.Invoke();
            };

            // 开始storyboard：
            storyboard.Begin();
        }
        #endregion

        #region 设置“高度”
        /// <summary>
        /// 设置“高度”
        /// </summary>
        private void SetHeight(double from, double to, Action? completed = null)
        {
            Storyboard storyboard = new Storyboard();

            // 渐变动画：
            var gradient = DoubleAnimations.Gradient(
                this,
                HeightProperty,
                Generic.DurationTotalSeconds,
                from,
                to);
            storyboard.Children.Add(gradient);

            VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;

            // 完成后：
            storyboard.Completed += (s, args) =>
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                completed?.Invoke();
            };

            // 开始storyboard：
            storyboard.Begin();
        }
        #endregion

        #region 添加“事件”
        private void AddEvents()
        {
            Window window = GetWindow(_control);
            if (window == null)
            {
                LogHelper.Instance.IsNull(nameof(window));
                return;
            }

            window.Closing += (s, args) =>
            {
                Close();
            }; // 防止“主进程无法结束”
        }
        #endregion
        #endregion [Private]

        #region [Override]
        #region 显示
        public new void Show()
        {
            if (IsShowed(_control) || !IsAllClosed(_control)) return; // 防止“多开”

            _openedMenuWindow?.SetHeightAndClose();
            _openedMenuWindow = this;

            new ActionTimer(50, false, () =>
            {
                base.Show();
            }).Start(); // 防止“漏失活事件”
        }
        #endregion

        #region 关闭
        private new void Close()
        {
            if (IsClosed) return;

            try
            {
                base.Close();
                _closed?.Invoke();
                _openedMenuWindow = null;
            }
            catch (Exception e)
            {
                LogHelper.Instance.Error(e.Message);
            }
        }
        #endregion
        #endregion [Override]

        #region 设置高度并关闭
        public void SetHeightAndClose()
        {
            switch (_openDirection)
            {
                case Direction.Top:
                    // 位置：
                    SetTop(_rectMenu.Top, _rectControl.Top);
                    // 尺寸：
                    SetHeight(Height, 0, Close);
                    break;
                case Direction.Bottom:
                    // 尺寸：
                    SetHeight(Height, 0, Close);
                    break;
                case Direction.Left:
                    // 位置：
                    SetLeft(_rectMenu.Left, _rectControl.Left);
                    // 尺寸：
                    SetWidth(Width, 0, Close);
                    break;
                case Direction.Right:
                    // 尺寸：
                    SetWidth(Width, 0, Close);
                    break;
                default:
                    break;
            }
        }
        #endregion
        #endregion 【Functions】
    }

    #region 设计数据
    public class MenuItemDesignModel : MenuItemModel
    {
    }

    public class MenuWindowDesignData
    {
        public ObservableCollection<MenuItemDesignModel> Items { get; set; } = new ObservableCollection<MenuItemDesignModel>();

        public MenuWindowDesignData()
        {
            Items.Add(new MenuItemDesignModel() { Source = "Item 1" });
            Items.Add(new MenuItemDesignModel() { Source = "Item 2" });
            Items.Add(new MenuItemDesignModel() { Source = "Item 3" });
        }
    }
    #endregion
}
