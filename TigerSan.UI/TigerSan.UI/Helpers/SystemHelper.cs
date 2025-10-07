using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Runtime.InteropServices;
using TigerSan.CsvLog;

namespace TigerSan.UI.Helpers
{
    public static class SystemHelper
    {
        #region 【Fields】
        /// <summary>
        /// 定义WINDOWPOS结构体（需添加using System.Runtime.InteropServices;）
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct WINDOWPOS
        {
            public IntPtr hwnd;
            public IntPtr hwndInsertAfter;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public uint flags;
        }

        /// <summary>
        /// SWP_NOSIZE标志（禁止调整窗口尺寸）
        /// </summary>
        private const int SWP_NOSIZE = 0x0001;
        #endregion 【Fields】

        #region 【Functions】
        #region 获取屏幕缩放比例
        /// <summary>
        /// 获取屏幕缩放比例
        /// </summary>
        public static double GetDpiScale()
        {
            var visual = new Grid();
            var dpiInfo = VisualTreeHelper.GetDpi(visual);
            return dpiInfo.DpiScaleX;
        }
        #endregion

        #region 窗口消息处理（禁止修改尺寸）
        /// <summary>
        /// 窗口消息处理（禁止修改尺寸）
        /// </summary>
        public static IntPtr WndProc_NoResize(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_WINDOWPOSCHANGING = 0x46;
            if (msg == WM_WINDOWPOSCHANGING)
            {
                var windowPos = Marshal.PtrToStructure<WINDOWPOS>(lParam);
                // 阻止系统自动调整窗口尺寸
                windowPos.flags |= (int)SWP_NOSIZE;
                Marshal.StructureToPtr(windowPos, lParam, true);
                handled = true;
            }
            return IntPtr.Zero;
        }
        #endregion

        #region 获取控件相对于屏幕的位置
        public static Point? GetScreenPosition(Control control)
        {
            var strError = "The screen position of the control cannot be obtained!";

            if (control == null)
            {
                LogHelper.Instance.IsNull(nameof(control));
                return null;
            }

            if (!control.IsLoaded)
            {
                return null;
            }

            // 获取屏幕坐标：
            try
            {
                var window = Window.GetWindow(control);
                if (window != null)
                {
                    // 正确获取窗口在屏幕中的位置：
                    var windowLeft = window.Left;
                    var windowTop = window.Top;

                    // 获取控件在窗口中的位置：
                    var elementInWindow = control.TransformToVisual(window)
                                         .Transform(new Point(0, 0));

                    // 计算屏幕绝对坐标：
                    return new Point(
                        windowLeft + elementInWindow.X,
                        windowTop + elementInWindow.Y
                    );
                }
                LogHelper.Instance.Warning(strError);
                return null;
            }
            catch
            {
                LogHelper.Instance.Warning(strError);
                return null;
            }
        }
        #endregion
        #endregion 【Functions】
    }
}
