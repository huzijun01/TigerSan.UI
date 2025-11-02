using System.Web;
using System.Text;
using System.Net.Http;
using System.Net.NetworkInformation;

namespace TigerSan.UI.Helpers
{
    #region 行为结果
    public class ActionResult
    {
        public bool isSuccess = true;
        public string response = string.Empty;
    }
    #endregion

    public static class NetworkHelper
    {
        #region 判断网络是否可用
        /// <summary>
        /// 判断网络是否可用
        /// </summary>
        /// <returns></returns>
        public static bool IsNetworkAvailable()
        {
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface ni in interfaces)
            {
                OperationalStatus status = ni.OperationalStatus;
                if (status == OperationalStatus.Up)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 发送Get请求（异步）
        /// <summary>
        /// 发送Get请求（异步）
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns></returns>
        public static async Task<ActionResult> GetAsync(string url)
        {
            ActionResult result = new ActionResult();

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // 发送GET请求  
                    HttpResponseMessage response = await client.GetAsync(url);

                    // 确保HTTP成功状态值
                    response.EnsureSuccessStatusCode();

                    // 读取响应内容
                    result.response = await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException e)
                {
                    result.isSuccess = false;
                    result.response = e.Message;
                }
            }
            return result;
        }
        #endregion

        #region 发送Get请求（异步，带Queue参数）
        /// <summary>
        /// 发送Get请求（异步，带Queue参数）
        /// </summary>
        /// <param name="url">Base URL</param>
        /// <param name="param">参数字典</param>
        /// <returns></returns>
        public static async Task<ActionResult> GetAsync(string url, Dictionary<string, string> param)
        {
            var queryString = new StringBuilder();
            bool isFirst = true;

            foreach (var kvp in param)
            {
                if (!isFirst)
                {
                    queryString.Append("&");
                }
                queryString.Append($"{HttpUtility.UrlEncode(kvp.Key)}={HttpUtility.UrlEncode(kvp.Value)}");
                isFirst = false;
            }

            var fullUrl = string.IsNullOrEmpty(queryString.ToString()) ? url : $"{url}?{queryString.ToString()}";

            return await GetAsync(fullUrl);
        }
        #endregion

        #region 判断code是否为成功
        /// <summary>
        /// 判断code是否为成功
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns></returns>
        public static bool IsOK(int code)
        {
            return code >= 200 && code < 300;
        }
        #endregion
    }
}
