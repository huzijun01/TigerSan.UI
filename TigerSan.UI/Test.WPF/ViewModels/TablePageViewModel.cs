using System.Windows.Input;
using System.Collections.ObjectModel;
using TigerSan.UI;
using TigerSan.UI.Models;
using TigerSan.UI.Helpers;
using TigerSan.UI.Converters;
using Test.WPF.Models;

namespace Test.WPF.ViewModels
{
    public class TablePageViewModel : BindableBase
    {
        #region 【Properties】
        /// <summary>
        /// 员工表格模型
        /// </summary>
        public TableModel EmployeeTable { get; set; } = new TableModel(typeof(EmployeeInfo));

        /// <summary>
        /// 被选中的行数
        /// </summary>
        public int SelectedRowCount
        {
            get { return _selectedRowCount; }
            set { SetProperty(ref _selectedRowCount, value); }
        }
        private int _selectedRowCount = 0;
        #endregion 【Properties】

        #region 【Ctor】
        public TablePageViewModel()
        {
            InitTable();
        }
        #endregion 【Ctor】

        #region 【Evnets】
        #region “项目源数据”改变
        private void OnItemSourceChanged(ItemModel itemModel)
        {
            MsgBox.ShowInformation($"Source = {itemModel.Target}");
        }
        #endregion

        #region “选中行集合”改变
        private void OnSelectedRowDatasChanged()
        {
            SelectedRowCount = EmployeeTable.SelectedRowCount;
        }
        #endregion
        #endregion 【Evnets】

        #region 【Commands】
        #region 点击“Refresh”按钮
        public ICommand btnRefresh_ClickCommand { get => new DelegateCommand(btnRefresh_Click); }
        private void btnRefresh_Click()
        {
            EmployeeTable.Refresh();
        }
        #endregion

        #region 点击“UpdateOldRowDatas”按钮
        public ICommand btnUpdateOldRowDatas_ClickCommand { get => new DelegateCommand(btnUpdateOldRowDatas_Click); }
        private void btnUpdateOldRowDatas_Click()
        {
            EmployeeTable.UpdateOldRowDatas();
        }
        #endregion

        #region 点击“Add”按钮
        public ICommand btnAdd_ClickCommand { get => new DelegateCommand(btnAdd_Click); }
        private void btnAdd_Click()
        {
        }
        #endregion

        #region 点击“Delete”按钮
        public ICommand btnDelete_ClickCommand { get => new DelegateCommand(btnDelete_Click); }
        private void btnDelete_Click()
        {
        }
        #endregion
        #endregion 【Commands】

        #region 【Functions】
        #region 初始化“表格”
        public void InitTable()
        {
            // 是否显示复选框：
            EmployeeTable.IsShowCheckBox = true;

            #region 设置“数据改变”委托
            EmployeeTable._onSelectedRowDatasChanged = OnSelectedRowDatasChanged;
            EmployeeTable._onItemSourceChanged = OnItemSourceChanged;
            #endregion 设置“数据改变”委托

            #region 设置“表头初始化”委托
            EmployeeTable._onHeaderInit = (headerModel) =>
            {
                if (string.Equals(headerModel.PropName, nameof(EmployeeInfo.Id)))
                {
                    headerModel.Converter = new Int2StringConverter();
                }
                else if (string.Equals(headerModel.PropName, nameof(EmployeeInfo.Name)))
                {
                    headerModel.Verification = Verifications.IsNotNullOrEmpty;
                    headerModel.Background = Generic.Brand;
                }
                else if (string.Equals(headerModel.PropName, nameof(EmployeeInfo.Age)))
                {
                    headerModel.Converter = new Int2StringConverter();
                }
                else if (string.Equals(headerModel.PropName, nameof(EmployeeInfo.Gender)))
                {
                    headerModel.Converter = new Bool2StringConverter("男", "女");
                    headerModel._menuWidth = 80;
                    headerModel._menuDatas = [true, false];
                }
                else if (string.Equals(headerModel.PropName, nameof(EmployeeInfo.Salary)))
                {
                    headerModel.Converter = new Double2StringConverter();
                }
                else if (string.Equals(headerModel.PropName, nameof(EmployeeInfo.JoinDate)))
                {
                    headerModel.Converter = new DateTime2StringConverter();
                }
            };
            #endregion 设置“表头初始化”委托

            #region 设置“项目初始化”委托
            EmployeeTable._onItemInit = (itemModel) =>
            {
                if (string.Equals(itemModel._headerModel.PropName, nameof(EmployeeInfo.Name)))
                {
                    itemModel.Background = Generic.Brand;
                }
            };
            #endregion 设置“项目初始化”委托

            #region 添加数据
            var RowDatas = new ObservableCollection<object>
            {
                new EmployeeInfo() { Id = 1, Name = "张三", Age = 18, Gender = true, Salary = 8000.01, JoinDate = DateTime.Now },
                new EmployeeInfo() { Id = 2, Name = "李四", Age = 19, Gender = false, Salary = 9000.02, JoinDate = DateTime.Now },
                new EmployeeInfo() { Id = 3, Name = "王五", Age = 20, Gender = true, Salary = 10000.03, JoinDate = DateTime.Now },
                new EmployeeInfo() { Id = 4, Name = "赵六", Age = 21, Gender = false, Salary = 11000.04, JoinDate = DateTime.Now },
                new EmployeeInfo() { Id = 5, Name = "吴七", Age = 22, Gender = true, Salary = 12000.05, JoinDate = DateTime.Now },
                new EmployeeInfo() { Id = 6, Name = "周八", Age = 23, Gender = true, Salary = 13000.06, JoinDate = DateTime.Now }
            };
            EmployeeTable.RowDatas = RowDatas; // 触发一次刷新
            EmployeeTable.RowDatas.Add(new EmployeeInfo() { Id = 7, Name = "郑九", Age = 24, Gender = true, Salary = 14000.07, JoinDate = DateTime.Now }); // 触发一次刷新
            #endregion 添加数据
        }
        #endregion
        #endregion 【Functions】
    }
}
