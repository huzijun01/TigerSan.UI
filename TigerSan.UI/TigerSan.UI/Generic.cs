using System.Windows;
using System.Windows.Media;
using System.Windows.Shell;

namespace TigerSan.UI
{
    public static class Generic
    {
        #region 【Fields】
        private static FrameworkElement _element = new FrameworkElement();
        #endregion 【Fields】

        #region 【Images】
        public static ImageSource Bye { get => (ImageSource)_element.FindResource(nameof(Bye)); }
        public static ImageSource logo_32 { get => (ImageSource)_element.FindResource(nameof(logo_32)); }
        #endregion 【Images】

        #region 【Styles】
        public static Style TableCheckBoxStyle { get => (Style)_element.FindResource(nameof(TableCheckBoxStyle)); }
        public static Style RowBackgroundBorderStyle { get => (Style)_element.FindResource(nameof(RowBackgroundBorderStyle)); }
        public static Style HorizontalDividingLineStyle { get => (Style)_element.FindResource(nameof(HorizontalDividingLineStyle)); }
        public static Style TransparentUserControlStyle { get => (Style)_element.FindResource(nameof(TransparentUserControlStyle)); }
        #endregion 【Styles】

        #region 【Colors】
        // [主色]:
        public static SolidColorBrush Brand { get => (SolidColorBrush)_element.FindResource($"Colors.{nameof(Brand)}"); }

        // [辅助色]:
        public static SolidColorBrush Success { get => (SolidColorBrush)_element.FindResource($"Colors.{nameof(Success)}"); }
        public static SolidColorBrush Warning { get => (SolidColorBrush)_element.FindResource($"Colors.{nameof(Warning)}"); }
        public static SolidColorBrush Danger { get => (SolidColorBrush)_element.FindResource($"Colors.{nameof(Danger)}"); }
        public static SolidColorBrush Info { get => (SolidColorBrush)_element.FindResource($"Colors.{nameof(Info)}"); }

        // [中性色]:
        // 基础:
        public static SolidColorBrush BasicBlack { get => (SolidColorBrush)_element.FindResource($"Colors.{nameof(BasicBlack)}"); }
        public static SolidColorBrush BasicWhite { get => (SolidColorBrush)_element.FindResource($"Colors.{nameof(BasicWhite)}"); }
        public static SolidColorBrush Transparent { get => (SolidColorBrush)_element.FindResource($"Colors.{nameof(Transparent)}"); }
        // 文本:
        public static SolidColorBrush PrimaryText { get => (SolidColorBrush)_element.FindResource($"Colors.{nameof(PrimaryText)}"); }
        public static SolidColorBrush RegularText { get => (SolidColorBrush)_element.FindResource($"Colors.{nameof(RegularText)}"); }
        public static SolidColorBrush SecondaryText { get => (SolidColorBrush)_element.FindResource($"Colors.{nameof(SecondaryText)}"); }
        public static SolidColorBrush PlaceholderText { get => (SolidColorBrush)_element.FindResource($"Colors.{nameof(PlaceholderText)}"); }
        public static SolidColorBrush DisabledText { get => (SolidColorBrush)_element.FindResource($"Colors.{nameof(DisabledText)}"); }
        // 边框:
        public static SolidColorBrush BaseBorder { get => (SolidColorBrush)_element.FindResource($"Colors.{nameof(BaseBorder)}"); }
        public static SolidColorBrush DarkBorder { get => (SolidColorBrush)_element.FindResource($"Colors.{nameof(DarkBorder)}"); }
        public static SolidColorBrush DarkerBorder { get => (SolidColorBrush)_element.FindResource($"Colors.{nameof(DarkerBorder)}"); }
        public static SolidColorBrush LightBorder { get => (SolidColorBrush)_element.FindResource($"Colors.{nameof(LightBorder)}"); }
        public static SolidColorBrush LighterBorder { get => (SolidColorBrush)_element.FindResource($"Colors.{nameof(LighterBorder)}"); }
        public static SolidColorBrush ExtraLightBorder { get => (SolidColorBrush)_element.FindResource($"Colors.{nameof(ExtraLightBorder)}"); }
        // 填充:
        public static SolidColorBrush BaseFill { get => (SolidColorBrush)_element.FindResource($"Colors.{nameof(BaseFill)}"); }
        public static SolidColorBrush DarkFill { get => (SolidColorBrush)_element.FindResource($"Colors.{nameof(DarkFill)}"); }
        public static SolidColorBrush BlankFill { get => (SolidColorBrush)_element.FindResource($"Colors.{nameof(BlankFill)}"); }
        public static SolidColorBrush DarkerFill { get => (SolidColorBrush)_element.FindResource($"Colors.{nameof(DarkerFill)}"); }
        public static SolidColorBrush LightFill { get => (SolidColorBrush)_element.FindResource($"Colors.{nameof(LightFill)}"); }
        public static SolidColorBrush LighterFill { get => (SolidColorBrush)_element.FindResource($"Colors.{nameof(LighterFill)}"); }
        public static SolidColorBrush ExtraLightFill { get => (SolidColorBrush)_element.FindResource($"Colors.{nameof(ExtraLightFill)}"); }
        // 背景:
        public static SolidColorBrush PageBackground { get => (SolidColorBrush)_element.FindResource($"Colors.{nameof(PageBackground)}"); }
        public static SolidColorBrush BaseBackground { get => (SolidColorBrush)_element.FindResource($"Colors.{nameof(BaseBackground)}"); }
        public static SolidColorBrush OverlayBackground { get => (SolidColorBrush)_element.FindResource($"Colors.{nameof(OverlayBackground)}"); }
        // 遮罩:
        public static SolidColorBrush Hover { get => (SolidColorBrush)_element.FindResource($"Colors.Mask.{nameof(Hover)}"); }
        public static SolidColorBrush Pressed { get => (SolidColorBrush)_element.FindResource($"Colors.Mask.{nameof(Pressed)}"); }
        // 标题栏:
        public static SolidColorBrush ToolBar_Static_Background { get => (SolidColorBrush)_element.FindResource(nameof(ToolBar_Static_Background)); }
        public static SolidColorBrush ToolBar_Active_Background { get => (SolidColorBrush)_element.FindResource(nameof(ToolBar_Active_Background)); }
        // 像素点:
        public static SolidColorBrush PixelDot_Selected_Background { get => (SolidColorBrush)_element.FindResource(nameof(PixelDot_Selected_Background)); }
        public static SolidColorBrush PixelDot_Selected_BorderBrush { get => (SolidColorBrush)_element.FindResource(nameof(PixelDot_Selected_BorderBrush)); }
        public static SolidColorBrush PixelDot_NotSelected_Background { get => (SolidColorBrush)_element.FindResource(nameof(PixelDot_NotSelected_Background)); }
        public static SolidColorBrush PixelDot_NotSelected_BorderBrush { get => (SolidColorBrush)_element.FindResource(nameof(PixelDot_NotSelected_BorderBrush)); }
        #endregion

        #region 【Constants】
        public static Duration Duration { get => (Duration)_element.FindResource($"Global.{nameof(Duration)}"); }
        public static double DurationTotalSeconds { get => Duration.TimeSpan.TotalSeconds; }
        public static GridLength DefaultGridWidth { get => (GridLength)_element.FindResource(nameof(DefaultGridWidth)); }
        public static GridLength DefaultGridHeight { get => (GridLength)_element.FindResource(nameof(DefaultGridHeight)); }
        #endregion 【Images】

        #region 【Window】
        public static Thickness ZeroThickness { get => (Thickness)_element.FindResource(nameof(ZeroThickness)); }
        public static Thickness WindowPadding { get => (Thickness)_element.FindResource(nameof(WindowPadding)); }
        public static WindowChrome CustomWindowChrome { get => (WindowChrome)_element.FindResource(nameof(CustomWindowChrome)); }
        public static WindowChrome NoResizeWindowChrome { get => (WindowChrome)_element.FindResource(nameof(NoResizeWindowChrome)); }
        public static Style DialogWindowStyle { get => (Style)_element.FindResource(nameof(DialogWindowStyle)); }
        public static Style PopWindowStyle { get => (Style)_element.FindResource(nameof(PopWindowStyle)); }
        public static Style NoToolBarPopWindowStyle { get => (Style)_element.FindResource(nameof(NoToolBarPopWindowStyle)); }
        #endregion 【Window】
    }
}
