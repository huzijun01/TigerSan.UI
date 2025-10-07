using System.Windows.Controls;
using Test.WPF.ViewModels;

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
