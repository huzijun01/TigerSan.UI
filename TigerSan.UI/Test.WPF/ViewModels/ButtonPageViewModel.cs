using System.Windows.Input;
using TigerSan.UI.Helpers;
using TigerSan.UI.Windows;

namespace Test.WPF.ViewModels
{
    public class ButtonPageViewModel : BindableBase
    {
        #region 【Properties】
        /// <summary>
        /// 开关值
        /// </summary>
        public bool SwitchValue
        {
            get { return _switchValue; }
            set { SetProperty(ref _switchValue, value); }
        }
        private bool _switchValue = false;
        #endregion 【Properties】

        #region 【Ctor】
        public ButtonPageViewModel()
        {
        }
        #endregion 【Ctor】

        #region 【Commands】
        #region [按钮]
        #region 点击“确定”按钮
        public ICommand btnOK_ClickCommand { get => new DelegateCommand(btnOK_Click); }
        private void btnOK_Click()
        {
        }
        #endregion

        #region 点击“Bye”按钮
        public ICommand btnBye_ClickCommand { get => new DelegateCommand(btnBye_Click); }
        private void btnBye_Click()
        {
            var bye = new ByeWindow();
            bye.Show();
        }
        #endregion

        #region 开关值改变
        public ICommand Switch_ValueChangedCommand { get => new DelegateCommand<object>(Switch_ValueChanged); }
        private void Switch_ValueChanged(object param)
        {
            MsgBox.ShowInformation($"Command: {nameof(SwitchValue)} = {param.ToString()}");
        }
        #endregion

        #region 开关值改变（事件）
        public ICommand Switch_ValueChangedEvent { get => new DelegateCommand<object>(OnSwitch_ValueChanged); }
        private void OnSwitch_ValueChanged(object args)
        {
            MsgBox.ShowInformation($"Event: {nameof(SwitchValue)} = {SwitchValue.ToString()}");
        }
        #endregion
        #endregion [按钮]
        #endregion 【Commands】
    }
}
