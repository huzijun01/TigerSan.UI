using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using TigerSan.CsvLog;

namespace TigerSan.UI.Models
{
    #region 委托
    public delegate void NavFolderHandler(NavFolderModel folderModel);
    public delegate void NavButtonHandler(NavButtonModel folderModel);
    #endregion

    #region 子项个数
    public class SubItemCount
    {
        public int FolderCount { get; set; }
        public int ButtonCount { get; set; }
    }
    #endregion

    public class NavFolderModel : BindableBase
    {
        #region 【Fields】
        /// <summary>
        /// 更新“文件夹”高度
        /// （NavFolder内部会自动添加回调）
        /// </summary>
        public Action? _updateFolderHeight;
        #endregion 【Fields】

        #region 【Properties】
        public NavBarModel NavBarModel
        {
            get { return _NavBarModel; }
            set { SetProperty(ref _NavBarModel, value); }
        }
        private NavBarModel _NavBarModel;

        public string Icon
        {
            get { return _Icon; }
            set { SetProperty(ref _Icon, value); }
        }
        private string _Icon = Icons.Folder_Linear;

        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value); }
        }
        private string _Title = "null";

        public bool IsOpen
        {
            get { return _IsOpen; }
            set { SetProperty(ref _IsOpen, value); }
        }
        private bool _IsOpen = true;

        public Visibility Visibility
        {
            get { return _Visibility; }
            set { SetProperty(ref _Visibility, value); }
        }
        private Visibility _Visibility = Visibility.Visible;

        public ObservableCollection<NavFolderModel> FolderModels { get; set; } = new ObservableCollection<NavFolderModel>();

        public ObservableCollection<NavButtonModel> ButtonModels { get; set; } = new ObservableCollection<NavButtonModel>();
        #endregion 【Properties】

        #region 【Commands】
        public ICommand? Command { get; set; }
        public ICommand? OpenedCommand { get; set; }
        public ICommand? ClosedCommand { get; set; }
        #endregion 【Commands】

        #region 【Ctor】
        public NavFolderModel(NavBarModel navBarModel)
        {
            _NavBarModel = navBarModel;
        }
        #endregion 【Ctor】

        #region 【Functions】
        #region 添加“文件夹”
        public void AddFolder(NavFolderModel folderModel)
        {
            FolderModels.Add(folderModel);
        }
        #endregion

        #region 添加“按钮”
        public void AddButton(NavButtonModel buttonModel)
        {
            ButtonModels.Add(buttonModel);
        }
        #endregion

        #region 获取“子项”的“个数”
        public SubItemCount GetOpenedSubItemCount(bool isExcludeNotDisplayButton = true)
        {
            var count = new SubItemCount();

            RecursivelyOperateSubItems(
                this,
                folderModel => { ++count.FolderCount; },
                buttonModel =>
                {
                    if (
                    isExcludeNotDisplayButton
                    && !Equals(buttonModel.NavFolderModel, this)
                    && !buttonModel.NavFolderModel.IsOpen)
                    {
                        return;
                    }
                    ++count.ButtonCount;
                });

            return count;
        }
        #endregion

        #region 获取“子项”的“总高度”
        public double GetSubItemsHeight()
        {
            var count = GetOpenedSubItemCount();

            if (NavBarModel._getFolderHeight == null)
            {
                LogHelper.Instance.IsNull(nameof(NavBarModel._getFolderHeight));
                return 0;
            }

            if (NavBarModel._getButtonHeight == null)
            {
                LogHelper.Instance.IsNull(nameof(NavBarModel._getButtonHeight));
                return 0;
            }

            return count.FolderCount * NavBarModel._getFolderHeight()
                + count.ButtonCount * NavBarModel._getButtonHeight();
        }
        #endregion

        #region 递归操作“子项”集合
        /// <summary>
        /// 递归操作“子项”集合
        /// </summary>
        public static void RecursivelyOperateSubItems(
            NavFolderModel folderModel,
            NavFolderHandler? fnFolder,
            NavButtonHandler? fnButton = null)
        {
            // 目录：
            foreach (var subFolderModel in folderModel.FolderModels)
            {
                fnFolder?.Invoke(subFolderModel);
                RecursivelyOperateSubItems(subFolderModel, fnFolder, fnButton);
            }

            // 按钮：
            if (fnButton == null) return;

            foreach (var buttonModel in folderModel.ButtonModels)
            {
                fnButton.Invoke(buttonModel);
            }
        }
        #endregion
        #endregion 【Functions】
    }
}
