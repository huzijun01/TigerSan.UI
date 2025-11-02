using System.IO;
using System.Media;
using System.Windows;
using TigerSan.CsvLog;

namespace TigerSan.UI.Helpers
{
    public class StartupOnce
    {
        #region 【Fields】
        /// <summary>
        /// 互斥量
        /// </summary>
        private Mutex _mutex;

        /// <summary>
        /// 当前应用
        /// </summary>
        private Application _current;
        #endregion 【Fields】

        #region 【Ctor】
        public StartupOnce(Application current)
        {
            _current = current;
            var name = Path.GetFileNameWithoutExtension(Environment.ProcessPath);
            _mutex = new Mutex(true, name);
        }
        #endregion 【Ctor】

        #region 【Functions】
        #region 启动检测
        public void StartupCheck()
        {
            try
            {
                if (!_mutex.WaitOne(TimeSpan.Zero, true))
                {
                    SystemSounds.Beep.Play();
                    _current.Shutdown();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Error(ex.Message);
            }
        }
        #endregion
        #endregion 【Functions】
    }
}
