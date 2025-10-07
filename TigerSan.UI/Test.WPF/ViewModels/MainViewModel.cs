using System.IO;
using System.Text;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Test.WPF.Models;
using TigerSan.CsvLog;
using TigerSan.UI;
using TigerSan.UI.Models;
using TigerSan.UI.Helpers;
using TigerSan.UI.Windows;
using TigerSan.UI.Converters;

namespace Test.WPF.ViewModels
{
    public class MainViewModel : BindableBase
    {
        #region 【Fields】
        DialogService _dialogService;
        #endregion 【Properties】

        #region 【Properties】
        /// <summary>
        /// 文本
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }
        private string _text = string.Empty;

        /// <summary>
        /// 文本域文本
        /// </summary>
        public string TextAreaText
        {
            get { return _textAreaText; }
            set { SetProperty(ref _textAreaText, value); }
        }
        private string _textAreaText = string.Empty;

        /// <summary>
        /// 开关值
        /// </summary>
        public bool SwitchValue
        {
            get { return _switchValue; }
            set { SetProperty(ref _switchValue, value); }
        }
        private bool _switchValue = false;

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

        /// <summary>
        /// 菜单项目模型集合
        /// </summary>
        public ObservableCollection<MenuItemModel> MenuItemModels { get; set; } = new ObservableCollection<MenuItemModel>();
        #endregion 【Properties】

        #region 【Ctor】
        public MainViewModel(DialogService dialogService)
        {
            _dialogService = dialogService;
        }
        #endregion 【Ctor】

        #region 【Evnets】
        #region 选中行改变
        private void OnSelectedRowDatasChanged()
        {
            SelectedRowCount = EmployeeTable.SelectedRowCount;
        }
        #endregion
        #endregion 【Evnets】

        #region 【Commands】
        #region 窗口加载完成
        public ICommand Window_LoadedCommand { get => new DelegateCommand(Window_Loaded); }
        private void Window_Loaded()
        {
            InitTable();
            InitTextBox();
            InitMenuItemModels();
        }
        #endregion

        #region [表格]
        #region 点击“Refresh”按钮
        public ICommand btnRefresh_ClickCommand { get => new DelegateCommand(btnRefresh_Click); }
        private void btnRefresh_Click()
        {
            EmployeeTable.Refresh();
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
        #endregion [表格]

        #region [弹窗]
        #region 点击“信息”按钮
        public ICommand btnInformation_ClickCommand { get => new DelegateCommand(btnInformation_Click); }
        private void btnInformation_Click()
        {
            MsgBox.ShowInformation("信息内容");
        }
        #endregion

        #region 点击“成功”按钮
        public ICommand btnSucces_ClickCommand { get => new DelegateCommand(btnSucces_Click); }
        private void btnSucces_Click()
        {
            MsgBox.ShowSuccess("成功内容");
        }
        #endregion

        #region 点击“警告”按钮
        public ICommand btnWarning_ClickCommand { get => new DelegateCommand(btnWarning_Click); }
        private void btnWarning_Click()
        {
            MsgBox.ShowWarning("警告内容");
        }
        #endregion

        #region 点击“错误”按钮
        public ICommand btnError_ClickCommand { get => new DelegateCommand(btnError_Click); }
        private void btnError_Click()
        {
            MsgBox.ShowError("错误内容");
        }
        #endregion

        #region 点击“弹窗”按钮
        public ICommand btnShowDialog_ClickCommand { get => new AsyncDelegateCommand(btnShowDialog_Click); }
        private async Task btnShowDialog_Click()
        {
            var text = File.ReadAllText(@"Files\text.txt", Encoding.UTF8);
            var res = await MsgBox.ShowDialogAsync(MsgType.Information, text);

            string sreResult;

            switch (res)
            {
                case DialogResults.Yes:
                    sreResult = nameof(DialogResults.Yes);
                    break;
                case DialogResults.No:
                    sreResult = nameof(DialogResults.No);
                    break;
                default:
                    sreResult = nameof(DialogResults.Cancel);
                    break;
            }

            MsgBox.ShowInformation(sreResult);
        }
        #endregion
        #endregion [弹窗]

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

        #region [表单]
        #region “选择器”值改变后
        public ICommand Select_ValueChangedCommand { get => new DelegateCommand<object>(Select_ValueChanged); }
        private void Select_ValueChanged(object value)
        {
            MsgBox.ShowInformation($"The \"{nameof(value)}\" is \"{value}\"!");
        }
        #endregion
        #endregion [表单]
        #endregion 【Commands】

        #region 【Functions】
        #region 初始化“表格”
        public void InitTable()
        {
            // 是否显示复选框：
            EmployeeTable.IsShowCheckBox = true;
            EmployeeTable._onSelectedRowDatasChanged -= OnSelectedRowDatasChanged;
            EmployeeTable._onSelectedRowDatasChanged += OnSelectedRowDatasChanged;

            // 设置表头：
            var headerId = EmployeeTable.GetHeaderModel(nameof(EmployeeInfo.Id));
            if (headerId == null) return;
            headerId.Converter = new Int2StringConverter();

            var headerName = EmployeeTable.GetHeaderModel(nameof(EmployeeInfo.Name));
            if (headerName == null) return;
            headerName.Verification = Verifications.IsNotNullOrEmpty;
            headerName.Background = Generic.Brand;

            var headerAge = EmployeeTable.GetHeaderModel(nameof(EmployeeInfo.Age));
            if (headerAge == null) return;
            headerAge.Converter = new Int2StringConverter();

            var headerGender = EmployeeTable.GetHeaderModel(nameof(EmployeeInfo.Gender));
            if (headerGender == null) return;
            headerGender.Converter = new Bool2StringConverter("男", "女");

            var headerSalary = EmployeeTable.GetHeaderModel(nameof(EmployeeInfo.Salary));
            if (headerSalary == null) return;
            headerSalary.Converter = new Double2StringConverter();

            var headerJoinDate = EmployeeTable.GetHeaderModel(nameof(EmployeeInfo.JoinDate));
            if (headerJoinDate == null) return;
            headerJoinDate.Converter = new DateTime2StringConverter();

            // 添加数据：
            var RowDatas = new ObservableCollection<object>
            {
                new EmployeeInfo() { Id = 1, Name = "张三", Age = 18, Gender = true, Salary = 8000.01, JoinDate = DateTime.Now },
                new EmployeeInfo() { Id = 2, Name = "李四", Age = 19, Gender = false, Salary = 9000.02, JoinDate = DateTime.Now },
                new EmployeeInfo() { Id = 3, Name = "王五", Age = 20, Gender = true, Salary = 10000.03, JoinDate = DateTime.Now },
                new EmployeeInfo() { Id = 4, Name = "赵六", Age = 21, Gender = false, Salary = 11000.04, JoinDate = DateTime.Now },
                new EmployeeInfo() { Id = 5, Name = "吴七", Age = 22, Gender = true, Salary = 12000.05, JoinDate = DateTime.Now },
                new EmployeeInfo() { Id = 6, Name = "周八", Age = 23, Gender = true, Salary = 13000.06, JoinDate = DateTime.Now }
            };
            EmployeeTable.RowDatas = RowDatas;
            EmployeeTable.RowDatas.Add(new EmployeeInfo() { Id = 7, Name = "郑九", Age = 24, Gender = true, Salary = 14000.07, JoinDate = DateTime.Now });

            // 设置背景：
            var count = EmployeeTable.RowDatas.Count;
            for (int iRow = 0; iRow < count; iRow++)
            {
                var item = EmployeeTable.GetItemModel(iRow, nameof(EmployeeInfo.Name));
                if (item == null)
                {
                    LogHelper.Instance.IsNull(nameof(item));
                    continue;
                }
                item.Background = Generic.Brand;
            }
        }
        #endregion

        #region 初始化“文本框”
        public void InitTextBox()
        {
            TextAreaText = File.ReadAllText(@"Files\text_area_text.txt", Encoding.UTF8);
        }
        #endregion

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

        #region “菜单项目”被点击
        private void MenuItemClicked(MenuItemModel itemModel)
        {
            MsgBox.ShowInformation($"The \"{itemModel.Source}\" has been clicked!");
        }
        #endregion
        #endregion 【Functions】
    }
}
