using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using TigerSan.UI.Animations;

namespace TigerSan.UI.Controls
{
    public partial class Loading : UserControl
    {
        #region 【Fields】
        private Storyboard? _storyboard;
        public double _secDuration = 1.2;
        #endregion 【Fields】

        #region 【DependencyProperties】
        #region Angle01
        public double Angle01
        {
            get { return (double)GetValue(Angle01Property); }
            set { SetValue(Angle01Property, value); }
        }
        public static readonly DependencyProperty Angle01Property =
            DependencyProperty.Register(
                nameof(Angle01),
                typeof(double),
                typeof(Loading),
                new PropertyMetadata(0.0));
        #endregion

        #region Angle02
        public double Angle02
        {
            get { return (double)GetValue(Angle02Property); }
            set { SetValue(Angle02Property, value); }
        }
        public static readonly DependencyProperty Angle02Property =
            DependencyProperty.Register(
                nameof(Angle02),
                typeof(double),
                typeof(Loading),
                new PropertyMetadata(0.0));
        #endregion

        #region Angle03
        public double Angle03
        {
            get { return (double)GetValue(Angle03Property); }
            set { SetValue(Angle03Property, value); }
        }
        public static readonly DependencyProperty Angle03Property =
            DependencyProperty.Register(
                nameof(Angle03),
                typeof(double),
                typeof(Loading),
                new PropertyMetadata(0.0));
        #endregion

        #region Angle04
        public double Angle04
        {
            get { return (double)GetValue(Angle04Property); }
            set { SetValue(Angle04Property, value); }
        }
        public static readonly DependencyProperty Angle04Property =
            DependencyProperty.Register(
                nameof(Angle04),
                typeof(double),
                typeof(Loading),
                new PropertyMetadata(0.0));
        #endregion

        #region Angle05
        public double Angle05
        {
            get { return (double)GetValue(Angle05Property); }
            set { SetValue(Angle05Property, value); }
        }
        public static readonly DependencyProperty Angle05Property =
            DependencyProperty.Register(
                nameof(Angle05),
                typeof(double),
                typeof(Loading),
                new PropertyMetadata(0.0));
        #endregion
        #endregion 【DependencyProperties】

        #region 【Ctor】
        public Loading()
        {
            InitializeComponent();
            Loaded += Loading_Loaded;
            IsVisibleChanged += Loading_IsVisibleChanged;
        }
        #endregion 【Ctor】

        #region 【Events】
        #region 加载完成
        private void Loading_Loaded(object sender, RoutedEventArgs e)
        {
            InitAnimation();
        }
        #endregion

        #region 可见性改变
        private void Loading_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var loading = (Loading)sender;
            if ((bool)e.NewValue)
            {
                _storyboard?.Begin();
            }
            else
            {
                _storyboard?.Stop();
                Angle01 = Angle02 = Angle03 = Angle04 = Angle05 = 0;
            }
        }
        #endregion
        #endregion 【Events】

        #region 【Functions】
        #region 初始化动画
        private void InitAnimation()
        {
            if (_storyboard != null) return;

            _storyboard = new Storyboard()
            {
                AutoReverse = false,
                RepeatBehavior = RepeatBehavior.Forever
            };

            _storyboard.Children.Add(DoubleAnimations.Rotate(this, Angle01Property, 0.0, _secDuration));
            _storyboard.Children.Add(DoubleAnimations.Rotate(this, Angle02Property, 0.1, _secDuration));
            _storyboard.Children.Add(DoubleAnimations.Rotate(this, Angle03Property, 0.2, _secDuration));
            _storyboard.Children.Add(DoubleAnimations.Rotate(this, Angle04Property, 0.3, _secDuration));
            _storyboard.Children.Add(DoubleAnimations.Rotate(this, Angle05Property, 0.4, _secDuration));
        }
        #endregion
        #endregion 【Functions】
    }
}
