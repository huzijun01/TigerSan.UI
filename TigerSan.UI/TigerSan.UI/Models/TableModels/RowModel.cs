using System.Windows.Input;
using System.Windows.Media;

namespace TigerSan.UI.Models
{
    /// <summary>
    /// 行模型
    /// </summary>
    public class RowModel : BindableBase
    {
        #region 【Fields】
        /// <summary>
        /// 行数据
        /// </summary>
        public TableModel _tableModel;
        #endregion【Fields】

        #region 【Properties】
        #region [引用]
        #region 是否显示复选框
        /// <summary>
        /// 是否显示复选框
        /// </summary>
        public bool IsShowCheckBox { get => _tableModel.IsShowCheckBox; }
        #endregion
        #endregion [引用]

        #region [OneWay]
        /// <summary>
        /// 背景
        /// </summary>
        public Brush Background
        {
            get { return _background; }
            private set { SetProperty(ref _background, value); }
        }
        private Brush _background = Generic.Transparent;
        #endregion [OneWay]

        #region [更新状态]
        /// <summary>
        /// 行数据
        /// </summary>
        public object RowData
        {
            get { return _RowData; }
            set
            {
                _RowData = value;
                UpdateSources();
                UpdateItemStates();
            }
        }
        private object _RowData;

        /// <summary>
        /// 旧行数据
        /// </summary>
        public object? OldRowData
        {
            get { return _OldRowData; }
            set
            {
                _OldRowData = value;
                UpdateOldSources();
                UpdateItemStates();
            }
        }
        private object? _OldRowData;
        #endregion [更新状态]

        #region [Others]
        #region 是否选中
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                SetProperty(ref _isChecked, value);
                Background = value ? Generic.Brand : Generic.Transparent;
            }
        }
        private bool _isChecked = false;
        #endregion

        #region “项目模型”集合
        /// <summary>
        /// “项目模型”集合
        /// </summary>
        public Dictionary<HeaderModel, ItemModel> ItemModels { get; private set; } = new Dictionary<HeaderModel, ItemModel>();
        #endregion
        #endregion [Others]
        #endregion 【Properties】

        #region 【Ctor】
        public RowModel(
            TableModel tableModel,
            object rowData,
            object? oldRowData)
        {
            _RowData = rowData;
            _OldRowData = oldRowData;
            _tableModel = tableModel;
        }
        #endregion 【Ctor】

        #region 【Commands】
        #region 选中
        public ICommand CheckedCommand { get => new DelegateCommand<RowModel>(Checked); }
        private void Checked(RowModel rowModel)
        {
            UpdateIsSelectAll();
        }
        #endregion

        #region 未选中
        public ICommand UncheckedCommand { get => new DelegateCommand<RowModel>(Unchecked); }
        private void Unchecked(RowModel rowModel)
        {
            UpdateIsSelectAll();
        }
        #endregion
        #endregion 【Commands】

        #region 【Functions】
        #region [Private]
        #region 更新“源数据”集合
        private void UpdateSources()
        {
            foreach (var itemModel in ItemModels)
            {
                itemModel.Value.SetSource();
            }
        }
        #endregion

        #region 更新“旧源数据”集合
        private void UpdateOldSources()
        {
            foreach (var itemModel in ItemModels)
            {
                itemModel.Value.UpdateOldSource();
            }
        }
        #endregion

        #region 更新“项目状态”集合
        private void UpdateItemStates()
        {
            foreach (var itemModel in ItemModels)
            {
                itemModel.Value.UpdateItemState();
            }
        }
        #endregion

        #region 更新“是否全选”
        private void UpdateIsSelectAll()
        {
            if (!_tableModel.IsUpdateIsSelectAll) return;

            _tableModel._isUpdateRowIsChecked = false;

            _tableModel.IsSelectAll = !_tableModel.RowModels.Any(row => !row.Value.IsChecked);

            _tableModel._onSelectedRowDatasChanged?.Invoke();

            _tableModel._isUpdateRowIsChecked = true;
        }
        #endregion
        #endregion [Private]
        #endregion 【Functions】
    }
}
