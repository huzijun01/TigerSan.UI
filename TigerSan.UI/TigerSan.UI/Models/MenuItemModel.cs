using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using TigerSan.UI.Converters;

namespace TigerSan.UI.Models
{
    public class MenuItemModel : BindableBase
    {
        #region 【Fields】
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

        /// <summary>
        /// 转换器
        /// </summary>
        public IValueConverter? _converter { get; set; }
        #endregion 【Fields】

        #region 【Properties】
        #region [引用]
        /// <summary>
        /// 文本
        /// </summary>
        public string Text
        {
            get { return GetText(); }
        }
        #endregion [引用]

        /// <summary>
        /// 源数据
        /// </summary>
        public object? Source
        {
            get { return _source; }
            set { SetProperty(ref _source, value); }
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

        #region 获取文本
        private string GetText()
        {
            if (_converter == null)
            {
                _converter = new Object2StringConverter();
                return ((Object2StringConverter)_converter).Convert(Source);
            }

            return _converter.Convert(Source, null, null, null) as string ?? string.Empty;
        }
        #endregion
        #endregion
    }
}
