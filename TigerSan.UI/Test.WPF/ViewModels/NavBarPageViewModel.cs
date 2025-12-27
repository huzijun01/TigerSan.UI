using TigerSan.UI.Models;
using TigerSan.UI.Controls;

namespace Test.WPF.ViewModels
{
    public class NavBarPageViewModel : BindableBase
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
        public NavBarPageViewModel()
        {
            NavBarDesignData.Init(NavBarModel, NavBarModel.FolderModel, false);
        }
        #endregion 【Ctor】
    }
}
