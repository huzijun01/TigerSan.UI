using TigerSan.CsvLog;
using System.Reflection;

namespace TigerSan.UI.Helpers
{
    public static class TypeHelper
    {
        #region 获取属性
        public static PropertyInfo? GetProp(Type type, int index)
        {
            // 获取属性集合：
            var props = type.GetProperties();
            if (props == null)
            {
                LogHelper.Instance.Warning($"The {nameof(props)} is null!");
                return null;
            }
            if (props.Length <= index)
            {
                LogHelper.Instance.Warning($"The {nameof(index)} is beyond the range of {nameof(props)}!");
                return null;
            }

            // 获取属性名：
            var prop = props[index];
            if (prop == null)
            {
                LogHelper.Instance.Warning($"The {nameof(prop)} is null!");
                return null;
            }

            return prop;
        }
        #endregion

        #region 获取属性名集合
        public static List<string> GetPropNames(Type type)
        {
            var list = new List<string>();

            // 获取属性集合：
            var props = type.GetProperties();
            if (props == null)
            {
                LogHelper.Instance.Warning($"The {nameof(props)} is null!");
                return list;
            }

            foreach (var prop in props)
            {
                list.Add(prop.Name);
            }

            return list;
        }
        #endregion

        #region 深拷贝集合
        public static List<T> DeepCopyList<T>(IList<T> src)
        {
            var dest = new List<T>();

            if (src == null)
            {
                LogHelper.Instance.Warning($"The {nameof(src)} is null!");
                return dest;
            }

            foreach (var item in src)
            {
                var obj = DeepCopyObject(item);
                if (obj == null) continue;

                dest.Add(obj);
            }

            return dest;
        }
        #endregion

        #region 拷贝对象
        private static T? DeepCopyObject<T>(T src)
        {
            if (src == null)
            {
                LogHelper.Instance.Warning($"The {nameof(src)} is null!");
                return default;
            }

            var type = src.GetType();
            if (type == null)
            {
                LogHelper.Instance.Warning($"The {nameof(type)} is null!");
                return default;
            }

            var obj = Activator.CreateInstance(type);
            if (obj == null)
            {
                LogHelper.Instance.Warning($"The {nameof(obj)} is null!");
                return default;
            }

            var props = type.GetProperties();
            if (props == null)
            {
                LogHelper.Instance.Warning($"The {nameof(obj)} is null!");
                return default;
            }

            foreach (var prop in props)
            {
                if (prop == null)
                {
                    LogHelper.Instance.Warning($"The {nameof(prop)} is null!");
                    continue;
                }

                prop.SetValue(obj, prop.GetValue(src));
            }

            return (T)obj;
        }
        #endregion

        #region 比较
        public static bool IsEqual(object? obj1, object? obj2)
        {
            // 是否为空：
            if (obj1 == null && obj1 == null)
            {
                return true;
            }
            else if (obj1 == null || obj2 == null)
            {
                return false;
            }

            return Equals(obj1, obj2);
        }
        #endregion
    }
}
