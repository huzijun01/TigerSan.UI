using System.Windows.Input;
using System.Windows.Controls;
using Test.WPF.ViewModels;
using TigerSan.UI.Helpers;

namespace Test.WPF.Pages
{
    public partial class FormPage : UserControl
    {
        #region 【Ctor】
        public FormPage()
        {
            InitializeComponent();
            DataContext = new FormPageViewModel();
        }
        #endregion 【Ctor】
    }
}
