using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using TigerSan.UI.Models;
using TigerSan.UI.Helpers;
using TigerSan.PathOperation;
using TigerSan.UI.Animations;

namespace TigerSan.UI.Controls
{
    public partial class NavBar : UserControl
    {
        #region 【Fields】
        /// <summary>
        /// 宽度
        /// </summary>
        private double _width = double.NaN;
        #endregion 【Fields】

        #region 【DependencyProperties】
        #region 图片
        /// <summary>
        /// 图片
        /// </summary>
        public ImageSource Logo
        {
            get { return (ImageSource)GetValue(LogoProperty); }
            set { SetValue(LogoProperty, value); }
        }
        public static readonly DependencyProperty LogoProperty =
            DependencyProperty.Register(
                nameof(Logo),
                typeof(ImageSource),
                typeof(NavBar),
                new PropertyMetadata(Generic.Tiger));
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
                typeof(NavBar),
                new PropertyMetadata(Generic._defaultTitle));
        #endregion

        #region 地址
        /// <summary>
        /// 地址
        /// </summary>
        public string Url
        {
            get { return (string)GetValue(UrlProperty); }
            set { SetValue(UrlProperty, value); }
        }
        public static readonly DependencyProperty UrlProperty =
            DependencyProperty.Register(
                nameof(Url),
                typeof(string),
                typeof(NavBar),
                new PropertyMetadata(Generic._defaultUrl));
        #endregion

        #region Logo可见性
        /// <summary>
        /// Logo可见性
        /// </summary>
        public Visibility LogoVisibility
        {
            get { return (Visibility)GetValue(LogoVisibilityProperty); }
            set { SetValue(LogoVisibilityProperty, value); }
        }
        public static readonly DependencyProperty LogoVisibilityProperty =
            DependencyProperty.Register(
                nameof(LogoVisibility),
                typeof(Visibility),
                typeof(NavBar),
                new PropertyMetadata(Visibility.Visible));
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
                typeof(NavBar),
                new PropertyMetadata(true, IsOpenChanged));

        private static void IsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = (NavBar)d;
            sender.UpdateState();
        }
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
                typeof(NavBar),
                new PropertyMetadata(new NavBarModel(), NavBarModelChanged));

        private static void NavBarModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = (NavBar)d;
            sender.AddHeightGetter();
        }
        #endregion
        #endregion 【DependencyProperties】

        #region 【Ctor】
        public NavBar()
        {
            InitializeComponent();
            Init();
            _width = Width;
        }
        #endregion 【Ctor】

        #region 【Events】
        #region 点击“遮罩”
        private void Mask_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ExeHelper.OpenUrl(Url);
        }
        #endregion
        #endregion 【Events】

        #region 【Functions】
        #region 初始化
        private void Init()
        {
            if (string.IsNullOrEmpty(Url)) return;

            mask.Cursor = Cursors.Hand;
            mask.MouseLeftButtonDown += Mask_MouseLeftButtonDown;
        }
        #endregion

        #region 更新状态
        public void UpdateState()
        {
            if (IsOpen)
            {
                Open();
            }
            else
            {
                Close();
            }
        }
        #endregion

        #region 添加“高度获取器”
        private void AddHeightGetter()
        {
            NavBarModel._getFolderHeight = () => tempFolder.ActualHeight;
            NavBarModel._getButtonHeight = () => tempButton.ActualHeight;
        }
        #endregion

        #region 打开
        private void Open()
        {
            if (Equals(_width, double.NaN))
            {
                _width = ActualWidth;
            }

            new DoubleGradientStoryboard(
                this,
                WidthProperty,
                Generic.DurationTotalSeconds,
                0,
                _width).Begin();
        }
        #endregion

        #region 关闭
        private void Close()
        {
            if (Equals(_width, double.NaN))
            {
                _width = ActualWidth;
            }

            new DoubleGradientStoryboard(
                this,
                WidthProperty,
                Generic.DurationTotalSeconds,
                _width,
                0).Begin();
        }
        #endregion
        #endregion 【Functions】
    }

    #region 设计数据
    public class NavBarDesignData : UserControl
    {
        #region 【Properties】
        public ImageSource Logo { get; set; } = Generic.Tiger;
        public string Title { get; set; } = Generic._defaultTitle;
        public string Url { get; set; } = Generic._defaultUrl;
        public bool IsOpen { get; set; } = true;
        public Visibility LogoVisibility { get; set; } = Visibility.Visible;
        public NavBarModel NavBarModel { get; set; } = new NavBarModel();
        #endregion 【Properties】

        #region 【Ctor】
        public NavBarDesignData()
        {
            var navBarModel = new NavBarModel();
            Init(navBarModel, navBarModel.FolderModel);
        }
        #endregion 【Ctor】

        #region 【Commands】
        #region 点击“文件夹”
        public static ICommand folder_ClickCommand { get => new DelegateCommand<NavFolderModel>(folder_Click); }
        private static void folder_Click(NavFolderModel folderModel)
        {
            MsgBox.ShowInformation($"NavFolder has been clicked! ({folderModel.Title})");
        }
        #endregion

        #region 打开“文件夹”
        public static ICommand folder_OpenedCommand { get => new DelegateCommand<NavFolderModel>(folder_Opened); }
        private static void folder_Opened(NavFolderModel folderModel)
        {
            MsgBox.ShowInformation($"NavFolder has been opened! ({folderModel.Title})");
        }
        #endregion

        #region 关闭“文件夹”
        public static ICommand folder_ClosedCommand { get => new DelegateCommand<NavFolderModel>(folder_ClosedClick); }
        private static void folder_ClosedClick(NavFolderModel folderModel)
        {
            MsgBox.ShowInformation($"NavFolder has been closed! ({folderModel.Title})");
        }
        #endregion

        #region 点击“按钮”
        public static ICommand btn_ClickCommand { get => new DelegateCommand<NavButtonModel>(btn_Click); }
        private static void btn_Click(NavButtonModel buttonModel)
        {
            MsgBox.ShowInformation($"NavButton has been clicked! ({buttonModel.Title})");
        }
        #endregion

        #region 选中“按钮”
        public static ICommand btn_CheckedCommand { get => new DelegateCommand<NavButtonModel>(btn_Checked); }
        private static void btn_Checked(NavButtonModel buttonModel)
        {
            MsgBox.ShowInformation($"NavButton has been checked! ({buttonModel.Title})");
        }
        #endregion
        #endregion 【Commands】

        #region 【Functions】
        #region 初始化
        public static void Init(
            NavBarModel navBarModel,
            NavFolderModel folderModel,
            bool isAddAddCommands = true)
        {
            folderModel.FolderModels.Clear();

            // 顶级文件夹：
            var topFolder = new NavFolderModel(navBarModel) { Title = "顶级文件夹 1" };
            folderModel.AddFolder(topFolder);

            // 顶级按钮：
            folderModel.AddButton(new NavButtonModel(navBarModel, folderModel) { Title = "顶级按钮 1", Visibility = Visibility.Collapsed });
            folderModel.AddButton(new NavButtonModel(navBarModel, folderModel) { Title = "顶级按钮 2" });
            folderModel.AddButton(new NavButtonModel(navBarModel, folderModel) { Title = "顶级按钮 3" });

            // 子目录：
            var subFolder = new NavFolderModel(navBarModel) { Title = "子文件夹 1" };
            subFolder.AddButton(new NavButtonModel(navBarModel, subFolder) { Title = "子文件 1" });
            subFolder.AddButton(new NavButtonModel(navBarModel, subFolder) { Title = "子文件 2" });
            subFolder.AddButton(new NavButtonModel(navBarModel, subFolder) { Title = "子文件 3" });
            topFolder.AddFolder(subFolder);

            folderModel.AddFolder(new NavFolderModel(navBarModel) { Title = "文件夹 2", Visibility = Visibility.Collapsed });

            if (isAddAddCommands)
            {
                AddCommands(folderModel);
            }
        }
        #endregion

        #region 添加“命令”
        private static void AddCommands(NavFolderModel folderModel)
        {
            NavFolderModel.RecursivelyOperateSubItems(
                folderModel,
                folderModel =>
                {
                    folderModel.Command = folder_ClickCommand;
                    folderModel.OpenedCommand = folder_OpenedCommand;
                    folderModel.ClosedCommand = folder_ClosedCommand;
                },
                buttonModel =>
                {
                    buttonModel.Command = btn_ClickCommand;
                    buttonModel.CheckedCommand = btn_CheckedCommand;
                });
        }
        #endregion
        #endregion 【Functions】
    }
    #endregion
}
