using System.Windows.Controls;
using Test.WPF.ViewModels;

namespace Test.WPF.Pages
{
    public partial class NavBarPage : UserControl
    {
        #region 【Ctor】
        public NavBarPage()
        {
            InitializeComponent();
            DataContext = new NavBarPageViewModel();
        }
        #endregion 【Ctor】
    }
}
