using System.Windows.Controls;
using Test.WPF.ViewModels;

namespace Test.WPF.Pages
{
    public partial class ButtonPage : UserControl
    {
        #region 【Ctor】
        public ButtonPage()
        {
            InitializeComponent();
            DataContext = new ButtonPageViewModel();
        }
        #endregion 【Ctor】
    }
}
