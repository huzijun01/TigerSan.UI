using TigerSan.UI.Windows;
using TigerSan.UI.ViewModels;

namespace TigerSan.UI.Helpers
{
    public class AboutHelper
    {
        #region 【Fields】
        private AboutWindow? _aboutView;
        private UpdateWindow? _updateView;
        public AboutViewModel _aboutViewModel;
        public UpdateViewModel _updateViewModel;
        /// <summary>
        /// 实例（可用作为全局数据）
        /// </summary>
        public static AboutHelper? _instance;
        #endregion 【Fields】

        #region 【Ctor】
        public AboutHelper(
            AboutViewModel aboutViewModel,
            UpdateViewModel updateViewModel)
        {
            _aboutViewModel = aboutViewModel;
            _updateViewModel = updateViewModel;
        }
        #endregion 【Ctor】

        #region 【Functions】
        #region 显示“关于窗口”
        public void ShowAboutView()
        {
            if (_aboutView != null) return;

            _aboutView = new AboutWindow();
            _aboutView.DataContext = _aboutViewModel;
            _aboutView.Closed += (s, e) => { _aboutView = null; };
            _aboutView.Show();
        }
        #endregion

        #region 显示“升级窗口”
        public void ShowUpdateView(
            string version,
            string updateMessage)
        {
            _updateViewModel.Version = version;
            _updateViewModel.UpdateMessage = updateMessage;

            if (_updateView != null) return;

            _updateView = new UpdateWindow();
            _updateView.DataContext = _updateViewModel;
            _updateView.Closed += (s, e) => { _updateView = null; };
            _updateView.Show();
        }
        #endregion
        #endregion 【Functions】
    }
}
