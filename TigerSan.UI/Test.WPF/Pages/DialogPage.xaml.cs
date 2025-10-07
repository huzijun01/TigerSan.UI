using System.Windows.Controls;
using Test.WPF.ViewModels;

namespace Test.WPF.Pages
{
    public partial class DialogPage : UserControl
    {
        #region 【Ctor】
        public DialogPage()
        {
            InitializeComponent();
            DataContext = new DialogPageViewModel();
        }
        #endregion 【Ctor】
    }
}
