using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Console.Business
{
    public static class fsHelper
    {
        /// <summary>
        /// 利用反射来判断对象是否包含某个属性
        /// </summary>
        /// <param name="instance">object</param>
        /// <param name="propertyName">需要判断的属性</param>
        /// <returns>是否包含</returns>
        public static bool ContainProperty(this object instance, string propertyName)
        {
            if (instance != null && !string.IsNullOrEmpty(propertyName))
            {
                PropertyInfo _findedPropertyInfo = instance.GetType().GetProperty(propertyName);
                return (_findedPropertyInfo != null);
            }
            return false;
        }

        /// <summary>
        /// 获取对象属性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="propertyname"></param>
        /// <returns></returns>
        public static string GetObjectPropertyValue<T>(T t, string propertyname)
        {
            Type type = typeof(T);
            PropertyInfo property = type.GetProperty(propertyname);
            if (property == null)
            {
                return string.Empty;
            }
            object o = property.GetValue(t, null);
            if (o == null)
            {
                return string.Empty;
            }
            return o.ToString();
        }

        public static void SetObjectPropertyValue<T>(T t, string propertyname, string value)
        {
            PropertyInfo property = t.GetType().GetProperty(propertyname);
            if (property != null)
            {
              
                if (property.PropertyType.Name == typeof(int).Name)
                {
                    int newValue = 0;
                    int.TryParse(value, out newValue);
                    property.SetValue(t, newValue, null);
                }


                var newValue1 = Convert.ChangeType(value, property.PropertyType);
                property.SetValue(t, newValue1, null);


            }
        }

      

        /// <summary>
        /// 获取属性列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void GetObjectProperty<T>()
        {
            Type t = typeof(T);
            System.Reflection.PropertyInfo[] properties = t.GetProperties();
            foreach (System.Reflection.PropertyInfo info in properties)
            {
                System.Console.Write("name=" + info.Name + ";" + "type=" + info.PropertyType.Name + ";value=" + GetObjectPropertyValue<Object>(new object(), info.Name) + "<br />");
            }
        }
    }
}
