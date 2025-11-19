using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Controls;
using System.ComponentModel;
using System.Collections.ObjectModel;
using TigerSan.CsvLog;
using TigerSan.UI.Models;
using TigerSan.UI.Windows;

namespace TigerSan.UI.Controls
{
    /// <summary>
    /// 表格项目
    /// </summary>
    public partial class TableItem : UserControl
    {
        #region 【Fields】
        /// <summary>
        /// 菜单
        /// </summary>
        private MenuWindow? _menu;
        #endregion 【Fields】

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
                new PropertyMetadata(ItemModelChanged));

        private static void ItemModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tableItem = (TableItem)d;

            tableItem.AddEventToItemModel();
            tableItem.DataBinding();
        }
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
                new FrameworkPropertyMetadata(
                    "null",
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        #region 鼠标进入背景
        private void Background_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (ItemModel == null)
            {
                LogHelper.Instance.IsNull(nameof(ItemModel));
                return;
            }
            ItemModel.IsHover = true;
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
            ItemModel.IsHover = false;
        }
        #endregion

        #region 获得焦点
        private void content_GotFocus(object sender, RoutedEventArgs e)
        {
            ShowMenuWindow();
            RaiseGotFocusEvent();
        }
        #endregion

        #region 失去焦点
        private void content_LostFocus(object sender, RoutedEventArgs e)
        {
            RaiseLostFocusEvent();
            SetTextToTarget();
        }
        #endregion

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

            Style = Generic.TransparentUserControlStyle;
        }
        #endregion

        #region 添加事件
        private void AddEvent()
        {
            #region [减]
            #region 背景
            background.MouseEnter -= Background_MouseEnter;
            background.MouseLeave -= Background_MouseLeave;
            content.MouseEnter -= Background_MouseEnter;
            content.MouseLeave -= Background_MouseLeave;
            #endregion 背景

            #region 内容
            content.GotFocus -= content_GotFocus;
            content.LostFocus -= content_LostFocus;
            #endregion 内容
            #endregion [减]

            #region [加]
            #region 背景
            background.MouseEnter += Background_MouseEnter;
            background.MouseLeave += Background_MouseLeave;
            content.MouseEnter += Background_MouseEnter;
            content.MouseLeave += Background_MouseLeave;
            #endregion 背景

            #region 内容
            content.GotFocus += content_GotFocus;
            content.LostFocus += content_LostFocus;
            #endregion 内容
            #endregion [加]
        }
        #endregion

        #region 数据绑定
        private void DataBinding()
        {
            if (ItemModel == null)
            {
                LogHelper.Instance.IsNull(nameof(ItemModel));
                return;
            }

            #region 绑定“Background”
            // 创建双向绑定对象：
            var bindingBackground = new Binding(nameof(ItemModel.Background))
            {
                Source = ItemModel,
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged // 实时更新
            };

            // 应用绑定到目标控件：
            SetBinding(BackgroundProperty, bindingBackground);
            #endregion 绑定“Background”

            #region 绑定“ItemState”
            // 创建双向绑定对象：
            var bindingItemState = new Binding(nameof(ItemModel.ItemState))
            {
                Source = ItemModel,
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, // 实时更新
            };

            // 应用绑定到目标控件：
            SetBinding(ItemStateProperty, bindingItemState);
            #endregion 绑定“ItemState”

            #region 绑定“IsReadOnly”
            // 创建双向绑定对象：
            var bindingIsReadOnly = new Binding(nameof(ItemModel.IsReadOnly))
            {
                Source = ItemModel,
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, // 实时更新
            };

            // 应用绑定到目标控件：
            SetBinding(IsReadOnlyProperty, bindingIsReadOnly);
            #endregion 绑定“IsReadOnly”

            #region 绑定“TextAlignment”
            // 创建双向绑定对象：
            var bindingTextAlignment = new Binding(nameof(ItemModel.TextAlignment))
            {
                Source = ItemModel,
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, // 实时更新
            };

            // 应用绑定到目标控件：
            SetBinding(TextAlignmentProperty, bindingTextAlignment);
            #endregion 绑定“TextAlignment”
        }
        #endregion

        #region 显示“菜单窗口”
        private void ShowMenuWindow()
        {
            if (IsReadOnly) return;

            if (_menu != null) return;

            if (ItemModel == null)
            {
                LogHelper.Instance.IsNull(nameof(ItemModel));
                return;
            }

            // 添加项目：
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

            // 显示菜单：
            _menu = new MenuWindow(this, ItemModels)
            {
                _itemClicked = itemClicked,
                _closed = () => { _menu = null; }
            };

            if (menuWidth != null)
            {
                _menu.Width = menuWidth.Value;
            }

            if (converter != null)
            {
                _menu.Converter = converter;
            }

            _menu.Show();
        }
        #endregion

        #region 向“项目模型”添加事件
        private void AddEventToItemModel()
        {
            if (ItemModel == null)
            {
                LogHelper.Instance.IsNull(nameof(ItemModel));
                return;
            }

            ItemModel._onTargetChanged = SetTargetToText;

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

            ItemModel.Target = Text;
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

            Text = ItemModel.Target;
        }
        #endregion
        #endregion 【Functions】
    }

    #region 设计数据
    public class TableItemDesignData : UserControl
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
        public string Text { get; set; }
        #endregion
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
