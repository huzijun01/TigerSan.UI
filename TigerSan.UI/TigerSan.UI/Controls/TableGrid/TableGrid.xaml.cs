using System.Windows;
using System.Windows.Data;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Collections.Specialized;
using TigerSan.CsvLog;
using TigerSan.UI.Models;
using TigerSan.UI.Helpers;

namespace TigerSan.UI.Controls
{
    public partial class TableGrid : UserControl
    {
        #region 【Fields】
        /// <summary>
        /// 表头行UI元素
        /// </summary>
        private HeaderRowUIElement _headerRowUIElement = new HeaderRowUIElement();

        /// <summary>
        /// 浮动表头行UI元素
        /// </summary>
        private HeaderRowUIElement _floatHeaderRowUIElement = new HeaderRowUIElement();

        /// <summary>
        /// 项目行UI元素集合
        /// </summary>
        private Dictionary<RowModel, ItemRowUIElement> _itemRowUIElements = new Dictionary<RowModel, ItemRowUIElement>();

        /// <summary>
        /// 是否不改变全选状态
        /// </summary>
        private bool _isDoNotChangeIsSelectAll = false;

        /// <summary>
        /// 是否不改变项目选中状态
        /// </summary>
        private bool _isDoNotChangeItemCheckedState = false;
        #endregion 【Fields】

        #region 【DependencyProperties】
        #region 表格模型
        /// <summary>
        /// 表格模型
        /// </summary>
        public TableModel? TableModel
        {
            get { return (TableModel)GetValue(TableModelProperty); }
            set { SetValue(TableModelProperty, value); }
        }
        public static readonly DependencyProperty TableModelProperty =
            DependencyProperty.Register(
                nameof(TableModel),
                typeof(TableModel),
                typeof(TableGrid),
                new PropertyMetadata(TableModelChanged));

        private static void TableModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TableGrid table = (TableGrid)d;

            table.InitTableModelAndUIElement();
        }
        #endregion
        #endregion 【DependencyProperties】

        #region 【Ctor】
        public TableGrid()
        {
            InitializeComponent();
            Style = Generic.TransparentUserControlStyle;
            Loaded += OnLoaded;
        }
        #endregion 【Ctor】

        #region 【Events】
        #region 加载完成
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var table = (TableGrid)sender;
            if (table.TableModel == null)
            {
                LogHelper.Instance.IsNull(nameof(table.TableModel));
                return;
            }

            table.TableModel._onLoaded?.Invoke();
        }
        #endregion

        #region “行数据集合”改变
        private void RowDatas_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            InitUIElements();
        }
        #endregion

        #region “表头复选框”选中
        private void HeaderCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _isDoNotChangeIsSelectAll = true;
            UpdateItemIsChecked();
            _isDoNotChangeIsSelectAll = false;
        }
        #endregion

        #region “表头复选框”未选中
        private void HeaderCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _isDoNotChangeIsSelectAll = true;
            UpdateItemIsChecked();
            _isDoNotChangeIsSelectAll = false;
        }
        #endregion

        #region “项目复选框”选中
        private void ItemCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _isDoNotChangeItemCheckedState = true;
            UpdateIsSelectAll();
            _isDoNotChangeItemCheckedState = false;
        }
        #endregion

        #region “项目复选框”未选中
        private void ItemCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _isDoNotChangeItemCheckedState = true;
            UpdateIsSelectAll();
            _isDoNotChangeItemCheckedState = false;
        }
        #endregion
        #endregion 【Events】

        #region 【Functions】
        #region [Private]
        #region 初始化“表格模型”和“UI元素”
        private void InitTableModelAndUIElement()
        {
            if (TableModel == null)
            {
                LogHelper.Instance.IsNull(nameof(TableModel));
                return;
            }

            TableModel.InitTableModel(this);

            InitUIElements();

            TableModel._onRowDatasCollectionChanged = RowDatas_CollectionChanged;
        }
        #endregion

        #region 清空“UI元素”
        private void ClearUIElements()
        {
            // 网格：
            PART_ItemGrid.Children.Clear();
            PART_ItemGrid.RowDefinitions.Clear();
            PART_ItemGrid.ColumnDefinitions.Clear();
            PART_FloatGrid.Children.Clear();
            PART_FloatGrid.ColumnDefinitions.Clear();
            // 集合：
            _headerRowUIElement.TableHeaders.Clear();
            _floatHeaderRowUIElement.TableHeaders.Clear();
            _itemRowUIElements.Clear();
        }
        #endregion

        #region 初始化“网格”
        private void InitGrid()
        {
            if (TableModel == null)
            {
                LogHelper.Instance.Warning($"The {nameof(TableModel)} is null!");
                return;
            }

            #region 项目网格
            // 行定义：
            var rowCount = TableModel.RowDatas.Count + 1;
            for (int index = 0; index < rowCount; ++index)
            {
                PART_ItemGrid.RowDefinitions.Add(new RowDefinition()
                {
                    Height = TableModel.HeightGridLength,
                });
            }

            // 列定义：
            foreach (var colDef in TableModel._colDefs)
            {
                PART_ItemGrid.ColumnDefinitions.Add(colDef);
            }
            #endregion 项目网格

            #region 浮动网格
            // 列定义：
            foreach (var floatCol in TableModel._floatColDefs)
            {
                PART_FloatGrid.ColumnDefinitions.Add(floatCol);
            }
            #endregion 浮动网格
        }
        #endregion

        #region 初始化“表头行UI元素”集合
        /// <summary>
        /// 初始化“表头行UI元素”集合
        /// </summary>
        private void InitHeaderRowUIElement()
        {
            if (TableModel == null)
            {
                LogHelper.Instance.Warning($"The {nameof(TableModel)} is null!");
                return;
            }

            // 复选框：
            InitTableHeaderCheckBox(_headerRowUIElement.CheckBox, false);
            PART_ItemGrid.Children.Add(_headerRowUIElement.CheckBox);
            // 浮动复选框：
            InitTableHeaderCheckBox(_floatHeaderRowUIElement.CheckBox);
            PART_FloatGrid.Children.Add(_floatHeaderRowUIElement.CheckBox);

            foreach (var headerModel in TableModel.HeaderModels)
            {
                // 表头：
                var tableHeader = new TableHeader(headerModel) { Visibility = Visibility.Hidden };
                _headerRowUIElement.TableHeaders.Add(headerModel, tableHeader);
                GridHelper.SetRowColumn(tableHeader, headerModel.RowIndex, headerModel.ColIndex);
                PART_ItemGrid.Children.Add(tableHeader);

                // 浮动表头：
                var floatTableHeader = new TableHeader(headerModel);
                _floatHeaderRowUIElement.TableHeaders.Add(headerModel, floatTableHeader);
                GridHelper.SetRowColumn(floatTableHeader, headerModel.RowIndex, headerModel.ColIndex);
                PART_FloatGrid.Children.Add(floatTableHeader);

                // 同步宽度：
                tableHeader.SizeChanged += (s, args) =>
                {
                    floatTableHeader.Width = tableHeader.ActualWidth;
                };
            }

            // 水平分割线：
            InitHorizontalDividingLine(_headerRowUIElement.HorizontalDividingLine, 0);
            PART_ItemGrid.Children.Add(_headerRowUIElement.HorizontalDividingLine);
            // 浮动水平分割线：
            InitHorizontalDividingLine(_floatHeaderRowUIElement.HorizontalDividingLine, 0);
            PART_FloatGrid.Children.Add(_floatHeaderRowUIElement.HorizontalDividingLine);
        }
        #endregion

        #region 初始化“项目行UI元素”集合
        /// <summary>
        /// 初始化“项目行UI元素”集合
        /// </summary>
        private void InitItemRowUIElement()
        {
            if (TableModel == null)
            {
                LogHelper.Instance.Warning($"The {nameof(TableModel)} is null!");
                return;
            }

            foreach (var rowModel in TableModel.RowModels)
            {
                var itemRow = new ItemRowUIElement();
                _itemRowUIElements.Add(rowModel.Value, itemRow);

                // 行背景：
                var iRow = TableModel.RowDatas.IndexOf(rowModel.Key);
                InitRowBackground(itemRow.Background, rowModel.Value, iRow + 1);
                PART_ItemGrid.Children.Add(itemRow.Background);

                // 复选框：
                InitItemCheckBox(itemRow.CheckBox, rowModel.Value);
                PART_ItemGrid.Children.Add(itemRow.CheckBox);

                // 水平分割线：
                InitHorizontalDividingLine(itemRow.HorizontalDividingLine, rowModel.Value.RowIndex);
                PART_ItemGrid.Children.Add(itemRow.HorizontalDividingLine);

                // 项目：
                foreach (var itemModel in rowModel.Value.ItemModels)
                {
                    var tableItem = GetTableItem(itemModel.Value);
                    itemRow.TableItems.Add(itemModel.Value, tableItem);
                    PART_ItemGrid.Children.Add(tableItem);
                }
            }
        }
        #endregion

        #region 初始化“行背景”
        private void InitRowBackground(Border border, RowModel rowModel, int row)
        {
            if (TableModel == null)
            {
                LogHelper.Instance.Warning($"The {nameof(TableModel)} is null!");
                return;
            }

            border.Style = Generic.RowBackgroundBorderStyle;
            GridHelper.SetRowColumn(border, row, 0);
            GridHelper.SetColumnSpan(border, TableModel.HeaderModels.Count + 1);

            #region 绑定“Background”
            // 创建双向绑定对象：
            var bindingBackground = new Binding(nameof(rowModel.Background))
            {
                Source = rowModel,
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, // 实时更新
            };

            // 应用绑定到目标控件：
            border.SetBinding(Border.BackgroundProperty, bindingBackground);
            #endregion 绑定“Background”
        }
        #endregion

        #region 初始化“表头复选框”
        private void InitTableHeaderCheckBox(CheckBox checkBox, bool isAddEvent = true)
        {
            checkBox.Style = Generic.TableCheckBoxStyle;
            GridHelper.SetRowColumn(checkBox, 0, 0);

            if (isAddEvent)
            {
                checkBox.Checked += HeaderCheckBox_Checked;
                checkBox.Unchecked += HeaderCheckBox_Unchecked;
            }

            #region 绑定“IsSelectAll”
            // 创建双向绑定对象：
            var bindingIsChecked = new Binding(nameof(TableModel.IsSelectAll))
            {
                Source = TableModel,
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged // 实时更新
            };

            // 应用绑定到目标控件：
            checkBox.SetBinding(CheckBox.IsCheckedProperty, bindingIsChecked);
            #endregion 绑定“IsSelectAll”

            #region 绑定“CheckBoxVisibility”
            // 创建双向绑定对象：
            var bindingVisibility = new Binding(nameof(TableModel.CheckBoxVisibility))
            {
                Source = TableModel,
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged // 实时更新
            };

            // 应用绑定到目标控件：
            checkBox.SetBinding(CheckBox.VisibilityProperty, bindingVisibility);
            #endregion 绑定“CheckBoxVisibility”
        }
        #endregion

        #region 初始化“项目复选框”
        private void InitItemCheckBox(CheckBox checkBox, RowModel rowModel)
        {
            checkBox.Style = Generic.TableCheckBoxStyle;
            checkBox.Checked += ItemCheckBox_Checked;
            checkBox.Unchecked += ItemCheckBox_Unchecked;
            GridHelper.SetRowColumn(checkBox, rowModel.RowIndex, 0);

            #region 绑定“IsSelectAll”
            // 创建双向绑定对象：
            var binding = new Binding(nameof(RowModel.IsChecked))
            {
                Source = rowModel,
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged // 实时更新
            };

            // 应用绑定到目标控件：
            checkBox.SetBinding(CheckBox.IsCheckedProperty, binding);
            #endregion 绑定“IsSelectAll”

            #region 绑定“CheckBoxVisibility”
            // 创建双向绑定对象：
            var bindingVisibility = new Binding(nameof(TableModel.CheckBoxVisibility))
            {
                Source = TableModel,
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged // 实时更新
            };

            // 应用绑定到目标控件：
            checkBox.SetBinding(CheckBox.VisibilityProperty, bindingVisibility);
            #endregion 绑定“CheckBoxVisibility”
        }
        #endregion

        #region 初始化“水平分割线”
        private void InitHorizontalDividingLine(Line line, int row)
        {
            if (TableModel == null)
            {
                LogHelper.Instance.Warning($"The {nameof(TableModel)} is null!");
                return;
            }

            // 添加浮动表头水平分割线：
            line.StrokeThickness = 2;
            line.Style = Generic.HorizontalDividingLineStyle;
            GridHelper.SetRowColumn(line, row, 0);
            GridHelper.SetColumnSpan(line, TableModel.HeaderModels.Count + 1);
        }
        #endregion

        #region 更新“是否全选”
        private void UpdateIsSelectAll()
        {
            if (_isDoNotChangeIsSelectAll) return;

            if (TableModel == null)
            {
                LogHelper.Instance.IsNull(nameof(TableModel));
                return;
            }

            TableModel.IsSelectAll = !TableModel.RowModels.Any(row => !row.Value.IsChecked);

            TableModel._onSelectedRowDatasChanged?.Invoke();
        }
        #endregion

        #region 更新“项目是否选中”
        private void UpdateItemIsChecked()
        {
            if (_isDoNotChangeItemCheckedState) return;

            if (TableModel == null)
            {
                LogHelper.Instance.IsNull(nameof(TableModel));
                return;
            }

            foreach (var rowModel in TableModel.RowModels)
            {
                rowModel.Value.IsChecked = TableModel.IsSelectAll;
            }

            TableModel._onSelectedRowDatasChanged?.Invoke();
        }
        #endregion

        #region 获取“项目”
        private TableItem GetTableItem(ItemModel itemModel)
        {
            var tableItem = new TableItem(itemModel);
            GridHelper.SetRowColumn(tableItem, itemModel.RowIndex, itemModel.ColIndex);

            #region 绑定“ItemState”
            // 创建双向绑定对象：
            var bindingItemState = new Binding(nameof(itemModel.ItemState))
            {
                Source = itemModel,
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, // 实时更新
            };

            // 应用绑定到目标控件：
            tableItem.SetBinding(TableItem.ItemStateProperty, bindingItemState);
            #endregion 绑定“ItemState”

            #region 绑定“IsReadOnly”
            // 创建双向绑定对象：
            var bindingIsReadOnly = new Binding(nameof(itemModel.IsReadOnly))
            {
                Source = itemModel,
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, // 实时更新
            };

            // 应用绑定到目标控件：
            tableItem.SetBinding(TableItem.IsReadOnlyProperty, bindingIsReadOnly);
            #endregion 绑定“IsReadOnly”

            #region 绑定“TextAlignment”
            // 创建双向绑定对象：
            var bindingTextAlignment = new Binding(nameof(itemModel.TextAlignment))
            {
                Source = itemModel,
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, // 实时更新
            };

            // 应用绑定到目标控件：
            tableItem.SetBinding(TableItem.TextAlignmentProperty, bindingTextAlignment);
            #endregion 绑定“TextAlignment”

            return tableItem;
        }
        #endregion
        #endregion [Private]

        #region 初始化“UI元素”
        public void InitUIElements()
        {
            if (TableModel == null)
            {
                LogHelper.Instance.Warning($"The {nameof(TableModel)} is null!");
                return;
            }

            // 清空：
            ClearUIElements();

            // 初始化网格：
            InitGrid();

            // 初始化“表头行UI元素集合”：
            InitHeaderRowUIElement();

            // 初始化“项目行UI元素集合”：
            InitItemRowUIElement();
        }
        #endregion

        #region 初始化“列宽”
        /// <summary>
        /// 初始化“列宽”
        /// </summary>
        public void InitColumnsWidth()
        {
            foreach (var header in _headerRowUIElement.TableHeaders)
            {
                header.Key.SetWidth(header.Value.ActualWidth);
            }
        }
        #endregion
        #endregion 【Functions】
    }
}
