using System.Xml.Serialization;
using System.IO;
using System;

namespace ComplexServerCommon
{
	public static class Xml
	{
		public static string Serialize<T>(T obj)
		{
			XmlSerializer mySerializer = new XmlSerializer(typeof(T));
			StringWriter outStream = new StringWriter();
			mySerializer.Serialize(outStream, obj);
			return outStream.ToString();
		}
		
		public static T Deserialize<T>(object obj) where T : class, new()
		{
			XmlSerializer mySerializer = new XmlSerializer(typeof(T));
			StringReader inStream = new StringReader(Convert.ToString(obj));
			return (T)mySerializer.Deserialize(inStream);
		}
	}
}

