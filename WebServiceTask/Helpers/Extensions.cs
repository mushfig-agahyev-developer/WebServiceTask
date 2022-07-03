//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Runtime.Serialization;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.Runtime.Serialization.Json;
//using System.Text;
//using System.Threading.Tasks;
//using WebServiceTask.DTO;
//using WebServiceTask.Models;

//namespace WebServiceTask.Helpers
//{
//    public static class Extensions
//    {


//        /// <summary>
//        /// serializes the given object into memory stream
//        /// </summary>
//        /// <param name="objectType">the object to be serialized</param>
//        /// <returns>The serialized object as memory stream</returns>
//        public static MemoryStream SerializeToStream(object objectType)
//        {
//            MemoryStream stream = new MemoryStream();
//            IFormatter formatter = new BinaryFormatter();
//            formatter.Serialize(stream, objectType);
//            return stream;
//        }

//        /// <summary>
//        /// deserializes as an object
//        /// </summary>
//        /// <param name="stream">the stream to deserialize</param>
//        /// <returns>the deserialized object</returns>
//        public static object DeserializeFromStream(MemoryStream stream)
//        {
//            IFormatter formatter = new BinaryFormatter();
//            stream.Seek(0, SeekOrigin.Begin);
//            object objectType = formatter.Deserialize(stream);
//            return objectType;
//        }

//        public static void test()
//        {
//            Person person = new Person()
//            {
//                Id = 1,
//                FirstName = "Ivan",
//                LastName = "Nana",
//                Address = new Address { City = "Baku", AddressLine = "Baku 28May" }
//            };

//            PersonDTO personDTO = person;
//            string json = string.Empty;

//            //byte[] byteArray = Encoding.UTF8.GetBytes(Serialize<PersonDTO>(personDTO));
//            //MemoryStream stream = new MemoryStream(byteArray);



//            MemoryStream memory = new MemoryStream();
//            var formatter = new BinaryFormatter();
//            formatter.Serialize(memory, personDTO);
//            byte[] buf = memory.ToArray();
//            MemoryStream stream = new MemoryStream(buf);

//            var Amin = ObjectToByteArray(personDTO);
//        }
//        private byte[] ObjectToByteArray(Object obj)
//        {
//            if (obj == null)
//                return null;

//            BinaryFormatter bf = new BinaryFormatter();
//            MemoryStream ms = new MemoryStream();
//            bf.Serialize(ms, obj);

//            return ms.ToArray();
//        }

//        public static T ToObject(string json)
//        {
//            var ser = new DataContractJsonSerializer(typeof(T));

//            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
//            {
//                var result = (T)ser.ReadObject(stream);
//                return result;
//            }
//        }

//        public static string ToJsonString(T obj)
//        {
//            var ms = new MemoryStream();
//            var ser = new DataContractJsonSerializer(typeof(T));
//            ser.WriteObject(ms, obj);
//            byte[] json = ms.ToArray();
//            ms.Close();
//            return Encoding.UTF8.GetString(json, 0, json.Length);
//        }
//    }
//}
