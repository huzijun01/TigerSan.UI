using System.Windows.Input;
using System.Collections.ObjectModel;
using TigerSan.UI.Models;
using TigerSan.UI.Helpers;

namespace Test.WPF.ViewModels
{
    public class FormPageViewModel : BindableBase
    {
        #region 【Properties】
        /// <summary>
        /// 菜单项目模型集合
        /// </summary>
        public ObservableCollection<MenuItemModel> MenuItemModels { get; set; } = new ObservableCollection<MenuItemModel>();
        #endregion 【Properties】

        #region 【Ctor】
        public FormPageViewModel()
        {
            InitMenuItemModels();
        }
        #endregion 【Ctor】

        #region 【Evnets】
        #region “菜单项目”被点击
        private void MenuItemClicked(MenuItemModel itemModel)
        {
            MsgBox.ShowInformation($"The \"{itemModel.Source}\" has been clicked!");
        }
        #endregion
        #endregion 【Evnets】

        #region 【Commands】
        #region “选择器”值改变后
        public ICommand Select_ValueChangedCommand { get => new DelegateCommand<object>(Select_ValueChanged); }
        private void Select_ValueChanged(object value)
        {
            MsgBox.ShowInformation($"The \"{nameof(value)}\" is \"{value}\"!");
        }
        #endregion
        #endregion 【Commands】

        #region 【Functions】
        #region 初始化“菜单项目模型集合”
        private void InitMenuItemModels()
        {
            MenuItemModels.Clear();
            MenuItemModels.Add(new MenuItemModel() { Source = "Item 1", _clicked = MenuItemClicked });
            MenuItemModels.Add(new MenuItemModel() { Source = "Item 2", _clicked = MenuItemClicked });
            MenuItemModels.Add(new MenuItemModel() { Source = "Item 3", _clicked = MenuItemClicked });
            MenuItemModels.Add(new MenuItemModel() { Source = "Item 4", _clicked = MenuItemClicked });
            MenuItemModels.Add(new MenuItemModel() { Source = "Item 5", _clicked = MenuItemClicked });
        }
        #endregion
        #endregion 【Functions】
    }
}
