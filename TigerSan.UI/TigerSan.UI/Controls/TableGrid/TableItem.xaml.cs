using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using TigerSan.CsvLog;
using TigerSan.UI.Models;

namespace TigerSan.UI.Controls
{
    /// <summary>
    /// 表格项目
    /// </summary>
    public partial class TableItem : UserControl
    {
        #region 【DependencyProperties】
        #region [OneWay]
        #region 是否只读
        /// <summary>
        /// 是否只读
        /// </summary>
        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            private set { SetValue(IsReadOnlyProperty, value); }
        }
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register(
                nameof(IsReadOnly),
                typeof(bool),
                typeof(TableItem),
                new PropertyMetadata(false));
        #endregion

        #region 项目状态
        /// <summary>
        /// 项目状态
        /// </summary>
        public ItemState ItemState
        {
            get { return (ItemState)GetValue(ItemStateProperty); }
            private set { SetValue(ItemStateProperty, value); }
        }
        public static readonly DependencyProperty ItemStateProperty =
            DependencyProperty.Register(
                nameof(ItemState),
                typeof(ItemState),
                typeof(TableItem),
                new PropertyMetadata(ItemState.Normal, ItemStateChanged));

        private static void ItemStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var state = (ItemState)e.NewValue;
            var item = (TableItem)d;

            switch (state)
            {
                case ItemState.Error:
                    item.MaskBackground = Generic.Danger;
                    break;
                case ItemState.Modified:
                    item.MaskBackground = Generic.Warning;
                    break;
                case ItemState.Hover:
                    item.MaskBackground = Generic.BasicWhite;
                    break;
                default:
                    item.MaskBackground = Generic.Transparent;
                    break;
            }
        }
        #endregion

        #region 遮罩背景
        /// <summary>
        /// 遮罩背景
        /// </summary>
        public Brush MaskBackground
        {
            get { return (Brush)GetValue(MaskBackgroundProperty); }
            private set { SetValue(MaskBackgroundProperty, value); }
        }
        public static readonly DependencyProperty MaskBackgroundProperty =
            DependencyProperty.Register(
                nameof(MaskBackground),
                typeof(Brush),
                typeof(TableItem),
                new PropertyMetadata(new SolidColorBrush()));
        #endregion

        #region 文本对齐方式
        /// <summary>
        /// 文本对齐方式
        /// </summary>
        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            private set { SetValue(TextAlignmentProperty, value); }
        }
        public static readonly DependencyProperty TextAlignmentProperty =
            DependencyProperty.Register(
                nameof(TextAlignment),
                typeof(TextAlignment),
                typeof(TableItem),
                new PropertyMetadata(TextAlignment.Left));
        #endregion
        #endregion [OneWay]

        #region 项目模型
        /// <summary>
        /// 项目模型
        /// </summary>
        public ItemModel? ItemModel
        {
            get { return (ItemModel)GetValue(ItemModelProperty); }
            set { SetValue(ItemModelProperty, value); }
        }
        public static readonly DependencyProperty ItemModelProperty =
            DependencyProperty.Register(
                nameof(ItemModel),
                typeof(ItemModel),
                typeof(TableItem),
                new PropertyMetadata());
        #endregion

        #region 文本
        /// <summary>
        /// 文本
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                nameof(Text),
                typeof(string),
                typeof(TableItem),
                new PropertyMetadata("null"));
        #endregion
        #endregion 【DependencyProperties】

        #region 【CustomEvents】
        #region 失去焦点
        public new event RoutedEventHandler LostFocus
        {
            add { AddHandler(LostFocusEvent, value); }
            remove { RemoveHandler(LostFocusEvent, value); }
        }
        [Category("Behavior")]
        public static readonly new RoutedEvent LostFocusEvent =
        EventManager.RegisterRoutedEvent(
            nameof(LostFocus),
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(ImageButton));

        protected virtual void RaiseLostFocusEvent()
        {
            var args = new RoutedEventArgs(LostFocusEvent, this);
            RaiseEvent(args);
        }
        #endregion
        #endregion 【CustomEvents】

        #region 【Ctor】
        public TableItem()
        {
            InitializeComponent();
            Loaded += TableItem_Loaded;
        }

        public TableItem(ItemModel item)
        {
            InitializeComponent();
            ItemModel = item;
            Loaded += TableItem_Loaded;
        }
        #endregion 【Ctor】

        #region 【Events】
        #region 初始化完成
        private void TableItem_Loaded(object sender, RoutedEventArgs e)
        {
            AddEvent();
            DataBinding();
            Style = Generic.TransparentUserControlStyle;
        }
        #endregion

        #region 鼠标进入背景
        private void Background_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (ItemModel == null)
            {
                LogHelper.Instance.IsNull(nameof(ItemModel));
                return;
            }
            ItemModel._isHover = true;
            ItemModel.UpdateItemState();
        }
        #endregion

        #region 鼠标离开背景
        private void Background_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (ItemModel == null)
            {
                LogHelper.Instance.IsNull(nameof(ItemModel));
                return;
            }
            ItemModel._isHover = false;
            ItemModel.UpdateItemState();
        }
        #endregion

        #region 失去焦点
        private void content_LostFocus(object sender, RoutedEventArgs e)
        {
            RaiseLostFocusEvent();
        }
        #endregion
        #endregion 【Events】

        #region 【Functions】
        #region 数据绑定
        private void DataBinding()
        {
            #region 绑定“Background”
            // 创建双向绑定对象：
            var bindingBackground = new Binding(nameof(ItemModel.Background))
            {
                Source = ItemModel,
                Mode = BindingMode.OneWay, // 启用双向绑定
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged // 实时更新
            };

            // 应用绑定到目标控件：
            SetBinding(BackgroundProperty, bindingBackground);
            #endregion 绑定“Background”
        }
        #endregion

        #region 添加事件
        private void AddEvent()
        {
            #region 背景
            // 减：
            background.MouseEnter -= Background_MouseEnter;
            background.MouseLeave -= Background_MouseLeave;
            content.MouseEnter -= Background_MouseEnter;
            content.MouseLeave -= Background_MouseLeave;
            // 加：
            background.MouseEnter += Background_MouseEnter;
            background.MouseLeave += Background_MouseLeave;
            content.MouseEnter += Background_MouseEnter;
            content.MouseLeave += Background_MouseLeave;
            #endregion
        }
        #endregion
        #endregion 【Functions】
    }

    #region 设计数据
    public class TableItemDesignData : TableItem
    {
        public TableItemDesignData()
        {
            Text = "Item";
        }
    }
    #endregion
}
