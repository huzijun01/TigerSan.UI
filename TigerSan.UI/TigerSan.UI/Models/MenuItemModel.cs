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
        #endregion 【Fields】

        #region 【Properties】
        #region [OneWay]
        /// <summary>
        /// 文本
        /// </summary>
        public string Text
        {
            get { return _Text; }
            private set { SetProperty(ref _Text, value); }
        }
        private string _Text = string.Empty;
        #endregion [OneWay]

        /// <summary>
        /// 源数据
        /// </summary>
        public object? Source
        {
            get { return _Source; }
            set
            {
                SetProperty(ref _Source, value);
                UpdateText();
            }
        }
        private object? _Source;

        /// <summary>
        /// 转换器（初始化“项目集合”时，自动添加）
        /// </summary>
        public IValueConverter? Converter
        {
            get { return _Converter; }
            set
            {
                SetProperty(ref _Converter, value);
                UpdateText();
            }
        }
        private IValueConverter? _Converter;

        /// <summary>
        /// 可见性
        /// </summary>
        public Visibility Visibility
        {
            get { return _Visibility; }
            set { SetProperty(ref _Visibility, value); }
        }
        private Visibility _Visibility = Visibility.Visible;
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

        #region 更新“文本”
        private void UpdateText()
        {
            if (Converter == null)
            {
                Converter = new Object2StringConverter();
                Text = ((Object2StringConverter)Converter).Convert(Source);
            }

            Text = Converter.Convert(Source, null, null, null) as string ?? string.Empty;
        }
        #endregion
        #endregion 【Commands】
    }
}
