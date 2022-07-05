using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebServiceTask.Helpers
{
    public static class CustomJsonConverter
    {
        /*Custom Serialize<T>*/
        public static string Serialize(object obj)
        {
            StringBuilder stringBuilder = new StringBuilder();

            IEnumerable<PropertyInfo> properties = obj.GetType().GetProperties();

            if (!obj.GetType().IsPrimitive && obj.GetType() != typeof(string))
            {
                bool isArray = typeof(IEnumerable).IsAssignableFrom(obj.GetType()) ? true : false;

                if (isArray)
                    stringBuilder.Append($"[");
                else
                    stringBuilder.Append($"{{");

                if (!isArray)
                    foreach (var property in properties)
                        if (property.PropertyType.IsPrimitive || property.PropertyType == typeof(string))
                            stringBuilder.Append($"\"{property.Name}\": \"{property.GetValue(obj)}\", ");
                        else
                            stringBuilder.Append($"\"{property.Name}\": {Serialize(property.GetValue(obj))}, ");
                else
                    foreach (var i in obj as IEnumerable)
                        if (i.GetType().IsPrimitive || i.GetType() == typeof(string))
                            stringBuilder.Append($"\"{i}\", ");
                        else
                            stringBuilder.Append($"{Serialize(i)}, ");

                stringBuilder.Remove(stringBuilder.ToString().Length - 2, 2);

                if (isArray)
                    stringBuilder.Append($"]");
                else
                    stringBuilder.Append($"}}");
            }
            else
                stringBuilder.Append(obj.ToString());

            return stringBuilder.ToString();
        }

        /*Custom Deserialize<T>*/
        public static T Deserialize<T>(string json)
        {
            try
            {
                var myObject = ParseJSON(json);
                T result = CastAs<T>(myObject, new Dictionary<string, string>());
                return result;
            }
            catch
            {
                T _error = (T)Activator.CreateInstance(typeof(T));
                return _error;
            }
        }

        /************************************************/
        private static Dictionary<string, object> ParseJSON(string json)
        {
            int end;
            return ParseJSON(json, 0, out end);
        }

        private static Dictionary<string, object> ParseJSON(string json, int start, out int end)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            bool escbegin = false;
            bool escend = false;
            bool inquotes = false;
            string key = null;
            int cend;
            StringBuilder sb = new StringBuilder();
            Dictionary<string, object> child = null;
            List<object> arraylist = null;
            Regex regex = new Regex(@"\\u([0-9a-z]{4})", RegexOptions.IgnoreCase);
            int autoKey = 0;
            int subArrayCount = 0;
            List<int> arrayIndexes = new List<int>();
            bool inSingleQuotes = false;
            bool inDoubleQuotes = false;
            for (int i = start; i < json.Length; i++)
            {
                char c = json[i];
                if (c == '\\') escbegin = !escbegin;
                if (!escbegin)
                {
                    if (c == '"' && !inSingleQuotes)
                    {
                        inDoubleQuotes = !inDoubleQuotes;
                        inquotes = !inquotes;
                        if (!inquotes && arraylist != null)
                        {
                            arraylist.Add(DecodeString(regex, sb.ToString()));
                            sb.Length = 0;
                        }
                        continue;
                    }
                    else if (c == '\'' && !inDoubleQuotes)
                    {
                        inSingleQuotes = !inSingleQuotes;
                        inquotes = !inquotes;
                        if (!inquotes && arraylist != null)
                        {
                            arraylist.Add(DecodeString(regex, sb.ToString()));
                            sb.Length = 0;
                        }
                        continue;
                    }
                    if (!inquotes)
                    {
                        switch (c)
                        {
                            case '{':
                                if (i != start)
                                {
                                    child = ParseJSON(json, i, out cend);
                                    if (arraylist != null)
                                    {
                                        arraylist.Add(child);
                                    }
                                    else
                                    {
                                        dict.Add(key.Trim(), child);
                                        key = null;
                                    }
                                    i = cend;
                                }
                                continue;
                            case '}':
                                end = i;
                                if (key != null)
                                {
                                    if (arraylist != null) dict.Add(key.Trim(), arraylist);
                                    else dict.Add(key.Trim(), DecodeString(regex, sb.ToString().Trim()));
                                }
                                return dict;
                            case '[':
                                if (arraylist != null)
                                {
                                    List<object> _tempArrayList = arraylist;
                                    for (int l = 0; l < subArrayCount; l++)
                                    {
                                        if (l == subArrayCount - 1)
                                        {
                                            _tempArrayList.Add(new List<object>());
                                        }
                                        else
                                        {
                                            _tempArrayList = (List<object>)_tempArrayList[arrayIndexes[l]];
                                        }
                                    }

                                    if (arrayIndexes.Count < subArrayCount)
                                    {
                                        arrayIndexes.Add(0);
                                    }
                                    subArrayCount++;
                                }
                                else
                                {
                                    arraylist = new List<object>();
                                    subArrayCount++;
                                }
                                continue;
                            case ']':
                                if (key == null)
                                {
                                    key = "array" + autoKey.ToString();
                                    autoKey++;
                                }
                                if (arraylist != null)
                                {
                                    List<object> _tempArrayList = arraylist;
                                    for (int l = 0; l < subArrayCount; l++)
                                    {
                                        if (l == subArrayCount - 1)
                                        {
                                            if (sb.Length > 0)
                                            {
                                                _tempArrayList.Add(sb.ToString());
                                            }
                                            subArrayCount--;
                                            if (subArrayCount == arrayIndexes.Count)
                                            {
                                                if (arrayIndexes.Count > 0)
                                                {
                                                    arrayIndexes[arrayIndexes.Count - 1]++;
                                                }
                                            }
                                            else if (subArrayCount == arrayIndexes.Count - 1)
                                            {
                                                arrayIndexes.RemoveAt(arrayIndexes.Count - 1);
                                                if (arrayIndexes.Count > 0)
                                                {
                                                    arrayIndexes[arrayIndexes.Count - 1]++;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            _tempArrayList = (List<object>)_tempArrayList[arrayIndexes[l]];
                                        }
                                    }
                                    sb.Length = 0;
                                }
                                if (subArrayCount == 0)
                                {
                                    dict.Add(key.Trim(), arraylist);
                                    arraylist = null;
                                    key = null;
                                }
                                continue;
                            case ',':
                                if (arraylist == null && key != null)
                                {
                                    dict.Add(key.Trim(), DecodeString(regex, sb.ToString().Trim()));
                                    key = null;
                                    sb.Length = 0;
                                }
                                if (arraylist != null && sb.Length > 0)
                                {
                                    List<object> _tempArrayList = arraylist;
                                    for (int l = 0; l < subArrayCount; l++)
                                    {
                                        if (l == subArrayCount - 1)
                                        {
                                            _tempArrayList.Add(sb.ToString());
                                        }
                                        else
                                        {
                                            _tempArrayList = (List<object>)_tempArrayList[arrayIndexes[l]];
                                        }
                                    }
                                    sb.Length = 0;
                                }
                                continue;
                            case ':':
                                key = DecodeString(regex, sb.ToString());
                                sb.Length = 0;
                                continue;
                        }
                    }
                }
                sb.Append(c);
                if (escend) escbegin = false;
                if (escbegin) escend = true;
                else escend = false;
            }
            end = json.Length - 1;
            return dict; //shouldn't ever get here unless the JSON is malformed
        }

        private static string DecodeString(Regex regex, string str)
        {
            return Regex.Unescape(regex.Replace(str, match => char.ConvertFromUtf32(Int32.Parse(match.Groups[1].Value, System.Globalization.NumberStyles.HexNumber))));
        }
        
        private static T CastAs<T>(Dictionary<string, object> source, Dictionary<string, string> mappingTable = null)
        {
            T outputData = (T)Activator.CreateInstance(typeof(T));
            TrySet(outputData, source, mappingTable);
            return outputData;
        }

        private static void TrySet(object target, Dictionary<string, object> source, Dictionary<string, string> mappingTable = null)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }
            bool useMappingTable = mappingTable != null && mappingTable.Count > 0;
            foreach (KeyValuePair<string, object> kv in source)
            {
                string propertyName = null;
                if (useMappingTable && mappingTable.ContainsKey(kv.Key))
                {
                    propertyName = mappingTable[kv.Key];
                }
                else
                {
                    propertyName = kv.Key;
                }

                if (!string.IsNullOrEmpty(propertyName))
                {
                    UpdateMember(target, propertyName, kv.Value, mappingTable);
                }
            }
        }

        private static void UpdateMember(object target, string propertyName, object value, Dictionary<string, string> mappingTable)
        {
            try
            {
                FieldInfo fieldInfo = target.GetType().GetField(propertyName);

                if (fieldInfo != null)
                {
                    value = ConvertTo(value, fieldInfo.FieldType, mappingTable);
                    fieldInfo.SetValue(target, value);
                }
                else
                {
                    PropertyInfo propInfo = target.GetType().GetProperty(propertyName);

                    if (propInfo != null)
                    {
                        value = ConvertTo(value, propInfo.PropertyType, mappingTable);
                        propInfo.SetValue(target, value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        private static object ConvertTo(object value, Type targetType, Dictionary<string, string> mappingTable)
        {
            try
            {
                bool isNullable = false;
                Type sourceType = value.GetType();

                //Obtain actual type to convert to (this is necessary in case of Nullable types)
                if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    isNullable = true;
                    targetType = targetType.GetGenericArguments()[0];
                }

                if (isNullable && string.IsNullOrWhiteSpace(Convert.ToString(value)))
                {
                    return null;
                }
                //if we are converting from a dictionary to a class, call the TrySet method to convert its members
                else if (targetType.IsClass && sourceType.IsGenericType && sourceType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    //make sure our value is actually a Dictionary<string, object> in order to be able to cast
                    if (sourceType.GetGenericArguments()[0] == typeof(string))
                    {
                        object convertedValue = Activator.CreateInstance(targetType);
                        TrySet(convertedValue, (Dictionary<string, object>)value, mappingTable);
                        return convertedValue;
                    }
                    return null;
                }
                else if (IsCollection(value))
                {
                    Type elementType = GetCollectionElementType(targetType);

                    if (elementType != null)
                    {
                        if (targetType.BaseType == typeof(Array))
                        {
                            return ConvertToArray(elementType, value, mappingTable);
                        }
                        else
                        {
                            return ConvertToList(elementType, value, mappingTable);
                        }
                    }
                    else
                    {
                        throw new NullReferenceException();
                    }
                }
                else if (targetType == typeof(DateTimeOffset))
                {
                    return new DateTimeOffset((DateTime)ChangeType(value, typeof(DateTime)));
                }
                else if (targetType == typeof(object))
                {
                    return value;
                }
                else
                {
                    return ChangeType(value, targetType);
                }
            }
            catch (Exception ex)
            {
                if (targetType.IsValueType)
                {
                    return Activator.CreateInstance(targetType);
                }
                return null;
            }
        }

        private static Array ConvertToArray(Type elementType, object value, Dictionary<string, string> mappingTable)
        {
            Array collection = Array.CreateInstance(elementType, ((ICollection)value).Count);

            int i = 0;
            foreach (object item in (IEnumerable)value)
            {
                try
                {
                    collection.SetValue(ConvertTo(item, elementType, mappingTable), i);
                    i++;
                }
                catch (Exception ex)
                {
                    //var log = ex.ToString();
                }
            }

            return collection;
        }

        private static IList ConvertToList(Type elementType, object value, Dictionary<string, string> mappingTable)
        {
            Type listType = typeof(List<>);
            Type constructedListType = listType.MakeGenericType(elementType);
            IList collection = (IList)Activator.CreateInstance(constructedListType);

            foreach (object item in (IEnumerable)value)
            {
                try
                {
                    collection.Add(ConvertTo(item, elementType, mappingTable));
                }
                catch (Exception ex)
                {
                    //var log = ex.ToString();
                }
            }

            return collection;
        }

        private static bool IsCollection(object obj)
        {
            bool isCollection = false;

            Type objType = obj.GetType();
            if (!typeof(string).IsAssignableFrom(objType) && typeof(IEnumerable).IsAssignableFrom(objType))
            {
                isCollection = true;
            }

            return isCollection;
        }

        private static Type GetCollectionElementType(Type objType)
        {
            Type elementType;
            Type[] genericArgs = objType.GenericTypeArguments;
            if (genericArgs.Length > 0)
            {
                elementType = genericArgs[0];
            }
            else
            {
                elementType = objType.GetElementType();
            }

            return elementType;
        }

        private static object ChangeType(object value, Type castTo)
        {
            try
            {
                return Convert.ChangeType(value, castTo);
            }
            catch (Exception ex)
            {
                return value;
            }
        }
    }
}
