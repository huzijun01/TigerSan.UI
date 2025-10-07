using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media;

namespace TigerSan.UI.Controls
{
    public partial class PixelDot : UserControl
    {
        #region 【DependencyProperties】
        #region 是否为点
        /// <summary>
        /// 是否为点
        /// </summary>
        public bool IsDot
        {
            get { return (bool)GetValue(IsDotProperty); }
            set { SetValue(IsDotProperty, value); }
        }
        public static readonly DependencyProperty IsDotProperty =
            DependencyProperty.Register(nameof(IsDot),
                typeof(bool),
                typeof(PixelDot),
                new PropertyMetadata(true));
        #endregion

        #region 是否选中
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register(
                nameof(IsSelected),
                typeof(bool),
                typeof(PixelDot),
                new PropertyMetadata(false, IsSelectedChanged));

        private static void IsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = (PixelDot)d;
            sender.UpdateColor();
        }
        #endregion

        #region 选中后的背景色
        /// <summary>
        /// 选中后的背景色
        /// </summary>
        public Brush SelectedBackground
        {
            get { return (Brush)GetValue(SelectedBackgroundProperty); }
            set { SetValue(SelectedBackgroundProperty, value); }
        }
        public static readonly DependencyProperty SelectedBackgroundProperty =
            DependencyProperty.Register(
                nameof(SelectedBackground),
                typeof(Brush),
                typeof(PixelDot),
                new PropertyMetadata(new SolidColorBrush()));
        #endregion

        #region 选中后的边框色
        /// <summary>
        /// 选中后的边框色
        /// </summary>
        public Brush SelectedBorderBrush
        {
            get { return (Brush)GetValue(SelectedBorderBrushProperty); }
            set { SetValue(SelectedBorderBrushProperty, value); }
        }
        public static readonly DependencyProperty SelectedBorderBrushProperty =
            DependencyProperty.Register(
                nameof(SelectedBorderBrush),
                typeof(Brush),
                typeof(PixelDot),
                new PropertyMetadata(new SolidColorBrush()));
        #endregion

        #region 圆角
        /// <summary>
        /// 圆角
        /// </summary>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(PixelDot),
                new PropertyMetadata(new CornerRadius(3)));
        #endregion
        #endregion 【DependencyProperties】

        #region 【CustomEvents】
        #region 选中
        [Category("Behavior")]
        public static readonly RoutedEvent CheckedEvent =
        EventManager.RegisterRoutedEvent(
            "Checked",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(PixelDot));

        public event RoutedEventHandler Checked
        {
            add { AddHandler(CheckedEvent, value); }
            remove { RemoveHandler(CheckedEvent, value); }
        }

        protected virtual void RaiseCheckedEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(CheckedEvent, this);
            RaiseEvent(newEventArgs);
        }
        #endregion

        #region 未选中
        [Category("Behavior")]
        public static readonly RoutedEvent UncheckedEvent =
        EventManager.RegisterRoutedEvent(
            "Unchecked",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(PixelDot));

        public event RoutedEventHandler Unchecked
        {
            add { AddHandler(UncheckedEvent, value); }
            remove { RemoveHandler(UncheckedEvent, value); }
        }

        protected virtual void RaiseUncheckedEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(UncheckedEvent, this);
            RaiseEvent(newEventArgs);
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
                typeof(PixelDot),
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
                typeof(PixelDot),
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
                typeof(PixelDot),
                new PropertyMetadata(null));

        protected void RaiseMouseLeftButtonDownCommand()
        {
            MouseLeftButtonDownCommand?.Execute(null);
        }
        #endregion
        #endregion 【CustomCommands】

        #region 【Ctor】
        public PixelDot()
        {
            InitializeComponent();
            UpdateColor();
            MouseLeftButtonDown += PixelDot_MouseLeftButtonDown;
        }
        #endregion 【Ctor】

        #region 【Events】
        #region 左键按下
        private void PixelDot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsSelected = !IsSelected;

            if (IsSelected)
            {
                RaiseCheckedEvent();
                RaiseCheckedCommand();
            }
            else
            {
                RaiseUncheckedEvent();
                RaiseUncheckedCommand();
            }

            RaiseMouseLeftButtonDownCommand();
        }
        #endregion
        #endregion 【Events】

        #region 更新颜色
        private void UpdateColor()
        {
            if (IsSelected)
            {
                Background = SelectedBackground;
                BorderBrush = SelectedBorderBrush;
            }
            else
            {
                Background = Generic.PixelDot_NotSelected_Background;
                BorderBrush = Generic.PixelDot_NotSelected_BorderBrush;
            }
        }
        #endregion
    }

    #region 设计数据
    public class PixelDotDesignData : UserControl
    {
        #region 圆角
        /// <summary>
        /// 圆角
        /// </summary>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(PixelDot),
                new PropertyMetadata(new CornerRadius(3)));
        #endregion

        public PixelDotDesignData()
        {
            Background = Generic.PixelDot_Selected_Background;
            BorderBrush = Generic.PixelDot_Selected_BorderBrush;
        }
    }
    #endregion
}
