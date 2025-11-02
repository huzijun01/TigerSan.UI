using System.IO;
using System.Windows;
using System.Windows.Shell;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TigerSan.CsvLog;
using TigerSan.ImageOperation;

namespace TigerSan.UI
{
    public static class Generic
    {
        #region 【Fields】
        /// <summary>
        /// 元素实例
        /// （用于读取资源字典）
        /// </summary>
        private static FrameworkElement _element = new FrameworkElement();

        /// <summary>
        /// 应用启动路径
        /// </summary>
        public static readonly string? _appStartupPath = Path.GetDirectoryName(Environment.ProcessPath);

        /// <summary>
        /// 默认标题
        /// </summary>
        public static readonly string _defaultTitle = Path.GetFileNameWithoutExtension(Environment.ProcessPath) ?? "TigerSan.UI";
        #endregion 【Fields】

        #region 【Images】
        public static ImageSource logo_32 { get => GetBitmapSource(Resources.logo_32); }
        public static ImageSource Hello { get => GetBitmapSource(Resources.hello); }
        public static ImageSource Bye { get => GetBitmapSource(Resources.bye); }
        #endregion 【Images】

        #region 【Styles】
        public static Style TableCheckBoxStyle { get => (Style)_element.FindResource(nameof(TableCheckBoxStyle)); }
        public static Style RowBackgroundBorderStyle { get => (Style)_element.FindResource(nameof(RowBackgroundBorderStyle)); }
        public static Style HorizontalDividingLineStyle { get => (Style)_element.FindResource(nameof(HorizontalDividingLineStyle)); }
        public static Style TransparentUserControlStyle { get => (Style)_element.FindResource(nameof(TransparentUserControlStyle)); }
        public static Style FilePickerStyle { get => (Style)_element.FindResource(nameof(FilePickerStyle)); }
        #endregion 【Styles】

        #region 【Colors】
        // [主色]:
        public static SolidColorBrush Brand { get => GetSolidColorBrush("#409EFF"); }

        // [辅助色]:
        public static SolidColorBrush Success { get => GetSolidColorBrush("#67C23A"); }
        public static SolidColorBrush Warning { get => GetSolidColorBrush("#E6A23C"); }
        public static SolidColorBrush Danger { get => GetSolidColorBrush("#F56C6C"); }
        public static SolidColorBrush Info { get => GetSolidColorBrush("#909399"); }

        // [中性色]:
        // 基础:
        public static SolidColorBrush BasicBlack { get => GetSolidColorBrush("#000000"); }
        public static SolidColorBrush BasicWhite { get => GetSolidColorBrush("#FFFFFF"); }
        public static SolidColorBrush Transparent { get => new SolidColorBrush(Colors.Transparent); }
        // 文本:
        public static SolidColorBrush PrimaryText { get => GetSolidColorBrush("#E5EAF3"); }
        public static SolidColorBrush RegularText { get => GetSolidColorBrush("#CFD3DC"); }
        public static SolidColorBrush SecondaryText { get => GetSolidColorBrush("#A3A6AD"); }
        public static SolidColorBrush PlaceholderText { get => GetSolidColorBrush("#8D9095"); }
        public static SolidColorBrush DisabledText { get => GetSolidColorBrush("#6C6E72"); }
        // 边框:
        public static SolidColorBrush BaseBorder { get => GetSolidColorBrush("#4C4D4F"); }
        public static SolidColorBrush DarkBorder { get => GetSolidColorBrush("#58585B"); }
        public static SolidColorBrush DarkerBorder { get => GetSolidColorBrush("#636466"); }
        public static SolidColorBrush LightBorder { get => GetSolidColorBrush("#414243"); }
        public static SolidColorBrush LighterBorder { get => GetSolidColorBrush("#363637"); }
        public static SolidColorBrush ExtraLightBorder { get => GetSolidColorBrush("#2B2B2C"); }
        // 填充:
        public static SolidColorBrush BaseFill { get => GetSolidColorBrush("#303030"); }
        public static SolidColorBrush DarkFill { get => GetSolidColorBrush("#39393A"); }
        public static SolidColorBrush BlankFill { get => new SolidColorBrush(Colors.Transparent); }
        public static SolidColorBrush DarkerFill { get => GetSolidColorBrush("#424243"); }
        public static SolidColorBrush LightFill { get => GetSolidColorBrush("#262727"); }
        public static SolidColorBrush LighterFill { get => GetSolidColorBrush("#1D1D1D"); }
        public static SolidColorBrush ExtraLightFill { get => GetSolidColorBrush("#191919"); }
        // 背景:
        public static SolidColorBrush PageBackground { get => GetSolidColorBrush("#0A0A0A"); }
        public static SolidColorBrush BaseBackground { get => GetSolidColorBrush("#141414"); }
        public static SolidColorBrush OverlayBackground { get => GetSolidColorBrush("#1D1E1F"); }
        // 遮罩:
        public static SolidColorBrush MaskHover { get => GetSolidColorBrush("#22000000"); }
        public static SolidColorBrush MaskPressed { get => GetSolidColorBrush("#22FFFFFF"); }
        // 标题栏:
        public static SolidColorBrush ToolBar_Static_Background { get => GetSolidColorBrush("#2b2b2b"); }
        public static SolidColorBrush ToolBar_Active_Background { get => GetSolidColorBrush("#0063b1"); }
        // 像素点:
        public static SolidColorBrush PixelDot_Selected_Background { get => GetSolidColorBrush("#9F3"); }
        public static SolidColorBrush PixelDot_Selected_BorderBrush { get => GetSolidColorBrush("#9F3"); }
        public static SolidColorBrush PixelDot_NotSelected_Background { get => GetSolidColorBrush("#555"); }
        public static SolidColorBrush PixelDot_NotSelected_BorderBrush { get => GetSolidColorBrush("#777"); }
        // 菜单项目:
        public static SolidColorBrush MenuItem_Hover { get => GetSolidColorBrush("#404040"); }
        #endregion

        #region 【Constants】
        public static double ColumnWidthHandleWidth = 12;
        public static double ColumnWidthHandlePressedWidth = 25;
        public static Thickness ButtonPadding { get => new Thickness(15, 8, 15, 8); }
        public static Duration Duration { get => (Duration)_element.FindResource($"Global.{nameof(Duration)}"); }
        public static double DurationTotalSeconds { get => Duration.TimeSpan.TotalSeconds; }
        public static GridLength DefaultGridWidth { get => (GridLength)_element.FindResource(nameof(DefaultGridWidth)); }
        public static GridLength DefaultGridHeight { get => (GridLength)_element.FindResource(nameof(DefaultGridHeight)); }
        #endregion 【Constants】

        #region 【Window】
        public static Thickness ZeroThickness { get => (Thickness)_element.FindResource(nameof(ZeroThickness)); }
        public static Thickness WindowPadding { get => (Thickness)_element.FindResource(nameof(WindowPadding)); }
        public static WindowChrome CustomWindowChrome { get => (WindowChrome)_element.FindResource(nameof(CustomWindowChrome)); }
        public static WindowChrome NoResizeWindowChrome { get => (WindowChrome)_element.FindResource(nameof(NoResizeWindowChrome)); }
        public static Style DialogWindowStyle { get => (Style)_element.FindResource(nameof(DialogWindowStyle)); }
        public static Style PopWindowStyle { get => (Style)_element.FindResource(nameof(PopWindowStyle)); }
        public static Style NoToolBarPopWindowStyle { get => (Style)_element.FindResource(nameof(NoToolBarPopWindowStyle)); }
        #endregion 【Window】

        #region 【Functions】
        #region 获取“图片”
        public static BitmapSource GetBitmapSource(System.Drawing.Bitmap bitmap)
        {
            return ImageHelper.Bitmap2BitmapImage(bitmap) ?? new BitmapImage();
        }
        #endregion

        #region 获取“笔刷”
        public static SolidColorBrush GetSolidColorBrush(string hexColor)
        {
            // 验证输入格式
            if (string.IsNullOrEmpty(hexColor) || hexColor[0] != '#')
            {
                LogHelper.Instance.Warning("Invalid hex color format. Expected format: #RRGGBB, #RGB, or #AARRGGBB");
                return new SolidColorBrush();
            }

            // 统一处理为小写（方便后续处理）
            hexColor = hexColor.ToLower();

            byte r = 0, g = 0, b = 0, a = 255; // 默认透明度为255（不透明）

            try
            {
                switch (hexColor.Length)
                {
                    case 7: // #RRGGBB
                        r = Convert.ToByte(hexColor.Substring(1, 2), 16);
                        g = Convert.ToByte(hexColor.Substring(3, 2), 16);
                        b = Convert.ToByte(hexColor.Substring(5, 2), 16);
                        break;

                    case 4: // #RGB
                        r = ExpandShortHex(hexColor[1]);
                        g = ExpandShortHex(hexColor[2]);
                        b = ExpandShortHex(hexColor[3]);
                        break;

                    case 9: // #RRGGBBAA
                        a = Convert.ToByte(hexColor.Substring(1, 2), 16);
                        r = Convert.ToByte(hexColor.Substring(3, 2), 16);
                        g = Convert.ToByte(hexColor.Substring(5, 2), 16);
                        b = Convert.ToByte(hexColor.Substring(7, 2), 16);
                        break;

                    default:
                        LogHelper.Instance.Warning($"Unsupported hex color length: {hexColor.Length}");
                        return new SolidColorBrush();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Error($"Color conversion error: {ex.Message}");
                return new SolidColorBrush();
            }

            // 创建带透明度的颜色（如果需要）
            if (a != 255)
            {
                return new SolidColorBrush(Color.FromArgb(a, r, g, b));
            }

            return new SolidColorBrush(Color.FromRgb(r, g, b));
        }
        #endregion

        #region 将“单字符”扩展为“两位十六进制”
        /// <summary>
        /// 将“单字符”扩展为“两位十六进制”
        /// </summary>
        private static byte ExpandShortHex(char c)
        {
            var hex = $"{c}{c}";
            return Convert.ToByte(hex, 16);
        }
        #endregion
        #endregion 【Functions】
    }
}
