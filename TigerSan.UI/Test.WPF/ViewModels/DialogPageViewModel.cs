using System.IO;
using System.Text;
using System.Windows.Input;
using TigerSan.UI.Helpers;
using TigerSan.UI.Windows;

namespace Test.WPF.ViewModels
{
    public class DialogPageViewModel : BindableBase
    {
        #region 【Ctor】
        public DialogPageViewModel()
        {
        }
        #endregion 【Ctor】

        #region 【Commands】
        #region 点击“信息”按钮
        public ICommand btnInformation_ClickCommand { get => new DelegateCommand(btnInformation_Click); }
        private void btnInformation_Click()
        {
            MsgBox.ShowInformation("信息内容");
        }
        #endregion

        #region 点击“成功”按钮
        public ICommand btnSucces_ClickCommand { get => new DelegateCommand(btnSucces_Click); }
        private void btnSucces_Click()
        {
            MsgBox.ShowSuccess("成功内容");
        }
        #endregion

        #region 点击“警告”按钮
        public ICommand btnWarning_ClickCommand { get => new DelegateCommand(btnWarning_Click); }
        private void btnWarning_Click()
        {
            MsgBox.ShowWarning("警告内容");
        }
        #endregion

        #region 点击“错误”按钮
        public ICommand btnError_ClickCommand { get => new DelegateCommand(btnError_Click); }
        private void btnError_Click()
        {
            MsgBox.ShowError("错误内容");
        }
        #endregion

        #region 点击“弹窗”按钮
        public ICommand btnShowDialog_ClickCommand { get => new AsyncDelegateCommand(btnShowDialog_Click); }
        private async Task btnShowDialog_Click()
        {
            var text = File.ReadAllText(@"Files\text.txt", Encoding.UTF8);
            var res = await MsgBox.ShowDialogAsync(MsgType.Information, text);

            string sreResult;

            switch (res)
            {
                case DialogResults.Yes:
                    sreResult = nameof(DialogResults.Yes);
                    break;
                case DialogResults.No:
                    sreResult = nameof(DialogResults.No);
                    break;
                default:
                    sreResult = nameof(DialogResults.Cancel);
                    break;
            }

            MsgBox.ShowInformation(sreResult);
        }
        #endregion
        #endregion 【Commands】
    }
}
