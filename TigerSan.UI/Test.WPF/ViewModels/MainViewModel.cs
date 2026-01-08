using Test.WPF.Pages;
using TigerSan.UI;
using TigerSan.UI.Models;

namespace Test.WPF.ViewModels
{
    public class MainViewModel : BindableBase
    {
        #region 【Properties】
        public NavBarModel NavBarModel
        {
            get { return _NavBarModel; }
            set { SetProperty(ref _NavBarModel, value); }
        }
        private NavBarModel _NavBarModel = new NavBarModel();
        #endregion 【Properties】

        #region 【Ctor】
        public MainViewModel()
        {
            #region 导航
            var fdrNav = new NavFolderModel(NavBarModel) { Title = "导航", Icon = Icons.Table };
            NavBarModel.AddFolder(fdrNav);

            var btnNavBarPage = new NavButtonModel(NavBarModel, fdrNav, typeof(NavBarPage)) { Title = "导航栏", Icon = Icons.Table };
            fdrNav.ButtonModels.Add(btnNavBarPage);
            #endregion 导航

            #region 基础
            var fdrBase = new NavFolderModel(NavBarModel) { Title = "基础" };
            NavBarModel.AddFolder(fdrBase);

            var btnButtonPage = new NavButtonModel(NavBarModel, fdrBase, typeof(ButtonPage)) { Title = "按钮" };
            fdrBase.ButtonModels.Add(btnButtonPage);
            #endregion 基础

            #region 表单
            var fdrForm = new NavFolderModel(NavBarModel) { Title = "表单", Icon = Icons.Menu_Icon };
            NavBarModel.AddFolder(fdrForm);

            var btnTextBoxPage = new NavButtonModel(NavBarModel, fdrForm, typeof(TextBoxPage)) { Title = "文本框", Icon = Icons.TextBox_T };
            fdrForm.ButtonModels.Add(btnTextBoxPage);

            var btnFormPage = new NavButtonModel(NavBarModel, fdrForm, typeof(FormPage)) { Title = "输入框", Icon = Icons.Switch };
            fdrForm.ButtonModels.Add(btnFormPage);

            var btnTablePage = new NavButtonModel(NavBarModel, fdrForm, typeof(TablePage)) { Title = "表格", Icon = Icons.Table };
            fdrForm.ButtonModels.Add(btnTablePage);
            #endregion 基础

            #region 反馈
            var fdrFeedback = new NavFolderModel(NavBarModel) { Title = "反馈", Icon = Icons.Msg_Circle };
            NavBarModel.AddFolder(fdrFeedback);

            var btnDialogPage = new NavButtonModel(NavBarModel, fdrFeedback, typeof(DialogPage)) { Title = "弹窗", Icon = Icons.Window };
            fdrFeedback.ButtonModels.Add(btnDialogPage);

            var btnLoadingPage = new NavButtonModel(NavBarModel, fdrFeedback, typeof(LoadingPage)) { Title = "加载", Icon = Icons.Loading_Dot };
            fdrFeedback.ButtonModels.Add(btnLoadingPage);
            #endregion 基础

            #region 展示
            var fdrShow = new NavFolderModel(NavBarModel) { Title = "展示", Icon = Icons.Album };
            NavBarModel.AddFolder(fdrShow);

            var btnImageButtonPage = new NavButtonModel(NavBarModel, fdrShow, typeof(ImageButtonPage)) { Title = "图片按钮", Icon = Icons.PIC };
            fdrShow.ButtonModels.Add(btnImageButtonPage);
            #endregion 展示

            #region 行为
            var fdrBehavior = new NavFolderModel(NavBarModel) { Title = "行为", Icon = Icons.Animation };
            NavBarModel.AddFolder(fdrBehavior);

            var btnDragPage = new NavButtonModel(NavBarModel, fdrBehavior, typeof(DragPage)) { Title = "拖拽", Icon = Icons.Drag_Hand };
            fdrBehavior.ButtonModels.Add(btnDragPage);
            #endregion 导航

            NavBarModel.OpenedButtonModels.Add(btnTablePage);
            NavBarModel.IsOpen = false;
            btnTablePage.IsShowCloseButton = false;
        }
        #endregion 【Ctor】
    }
}
