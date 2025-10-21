using System.Runtime.InteropServices;

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
        #endregion 【Functions】
    }
}
