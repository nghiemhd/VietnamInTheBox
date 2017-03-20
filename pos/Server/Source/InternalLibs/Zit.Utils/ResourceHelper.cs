using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace Zit.Utils
{
    /// <summary>
    /// Provide static function to get the resource by resource key
    /// </summary>
    public static class ResourceHelper
    {        
        /// <summary>
        /// Get a resource string
        /// </summary>
        /// <param name="key"></param>
        /// <param name="resourceType"></param>
        /// <returns>Empty if there is no resource</returns>
        public static string Get(string key, Type resourceType)
        {
            string val = null;
            ResourceManager resManager = new ResourceManager(resourceType);
            val = resManager.GetString(key);
            return val??key;
        }


        /// <summary>
        /// Translate Property of Entity In List From Enum With Display Resource
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="list"></param>
        /// <param name="expression"></param>
        /// <param name="enumType"></param>
        public static void TranslateFromEnum<T, TProperty>(this List<T> list, Expression<Func<T, TProperty>> expression, Type enumType)
        {
            string propertyName = expression.GetPropertyName();
            Type type = typeof(T);
            PropertyInfo pInfo = type.GetProperty(propertyName);
            Dictionary<string, string> dicValues = EnumToDictionary(enumType);

            foreach (var obj in list)
            {
                object keyobj = pInfo.GetValue(obj, null);
                string key = keyobj == null ? null : keyobj.ToString();
                if (key != null && dicValues.ContainsKey(key))
                    pInfo.SetValue(obj, dicValues[key], null);
            }
        }
        /// <summary>
        /// Translate Property of Entity In List From Enum With Display Resource
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="list"></param>
        /// <param name="expression"></param>
        /// <param name="enumType"></param>
        public static void TranslateFromEnum<T, TProperty>(this List<T> list, Expression<Func<T, TProperty>> fromExpression, Expression<Func<T, TProperty>> toExpression, Type enumType)
        {
            string fromPropertyName = fromExpression.GetPropertyName();
            string toPropertyName = toExpression.GetPropertyName();
            Type type = typeof(T);
            PropertyInfo pFromInfo = type.GetProperty(fromPropertyName);
            PropertyInfo pToInfo = type.GetProperty(toPropertyName);
            Dictionary<string, string> dicValues = EnumToDictionary(enumType);

            foreach (var obj in list)
            {
                object keyobj = pFromInfo.GetValue(obj, null);
                string key = keyobj == null ? null : keyobj.ToString();
                if (key != null && dicValues.ContainsKey(key))
                    pToInfo.SetValue(obj, dicValues[key], null);
            }
        }

        public static string DisplayForEnumValue(Object value)
        {
            Type enumType = value.GetType();
            var fieldInfo = enumType.GetField(value.ToString());
            if (fieldInfo != null)
            {
                DisplayAttribute displayAtt = fieldInfo.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;
                return displayAtt.GetName();
            }
            else
            {
                return null;//string.Format("Can't Get Display Text From Enum {0} With Value = {1}",enumType.Name,value);
            }
        }

        public static string DisplayForEnumValue_EmptyString(Object value)
        {
            if (value == null) return string.Empty;

            string ret = DisplayForEnumValue(value);
            
            if (ret == null) return string.Empty;

            return ret;
        }

        /// <summary>
        /// Convert Enum To Dictionary (Key = Enum Field, Value = Resource Display)
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static Dictionary<string,string> EnumToDictionary(Type enumType)
        {
            Dictionary<string, string> listEnumField = new Dictionary<string, string>();
            Type type = enumType;
            foreach (var evalue in type.GetEnumValues())
            {
                var valueName = type.GetField(evalue.ToString());
                string displayLabel = "";
                DisplayAttribute displayAtt = valueName.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;
                if (displayAtt != null)
                    displayLabel = displayAtt.GetName();
                listEnumField.Add(evalue.ToString(), displayLabel);
            }
            return listEnumField;
        }

        public static Dictionary<object, string> EnumToArray(Type enumType)
        {
            Dictionary<object, string> listEnumField = new Dictionary<object, string>();
            Type type = enumType;
            foreach (var evalue in type.GetEnumValues())
            {
                var valueName = type.GetField(evalue.ToString());
                string displayLabel = "";
                DisplayAttribute displayAtt = valueName.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;
                if (displayAtt != null)
                    displayLabel = displayAtt.GetName();
                listEnumField.Add(evalue, displayLabel);
            }
            return listEnumField;
        }
    }
}
