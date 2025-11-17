using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shell;
using System.Windows.Interop;
using TigerSan.CsvLog;
using TigerSan.TimerHelper.WPF;
using TigerSan.UI.Helpers;

namespace TigerSan.UI.Windows
{
    public class CustomWindow : Window
    {
        #region 【Fields】
        /// <summary>
        /// 临时工具栏前景色
        /// </summary>
        private Brush? _toolBarForeground;

        /// <summary>
        /// 临时工具栏背景色
        /// </summary>
        private Brush? _toolBarBackground;

        /// <summary>
        /// “点击次数”计数器
        /// </summary>
        private ClickCounter _clickCounter = new ClickCounter(200);
        #endregion 【Fields】

        #region 【DependencyProperties】
        #region 边框内边距
        /// <summary>
        /// 边框内边距
        /// </summary>
        public Thickness BorderPadding
        {
            get { return (Thickness)GetValue(BorderPaddingProperty); }
            private set { SetValue(BorderPaddingProperty, value); }
        }
        public static readonly DependencyProperty BorderPaddingProperty =
            DependencyProperty.Register(
                nameof(BorderPadding),
                typeof(Thickness),
                typeof(CustomWindow),
                new PropertyMetadata(Generic.ZeroThickness));
        #endregion

        #region 工具栏前景色
        /// <summary>
        /// 工具栏前景色
        /// </summary>
        public Brush ToolBarForeground
        {
            get { return (Brush)GetValue(ToolBarForegroundProperty); }
            set { SetValue(ToolBarForegroundProperty, value); }
        }
        public static readonly DependencyProperty ToolBarForegroundProperty =
            DependencyProperty.Register(
                nameof(ToolBarForeground),
                typeof(Brush),
                typeof(CustomWindow),
                new PropertyMetadata(new SolidColorBrush()));
        #endregion

        #region 工具栏背景色
        /// <summary>
        /// 工具栏背景色
        /// </summary>
        public Brush ToolBarBackground
        {
            get { return (Brush)GetValue(ToolBarBackgroundProperty); }
            set { SetValue(ToolBarBackgroundProperty, value); }
        }
        public static readonly DependencyProperty ToolBarBackgroundProperty =
            DependencyProperty.Register(
                nameof(ToolBarBackground),
                typeof(Brush),
                typeof(CustomWindow),
                new PropertyMetadata(new SolidColorBrush()));
        #endregion

        #region 最小化按钮可见性
        /// <summary>
        /// 最小化按钮可见性
        /// </summary>
        public Visibility BtnMinVisibility
        {
            get { return (Visibility)GetValue(BtnMinVisibilityProperty); }
            set { SetValue(BtnMinVisibilityProperty, value); }
        }
        public static readonly DependencyProperty BtnMinVisibilityProperty =
            DependencyProperty.Register(
                nameof(BtnMinVisibility),
                typeof(Visibility),
                typeof(CustomWindow),
                new PropertyMetadata(Visibility.Visible));
        #endregion

        #region 最大化按钮可见性
        /// <summary>
        /// 最大化按钮可见性
        /// </summary>
        public Visibility BtnMaxVisibility
        {
            get { return (Visibility)GetValue(BtnMaxVisibilityProperty); }
            set { SetValue(BtnMaxVisibilityProperty, value); }
        }
        public static readonly DependencyProperty BtnMaxVisibilityProperty =
            DependencyProperty.Register(
                nameof(BtnMaxVisibility),
                typeof(Visibility),
                typeof(CustomWindow),
                new PropertyMetadata(Visibility.Visible));
        #endregion

        #region 尺寸调整模式
        /// <summary>
        /// 尺寸调整模式
        /// </summary>
        public new ResizeMode ResizeMode
        {
            get { return (ResizeMode)GetValue(ResizeModeProperty); }
            set { SetValue(ResizeModeProperty, value); }
        }
        public static new readonly DependencyProperty ResizeModeProperty =
            DependencyProperty.Register(
                nameof(ResizeMode),
                typeof(ResizeMode),
                typeof(CustomWindow),
                new PropertyMetadata(ResizeMode.CanResize, ResizeModeChanged));

        private static void ResizeModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = (CustomWindow)d;
            window.Init();
        }
        #endregion
        #endregion 【DependencyProperties】

        #region 【Ctor】
        public CustomWindow()
        {
            // 初始化图标:
            Icon = WindowHelper.GetIcon();
            #region 标题
            if (string.IsNullOrEmpty(Title))
            {
                Title = Generic._defaultTitle;
            }
            #endregion 标题
            // 添加事件：
            Loaded += OnLoaded;
            Activated += OnActivated;
            Deactivated += OnDeactivated;
            SizeChanged += OnSizeChanged;
            StateChanged += OnStateChanged;
            MouseDown += CustomWindow_MouseDown;
            SourceInitialized += OnSourceInitialized_NoResize;
        }
        #endregion 【Ctor】

        #region 【Events】
        #region 加载完成
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Init();
        }
        #endregion

        #region 鼠标按下
        private void CustomWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;

            try
            {
                DragMove();
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Error(ex.Message);
            }
        }
        #endregion

        #region 窗口尺寸改变时
        private void OnStateChanged(object? sender, EventArgs e)
        {
            if (ResizeMode != ResizeMode.CanResize && ResizeMode != ResizeMode.CanResizeWithGrip)
            {
                if (WindowState == WindowState.Maximized)
                {
                    WindowState = WindowState.Normal;
                }
            }
        }
        #endregion

        #region 窗口尺寸改变时
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            // 内边距：
            BorderPadding = WindowState == WindowState.Maximized ? Generic.WindowPadding : Generic.ZeroThickness;
        }
        #endregion

        #region 窗口激活时
        private void OnActivated(object? sender, EventArgs e)
        {
            ToolBarForeground = _toolBarForeground ?? Generic.BasicWhite;
            ToolBarBackground = _toolBarBackground ?? Generic.ToolBar_Active_Background;
        }
        #endregion

        #region 窗口停用时
        private void OnDeactivated(object? sender, EventArgs e)
        {
            _toolBarForeground = ToolBarForeground;
            ToolBarForeground = Generic.PlaceholderText;
            _toolBarBackground = ToolBarBackground;
            ToolBarBackground = Generic.ToolBar_Static_Background;
        }
        #endregion
        #endregion 【Events】

        #region 【Commands】
        #region 最小化
        public ICommand MinimizeCommand { get => new DelegateCommand(Minimize_Executed); }
        private void Minimize_Executed()
        {
            WindowState = WindowState.Minimized;
        }
        #endregion

        #region 最大化
        public ICommand MaximizeCommand { get => new DelegateCommand(Maximize_Executed); }
        private void Maximize_Executed()
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }
        #endregion

        #region 关闭
        public ICommand CloseCommand { get => new DelegateCommand(Close_Executed); }
        private void Close_Executed()
        {
            Close();
        }
        #endregion

        #region 左键点击“工具栏”
        public ICommand ToolBar_MouseLeftButtonDownCommand { get => new DelegateCommand(ToolBar_MouseLeftButtonDown); }
        private void ToolBar_MouseLeftButtonDown()
        {
            if (ResizeMode != ResizeMode.CanResize && ResizeMode != ResizeMode.CanResizeWithGrip) return;

            //[若“左键”未曾按下]，则开始计时。
            if (_clickCounter._count < 1)
            {
                _clickCounter.Start();
            }
            ++_clickCounter._count;

            if (_clickCounter._count < 2) return;

            Maximize_Executed();
        }
        #endregion
        #endregion 【Commands】

        #region 【Functions】
        #region 初始化
        public void Init()
        {
            #region 工具栏
            _toolBarForeground = ToolBarForeground;
            _toolBarBackground = ToolBarBackground;
            #endregion 工具栏

            #region 按钮
            switch (ResizeMode)
            {
                case ResizeMode.NoResize:
                    BtnMaxVisibility = Visibility.Collapsed;
                    WindowChrome.SetWindowChrome(this, Generic.NoResizeWindowChrome);
                    break;
                case ResizeMode.CanMinimize:
                    BtnMaxVisibility = Visibility.Collapsed;
                    WindowChrome.SetWindowChrome(this, Generic.NoResizeWindowChrome);
                    break;
                case ResizeMode.CanResize:
                case ResizeMode.CanResizeWithGrip:
                    BtnMaxVisibility = Visibility.Visible;
                    WindowChrome.SetWindowChrome(this, Generic.CustomWindowChrome);
                    break;
                default:
                    break;
            }
            #endregion 按钮
        }
        #endregion

        #region 源初始化完成（禁止修改尺寸）
        /// <summary>
        /// 窗口消息处理（禁止修改尺寸）
        /// </summary>
        private void OnSourceInitialized_NoResize(object? sender, EventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            if (hwnd == 0)
            {
                LogHelper.Instance.Warning($"The {nameof(hwnd)} is 0!");
                return;
            }

            if (ResizeMode != ResizeMode.CanResize && ResizeMode != ResizeMode.CanResizeWithGrip)
            {
                HwndSource.FromHwnd(hwnd).AddHook(SystemHelper.WndProc_NoResize);
            }
            else
            {
                HwndSource.FromHwnd(hwnd).RemoveHook(SystemHelper.WndProc_NoResize);
            }
        }
        #endregion
        #endregion 【Functions】
    }
}
