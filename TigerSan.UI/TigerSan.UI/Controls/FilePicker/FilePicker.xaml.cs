using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows.Controls;
using Microsoft.Win32;
using TigerSan.CsvLog;

namespace TigerSan.UI.Controls
{
    #region 过滤器模型
    /// <summary>
    /// 过滤器模型
    /// </summary>
    public class FilterModel
    {
        #region 【Properties】
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 扩展名集合
        /// </summary>
        public string[] Extensions { get; set; }

        /// <summary>
        /// 扩展名文本
        /// </summary>
        public string ExtensionText { get => GetExtensionText(); }
        #endregion 【Properties】

        #region 【Ctor】
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="description">描述</param>
        /// <param name="extensions">扩展名集合（如"txt"）</param>
        public FilterModel(string description, string[] extensions)
        {
            Description = description;
            Extensions = extensions;
        }
        #endregion 【Ctor】

        #region 【Functions】
        #region 获取“扩展名文本”
        /// <summary>
        /// 获取“扩展名文本”
        /// </summary>
        private string GetExtensionText()
        {
            return string.Join(";", Extensions.Select(e => $"*.{e}"));
        }
        #endregion
        #endregion
    }
    #endregion

    #region 文件选择器参数
    /// <summary>
    /// 文件选择器参数
    /// </summary>
    public class FileSelectedEventArgs : RoutedEventArgs
    {
        public string[] Paths { get; }

        public FileSelectedEventArgs(RoutedEvent routedEvent, object source, string[] paths)
            : base(routedEvent, source)
        {
            Paths = paths ?? [];
        }
    }
    #endregion

    public partial class FilePicker : UserControl
    {
        #region 【Fields】
        /// <summary>
        /// 默认“文本”
        /// </summary>
        public static readonly string _defaultText = "Drop or click.";

        /// <summary>
        /// 默认“画笔粗细”
        /// </summary>
        public static readonly double _defaultStrokeThickness = 1;
        #endregion 【Fields】

        #region 【Properties】
        #region 过滤器
        /// <summary>
        /// 过滤器
        /// </summary>
        public string Filter
        {
            get { return GetFilter(); }
        }
        #endregion
        #endregion 【Properties】

        #region 【DependencyProperties】
        #region [Private]
        #region 内容
        /// <summary>
        /// 内容
        /// </summary>
        public new object Content
        {
            get { return GetValue(ContentProperty); }
            private set { SetValue(ContentProperty, value); }
        }
        public new static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(
                nameof(Content),
                typeof(object),
                typeof(FilePicker));
        #endregion

        #region 是否鼠标悬浮
        /// <summary>
        /// 是否鼠标悬浮
        /// </summary>
        public new bool IsMouseOver
        {
            get { return (bool)GetValue(IsMouseOverProperty); }
            private set { SetValue(IsMouseOverProperty, value); }
        }
        public new static readonly DependencyProperty IsMouseOverProperty =
            DependencyProperty.Register(
                nameof(IsMouseOver),
                typeof(bool),
                typeof(FilePicker),
                new PropertyMetadata(false));
        #endregion

        #region 是否按下
        /// <summary>
        /// 是否按下
        /// </summary>
        //public bool IsPressed
        //{
        //    get { return (bool)GetValue(IsPressedProperty); }
        //    private set { SetValue(IsPressedProperty, value); }
        //}
        //public static readonly DependencyProperty IsPressedProperty =
        //    DependencyProperty.Register(
        //        nameof(IsPressed),
        //        typeof(bool),
        //        typeof(FilePicker),
        //        new PropertyMetadata(false));
        #endregion

        #region 边框粗细
        /// <summary>
        /// 边框粗细
        /// </summary>
        public new Thickness BorderThickness
        {
            get { return (Thickness)GetValue(BorderThicknessProperty); }
            private set { SetValue(BorderThicknessProperty, value); }
        }
        public new static readonly DependencyProperty BorderThicknessProperty =
            DependencyProperty.Register(
                nameof(BorderThickness),
                typeof(Thickness),
                typeof(FilePicker),
                new PropertyMetadata(new Thickness(_defaultStrokeThickness)));
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
                typeof(FilePicker),
                new PropertyMetadata(Generic.Transparent));
        #endregion
        #endregion [Private]

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
                typeof(FilePicker),
                new PropertyMetadata(_defaultText));
        #endregion

        #region “过滤器模型”集合
        /// <summary>
        /// “过滤器模型”集合
        /// </summary>
        public FilterModel? FilterModel
        {
            get { return (FilterModel?)GetValue(FilterModelProperty); }
            set { SetValue(FilterModelProperty, value); }
        }
        public static readonly DependencyProperty FilterModelProperty =
            DependencyProperty.Register(
                nameof(FilterModel),
                typeof(FilterModel),
                typeof(FilePicker),
                new PropertyMetadata(null));
        #endregion

        #region 初始目录
        /// <summary>
        /// 初始目录
        /// </summary>
        public string InitialDirectory
        {
            get { return (string)GetValue(InitialDirectoryProperty); }
            set { SetValue(InitialDirectoryProperty, value); }
        }
        public static readonly DependencyProperty InitialDirectoryProperty =
            DependencyProperty.Register(
                nameof(InitialDirectory),
                typeof(string),
                typeof(FilePicker),
                new PropertyMetadata(string.Empty));
        #endregion

        #region 多选
        /// <summary>
        /// 多选
        /// </summary>
        public bool Multiselect
        {
            get { return (bool)GetValue(MultiselectProperty); }
            set { SetValue(MultiselectProperty, value); }
        }
        public static readonly DependencyProperty MultiselectProperty =
            DependencyProperty.Register(
                nameof(Multiselect),
                typeof(bool),
                typeof(FilePicker),
                new PropertyMetadata(false));
        #endregion

        #region 恢复目录
        /// <summary>
        /// 恢复目录
        /// </summary>
        public bool RestoreDirectory
        {
            get { return (bool)GetValue(RestoreDirectoryProperty); }
            set { SetValue(RestoreDirectoryProperty, value); }
        }
        public static readonly DependencyProperty RestoreDirectoryProperty =
            DependencyProperty.Register(
                nameof(RestoreDirectory),
                typeof(bool),
                typeof(FilePicker),
                new PropertyMetadata(false));
        #endregion

        #region 笔画粗细
        /// <summary>
        /// 笔画粗细
        /// </summary>
        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(
                nameof(StrokeThickness),
                typeof(double),
                typeof(FilePicker),
                new PropertyMetadata(_defaultStrokeThickness, StrokeThicknessChanged));

        private static void StrokeThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var picker = (FilePicker)d;
            picker.BorderThickness = new Thickness(picker.StrokeThickness);
        }
        #endregion
        #endregion 【DependencyProperties】

        #region 【Ctor】
        public FilePicker()
        {
            InitializeComponent();
            MouseEnter += OnMouseEnter;
            MouseLeave += OnMouseLeave;
            MouseLeftButtonDown += OnMouseLeftButtonDown;
            //MouseLeftButtonUp += OnMouseLeftButtonUp;
            Drop += FilePicker_Drop;
        }
        #endregion 【Ctor】

        #region 【CustomEvents】
        #region 选择后
        public event EventHandler<FileSelectedEventArgs> Selected
        {
            add { AddHandler(SelectedEvent, value); }
            remove { RemoveHandler(SelectedEvent, value); }
        }
        [Category("Behavior")]
        public static readonly RoutedEvent SelectedEvent =
        EventManager.RegisterRoutedEvent(
            nameof(Selected),
            RoutingStrategy.Bubble,
            typeof(EventHandler<FileSelectedEventArgs>),
            typeof(FilePicker));

        protected virtual void RaiseSelectedEvent(string[] paths)
        {
            var args = new FileSelectedEventArgs(SelectedEvent, this, paths);
            RaiseEvent(args);
        }
        #endregion
        #endregion 【CustomEvents】

        #region 【CustomCommands】
        #region 拖入
        public ICommand DropCommand
        {
            get => (ICommand)GetValue(DropCommandProperty);
            set => SetValue(DropCommandProperty, value);
        }
        public static readonly DependencyProperty DropCommandProperty =
            DependencyProperty.Register(
                nameof(DropCommand),
                typeof(ICommand),
                typeof(FilePicker),
                new PropertyMetadata(null));

        protected void RaiseDropCommand(string[] paths)
        {
            // 类型过滤：
            paths = paths.Where(p => IsAllowedExtension(p)).ToArray();
            DropCommand?.Execute(paths);
        }
        #endregion

        #region 选择后
        public ICommand SelectedCommand
        {
            get => (ICommand)GetValue(SelectedCommandProperty);
            set => SetValue(SelectedCommandProperty, value);
        }
        public static readonly DependencyProperty SelectedCommandProperty =
            DependencyProperty.Register(
                nameof(SelectedCommand),
                typeof(ICommand),
                typeof(FilePicker),
                new PropertyMetadata(null));

        protected void RaiseSelectedCommand(string[] paths)
        {
            SelectedCommand?.Execute(paths);
        }
        #endregion
        #endregion 【CustomCommands】

        #region 【Events】
        #region 鼠标进入
        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            IsMouseOver = true;
            UpdateState();
        }
        #endregion

        #region 鼠标离开
        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            IsMouseOver = false;
            UpdateState();
        }
        #endregion

        #region 鼠标左键按下
        private void OnMouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            //IsPressed = true;
            //UpdateState();
            OpenFileDialog();
        }
        #endregion

        #region 鼠标左键抬起
        private void OnMouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            //IsPressed = false;
            //UpdateState();
        }
        #endregion

        #region 拖入“文件选择器”
        public void FilePicker_Drop(object sender, DragEventArgs e)
        {
            var paths = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (paths == null)
            {
                LogHelper.Instance.IsNull(nameof(paths));
                return;
            }

            RaiseDropCommand(paths);
        }
        #endregion
        #endregion 【Events】

        #region 【Functions】
        #region 更新状态
        /// <summary>
        /// 更新状态
        /// </summary>
        private void UpdateState()
        {
            if (IsMouseOver)
            {
                Foreground = Generic.BasicWhite;
                Background = Generic.Brand;
            }
            else
            {
                Foreground = Generic.Brand;
                Background = Generic.Transparent;
            }

            //if (IsPressed)
            //{
            //    MaskBackground = Generic.MaskPressed;
            //}
            //else
            //{
            //    MaskBackground = Generic.Transparent;
            //}
        }
        #endregion

        #region 打开“文件对话框”
        public void OpenFileDialog()
        {
            var dialog = new OpenFileDialog
            {
                Filter = Filter,
                Multiselect = Multiselect,
                InitialDirectory = InitialDirectory,
                RestoreDirectory = RestoreDirectory,
            };

            if (dialog.ShowDialog() == false) return;

            // 触发事件：
            RaiseSelectedEvent(dialog.FileNames);
            RaiseSelectedCommand(dialog.FileNames);
        }
        #endregion

        #region 获取“过滤器”
        private string GetFilter()
        {
            if (FilterModel == null) return string.Empty;

            return $"{FilterModel.Description}|{FilterModel.ExtensionText}";
        }
        #endregion

        #region 是否为被允许的拓展名
        /// <summary>
        /// 是否为被允许的拓展名
        /// </summary>
        private bool IsAllowedExtension(string path)
        {
            if (FilterModel == null) return true;

            var extension = Path.GetExtension(path);
            if (extension == null)
            {
                LogHelper.Instance.IsNull(nameof(extension));
                return false;
            }

            return FilterModel.Extensions.Any(e => string.Equals($".{e}", extension.ToLower()));
        }
        #endregion
        #endregion 【Functions】
    }

    #region 设计数据
    public class FilePickerDesignData : UserControl
    {
        #region 【Properties】
        public string Text { get; set; }
        public double StrokeThickness { get; set; }
        #endregion

        #region 【Ctor】
        public FilePickerDesignData()
        {
            Text = FilePicker._defaultText;
            Padding = Generic.ButtonPadding;
            StrokeThickness = FilePicker._defaultStrokeThickness;
            BorderThickness = new Thickness(StrokeThickness);
            Foreground = Generic.Brand;
            BorderBrush = Generic.Brand;
            VerticalAlignment = VerticalAlignment.Center;
            HorizontalAlignment = HorizontalAlignment.Center;
        }
        #endregion 【Ctor】
    }
    #endregion
}
