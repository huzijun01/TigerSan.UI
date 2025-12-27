using System.Windows;
using System.ComponentModel;

namespace TigerSan.UI.Helpers
{
    public static class BindingHelper
    {
        #region 添加“值改变”事件
        /// <summary>
        /// 添加“值改变”事件
        /// </summary>
        public static void AddValueChanged(
            object component,
            Type targetType,
            DependencyProperty property,
            EventHandler handler)
        {
            DependencyPropertyDescriptor.FromProperty(
                property,
                targetType)
                .AddValueChanged(component, handler);
        }
        #endregion
    }
}
