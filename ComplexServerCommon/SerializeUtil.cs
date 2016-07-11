using Newtonsoft.Json;

namespace ComplexServerCommon
{
	public class SerializeUtil
	{
		public static string Serialize<T>(T obj)
		{
            return JsonConvert.SerializeObject(obj);

//          XmlSerializer mySerializer = new XmlSerializer(typeof(T));
//			StringWriter outStream = new StringWriter();
//			mySerializer.Serialize(outStream, obj);
//			return outStream.ToString();
		}
		
		public static T Deserialize<T>(object obj) where T : class, new()
		{
            return JsonConvert.DeserializeObject<T>(obj.ToString());

//          XmlSerializer mySerializer = new XmlSerializer(typeof(T));
//			StringReader inStream = new StringReader(Convert.ToString(obj));
//			return (T)mySerializer.Deserialize(inStream);
		}

/**
 * 
	    public static string SerializeJSON<T>(T obj)
	    {
            return JsonConvert.SerializeObject(obj);
        }

	    public static T DeserializeJSON<T>(object obj) where T : class, new()
	    {
            return JsonConvert.DeserializeObject<T>(Convert.ToString(obj));
        }


        public static T cloneByMsgPack<T>(T obj)
        {
            var stream = new MemoryStream();
            var serializer = MessagePackSerializer.Get<T>();

            serializer.Pack(stream, obj);
            stream.Position = 0;
            return serializer.Unpack(stream);
        }

        public static T cloneByBinaryF<T>(T obj)
        {
            var bf = new BinaryFormatter();
            var ms = new MemoryStream();
            bf.Serialize(ms, obj);
            ms.Position = 0;
            return (T)bf.Deserialize(ms);
        }

        public static string Serialize<T>(T obj)
        {
            var serializer = MessagePackSerializer.Get<T>();
            var stream = new MemoryStream();
            serializer.Pack(stream, obj);
            return stream.ToString();
        }

        public static T Deserialize<T>(object obj) where T : class, new()
        {
            var serializer = MessagePackSerializer.Get<T>();
            var stream = new MemoryStream((byte[])obj);
            return (T)serializer.Unpack(stream);
        }

**/

    }
}

