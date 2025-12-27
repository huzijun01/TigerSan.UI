using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Collections.ObjectModel;
using TigerSan.UI.Models;
using TigerSan.UI.Helpers;
using TigerSan.UI.Animations;
using TigerSan.TimerHelper.WPF;

namespace TigerSan.UI.Controls
{
    public partial class NavFolder : UserControl
    {
        #region 【Fields】
        /// <summary>
        /// 打开时的箭头角度
        /// </summary>
        private static double _arrowAngleOpen = 90.0;

        /// <summary>
        /// 关闭时的箭头角度
        /// </summary>
        private static double _arrowAngleClose = 0;

        /// <summary>
        /// 前景动画
        /// </summary>
        private BrushGradientAnimation _fgAnimation;

        /// <summary>
        /// 背景动画
        /// </summary>
        private BrushGradientAnimation _bgcAnimation;

        /// <summary>
        /// 旧高度
        /// </summary>
        private bool _oldIsOpened;

        /// <summary>
        /// 旧高度
        /// </summary>
        private double _oldHeight = 0;
        #endregion 【Fields】

        #region 【DependencyProperties】
        #region [OneWay]
        #region 背景
        /// <summary>
        /// 背景
        /// </summary>
        public new Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            private set { SetValue(BackgroundProperty, value); }
        }
        public static new readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register(
                nameof(Background),
                typeof(Brush),
                typeof(NavFolder),
                new PropertyMetadata(Generic.Transparent));
        #endregion

        #region 箭头角度
        /// <summary>
        /// 箭头角度
        /// </summary>
        public double ArrowAngle
        {
            get { return (double)GetValue(ArrowAngleProperty); }
            private set { SetValue(ArrowAngleProperty, value); }
        }
        public static readonly DependencyProperty ArrowAngleProperty =
            DependencyProperty.Register(
                nameof(ArrowAngle),
                typeof(double),
                typeof(NavFolder),
                new PropertyMetadata(_arrowAngleOpen));
        #endregion
        #endregion [OneWay]

        #region 图标
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(
                nameof(Icon),
                typeof(string),
                typeof(NavFolder),
                new PropertyMetadata(Icons.Folder_Linear));
        #endregion

        #region 标题
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                nameof(Title),
                typeof(string),
                typeof(NavFolder),
                new PropertyMetadata("null"));
        #endregion

        #region 是否打开
        /// <summary>
        /// 是否打开
        /// </summary>
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register(
                nameof(IsOpen),
                typeof(bool),
                typeof(NavFolder),
                new PropertyMetadata(true, IsOpenChanged));
        private static void IsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = (NavFolder)d;
            sender.UpdateArrowAngle();
            sender.UpdateOpenState();
        }
        #endregion

        #region 项目模板
        /// <summary>
        /// 项目模板
        /// </summary>
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register(
                nameof(ItemTemplate),
                typeof(DataTemplate),
                typeof(NavFolder),
                new PropertyMetadata(new DataTemplate()));
        #endregion

        #region “导航栏”模型
        /// <summary>
        /// “导航栏”模型
        /// </summary>
        public NavBarModel NavBarModel
        {
            get { return (NavBarModel)GetValue(NavBarModelProperty); }
            set { SetValue(NavBarModelProperty, value); }
        }
        public static readonly DependencyProperty NavBarModelProperty =
            DependencyProperty.Register(
                nameof(NavBarModel),
                typeof(NavBarModel),
                typeof(NavFolder),
                new PropertyMetadata(new NavBarModel()));
        #endregion

        #region “文件夹模型”集合
        /// <summary>
        /// “文件夹模型”集合
        /// </summary>
        public ObservableCollection<NavFolderModel> FolderModels
        {
            get { return (ObservableCollection<NavFolderModel>)GetValue(FolderModelsProperty); }
            set { SetValue(FolderModelsProperty, value); }
        }
        public static readonly DependencyProperty FolderModelsProperty =
            DependencyProperty.Register(
                nameof(FolderModels),
                typeof(ObservableCollection<NavFolderModel>),
                typeof(NavFolder),
                new PropertyMetadata(new ObservableCollection<NavFolderModel>(), FolderModelsChanged));

        private static void FolderModelsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = (NavFolder)d;
            sender.UpdateOldState();
        }
        #endregion

        #region “按钮模型”集合
        /// <summary>
        /// “按钮模型”集合
        /// </summary>
        public ObservableCollection<NavButtonModel> ButtonModels
        {
            get { return (ObservableCollection<NavButtonModel>)GetValue(ButtonModelsProperty); }
            set { SetValue(ButtonModelsProperty, value); }
        }
        public static readonly DependencyProperty ButtonModelsProperty =
            DependencyProperty.Register(
                nameof(ButtonModels),
                typeof(ObservableCollection<NavButtonModel>),
                typeof(NavFolder),
                new PropertyMetadata(new ObservableCollection<NavButtonModel>()));
        #endregion

        #region “文件夹”模型
        /// <summary>
        /// “文件夹”模型
        /// </summary>
        public NavFolderModel FolderModel
        {
            get { return (NavFolderModel)GetValue(FolderModelProperty); }
            set { SetValue(FolderModelProperty, value); }
        }
        public static readonly DependencyProperty FolderModelProperty =
            DependencyProperty.Register(
                nameof(FolderModel),
                typeof(NavFolderModel),
                typeof(NavFolder),
                new PropertyMetadata(new NavFolderModel(new NavBarModel()), FolderModelChanged));

        private static void FolderModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = (NavFolder)d;
            sender.AddUpdateHeightHandler();
        }
        #endregion
        #endregion 【DependencyProperties】

        #region 【CustomEvents】
        #region 点击
        // CLR事件包装器：
        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        // 声明事件：
        public static readonly RoutedEvent ClickEvent =
            EventManager.RegisterRoutedEvent(
                nameof(Click),
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(NavFolder));

        // 触发事件的方法：
        protected void RaiseClickEvent()
        {
            RaiseEvent(new RoutedEventArgs(ClickEvent, this) { Source = FolderModel });
        }
        #endregion

        #region 打开
        public event RoutedEventHandler Opened
        {
            add { AddHandler(OpenedEvent, value); }
            remove { RemoveHandler(OpenedEvent, value); }
        }
        [Category("Behavior")]
        public static readonly RoutedEvent OpenedEvent =
        EventManager.RegisterRoutedEvent(
            nameof(Opened),
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(NavFolder));

        protected virtual void RaiseOpenedEvent()
        {
            RaiseEvent(new RoutedEventArgs(OpenedEvent, this) { Source = FolderModel });
        }
        #endregion

        #region 关闭
        public event RoutedEventHandler Closed
        {
            add { AddHandler(ClosedEvent, value); }
            remove { RemoveHandler(ClosedEvent, value); }
        }
        [Category("Behavior")]
        public static readonly RoutedEvent ClosedEvent =
        EventManager.RegisterRoutedEvent(
            nameof(Closed),
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(NavFolder));

        protected virtual void RaiseClosedEvent()
        {
            RaiseEvent(new RoutedEventArgs(ClosedEvent, this) { Source = FolderModel });
        }
        #endregion
        #endregion 【CustomEvents】

        #region 【CustomCommands】
        #region 命令
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                nameof(Command),
                typeof(ICommand),
                typeof(NavFolder),
                new PropertyMetadata(null));

        protected void RaiseCommand()
        {
            Command?.Execute(FolderModel);
        }
        #endregion

        #region 打开
        public ICommand OpenedCommand
        {
            get => (ICommand)GetValue(OpenedCommandProperty);
            set => SetValue(OpenedCommandProperty, value);
        }
        public static readonly DependencyProperty OpenedCommandProperty =
            DependencyProperty.Register(
                nameof(OpenedCommand),
                typeof(ICommand),
                typeof(NavFolder),
                new PropertyMetadata(null));

        protected void RaiseOpenedCommand()
        {
            OpenedCommand?.Execute(FolderModel);
        }
        #endregion

        #region 关闭
        public ICommand ClosedCommand
        {
            get => (ICommand)GetValue(ClosedCommandProperty);
            set => SetValue(ClosedCommandProperty, value);
        }
        public static readonly DependencyProperty ClosedCommandProperty =
            DependencyProperty.Register(
                nameof(ClosedCommand),
                typeof(ICommand),
                typeof(NavFolder),
                new PropertyMetadata(null));

        protected void RaiseClosedCommand()
        {
            ClosedCommand?.Execute(FolderModel);
        }
        #endregion
        #endregion 【CustomCommands】

        #region 【Ctor】
        public NavFolder()
        {
            InitializeComponent();
            Foreground = Generic.PrimaryText;
            _oldIsOpened = IsOpen;
            _bgcAnimation = new BrushGradientAnimation(SetBackground, Colors.Transparent);
            _fgAnimation = new BrushGradientAnimation(SetForeground, Generic.PrimaryText.Color);
            mask.MouseLeftButtonDown += mask_MouseLeftButtonDown;
            AddValueChanged();
            Loaded += NavFolder_Loaded;
        }
        #endregion 【Ctor】

        #region 【Events】
        #region 加载完成
        private void NavFolder_Loaded(object sender, RoutedEventArgs e)
        {
            ItemTemplate = Generic.NavFolderTemplate;
        }
        #endregion

        #region 鼠标按下“遮罩”
        private void mask_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsOpen = !IsOpen;

            RaiseCommand();
            RaiseClickEvent();
        }
        #endregion
        #endregion 【Events】

        #region 【Functions】
        #region [Public]
        #region 更新“打开状态”
        public void UpdateOpenState()
        {
            if (IsOpen)
            {
                Open();
            }
            else
            {
                Close();
            }

            _oldIsOpened = IsOpen;
        }
        #endregion
        #endregion [Public]

        #region 添加“值改变事件”
        private void AddValueChanged()
        {
            BindingHelper.AddValueChanged(
                mask,
                typeof(NavFolder),
                IsMouseOverProperty,
                (s, e) =>
                {
                    new ActionTimer(50, false, () =>
                    {
                        UpdateState();
                        new ActionTimer(50, false, UpdateState).Start(); // 防止漏事件
                    }).Start(); // 防止点击时失去焦点
                });
        }
        #endregion

        #region 添加“更新高度”回调
        private void AddUpdateHeightHandler()
        {
            FolderModel._updateFolderHeight = UpdateHeight;
        }
        #endregion

        #region 更新“高度”
        private void UpdateHeight()
        {
            double from;
            double to;

            var newHeight = FolderModel.GetSubItemsHeight();

            if (Equals(_oldHeight, newHeight)
                && Equals(_oldIsOpened, IsOpen)) return;

            _oldHeight = newHeight;

            if (IsOpen)
            {
                from = 0;
                to = newHeight;
            }
            else
            {
                from = newHeight;
                to = 0;
            }

            new DoubleGradientStoryboard(
                content,
                HeightProperty,
                Generic.DurationTotalSeconds,
                from,
                to).Begin();
        }
        #endregion

        #region 更新“状态”
        private void UpdateState()
        {
            if (!IsEnabled)
            {
                _fgAnimation.SetColor(Generic.DisabledText.Color);
                _bgcAnimation.SetColor(Generic.BaseBorder.Color);
            }
            else if (mask.IsMouseOver)
            {
                _fgAnimation.SetColor(Generic.Brand.Color);
                _bgcAnimation.SetColor(Colors.Transparent);
            }
            else
            {
                _fgAnimation.SetColor(Generic.PrimaryText.Color);
                _bgcAnimation.SetColor(Colors.Transparent);
            }
        }
        #endregion

        #region 更新“旧状态”
        private void UpdateOldState()
        {
            _oldIsOpened = IsOpen;
            _oldHeight = FolderModel.GetSubItemsHeight();
        }
        #endregion

        #region 更新“箭头角度”
        private void UpdateArrowAngle()
        {
            Storyboard storyboard = new Storyboard();

            var from = IsOpen ? _arrowAngleClose : _arrowAngleOpen;
            var to = IsOpen ? _arrowAngleOpen : _arrowAngleClose;

            // 渐变动画：
            var gradient = DoubleAnimations.Gradient(
                this,
                ArrowAngleProperty,
                Generic.DurationTotalSeconds,
                from,
                to);
            storyboard.Children.Add(gradient);

            // 开始storyboard：
            storyboard.Begin();
        }
        #endregion

        #region 设置“前景”
        private void SetForeground(Brush brush)
        {
            Foreground = brush;
        }
        #endregion

        #region 设置“背景”
        private void SetBackground(Brush brush)
        {
            Background = brush;
        }
        #endregion

        #region 打开
        private void Open()
        {
            FolderModel.IsOpen = true;

            NavBarModel.UpdateAllFoldersHeight();

            RaiseOpenedEvent();
            RaiseOpenedCommand();
        }
        #endregion

        #region 关闭
        private void Close()
        {
            FolderModel.IsOpen = false;

            NavBarModel.UpdateAllFoldersHeight();

            RaiseClosedEvent();
            RaiseClosedCommand();
        }
        #endregion
        #endregion 【Functions】
    }

    #region 设计数据
    public class NavFolderDesignData : UserControl
    {
        #region 【Fields】
        private static double _arrowAngleOpen = 90.0;
        //private static double _arrowAngleClose = 0;
        #endregion 【Fields】

        #region 【Properties】
        public double ArrowAngle { get; set; } = _arrowAngleOpen;
        public string Icon { get; set; }
        public string Title { get; set; }
        public bool IsOpen { get; set; } = true;
        public DataTemplate ItemTemplate { get; set; } = new DataTemplate();
        public ObservableCollection<NavFolderModel> FolderModels { get; set; } = new ObservableCollection<NavFolderModel>();
        public ObservableCollection<NavButtonModel> ButtonModels { get; set; } = new ObservableCollection<NavButtonModel>();
        #endregion 【Properties】

        #region 【Ctor】
        public NavFolderDesignData()
        {
            Icon = Icons.Folder_Linear;
            Title = "文件夹";
            Foreground = Generic.PrimaryText;
            var navBarModel = new NavBarModel();
            var folderModel = new NavFolderModel(navBarModel);
            FolderModels.Add(folderModel);
            NavBarDesignData.Init(navBarModel, folderModel);
        }
        #endregion 【Ctor】
    }
    #endregion
}
