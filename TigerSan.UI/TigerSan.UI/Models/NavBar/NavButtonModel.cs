using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using TigerSan.CsvLog;

namespace TigerSan.UI.Models
{
    public class NavButtonModel : BindableBase
    {
        #region 【Fields】
        public Type? _typePageView;
        public Type? _typePageViewModel;
        #endregion 【Fields】

        #region 【Properties】
        public NavBarModel NavBarModel
        {
            get { return _NavBarModel; }
            set { SetProperty(ref _NavBarModel, value); }
        }
        private NavBarModel _NavBarModel;

        public NavFolderModel NavFolderModel
        {
            get { return _NavFolderModel; }
            set { SetProperty(ref _NavFolderModel, value); }
        }
        private NavFolderModel _NavFolderModel;

        public string Icon
        {
            get { return _Icon; }
            set { SetProperty(ref _Icon, value); }
        }
        private string _Icon = Icons.File_Linear;

        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value); }
        }
        private string _Title = "null";

        public bool IsSelected
        {
            get { return _IsSelected; }
            set { SetProperty(ref _IsSelected, value); }
        }
        private bool _IsSelected = false;

        public Visibility Visibility
        {
            get { return _Visibility; }
            set { SetProperty(ref _Visibility, value); }
        }
        private Visibility _Visibility = Visibility.Visible;
        #endregion 【Properties】

        #region 【Commands】
        public ICommand? Command { get; set; }
        public ICommand? CheckedCommand { get; set; }
        #endregion 【Commands】

        #region 【Ctor】
        public NavButtonModel(
            NavBarModel navBarModel,
            NavFolderModel navFolderModel,
            Type? typePageView = null,
            Type? typePageViewModel = null)
        {
            _NavBarModel = navBarModel;
            _NavFolderModel = navFolderModel;
            _typePageView = typePageView;
            _typePageViewModel = typePageViewModel;
        }
        #endregion 【Ctor】

        #region 【Functions】
        #region 获取“页面视图”
        public UserControl? GetPageView()
        {
            if (_typePageView == null)
            {
                LogHelper.Instance.IsNull(nameof(_typePageView));
                return null;
            }

            var obj = Activator.CreateInstance(_typePageView);
            if (obj == null)
            {
                LogHelper.Instance.IsNull(nameof(obj));
                return null;
            }

            var pageView = obj as UserControl;
            if (pageView == null)
            {
                LogHelper.Instance.Warning($"The {nameof(pageView)} is not {nameof(UserControl)}!");
                return null;
            }

            return pageView;
        }
        #endregion

        #region 获取“页面视图模型”
        public BindableBase? GetPageViewModel()
        {
            if (_typePageViewModel == null)
            {
                LogHelper.Instance.IsNull(nameof(_typePageViewModel));
                return null;
            }

            var obj = Activator.CreateInstance(_typePageViewModel);
            if (obj == null)
            {
                LogHelper.Instance.IsNull(nameof(obj));
                return null;
            }

            var pageViewModel = obj as BindableBase;
            if (pageViewModel == null)
            {
                LogHelper.Instance.Warning($"The {nameof(pageViewModel)} is not {nameof(BindableBase)}!");
                return null;
            }

            return pageViewModel;
        }
        #endregion
        #endregion 【Functions】
    }
}
