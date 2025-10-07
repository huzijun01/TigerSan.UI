using System.Windows.Controls;
using Test.WPF.ViewModels;

namespace Test.WPF.Pages
{
    public partial class TablePage : UserControl
    {
        #region 【Ctor】
        public TablePage()
        {
            InitializeComponent();
            DataContext = new TablePageViewModel();
        }
        #endregion 【Ctor】
    }
}
