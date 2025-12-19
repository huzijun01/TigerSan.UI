using System.Windows.Controls;
using Test.WPF.ViewModels;

namespace Test.WPF.Pages
{
    public partial class DragPage : UserControl
    {
        #region 【Ctor】
        public DragPage()
        {
            InitializeComponent();
            DataContext = new DragPageViewModel();
        }
        #endregion 【Ctor】
    }
}
