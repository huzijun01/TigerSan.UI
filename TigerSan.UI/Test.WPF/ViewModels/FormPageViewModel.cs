using System.Windows.Input;
using System.Collections.ObjectModel;
using TigerSan.UI.Models;
using TigerSan.UI.Helpers;
using TigerSan.UI.Controls;

namespace Test.WPF.ViewModels
{
    public class FormPageViewModel : BindableBase
    {
        #region 【Properties】
        /// <summary>
        /// “菜单项目模型”集合
        /// </summary>
        public ObservableCollection<MenuItemModel> MenuItemModels { get; set; } = new ObservableCollection<MenuItemModel>();

        /// <summary>
        /// 过滤器模型
        /// </summary>
        public FilterModel FilterModel { get; set; } = new FilterModel("图片", ["jpg", "png", "gif"]);
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

        #region “文件选择器”拖入
        public ICommand FilePicker_DropCommand { get => new DelegateCommand<string[]>(FilePicker_Drop); }
        private void FilePicker_Drop(string[] paths)
        {
            MsgBox.ShowInformation(string.Join(Environment.NewLine, paths));
        }
        #endregion

        #region “文件选择器”选择后
        public ICommand FilePicker_SelectedCommand { get => new DelegateCommand<string[]>(FilePicker_Selected); }
        private void FilePicker_Selected(string[] paths)
        {
            MsgBox.ShowInformation(string.Join(Environment.NewLine, paths));
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
            MenuItemModels.Add(new MenuItemModel() { Source = "Item 6", _clicked = MenuItemClicked });
            MenuItemModels.Add(new MenuItemModel() { Source = "Item 7", _clicked = MenuItemClicked });
            MenuItemModels.Add(new MenuItemModel() { Source = "Item 8", _clicked = MenuItemClicked });
            MenuItemModels.Add(new MenuItemModel() { Source = "Item 9", _clicked = MenuItemClicked });
            MenuItemModels.Add(new MenuItemModel() { Source = "Item 10", _clicked = MenuItemClicked });
            MenuItemModels.Add(new MenuItemModel() { Source = "Item 11", _clicked = MenuItemClicked });
            MenuItemModels.Add(new MenuItemModel() { Source = "Item 12", _clicked = MenuItemClicked });
            MenuItemModels.Add(new MenuItemModel() { Source = "Item 13", _clicked = MenuItemClicked });
            MenuItemModels.Add(new MenuItemModel() { Source = "Item 14", _clicked = MenuItemClicked });
            MenuItemModels.Add(new MenuItemModel() { Source = "Item 15", _clicked = MenuItemClicked });
            MenuItemModels.Add(new MenuItemModel() { Source = "Item 16", _clicked = MenuItemClicked });
            MenuItemModels.Add(new MenuItemModel() { Source = "Item 17", _clicked = MenuItemClicked });
            MenuItemModels.Add(new MenuItemModel() { Source = "Item 18", _clicked = MenuItemClicked });
            MenuItemModels.Add(new MenuItemModel() { Source = "Item 19", _clicked = MenuItemClicked });
            MenuItemModels.Add(new MenuItemModel() { Source = "Item 20", _clicked = MenuItemClicked });
        }
        #endregion
        #endregion 【Functions】
    }
}
