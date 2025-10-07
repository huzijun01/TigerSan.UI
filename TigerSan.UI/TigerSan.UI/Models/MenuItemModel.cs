using System.Windows;
using System.Windows.Input;
using TigerSan.UI.Controls;

namespace TigerSan.UI.Models
{
    public class MenuItemModel : BindableBase
    {
        #region 【Fields】
        /// <summary>
        /// 选择器
        /// </summary>
        public Select? _select;

        /// <summary>
        /// 点击
        /// </summary>
        public Action<MenuItemModel>? _clicked;

        /// <summary>
        /// 内部点击
        /// </summary>
        public Action<MenuItemModel>? _internalClicked;

        /// <summary>
        /// 点击（异步）
        /// </summary>
        public Action? _clickedAsync;
        #endregion 【Fields】

        #region 【Properties】
        /// <summary>
        /// 文本
        /// </summary>
        public string Text
        {
            get { return _text; }
            private set { SetProperty(ref _text, value); }
        }
        private string _text = string.Empty;

        /// <summary>
        /// 源数据
        /// </summary>
        public object? Source
        {
            get { return _source; }
            set
            {
                SetProperty(ref _source, value);
                UpdateText();
            }
        }
        private object? _source;

        /// <summary>
        /// 可见性
        /// </summary>
        public Visibility Visibility
        {
            get { return _visibility; }
            set { SetProperty(ref _visibility, value); }
        }
        private Visibility _visibility = Visibility.Visible;
        #endregion 【Properties】

        #region 【Commands】
        #region 点击
        public ICommand ClickedCommand { get => new DelegateCommand(OnClicked); }
        private void OnClicked()
        {
            _internalClicked?.Invoke(this);
            _clicked?.Invoke(this);
            _clickedAsync?.BeginInvoke(null, null);
        }
        #endregion
        #endregion

        #region 【Functions】
        #region 更新文本
        public void UpdateText()
        {
            if (_select == null) return;

            Text = _select.GetText(Source);
        }
        #endregion
        #endregion 【Functions】
    }
}
