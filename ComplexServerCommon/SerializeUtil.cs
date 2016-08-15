using Newtonsoft.Json;
using System;
using System.Runtime.Remoting.Messaging;

namespace ComplexServerCommon
{
	public class SerializeUtil
	{
		public static string Serialize<T>(T obj)
		{
		    return JsonConvert.SerializeObject(obj);
		}

	    public static string SerializeWithIgnore<T>(T obj)
	    {
	        try
	        {
	            return JsonConvert.SerializeObject(obj, Formatting.Indented,
	                new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
	        }
	        catch (JsonException e)
	        {
	        }
            return JsonConvert.SerializeObject(obj);
        }

        public static T Deserialize<T>(object obj) where T : class, new()
		{
            return JsonConvert.DeserializeObject<T>(obj.ToString());
		}

	    public static T cloneJSONE<T>(T obj) where T : class, new()
	    {
	        var jsone = Serialize<T>(obj);
	        return (T)Deserialize<T>(jsone);
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

