using TigerSan.CsvLog;

namespace TigerSan.UI.Models
{
    #region 更新信息
    /// <summary>
    /// 更新信息
    /// （FTP服务器中的配置文件）
    /// </summary>
    public class UpdateInfo
    {
        /// <summary>
        /// 版本
        /// </summary>
        public string version { get; set; } = string.Empty;
        /// <summary>
        /// 下载链接
        /// </summary>
        public string download_url { get; set; } = string.Empty;
        /// <summary>
        /// 信息
        /// </summary>
        public string msg { get; set; } = string.Empty;
    }
    #endregion

    #region 比较结果
    public enum ComparisonResults
    {
        Error = -2,
        Less = -1,
        Equal = 0,
        Greater = 1,
    }
    #endregion

    #region 版本
    /// <summary>
    /// 版本
    /// </summary>
    public class VersionModel
    {
        #region 【Fields】
        /// <summary>
        /// 不存在
        /// </summary>
        public static readonly int None = -1;
        #endregion 【Fields】

        #region 【Properties】
        public int majorVersion { get; set; } = None;
        public int minorVersion { get; set; } = None;
        public int patchNumber { get; set; } = None;
        #endregion 【Properties】

        #region 【Ctor】
        public VersionModel(string strVersion)
        {
            Init(strVersion);
        }
        #endregion 【Ctor】

        #region 【Functions】
        #region 初始化
        public void Init(string strVersion)
        {
            try
            {
                string[] arrStr = strVersion.Split('.');
                majorVersion = int.Parse(arrStr[0]);
                minorVersion = int.Parse(arrStr[1]);
                patchNumber = int.Parse(arrStr[2]);
            }
            catch (Exception e)
            {
                SetNone();
                LogHelper.Instance.Warning(e.Message);
                return;
            }
        }
        #endregion

        #region 置为None
        /// <summary>
        /// 置为None
        /// </summary>
        public void SetNone()
        {
            majorVersion = None;
            minorVersion = None;
            patchNumber = None;
        }
        #endregion

        #region 比较
        public static ComparisonResults Compare(VersionModel v1, VersionModel v2)
        {
            if (v1.majorVersion < 0 || v2.majorVersion < 0
                || v1.minorVersion < 0 || v2.minorVersion < 0
                || v1.patchNumber < 0 || v2.patchNumber < 0)
            {
                LogHelper.Instance.Warning("The version number has an incorrect value!");
                return ComparisonResults.Error;
            }

            #region majorVersion
            if (v1.majorVersion < v2.majorVersion)
            {
                return ComparisonResults.Less;
            }
            else if (v1.majorVersion > v2.majorVersion)
            {
                return ComparisonResults.Greater;
            }
            #endregion majorVersion

            #region minorVersion
            if (v1.minorVersion < v2.minorVersion)
            {
                return ComparisonResults.Less;
            }
            else if (v1.minorVersion > v2.minorVersion)
            {
                return ComparisonResults.Greater;
            }
            #endregion minorVersion

            #region patchNumber
            if (v1.patchNumber < v2.patchNumber)
            {
                return ComparisonResults.Less;
            }
            else if (v1.patchNumber > v2.patchNumber)
            {
                return ComparisonResults.Greater;
            }
            #endregion patchNumber

            return ComparisonResults.Equal;
        }
        #endregion
        #endregion 【Functions】
    }
    #endregion
}
