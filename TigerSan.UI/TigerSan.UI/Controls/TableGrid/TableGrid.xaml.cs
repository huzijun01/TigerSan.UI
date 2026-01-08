using System.Windows;
using System.Windows.Data;
using System.Windows.Shapes;
using System.Windows.Controls;
using TigerSan.CsvLog;
using TigerSan.UI.Models;
using TigerSan.UI.Helpers;

namespace TigerSan.UI.Controls
{
    public partial class TableGrid : UserControl
    {
        #region 【Fields】
        private List<TableHeader> _headers = new List<TableHeader>();
        private List<TableHeader> _floatHeaders = new List<TableHeader>();
        private List<double> _colWidths = new List<double>();
        #endregion 【Fields】

        #region 【DependencyProperties】
        #region 表格模型
        /// <summary>
        /// 表格模型
        /// </summary>
        public TableModel TableModel
        {
            get { return (TableModel)GetValue(TableModelProperty); }
            set { SetValue(TableModelProperty, value); }
        }
        public static readonly DependencyProperty TableModelProperty =
            DependencyProperty.Register(
                nameof(TableModel),
                typeof(TableModel),
                typeof(TableGrid),
                new PropertyMetadata(new TableModel(typeof(object)), TableModelChanged));

        private static void TableModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var table = (TableGrid)d;
            ((TableModel)e.OldValue)._tableGrid = null;
            ((TableModel)e.NewValue)._tableGrid = table;
        }
        #endregion
        #endregion 【DependencyProperties】

        #region 【Ctor】
        public TableGrid()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            ItemGrid.SizeChanged += ItemGrid_SizeChanged;
        }
        #endregion 【Ctor】

        #region 【Events】
        #region 加载完成
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Refresh();
            TableModel._onLoaded?.Invoke();
        }
        #endregion

        #region “项目网格”尺寸改变后
        /// <summary>
        /// “项目网格”尺寸改变后
        /// （内容相同时不会触发）
        /// </summary>
        private void ItemGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateFloatGridSize();
        }
        #endregion
        #endregion 【Events】

        #region 【Functions】
        #region [Private]
        #region 初始化“列头”集合
        private void InitHeaders()
        {
            ItemGrid.Children.Clear();
            FloatGrid.Children.Clear();

            AddHeaders(_headers, ItemGrid);
            AddHeaders(_floatHeaders, FloatGrid);

            BindingHeaderWidth();
        }
        #endregion

        #region 更新“悬浮网格”尺寸
        private void UpdateFloatGridSize()
        {
            if (ItemGrid.ColumnDefinitions.Count != FloatGrid.ColumnDefinitions.Count)
            {
                LogHelper.Instance.Warning("The number of grid columns is not equal!");
                return;
            }

            _colWidths.Clear();

            for (int iCol = 0; iCol < ItemGrid.ColumnDefinitions.Count; iCol++)
            {
                var itemCol = ItemGrid.ColumnDefinitions[iCol];
                var floatCol = FloatGrid.ColumnDefinitions[iCol];

                _colWidths.Add(itemCol.ActualWidth);
                floatCol.Width = new GridLength(itemCol.ActualWidth);
            }
        }
        #endregion

        #region 更新“悬浮网格”尺寸（使用缓存）
        private void UpdateFloatGridSizeFromCache()
        {
            if (_colWidths.Count != FloatGrid.ColumnDefinitions.Count)
            {
                //LogHelper.Instance.Warning("The number of grid columns is not equal!");
                return;
            }

            for (int iCol = 0; iCol < ItemGrid.ColumnDefinitions.Count; iCol++)
            {
                var width = _colWidths[iCol];
                var floatCol = FloatGrid.ColumnDefinitions[iCol];

                floatCol.Width = new GridLength(width);
            }
        }
        #endregion

        #region 绑定“列头宽度”
        private void BindingHeaderWidth()
        {
            var headerCount = TableModel.HeaderModels.Count;

            for (int iCol = 0; iCol < headerCount; iCol++)
            {
                var headerModel = TableModel.HeaderModels[iCol];
                var itemCol = ItemGrid.ColumnDefinitions[iCol + 1];

                #region 绑定“Width”
                // 创建双向绑定对象：
                var bindingWidth = new Binding(nameof(headerModel.WidthGridLength))
                {
                    Source = headerModel,
                    Mode = BindingMode.OneWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, // 实时更新
                };

                // 应用绑定到目标控件：
                itemCol.SetBinding(ColumnDefinition.WidthProperty, bindingWidth);
                #endregion 绑定“Width”
            }
        }
        #endregion

        #region 添加“列头”集合
        private void AddHeaders(List<TableHeader> headers, Grid grid)
        {
            headers.Clear();

            grid.Children.Add(GetHorizontalDividingLine(0, 2));

            #region 复选框
            var ckbSelectAll = GetHeaderCheckBox(TableModel);
            grid.Children.Add(ckbSelectAll);

            if (Equals(headers, _headers))
            {
                ckbSelectAll.DataContext = new TableModel(typeof(object))
                { IsShowCheckBox = TableModel.IsShowCheckBox }; // 防止命令重复触发
            }
            #endregion 复选框

            foreach (var headerModel in TableModel.HeaderModels)
            {
                var header = (TableHeader)Generic.TableHeaderTemplate.LoadContent();
                header.DataContext = headerModel;
                grid.Children.Add(header);
                headers.Add(header);
            }
        }
        #endregion

        #region 初始化“项目”集合
        private void InitItems()
        {
            for (int iRow = 0; iRow < TableModel.RowDatas.Count; iRow++)
            {
                var rowModel = TableModel.GetRowModel(iRow);
                if (rowModel == null)
                {
                    LogHelper.Instance.IsNull(nameof(rowModel));
                    return;
                }

                ItemGrid.Children.Add(GetHorizontalDividingLine(iRow + 1));
                ItemGrid.Children.Add(GetRowBackground(rowModel, iRow + 1));

                #region 复选框
                var ckbItem = GetHeaderCheckBox(rowModel, iRow + 1);
                ItemGrid.Children.Add(ckbItem);
                #endregion 复选框

                foreach (var itemModel in rowModel.ItemModels)
                {
                    var item = (TableItem)Generic.TableItemTemplate.LoadContent();
                    item.DataContext = itemModel.Value;
                    ItemGrid.Children.Add(item);
                }
            }
        }
        #endregion

        #region 获取“水平分隔线”
        private Line GetHorizontalDividingLine(int iRow, double strokeThickness = 1)
        {
            var line = new Line() { Style = Generic.HorizontalDividingLineStyle, StrokeThickness = strokeThickness };
            GridHelper.SetRowColumn(line, iRow, 0);
            GridHelper.SetColumnSpan(line, TableModel.HeaderModels.Count + 1);
            return line;
        }
        #endregion

        #region 获取“行背景”
        private Border GetRowBackground(RowModel rowModel, int row)
        {
            var border = new Border();

            if (TableModel == null)
            {
                LogHelper.Instance.Warning($"The {nameof(TableModel)} is null!");
                return border;
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

            return border;
        }
        #endregion

        #region 获取“列头复选框”
        private CheckBox GetHeaderCheckBox(TableModel tableModel)
        {
            var checkBox = (CheckBox)Generic.TableHeaderCheckBoxTemplate.LoadContent();
            checkBox.DataContext = tableModel;
            GridHelper.SetRowColumn(checkBox, 0, 0);
            return checkBox;
        }
        #endregion

        #region 获取“项目复选框”
        private CheckBox GetHeaderCheckBox(RowModel rowModel, int row)
        {
            var checkBox = (CheckBox)Generic.TableRowCheckBoxTemplate.LoadContent();
            checkBox.DataContext = rowModel;
            GridHelper.SetRowColumn(checkBox, row, 0);
            return checkBox;
        }
        #endregion
        #endregion [Private]

        #region 刷新
        public void Refresh()
        {
            InitHeaders();
            InitItems();
            UpdateFloatGridSizeFromCache();
        }
        #endregion
        #endregion 【Functions】
    }

    #region 测试表格数据
    public class TestTableDate
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
    }
    #endregion

    #region 设计数据
    public class TableGridDesignData : UserControl
    {
        #region 【Properties】
        public TableModel TableModel { get; set; } = new TableModel(typeof(TestTableDate));
        #endregion 【Properties】

        #region 【Ctor】
        public TableGridDesignData()
        {
            Init(TableModel);
        }
        #endregion 【Ctor】

        #region 【Functions】
        #region 初始化
        public static void Init(TableModel tableModel)
        {
            tableModel.RowDatas.Add(new TestTableDate() { ID = 1, Name = "张三", Age = 18 });
            tableModel.RowDatas.Add(new TestTableDate() { ID = 2, Name = "李四", Age = 19 });
            tableModel.RowDatas.Add(new TestTableDate() { ID = 3, Name = "王五", Age = 20 });
        }
        #endregion
        #endregion 【Functions】
    }
    #endregion
}
