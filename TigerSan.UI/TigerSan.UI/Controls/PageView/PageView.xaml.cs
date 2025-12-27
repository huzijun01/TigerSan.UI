using System.Windows;
using System.Windows.Controls;
using TigerSan.UI.Models;

namespace TigerSan.UI.Controls
{
    public partial class PageView : UserControl
    {
        #region 【Fields】
        /// <summary>
        /// “缓存视图”集合
        /// </summary>
        private Dictionary<NavButtonModel, UserControl> _cacheViews = new Dictionary<NavButtonModel, UserControl>();
        #endregion 【Fields】

        #region 【DependencyProperties】
        #region 导航栏模型
        /// <summary>
        /// 导航栏模型
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
                typeof(PageView),
                new PropertyMetadata(new NavBarModel(), NavBarModelChanged));

        private static void NavBarModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = (PageView)d;
            sender.AddValueChanged();
            sender.UpdateContent();
        }
        #endregion
        #endregion 【DependencyProperties】

        #region 【Ctor】
        public PageView()
        {
            InitializeComponent();
        }
        #endregion 【Ctor】

        #region 【Functions】
        #region 更新“内容”
        public void UpdateContent()
        {
            // 清空“视图”:
            panel.Children.Clear();

            var selectedButtonModel = NavBarModel.SelectedButtonModel;
            if (selectedButtonModel == null) return;

            // 清除“多余缓存”:
            ClearViewCache();

            // 获取“视图”:
            UserControl? pageView;
            _cacheViews.TryGetValue(selectedButtonModel, out pageView);

            // 添加“缓存视图”:
            if (pageView == null)
            {
                pageView = GetPageView();
                if (pageView == null) return;

                _cacheViews.Add(selectedButtonModel, pageView);
            }

            // 添加“视图”:
            panel.Children.Add(pageView);
        }
        #endregion

        #region 清除“多余缓存视图”
        private void ClearViewCache()
        {
            // 多余缓存视图:
            var clearKeys = new List<NavButtonModel>();

            foreach (var cacheView in _cacheViews)
            {
                // 添加“多余缓存视图”：
                if (!NavBarModel.IsPageOpened(cacheView.Key))
                {
                    clearKeys.Add(cacheView.Key);
                }
            }

            // 清除“多余缓存视图”：
            foreach (var clearKey in clearKeys)
            {
                _cacheViews.Remove(clearKey);
            }
        }
        #endregion

        #region 获取“视图”
        private UserControl? GetPageView()
        {
            if (NavBarModel.SelectedButtonModel == null) return null;

            if (NavBarModel.SelectedButtonModel._typePageView == null) return null;

            var pageView = NavBarModel.SelectedButtonModel.GetPageView();
            if (pageView == null) return null;

            if (NavBarModel.SelectedButtonModel._typePageViewModel != null)
            {
                var pageViewModel = NavBarModel.SelectedButtonModel.GetPageViewModel();
                if (pageViewModel == null) return null;

                pageView.DataContext = pageViewModel;
            }

            return pageView;
        }
        #endregion

        #region 添加“值改变”事件
        public void AddValueChanged()
        {
            NavBarModel._onSelectedButtonModelChanged -= OnSelectedButtonModelChanged;
            NavBarModel._onSelectedButtonModelChanged += OnSelectedButtonModelChanged;
        }
        #endregion

        #region “选中按钮”改变后回调
        private void OnSelectedButtonModelChanged(NavButtonModel? buttonModel)
        {
            UpdateContent();
        }
        #endregion
        #endregion 【Functions】
    }

    #region 设计数据
    public class PageViewDesignData : UserControl
    {
        #region 【Properties】
        #endregion 【Properties】

        #region 【Ctor】
        public PageViewDesignData()
        {
        }
        #endregion 【Ctor】
    }
    #endregion
}
