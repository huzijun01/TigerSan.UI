using System.Windows;
using System.Windows.Controls;
using TigerSan.UI.Models;

namespace TigerSan.UI.Controls
{
    public partial class PageBar : UserControl
    {
        #region 【DependencyProperties】
        #region 导航栏模型
        /// <summary>
        /// 导航栏模型
        /// </summary>
        public NavBarModel NavBarModel
        {
            get { return (NavBarModel)GetValue(NavBarModelProperty); }
            set { SetValue(NavBarModelProperty, value); }
        }
        public static readonly DependencyProperty NavBarModelProperty =
            DependencyProperty.Register(
                nameof(NavBarModel),
                typeof(NavBarModel),
                typeof(PageBar),
                new PropertyMetadata(new NavBarModel()));
        #endregion
        #endregion 【DependencyProperties】

        #region 【Ctor】
        public PageBar()
        {
            InitializeComponent();
        }
        #endregion 【Ctor】
    }

    #region 设计数据
    public class PageBarDesignData : UserControl
    {
        #region 【Properties】
        public NavBarModel NavBarModel { get; set; } = new NavBarModel();
        #endregion 【Properties】

        #region 【Ctor】
        public PageBarDesignData()
        {
            NavBarModel.OpenedButtonModels.Add(NavBarModel.GetDefaultButtonModel());
            NavBarModel.OpenedButtonModels.Add(NavBarModel.GetDefaultButtonModel());
            NavBarModel.OpenedButtonModels.Add(NavBarModel.GetDefaultButtonModel());
        }
        #endregion 【Ctor】
    }
    #endregion
}
