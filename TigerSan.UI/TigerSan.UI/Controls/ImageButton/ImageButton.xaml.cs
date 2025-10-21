using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows.Controls;

namespace TigerSan.UI.Controls
{
    public partial class ImageButton : UserControl
    {
        #region 【Fields】
        private static readonly Thickness _borderThicknessIsChecked = new Thickness(3);
        private static readonly Thickness _borderThicknessMouseEnter = new Thickness(1);
        private static readonly Thickness _borderThicknessDefault = new Thickness(0);
        private bool _isMouseOver = false;
        #endregion 【Fields】

        #region 【DependencyProperties】
        #region 高度
        public new double Height
        {
            get { return (double)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }
        public static new readonly DependencyProperty HeightProperty =
            DependencyProperty.Register(
                nameof(Height),
                typeof(double),
                typeof(ImageButton),
                new PropertyMetadata(200.0));
        #endregion

        #region 宽度
        public new double Width
        {
            get { return (double)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }
        public static new readonly DependencyProperty WidthProperty =
            DependencyProperty.Register(
                nameof(Width),
                typeof(double),
                typeof(ImageButton),
                new PropertyMetadata(200.0));
        #endregion

        #region 外边距
        public new Thickness Margin
        {
            get { return (Thickness)GetValue(MarginProperty); }
            set { SetValue(MarginProperty, value); }
        }
        public static new readonly DependencyProperty MarginProperty =
            DependencyProperty.Register(
                nameof(Margin),
                typeof(Thickness),
                typeof(ImageButton),
                new PropertyMetadata(new Thickness(0)));
        #endregion

        #region 图片
        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register(
                nameof(ImageSource),
                typeof(ImageSource),
                typeof(ImageButton),
                new PropertyMetadata(null));
        #endregion

        #region 标题
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                nameof(Title),
                typeof(string),
                typeof(ImageButton),
                new PropertyMetadata(null));
        #endregion

        #region 是否选中
        public bool? IsChecked
        {
            get { return (bool?)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register(
                nameof(IsChecked),
                typeof(bool?),
                typeof(ImageButton),
                new PropertyMetadata(false, OnIsCheckedChanged));
        private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ImageButton button = (ImageButton)d;
            bool? newValue = (bool?)e.NewValue;

            if (newValue == true)
            {
                button.RaiseCheckedEvent();
                button.RaiseCheckedCommand();
            }
            else
            {
                button.RaiseUncheckedEvent();
                button.RaiseUncheckedCommand();
            }
        }
        #endregion

        #region 复选框可见性
        public Visibility CheckBoxVisibility
        {
            get { return (Visibility)GetValue(CheckBoxVisibilityProperty); }
            set { SetValue(CheckBoxVisibilityProperty, value); }
        }
        public static readonly DependencyProperty CheckBoxVisibilityProperty =
            DependencyProperty.Register(
                nameof(CheckBoxVisibility),
                typeof(Visibility),
                typeof(ImageButton),
                new PropertyMetadata(Visibility.Collapsed));
        #endregion

        #region 边框粗细
        public new Thickness BorderThickness
        {
            get { return (Thickness)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }
        public static new readonly DependencyProperty BorderThicknessProperty =
            DependencyProperty.Register(
                nameof(BorderThickness),
                typeof(Thickness),
                typeof(ImageButton),
                new PropertyMetadata(new Thickness(0)));
        #endregion
        #endregion 【DependencyProperties】

        #region 【Ctor】
        public ImageButton()
        {
            InitializeComponent();
            DefaultStyleKey = typeof(ImageButton);
            MouseEnter += ImageButton_MouseEnter;
            MouseLeave += ImageButton_MouseLeave;
            checkBox.Checked += OnChecked;
            checkBox.Unchecked += OnUnchecked;
            MouseLeftButtonDown += OnMouseLeftButtonDown;
        }
        #endregion 【Ctor】

        #region 【CustomEvents】
        #region 选中
        public event RoutedEventHandler Checked
        {
            add { AddHandler(CheckedEvent, value); }
            remove { RemoveHandler(CheckedEvent, value); }
        }
        [Category("Behavior")]
        public static readonly RoutedEvent CheckedEvent =
        EventManager.RegisterRoutedEvent(
            nameof(Checked),
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(ImageButton));

        protected virtual void RaiseCheckedEvent()
        {
            var args = new RoutedEventArgs(CheckedEvent, this);
            RaiseEvent(args);
            BorderThickness = _borderThicknessIsChecked;
        }
        #endregion

        #region 未选中
        public event RoutedEventHandler Unchecked
        {
            add { AddHandler(UncheckedEvent, value); }
            remove { RemoveHandler(UncheckedEvent, value); }
        }
        [Category("Behavior")]
        public static readonly RoutedEvent UncheckedEvent =
        EventManager.RegisterRoutedEvent(
            nameof(Unchecked),
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(ImageButton));

        protected virtual void RaiseUncheckedEvent()
        {
            var args = new RoutedEventArgs(UncheckedEvent, this);
            RaiseEvent(args);
            BorderThickness = _borderThicknessMouseEnter;
        }
        #endregion
        #endregion 【CustomEvents】

        #region 【CustomCommands】
        #region 选中
        public ICommand CheckedCommand
        {
            get => (ICommand)GetValue(CheckedCommandProperty);
            set => SetValue(CheckedCommandProperty, value);
        }
        public static readonly DependencyProperty CheckedCommandProperty =
            DependencyProperty.Register(
                nameof(CheckedCommand),
                typeof(ICommand),
                typeof(ImageButton),
                new PropertyMetadata(null));

        protected void RaiseCheckedCommand()
        {
            CheckedCommand?.Execute(null);
        }
        #endregion

        #region 未选中
        public ICommand UncheckedCommand
        {
            get => (ICommand)GetValue(UncheckedCommandProperty);
            set => SetValue(UncheckedCommandProperty, value);
        }
        public static readonly DependencyProperty UncheckedCommandProperty =
            DependencyProperty.Register(
                nameof(UncheckedCommand),
                typeof(ICommand),
                typeof(ImageButton),
                new PropertyMetadata(null));

        protected void RaiseUncheckedCommand()
        {
            UncheckedCommand?.Execute(null);
        }
        #endregion

        #region 鼠标左键按下
        public ICommand MouseLeftButtonDownCommand
        {
            get => (ICommand)GetValue(MouseLeftButtonDownCommandProperty);
            set => SetValue(MouseLeftButtonDownCommandProperty, value);
        }
        public static readonly DependencyProperty MouseLeftButtonDownCommandProperty =
            DependencyProperty.Register(
                nameof(MouseLeftButtonDownCommand),
                typeof(ICommand),
                typeof(ImageButton),
                new PropertyMetadata(null));

        protected void RaiseMouseLeftButtonDownCommand()
        {
            MouseLeftButtonDownCommand?.Execute(null);
        }
        #endregion
        #endregion 【CustomCommands】

        #region 【Events】
        #region 鼠标左键按下
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RaiseMouseLeftButtonDownCommand();
        }
        #endregion

        #region 选中
        private void OnChecked(object sender, RoutedEventArgs e)
        {
            IsChecked = true;
            CheckBoxVisibility = Visibility.Visible;
        }
        #endregion

        #region 未选中
        private void OnUnchecked(object sender, RoutedEventArgs e)
        {
            IsChecked = false;
            if (!_isMouseOver)
            {
                CheckBoxVisibility = Visibility.Collapsed;
            }
        }
        #endregion

        #region 鼠标进入
        private void ImageButton_MouseEnter(object sender, MouseEventArgs e)
        {
            _isMouseOver = true;
            CheckBoxVisibility = Visibility.Visible;

            if (IsChecked != true)
            {
                BorderThickness = _borderThicknessMouseEnter;
            }
        }
        #endregion

        #region 鼠标离开
        private void ImageButton_MouseLeave(object sender, MouseEventArgs e)
        {
            _isMouseOver = false;

            if (IsChecked == null || IsChecked.Value == false)
            {
                CheckBoxVisibility = Visibility.Collapsed;
            }

            if (IsChecked != true)
            {
                BorderThickness = _borderThicknessDefault;
            }
        }
        #endregion
        #endregion 【Events】
    }
}
