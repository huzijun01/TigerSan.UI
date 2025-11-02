using System.Windows.Input;
using System.Windows.Media;
using TigerSan.PathOperation;

namespace TigerSan.UI.ViewModels
{
    #region 数据模型
    public class AboutViewDataModel : BindableBase
    {
        #region 【Properties】
        /// <summary>
        /// 图片
        /// </summary>
        public ImageSource Image
        {
            get { return _Image; }
            set { SetProperty(ref _Image, value); }
        }
        private ImageSource _Image = Generic.Hello;

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value); }
        }
        private string _Title;

        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName
        {
            get { return _appName; }
            set { SetProperty(ref _appName, value); }
        }
        private string _appName = string.Empty;

        /// <summary>
        /// 作者
        /// </summary>
        public string Author
        {
            get { return _author; }
            set { SetProperty(ref _author, value); }
        }
        private string _author = string.Empty;

        /// <summary>
        /// 版本
        /// </summary>
        public string Version
        {
            get { return _version; }
            set { SetProperty(ref _version, value); }
        }
        private string _version = string.Empty;

        /// <summary>
        /// 下载链接
        /// </summary>
        public string DownloadUrl
        {
            get { return _downloadUrl; }
            set { SetProperty(ref _downloadUrl, value); }
        }
        private string _downloadUrl = string.Empty;

        /// <summary>
        /// 主页链接
        /// </summary>
        public string HomePageUrl
        {
            get { return _homePageUrl; }
            set { SetProperty(ref _homePageUrl, value); }
        }
        private string _homePageUrl = string.Empty;
        #endregion 【Properties】

        #region 【Ctor】
        public AboutViewDataModel()
        {
            _Title = Generic._defaultTitle;
        }

        public AboutViewDataModel(AboutViewDataModel dataModel)
        {
            Author = dataModel.Author;
            AppName = dataModel.AppName;
            Version = dataModel.Version;
            DownloadUrl = dataModel.DownloadUrl;
            HomePageUrl = dataModel.HomePageUrl;
            #region 标题
            if (string.IsNullOrEmpty(dataModel.Title))
            {
                _Title = Generic._defaultTitle;
            }
            else
            {
                _Title = dataModel.Title;
            }
            #endregion 标题
        }
        #endregion 【Ctor】
    }
    #endregion

    public class AboutViewModel : AboutViewDataModel
    {
        #region 【Fields】
        /// <summary>
        /// 视图模型
        /// </summary>
        public static AboutViewModel? _viewModel;
        #endregion

        #region 【Ctor】
        public AboutViewModel(AboutViewDataModel dataModel) : base(dataModel)
        {
            _viewModel = this;
        }
        #endregion 【Ctor】

        #region 【Commands】
        #region “窗口”关闭
        public ICommand Window_ClosedCommand { get => new DelegateCommand(Window_Closed); }
        public void Window_Closed()
        {
            _viewModel = null;
        }
        #endregion

        #region 点击“下载链接”
        public ICommand tbkDownloadUrl_MouseDownCommand { get => new DelegateCommand(tbkDownloadUrl_MouseDown); }
        public void tbkDownloadUrl_MouseDown()
        {
            ExeHelper.OpenUrl(DownloadUrl);
        }
        #endregion

        #region 点击“主页链接”
        public ICommand tbkHomePageUrl_MouseDownCommand { get => new DelegateCommand(tbkHomePageUrl_MouseDown); }
        public void tbkHomePageUrl_MouseDown()
        {
            ExeHelper.OpenUrl(HomePageUrl);
        }
        #endregion
        #endregion 【Commands】
    }

    #region 设计数据
    public class DesignAboutViewModel : AboutViewDataModel
    {
        public DesignAboutViewModel()
        {
            AppName = "TigerSan.UI";
            Author = "bilibili 真的虎子君";
            Version = "1.0.0";
            DownloadUrl = "https://pan.quark.cn/s/2882f8436538";
            HomePageUrl = "https://space.bilibili.com/34323512";
        }
    }
    #endregion
}
