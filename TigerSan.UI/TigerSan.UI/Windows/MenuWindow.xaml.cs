using System.Collections.ObjectModel;
using System.Windows;
using TigerSan.CsvLog;
using TigerSan.UI.Controls;
using TigerSan.UI.Helpers;
using TigerSan.UI.Models;

namespace TigerSan.UI.Windows
{
    public partial class MenuWindow : Window
    {
        #region 【Fields】
        /// <summary>
        /// 选择器
        /// </summary>
        private Select _select;

        /// <summary>
        /// 是否被关闭
        /// </summary>
        private bool _isClosed = false;
        #endregion 【Fields】

        #region 【DependencyProperties】
        #region 项目集合
        /// <summary>
        /// 项目集合
        /// </summary>
        public ObservableCollection<MenuItemModel> Items
        {
            get { return (ObservableCollection<MenuItemModel>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register(
                nameof(Items),
                typeof(ObservableCollection<MenuItemModel>),
                typeof(MenuWindow),
                new PropertyMetadata(new ObservableCollection<MenuItemModel>()));
        #endregion
        #endregion 【DependencyProperties】

        #region 【Ctor】
        public MenuWindow(Select select)
        {
            InitializeComponent();
            _select = select;
            Loaded += OnLoaded;
            Closed += OnClosed;
            Deactivated += OnDeactivated;
            InitItems();
        }
        #endregion 【Ctor】

        #region 【Events】
        #region 加载完成
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            InitSize();
            InitPosition();
        }
        #endregion

        #region 关闭后
        private void OnClosed(object? sender, EventArgs e)
        {
            _isClosed = true;
        }
        #endregion

        #region 失活后
        private void OnDeactivated(object? sender, EventArgs e)
        {
            Close();
        }
        #endregion

        #region 项目被点击
        private void OnItemClicked(MenuItemModel itemModel)
        {
            Close();
            if (_select == null)
            {
                LogHelper.Instance.IsNull(nameof(_select));
                return;
            }
            _select.Value = itemModel.Source;
        }
        #endregion
        #endregion 【Events】

        #region 【Functions】
        #region 初始尺寸
        private void InitSize()
        {
            Width = _select.ActualWidth;

            if (itemsControl.ActualHeight < MaxHeight)
            {
                Height = itemsControl.ActualHeight;
            }
        }
        #endregion

        #region 初始位置
        private void InitPosition()
        {
            //ShowTop();
            ShowBottom();
            //ShowLeft();
            //ShowRight();
        }
        #endregion

        #region 初始位置
        private void InitItems()
        {
            Items = _select.Items;

            foreach (var item in Items)
            {
                item._select = _select;
                item._internalClicked = OnItemClicked;
                item.UpdateText();
            }
        }
        #endregion

        #region 安全关闭
        public new void Close()
        {
            if (_isClosed) return;
            _isClosed = true;
            base.Close();
            _select.IsOpen = false;
        }
        #endregion

        #region 上方显示
        private void ShowTop()
        {
            var position = SystemHelper.GetScreenPosition(_select);
            if (position == null)
            {
                LogHelper.Instance.IsNull(nameof(position));
                return;
            }

            Left = position.Value.X;
            Top = position.Value.Y - ActualHeight;
        }
        #endregion

        #region 下方显示
        private void ShowBottom()
        {
            var position = SystemHelper.GetScreenPosition(_select);
            if (position == null)
            {
                LogHelper.Instance.IsNull(nameof(position));
                return;
            }

            Left = position.Value.X;
            Top = position.Value.Y + _select.ActualHeight;
        }
        #endregion

        #region 左侧显示
        private void ShowLeft()
        {
            var position = SystemHelper.GetScreenPosition(_select);
            if (position == null)
            {
                LogHelper.Instance.IsNull(nameof(position));
                return;
            }

            Left = position.Value.X - ActualWidth;
            Top = position.Value.Y + _select.ActualHeight / 2 - ActualHeight / 2;
        }
        #endregion

        #region 右侧显示
        private void ShowRight()
        {
            var position = SystemHelper.GetScreenPosition(_select);
            if (position == null)
            {
                LogHelper.Instance.IsNull(nameof(position));
                return;
            }

            Left = position.Value.X + _select.ActualWidth;
            Top = position.Value.Y + _select.ActualHeight / 2 - ActualHeight / 2;
        }
        #endregion
        #endregion 【Functions】
    }

    #region 设计数据
    public class MenuItemDesignModel : MenuItemModel
    {
        public new string Text { get; set; } = string.Empty;
    }

    public class MenuWindowDesignData
    {
        public ObservableCollection<MenuItemDesignModel> Items { get; set; } = new ObservableCollection<MenuItemDesignModel>();

        public MenuWindowDesignData()
        {
            Items.Add(new MenuItemDesignModel() { Text = "Item 1" });
            Items.Add(new MenuItemDesignModel() { Text = "Item 2" });
            Items.Add(new MenuItemDesignModel() { Text = "Item 3" });
        }
    }
    #endregion
}
