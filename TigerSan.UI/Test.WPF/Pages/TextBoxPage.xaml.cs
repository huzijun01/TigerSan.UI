using System.Windows.Controls;
using Test.WPF.ViewModels;

namespace Test.WPF.Pages
{
    public partial class TextBoxPage : UserControl
    {
        #region 【Ctor】
        public TextBoxPage()
        {
            InitializeComponent();
            DataContext = new TextBoxPageViewModel();
        }
        #endregion 【Ctor】
    }
}
