namespace TigerSan.UI.Models
{
    #region 校验委托
    /// <summary>
    /// 校验委托
    /// </summary>
    /// <param name="source">源数据</param>
    /// <returns>是否正确</returns>
    public delegate bool Verification(object? source);
    #endregion

    public static class Verifications
    {
        #region 不为null
        /// <summary>
        /// 不为null
        /// </summary>
        /// <param name="source">源数据</param>
        /// <returns></returns>
        public static bool IsNotNull(object? source)
        {
            return source != null;
        }
        #endregion

        #region 不为null或空
        /// <summary>
        /// 不为null或空（只用于字符串）
        /// </summary>
        /// <param name="source">源数据</param>
        /// <returns></returns>
        public static bool IsNotNullOrEmpty(object? source)
        {
            var str = source as string;
            return !string.IsNullOrEmpty(str);
        }
        #endregion
    }
}
