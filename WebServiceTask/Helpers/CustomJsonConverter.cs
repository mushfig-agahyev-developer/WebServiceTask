using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceTask.Helpers
{
    public static class CustomJsonConverter
    {
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

        public static T Deserialize<T>(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new Exception("Json can't empty");
            try
            {
                T obj;
                var json = new DataContractJsonSerializer(typeof(T));
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(value)))
                {
                    obj = (T)json.ReadObject(stream);
                    stream.Flush();
                    stream.Close();
                }
                return obj;
            }
            catch (Exception)
            {
                throw new Exception("Json can't convert to Object because it isn't correct format.");
            }
        }
    }
}
