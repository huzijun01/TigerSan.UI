using System.Windows;
using System.Windows.Media;
using TigerSan.UI.Windows;
using DialogWindow = TigerSan.UI.Windows.DialogWindow;

namespace TigerSan.UI.Helpers
{
    #region 消息类型
    public enum MsgType
    {
        Error,
        Information,
        Success,
        Warning,
    }
    #endregion

    public static class MsgBox
    {
        #region 获取对话框
        /// <summary>
        /// 获取对话框
        /// </summary>
        public static DialogWindow GetDialog(
            MsgType type,
            string msg,
            bool showButtonPanel = false,
            DialogEvent? OnSelected = null,
            DialogAsyncEvent? OnSelectedAsync = null)
        {
            string title;
            SolidColorBrush titleBackground;

            switch (type)
            {
                case MsgType.Success:
                    title = nameof(MsgType.Success);
                    titleBackground = Generic.Success;
                    break;
                case MsgType.Warning:
                    title = nameof(MsgType.Warning);
                    titleBackground = Generic.Warning;
                    break;
                case MsgType.Error:
                    title = nameof(MsgType.Error);
                    titleBackground = Generic.Danger;
                    break;
                default:
                    title = nameof(MsgType.Information);
                    titleBackground = Generic.Brand;
                    break;
            }

            var dialog = new DialogWindow()
            {
                Title = title,
                TitleBackground = titleBackground,
                Text = msg,
                OnSelected = OnSelected,
                OnSelectedAsync = OnSelectedAsync,
                ButtonPanelVisibility = showButtonPanel ? Visibility.Visible : Visibility.Collapsed,
                CloseButtonHoverForeground = Generic.BasicBlack
            };

            return dialog;
        }
        #endregion

        #region 显示对话框
        /// <summary>
        /// 显示对话框
        /// </summary>
        public static void ShowDialog(
            MsgType type,
            string msg,
            bool showButtonPanel = true,
            DialogEvent? OnSelected = null,
            DialogAsyncEvent? OnSelectedAsync = null)
        {
            GetDialog(type, msg, showButtonPanel, OnSelected, OnSelectedAsync).Show().Await();
        }
        #endregion

        #region 异步显示对话框
        /// <summary>
        /// 异步显示对话框
        /// </summary>
        public static async Task<DialogResults> ShowDialogAsync(
            MsgType type,
            string msg,
            bool showButtonPanel = true,
            DialogEvent? OnSelected = null,
            DialogAsyncEvent? OnSelectedAsync = null)
        {
            return await GetDialog(type, msg, showButtonPanel, OnSelected, OnSelectedAsync).Show();
        }
        #endregion

        #region 信息
        /// <summary>
        /// 信息
        /// </summary>
        public static void ShowInformation(string msg)
        {
            ShowDialog(MsgType.Information, msg, false);
        }
        #endregion

        #region 成功
        /// <summary>
        /// 成功
        /// </summary>
        public static void ShowSuccess(string msg)
        {
            ShowDialog(MsgType.Success, msg, false);
        }
        #endregion

        #region 警告
        /// <summary>
        /// 警告
        /// </summary>
        public static void ShowWarning(string msg)
        {
            ShowDialog(MsgType.Warning, msg, false);
        }
        #endregion

        #region 错误
        /// <summary>
        /// 错误
        /// </summary>
        public static void ShowError(string msg)
        {
            ShowDialog(MsgType.Error, msg, false);
        }
        #endregion
    }
}
