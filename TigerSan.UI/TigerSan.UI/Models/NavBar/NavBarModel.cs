using System.Windows.Input;
using System.Collections.ObjectModel;

namespace TigerSan.UI.Models
{
    public class NavBarModel : BindableBase
    {
        #region 【Fields】
        /// <summary>
        /// “选中按钮”改变后委托
        /// （PageView内部会自动添加回调）
        /// </summary>
        public Action<NavButtonModel?>? _onSelectedButtonModelChanged;

        /// <summary>
        /// 获取“文件夹”高度
        /// （NavBar内部会自动添加回调）
        /// </summary>
        public Func<double>? _getFolderHeight;

        /// <summary>
        /// 获取“按钮”高度
        /// （NavBar内部会自动添加回调）
        /// </summary>
        public Func<double>? _getButtonHeight;
        #endregion 【Fields】

        #region 【Properties】
        /// <summary>
        /// 是否打开
        /// </summary>
        public bool IsOpen
        {
            get { return _IsOpen; }
            set { SetProperty(ref _IsOpen, value); }
        }
        private bool _IsOpen = true;

        /// <summary>
        /// 选中的“按钮模型”
        /// </summary>
        public NavButtonModel? SelectedButtonModel
        {
            get { return _SelectedButtonModel; }
            set
            {
                SetProperty(ref _SelectedButtonModel, value);
                UpdateSelectStates();
                _onSelectedButtonModelChanged?.Invoke(value);
            }
        }
        private NavButtonModel? _SelectedButtonModel;

        /// <summary>
        /// “文件夹模型”集合
        /// </summary>
        public NavFolderModel FolderModel { get; set; }

        /// <summary>
        /// 已打开的“按钮模型”集合
        /// </summary>
        public ObservableCollection<NavButtonModel> OpenedButtonModels { get; set; } = new ObservableCollection<NavButtonModel>();
        #endregion 【Properties】

        #region 【Commands】
        #region 点击“导航栏开关”按钮
        public ICommand btnNavSwitch_ClickCommand { get => new DelegateCommand(btnNavSwitch_Click); }
        private void btnNavSwitch_Click()
        {
            IsOpen = !IsOpen;
        }
        #endregion
        #endregion 【Commands】

        #region 【Ctor】
        public NavBarModel()
        {
            FolderModel = new NavFolderModel(this);
        }
        #endregion 【Ctor】

        #region 【Functions】
        #region [Static]
        #region 获取“默认按钮模型”
        public static NavButtonModel GetDefaultButtonModel()
        {
            var barModel = new NavBarModel();
            return new NavButtonModel(barModel, new NavFolderModel(barModel));
        }
        #endregion
        #endregion [Static]

        #region [Private]
        #region 更新“选中状态”
        private void UpdateSelectStates()
        {
            NavFolderModel.RecursivelyOperateSubItems(
                FolderModel,
                null,
                buttonModel =>
                {
                    buttonModel.IsSelected = Equals(buttonModel, SelectedButtonModel);
                });
        }
        #endregion
        #endregion [Private]

        #region 添加“文件夹”
        public void AddFolder(NavFolderModel folderModel)
        {
            FolderModel.FolderModels.Add(folderModel);
        }
        #endregion

        #region 添加“按钮”
        public void AddButton(NavButtonModel buttonModel)
        {
            FolderModel.ButtonModels.Add(buttonModel);
        }
        #endregion

        #region 更新“所有文件夹”的“高度”
        public void UpdateAllFoldersHeight()
        {
            NavFolderModel.RecursivelyOperateSubItems(
                FolderModel,
                folderModel => { folderModel._updateFolderHeight?.Invoke(); },
                null);
        }
        #endregion

        #region 判断“页面”是否已打开
        public bool IsPageOpened(NavButtonModel buttonModel)
        {
            return OpenedButtonModels.Contains(buttonModel);
        }
        #endregion
        #endregion 【Functions】
    }
}
