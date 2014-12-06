using System;
using System.Web;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;

namespace Loveii
{
    public class ClientDataCollection
    {
        private readonly bool _post;

        public ClientDataCollection(bool post)
        {
            _post = post;
        }

        private NameValueCollection Data
        {
            get
            {
                if (_post)
                    return HttpContext.Current.Request.Form;
                else
                    return HttpContext.Current.Request.QueryString;
            }
        }

        public string[] Variables
        {
            get { return Data.AllKeys; }
        }

        public bool Has(string name)
        {
            return Data[name] != null;
        }

        public string Get(string name)
        {
            return Data[name];
        }

        public string Get(string name, bool isEncode)
        {
            return isEncode ? HttpUtility.HtmlEncode(Get(name, "")) : Get(name, "");
        }

        public string[] GetValues(string name)
        {
            return Data.GetValues(name);
        }

        public string Get(string name, string defaultValue)
        {
            return Data[name] ?? defaultValue;
        }

        public string Get(string name, string defaultValue, bool isEncode)
        {
            return isEncode ? HttpUtility.HtmlEncode(Data[name] ?? defaultValue) : Data[name] ?? defaultValue;
        }

        public string this[string name]
        {
            get { return Data[name]; }
        }

        public T Get<T>(string name)
        {
            return Get(name, default(T));
        }

        public T Get<T>(string name, T defaultValue)
        {
            if (!Has(name))
                return defaultValue;

            object obj = Get(name, typeof(T));
            if (obj == null)
                return defaultValue;
            return (T)obj;
        }

        public object Get(string name, Type t)
        {
            if (typeof(Array).IsAssignableFrom(t))
            {
                string[] values = Data.GetValues(name);

                Type elementType = t.GetElementType();

                if (values == null || values.Length == 0)
                    return Array.CreateInstance(elementType, 0);

                Array array = Array.CreateInstance(elementType, values.Length);

                for (int i = 0; i < values.Length; i++)
                    array.SetValue(ConvertString(values[i], elementType), i);

                return array;
            }

            return ConvertString(Get(name), t);
        }

        private static object ConvertString(string stringValue, Type targetType)
        {
            if (stringValue == null)
                return null;

            if (targetType == typeof(string))
                return stringValue;

            object value = stringValue;

            if (IsNullable(targetType))
            {
                if (stringValue.Trim().Length == 0)
                    return null;

                targetType = GetRealType(targetType);
            }

            if (targetType != typeof(string))
            {
                if (targetType == typeof(double) || targetType == typeof(float))
                {
                    double doubleValue;

                    if (!Double.TryParse(stringValue.Replace(',', '.'), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out doubleValue))
                        value = null;
                    else
                        value = doubleValue;
                }
                else if (targetType == typeof(decimal))
                {
                    decimal decimalValue;

                    if (!Decimal.TryParse(stringValue.Replace(',', '.'), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out decimalValue))
                        value = null;
                    else
                        value = decimalValue;
                }
                else if (targetType == typeof(Int32) || targetType == typeof(Int16) || targetType == typeof(Int64) || targetType == typeof(SByte) || targetType.IsEnum)
                {
                    long longValue;

                    if (!Int64.TryParse(stringValue, out longValue))
                        value = null;
                    else
                        value = longValue;
                }
                else if (targetType == typeof(UInt32) || targetType == typeof(UInt16) || targetType == typeof(UInt64) || targetType == typeof(Byte))
                {
                    ulong longValue;

                    if (!UInt64.TryParse(stringValue, out longValue))
                        value = null;
                    else
                        value = longValue;
                }
                else if (targetType == typeof(DateTime))
                {
                    DateTime dateTime;

                    if (!DateTime.TryParseExact(stringValue, new[] { "yyyyMMdd", "yyyy-MM-dd", "yyyy.MM.dd", "yyyy/MM/dd" }, null, DateTimeStyles.NoCurrentDateDefault, out dateTime))
                        value = null;
                    else
                        value = dateTime;
                }
                else if (targetType == typeof(bool))
                {
                    value = (stringValue == "1" || stringValue.ToUpper() == "Y" || stringValue.ToUpper() == "YES" || stringValue.ToUpper() == "T" || stringValue.ToUpper() == "TRUE");
                }
                else
                {
                    value = null;
                }
            }

            if (value == null)
                return null;

            if (targetType.IsValueType)
            {
                if (!targetType.IsGenericType)
                {
                    if (targetType.IsEnum)
                        return Enum.ToObject(targetType, value);
                    else
                        return Convert.ChangeType(value, targetType);
                }

                if (targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    Type sourceType = value.GetType();

                    Type underlyingType = targetType.GetGenericArguments()[0];

                    if (sourceType == underlyingType)
                        return value;

                    if (underlyingType.IsEnum)
                    {
                        return Enum.ToObject(underlyingType, value);
                    }
                    else
                    {
                        return Convert.ChangeType(value, underlyingType);
                    }
                }
            }

            return value;
        }

        private static bool IsNullable(Type type)
        {
            return (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        private static Type GetRealType(Type type)
        {
            if (IsNullable(type))
                return type.GetGenericArguments()[0];

            return type;
        }
    }
}
