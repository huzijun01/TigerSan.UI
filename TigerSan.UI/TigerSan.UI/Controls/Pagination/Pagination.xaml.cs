using System.Windows;
using System.Windows.Controls;
using TigerSan.UI.Models;
using TigerSan.UI.Helpers;
using System.Windows.Input;

namespace TigerSan.UI.Controls
{
    public partial class Pagination : UserControl
    {
        #region 【DependencyProperties】
        #region [OneWay]
        #endregion [OneWay]

        #region 分页模型
        /// <summary>
        /// 分页模型
        /// </summary>
        public PaginationModel PaginationModel
        {
            get { return (PaginationModel)GetValue(PaginationModelProperty); }
            set { SetValue(PaginationModelProperty, value); }
        }
        public static readonly DependencyProperty PaginationModelProperty =
            DependencyProperty.Register(
                nameof(PaginationModel),
                typeof(PaginationModel),
                typeof(Pagination),
                new PropertyMetadata(new PaginationModel()));
        #endregion
        #endregion 【DependencyProperties】

        #region 【Ctor】
        public Pagination()
        {
            InitializeComponent();
            txtPageText.KeyDown += TxtPageText_KeyDown;
            txtPageText.TextChanged += TxtPageText_TextChanged;
        }
        #endregion 【Ctor】

        #region 【Events】
        #region “页文本框”键盘按下
        private void TxtPageText_KeyDown(object sender, KeyEventArgs e)
        {
            if (!Equals(e.Key, Key.Enter)) return;

            PaginationModel.GoToPage();
        }
        #endregion

        #region “页文本框”文本改变后
        private void TxtPageText_TextChanged(object sender, TextChangedEventArgs e)
        {
            PaginationModel.PageText = txtPageText.Text;
        }
        #endregion
        #endregion 【Events】
    }

    #region 设计数据
    public class PaginationDesignData : UserControl
    {
        #region 【Properties】
        public PaginationModel PaginationModel { get; set; } = new PaginationModel();
        #endregion 【Properties】

        #region 【Ctor】
        public PaginationDesignData()
        {
            Init(PaginationModel);
        }
        #endregion 【Ctor】

        #region 【Functions】
        public static void Init(PaginationModel paginationModel)
        {
            paginationModel.Count = 50;
            paginationModel.PageSize = 5;
            paginationModel.MaxShowPageCount = 5;
            paginationModel.OnChecked = buttonModel =>
            {
                MsgBox.ShowInformation($"{nameof(buttonModel.Num)} = {buttonModel.Num}");
            };
        }
        #endregion 【Functions】
    }
    #endregion
}
