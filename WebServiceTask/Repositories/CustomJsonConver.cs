//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.Runtime.Serialization;
//using System.Text;
//using System.Threading.Tasks;

//namespace WebServiceTask.Repositories
//{
//    public class CustomSerialize : IFormatter
//    {
//        private Type _type { get; }
//        public StringBuilder _json { get; set; }
//        public CustomSerialize(Type type)
//        {
//            _type = type;
//            _json = new StringBuilder();
//        }

//        public SerializationBinder Binder { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//        public StreamingContext Context { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//        public ISurrogateSelector SurrogateSelector { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

//        public object Deserialize(Stream serializationStream)
//        {
//            Object obj = Activator.CreateInstance(_type);

//            using (var stream = new StreamReader(serializationStream))
//            {
//                string type = stream.ReadLine();
//                string desc = stream.ReadToEnd();
//                List<string> pairs = desc.Split(new string[] { "\n", "\r\n" }, 
//                    StringSplitOptions.RemoveEmptyEntries).ToList();

//                string key, value;
//                pairs.ForEach(r =>
//                {
//                    string[] _keyvalue = r.Split(':');
//                    key = _keyvalue[0];
//                    value = _keyvalue[1];

//                    PropertyInfo propertyInfo = _type.GetProperty(key);
//                    if (propertyInfo != null)
//                    {
//                        propertyInfo.SetValue(obj, value, null);
//                    }
//                });
//            }
//            return obj;
//        }

//        public void Serialize(Stream serializationStream, object graph)
//        {
//            List<PropertyInfo> _properties = _type.GetProperties().ToList();
//            StreamWriter streamWriter = new StreamWriter(serializationStream);
//            streamWriter.WriteLine(_type.Name);

//            foreach (PropertyInfo info in _properties)
//            {
//                _json.Append(String.Format("{0},{1}", info.Name, info.GetValue(graph)));
//            }
//            streamWriter.Flush();
//            streamWriter.Dispose();
//        }
//    }
//}
