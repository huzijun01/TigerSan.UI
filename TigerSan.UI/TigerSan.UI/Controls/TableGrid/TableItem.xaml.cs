using System.Windows;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows.Controls;
using TigerSan.CsvLog;
using TigerSan.UI.Models;
using TigerSan.UI.Helpers;
using TigerSan.UI.Windows;
using System.Collections.ObjectModel;

namespace TigerSan.UI.Controls
{
    /// <summary>
    /// 表格项目
    /// </summary>
    public partial class TableItem : UserControl
    {
        #region 【Fields】
        #endregion 【Fields】

        #region 【DependencyProperties】
        #region [OneWay]
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
                new PropertyMetadata(ItemModelChanged));

        private static void ItemModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tableItem = (TableItem)d;

            tableItem.InitItemModel();
        }
        #endregion

        #region 接受换行
        /// <summary>
        /// 接受换行
        /// </summary>
        public bool AcceptsReturn
        {
            get { return (bool)GetValue(AcceptsReturnProperty); }
            set { SetValue(AcceptsReturnProperty, value); }
        }
        public static readonly DependencyProperty AcceptsReturnProperty =
            DependencyProperty.Register(
                nameof(AcceptsReturn),
                typeof(bool),
                typeof(TableItem),
                new PropertyMetadata(true));
        #endregion

        #region 项目状态
        /// <summary>
        /// 项目状态
        /// </summary>
        public ItemState ItemState
        {
            get { return (ItemState)GetValue(ItemStateProperty); }
            set { SetValue(ItemStateProperty, value); }
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

        #region 是否只读
        /// <summary>
        /// 是否只读
        /// </summary>
        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register(
                nameof(IsReadOnly),
                typeof(bool),
                typeof(TableItem),
                new PropertyMetadata(false));
        #endregion

        #region 文本对齐方式
        /// <summary>
        /// 文本对齐方式
        /// </summary>
        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }
        public static readonly DependencyProperty TextAlignmentProperty =
            DependencyProperty.Register(
                nameof(TextAlignment),
                typeof(TextAlignment),
                typeof(TableItem),
                new PropertyMetadata(TextAlignment.Left));
        #endregion
        #endregion 【DependencyProperties】

        #region 【CustomEvents】
        #region 获得焦点
        public new event RoutedEventHandler GotFocus
        {
            add { AddHandler(GotFocusEvent, value); }
            remove { RemoveHandler(GotFocusEvent, value); }
        }
        [Category("Behavior")]
        public static readonly new RoutedEvent GotFocusEvent =
        EventManager.RegisterRoutedEvent(
            nameof(GotFocus),
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(ImageButton));

        protected virtual void RaiseGotFocusEvent()
        {
            var args = new RoutedEventArgs(GotFocusEvent, this);
            RaiseEvent(args);
        }
        #endregion

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
            Init();
        }

        public TableItem(ItemModel item)
        {
            InitializeComponent();
            Init();
            ItemModel = item;
        }
        #endregion 【Ctor】

        #region 【Events】
        #region 点击“项目”
        private void itemClicked(MenuItemModel model)
        {
            if (ItemModel == null)
            {
                LogHelper.Instance.IsNull(nameof(ItemModel));
                return;
            }

            ItemModel.Source = model.Source;
        }
        #endregion
        #endregion 【Events】

        #region 【Functions】
        #region 初始化
        private void Init()
        {
            AddEvent();
            Loaded += (s, e) => { Style = Generic.TransparentUserControlStyle; };
        }
        #endregion

        #region 添加事件
        private void AddEvent()
        {
            BindingHelper.AddValueChanged(
                this,
                typeof(TableItem),
                IsMouseOverProperty,
                (s, e) =>
                {
                    if (ItemModel == null)
                    {
                        LogHelper.Instance.IsNull(nameof(ItemModel));
                        return;
                    }
                    ItemModel.IsHover = IsMouseOver;
                });

            BindingHelper.AddValueChanged(
                content,
                typeof(TextBox),
                IsKeyboardFocusedProperty,
                (s, e) =>
                {
                    if (content.IsKeyboardFocused)
                    {
                        ShowMenuWindow();
                        RaiseGotFocusEvent();
                    }
                    else
                    {
                        SetTextToTarget();
                        RaiseLostFocusEvent();
                    }
                });
        }
        #endregion

        #region 显示“菜单窗口”
        private void ShowMenuWindow()
        {
            if (IsReadOnly) return;

            if (ItemModel == null)
            {
                LogHelper.Instance.IsNull(nameof(ItemModel));
                return;
            }

            #region 添加项目
            var menuDatas = ItemModel._headerModel._menuDatas;
            var menuWidth = ItemModel._headerModel._menuWidth;
            var converter = ItemModel._headerModel.Converter;
            if (menuDatas == null || menuDatas.Count < 1) return;

            ObservableCollection<MenuItemModel> ItemModels = new ObservableCollection<MenuItemModel>();
            foreach (var data in menuDatas)
            {
                if (Equals(ItemModel.Source, data)) continue;

                ItemModels.Add(new MenuItemModel()
                {
                    Source = data
                });
            }
            #endregion 添加项目

            #region 显示菜单
            var menu = new MenuWindow(this, ItemModels)
            {
                _itemClicked = itemClicked
            };

            if (menuWidth != null)
            {
                menu.Width = menuWidth.Value;
            }

            if (converter != null)
            {
                menu.Converter = converter;
            }

            menu.Show();
            #endregion 显示菜单
        }
        #endregion

        #region 初始化“项目模型”
        private void InitItemModel()
        {
            if (ItemModel == null)
            {
                LogHelper.Instance.IsNull(nameof(ItemModel));
                return;
            }

            // 添加事件:
            ItemModel._onTargetChanged = SetTargetToText;

            // 设置文本:
            SetTargetToText();
        }
        #endregion

        #region 将“文本”赋值给“目标数据”
        private void SetTextToTarget()
        {
            if (ItemModel == null)
            {
                LogHelper.Instance.IsNull(nameof(ItemModel));
                return;
            }

            ItemModel.Target = content.Text;
        }
        #endregion

        #region 将“目标数据”赋值给“文本”
        private void SetTargetToText()
        {
            if (ItemModel == null)
            {
                LogHelper.Instance.IsNull(nameof(ItemModel));
                return;
            }

            content.Text = ItemModel.Target;
        }
        #endregion
        #endregion 【Functions】
    }

    #region 设计数据
    public class TableItemDesignData : UserControl
    {
        #region 【DependencyProperties】
        #region [OneWay]
        public bool IsReadOnly { get; set; } = false;
        public ItemState ItemState { get; set; } = ItemState.Normal;
        public Brush MaskBackground { get; set; } = new SolidColorBrush();
        public TextAlignment TextAlignment { get; set; } = TextAlignment.Left;
        #endregion [OneWay]

        public ItemModel? ItemModel { get; set; }
        public string Text { get; set; }
        public bool AcceptsReturn { get; set; } = true;
        #endregion 【DependencyProperties】

        #region 【Ctor】
        public TableItemDesignData()
        {
            Text = "Item";
        }
        #endregion 【Ctor】
    }
    #endregion
}
