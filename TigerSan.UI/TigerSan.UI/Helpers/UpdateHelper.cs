using TigerSan.CsvLog;
using TigerSan.UI.Models;
using TigerSan.JsonConverter;
using TigerSan.TimerHelper.WPF;

namespace TigerSan.UI.Helpers
{
    #region 检查结果
    public enum CheckResults
    {
        Error,
        Update,
        NoUpdate,
        NoNetwork,
    }
    #endregion

    public class UpdateHelper
    {
        #region 【Fields】
        #region [Private]
        /// <summary>
        /// 升级Url
        /// </summary>
        private string _updateUrl;

        /// <summary>
        /// 当前版本
        /// </summary>
        private string _currentVersion;

        /// <summary>
        /// 关于助手
        /// </summary>
        private AboutHelper _aboutHelper;
        #endregion [Private]

        /// <summary>
        /// 升级定时器
        /// </summary>
        public readonly ActionTimer _timerUpdate;
        #endregion 【Fields】

        #region 【Ctor】
        public UpdateHelper(
            string updateUrl,
            string currentVersion,
            AboutHelper aboutHelper)
        {
            _updateUrl = updateUrl;
            _currentVersion = currentVersion;
            _aboutHelper = aboutHelper;
            _timerUpdate = new ActionTimer(5000, true, () => { CheckUpdateAsync().Await(); });
        }
        #endregion 【Ctor】

        #region 【Functions】
        #region 检测升级
        /// <summary>  
        /// 检测升级
        /// </summary>
        public async Task<CheckResults> CheckUpdateAsync()
        {
            if (!NetworkHelper.IsNetworkAvailable()) return CheckResults.NoNetwork;

            _timerUpdate.Stop();

            #region 获取“版本信息”
            var result = await NetworkHelper.GetAsync(_updateUrl);

            if (!result.isSuccess)
            {
                LogHelper.Instance.Warning("Failed to get the update information!");
                return CheckResults.Error;
            }
            var versionInfo = JsonHelper.Deserialize<UpdateInfo>(result.response);

            if (versionInfo == null)
            {
                LogHelper.Instance.IsNull(nameof(versionInfo));
                return CheckResults.Error;
            }

            var oldVersion = new VersionModel(_currentVersion);
            var newVersion = new VersionModel(versionInfo.version);
            #endregion 获取“版本信息”

            if (VersionModel.Compare(oldVersion, newVersion) != ComparisonResults.Less) return CheckResults.NoUpdate;

            #region 显示“升级窗口”
            if (!string.IsNullOrEmpty(versionInfo.download_url))
            {
                _aboutHelper._updateViewModel.DownloadUrl = versionInfo.download_url;
            }

            _aboutHelper.ShowUpdateView(versionInfo.version, versionInfo.msg);
            #endregion 显示“升级窗口”

            return CheckResults.Update;
        }
        #endregion
        #endregion 【Functions】
    }
}
