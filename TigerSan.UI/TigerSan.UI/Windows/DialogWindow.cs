using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using TigerSan.CsvLog;

namespace TigerSan.UI.Windows
{
    #region 弹窗结果
    public enum DialogResults
    {
        Cancel,
        Yes,
        No,
    }
    #endregion

    #region 弹窗事件
    /// <summary>
    /// 
    /// </summary>
    public delegate void DialogEvent(DialogResults result);
    public delegate Task DialogAsyncEvent(DialogResults result);
    #endregion

    public class DialogWindow : Window
    {
        #region 【Fields】
        private Grid? _mainPanel;
        private Border? _titlePanel;
        private Button? _btnClose;
        private ScrollViewer? _contentPanel;
        private Border? _buttonPanel;
        private Brush? _closeButtonForeground;
        private DialogResults _dialogResults = DialogResults.Cancel;
        private TaskCompletionSource<bool> _tcs = new TaskCompletionSource<bool>();
        #endregion

        #region 【Properties】
        /// <summary>
        /// 选择后
        /// </summary>
        public DialogEvent? OnSelected { get; set; }
        /// <summary>
        /// 选择后（异步）
        /// </summary>
        public DialogAsyncEvent? OnSelectedAsync { get; set; }
        #endregion 【Properties】

        #region 【DependencyProperties】
        #region 主容器可见性
        /// <summary>
        /// 主容器可见性
        /// </summary>
        public Visibility MainPanelVisibility
        {
            get { return (Visibility)GetValue(MainPanelVisibilityProperty); }
            private set { SetValue(MainPanelVisibilityProperty, value); }
        }
        public static readonly DependencyProperty MainPanelVisibilityProperty =
            DependencyProperty.Register(
                nameof(MainPanelVisibility),
                typeof(Visibility),
                typeof(DialogWindow),
                new PropertyMetadata(Visibility.Hidden));
        #endregion

        #region No按钮可见性
        /// <summary>
        /// No按钮可见性
        /// </summary>
        public Visibility NoButtonVisibility
        {
            get { return (Visibility)GetValue(NoButtonVisibilityProperty); }
            set { SetValue(NoButtonVisibilityProperty, value); }
        }
        public static readonly DependencyProperty NoButtonVisibilityProperty =
            DependencyProperty.Register(
                nameof(NoButtonVisibility),
                typeof(Visibility),
                typeof(DialogWindow),
                new PropertyMetadata(Visibility.Visible));
        #endregion

        #region Yes按钮可见性
        /// <summary>
        /// Yes按钮可见性
        /// </summary>
        public Visibility YesButtonVisibility
        {
            get { return (Visibility)GetValue(YesButtonVisibilityProperty); }
            set { SetValue(YesButtonVisibilityProperty, value); }
        }
        public static readonly DependencyProperty YesButtonVisibilityProperty =
            DependencyProperty.Register(
                nameof(YesButtonVisibility),
                typeof(Visibility),
                typeof(DialogWindow),
                new PropertyMetadata(Visibility.Visible));
        #endregion

        #region 按钮容器可见性
        /// <summary>
        /// 按钮容器可见性
        /// </summary>
        public Visibility ButtonPanelVisibility
        {
            get { return (Visibility)GetValue(ButtonPanelVisibilityProperty); }
            set { SetValue(ButtonPanelVisibilityProperty, value); }
        }
        public static readonly DependencyProperty ButtonPanelVisibilityProperty =
            DependencyProperty.Register(
                nameof(ButtonPanelVisibility),
                typeof(Visibility),
                typeof(DialogWindow),
                new PropertyMetadata(Visibility.Visible));
        #endregion

        #region 文本
        /// <summary>
        /// 文本
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                nameof(Text),
                typeof(string),
                typeof(DialogWindow),
                new PropertyMetadata(string.Empty));
        #endregion

        #region No文本
        /// <summary>
        /// No文本
        /// </summary>
        public string TextNo
        {
            get { return (string)GetValue(TextNoProperty); }
            set { SetValue(TextNoProperty, value); }
        }
        public static readonly DependencyProperty TextNoProperty =
            DependencyProperty.Register(
                nameof(TextNo),
                typeof(string),
                typeof(DialogWindow),
                new PropertyMetadata("No"));
        #endregion

        #region Yes文本
        /// <summary>
        /// Yes文本
        /// </summary>
        public string TextYes
        {
            get { return (string)GetValue(TextYesProperty); }
            set { SetValue(TextYesProperty, value); }
        }
        public static readonly DependencyProperty TextYesProperty =
            DependencyProperty.Register(
                nameof(TextYes),
                typeof(string),
                typeof(DialogWindow),
                new PropertyMetadata("Yes"));
        #endregion

        #region 标题背景色
        /// <summary>
        /// 标题背景色
        /// </summary>
        public Brush TitleBackground
        {
            get { return (Brush)GetValue(TitleBackgroundProperty); }
            set { SetValue(TitleBackgroundProperty, value); }
        }
        public static readonly DependencyProperty TitleBackgroundProperty =
            DependencyProperty.Register(
                nameof(TitleBackground),
                typeof(Brush),
                typeof(DialogWindow),
                new PropertyMetadata(new SolidColorBrush()));
        #endregion

        #region 关闭按钮前景色
        /// <summary>
        /// 关闭按钮前景色
        /// </summary>
        public Brush CloseButtonForeground
        {
            get { return (Brush)GetValue(CloseButtonForegroundProperty); }
            set { SetValue(CloseButtonForegroundProperty, value); }
        }
        public static readonly DependencyProperty CloseButtonForegroundProperty =
            DependencyProperty.Register(
                nameof(CloseButtonForeground),
                typeof(Brush),
                typeof(DialogWindow),
                new PropertyMetadata(Generic.BasicWhite));
        #endregion

        #region 关闭按钮悬浮前景色
        /// <summary>
        /// 关闭按钮悬浮前景色
        /// </summary>
        public Brush CloseButtonHoverForeground
        {
            get { return (Brush)GetValue(CloseButtonHoverForegroundProperty); }
            set { SetValue(CloseButtonHoverForegroundProperty, value); }
        }
        public static readonly DependencyProperty CloseButtonHoverForegroundProperty =
            DependencyProperty.Register(
                nameof(CloseButtonHoverForegroundProperty),
                typeof(Brush),
                typeof(DialogWindow),
                new PropertyMetadata(Generic.Brand));
        #endregion
        #endregion 【DependencyProperties】

        #region 【Ctor】
        static DialogWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogWindow), new FrameworkPropertyMetadata(typeof(DialogWindow)));
        }

        public DialogWindow()
        {
            Style = Generic.DialogWindowStyle;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Loaded += DialogWindow_Loaded;
            MouseDown += DialogWindow_MouseDown;
        }
        #endregion 【Ctor】

        #region 【Events】
        #region 加载完成
        private void DialogWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var dialog = (DialogWindow)sender;
            if (dialog == null)
            {
                LogHelper.Instance.IsNull(nameof(dialog));
                return;
            }

            Init();

            MainPanelVisibility = Visibility.Visible;
        }
        #endregion

        #region 鼠标按下
        private void DialogWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
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
        }
        #endregion

        #region 鼠标进入关闭按钮
        private void btnClose_MouseEnter(object sender, MouseEventArgs e)
        {
            _closeButtonForeground = CloseButtonForeground;
            CloseButtonForeground = CloseButtonHoverForeground;
        }
        #endregion

        #region 鼠标进入关闭按钮
        private void btnClose_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_closeButtonForeground == null)
            {
                LogHelper.Instance.IsNull(nameof(_closeButtonForeground));
                return;
            }

            CloseButtonForeground = _closeButtonForeground;
        }
        #endregion
        #endregion

        #region 【Commands】
        #region 点击“取消”按钮
        public ICommand btnClose_ClickCommand { get => new DelegateCommand(btnClose_Click); }
        private void btnClose_Click()
        {
            _dialogResults = DialogResults.Cancel;
            _tcs.TrySetResult(true);
        }
        #endregion

        #region 点击“否”按钮
        public ICommand btnNo_ClickCommand { get => new DelegateCommand(btnNo_Click); }
        private void btnNo_Click()
        {
            _dialogResults = DialogResults.No;
            _tcs.TrySetResult(true);
        }
        #endregion

        #region 点击“是”按钮
        public ICommand btnYes_ClickCommand { get => new DelegateCommand(btnYes_Click); }
        private void btnYes_Click()
        {
            _dialogResults = DialogResults.Yes;
            _tcs.TrySetResult(true);
        }
        #endregion
        #endregion 【Commands】

        #region 【Functions】
        #region 初始化
        private void Init()
        {
            #region 获取元素
            _mainPanel = GetTemplateChild("mainPanel") as Grid;
            _titlePanel = GetTemplateChild("titlePanel") as Border;
            _btnClose = GetTemplateChild("btnClose") as Button;
            _contentPanel = GetTemplateChild("contentPanel") as ScrollViewer;
            _buttonPanel = GetTemplateChild("buttonPanel") as Border;

            if (_mainPanel == null)
            {
                LogHelper.Instance.IsNull(nameof(_mainPanel));
                return;
            }

            if (_titlePanel == null)
            {
                LogHelper.Instance.IsNull(nameof(_titlePanel));
                return;
            }

            if (_btnClose == null)
            {
                LogHelper.Instance.IsNull(nameof(_btnClose));
                return;
            }

            if (_contentPanel == null)
            {
                LogHelper.Instance.IsNull(nameof(_contentPanel));
                return;
            }

            if (_buttonPanel == null)
            {
                LogHelper.Instance.IsNull(nameof(_buttonPanel));
                return;
            }
            #endregion 获取元素

            #region 设置宽高
            var maxContentPanelWidth = MaxWidth;
            var maxContentPanelHeight = MaxHeight - _titlePanel.ActualHeight - _buttonPanel.ActualHeight;

            if (_contentPanel.ActualWidth <= maxContentPanelWidth
               && _contentPanel.ActualHeight <= maxContentPanelHeight)
            {
                Width = Math.Ceiling(_contentPanel.ActualWidth);
                Height = Math.Ceiling(_contentPanel.ActualHeight + _titlePanel.ActualHeight + _buttonPanel.ActualHeight);
            }

            CenterScreen();
            #endregion 设置宽高

            #region 添加事件
            _btnClose.MouseEnter -= btnClose_MouseEnter;
            _btnClose.MouseLeave -= btnClose_MouseLeave;
            _btnClose.MouseEnter += btnClose_MouseEnter;
            _btnClose.MouseLeave += btnClose_MouseLeave;
            #endregion 添加事件
        }
        #endregion

        #region 移至屏幕中心
        private void CenterScreen()
        {
            // 获取主屏幕的工作区域
            var workingArea = SystemParameters.WorkArea;

            // 设置窗口的位置
            Left = (workingArea.Width - Width) / 2 + workingArea.Left;
            Top = (workingArea.Height - Height) / 2 + workingArea.Top;
        }
        #endregion

        #region 显示
        public new async Task<DialogResults> Show()
        {
            // 显示窗口：
            base.Show();

            // 等待用户点击按钮：
            await _tcs.Task;

            // 执行委托：
            OnSelected?.Invoke(_dialogResults);
            OnSelectedAsync?.BeginInvoke(_dialogResults, null, null);

            // 关闭窗口：
            Close();

            return _dialogResults;
        }
        #endregion
        #endregion 【Functions】
    }
}
