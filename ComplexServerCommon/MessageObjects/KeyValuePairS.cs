using System;
using System.Xml.Serialization;
using System.Linq;
using System.Collections;
using System.Linq.Expressions;


namespace ComplexServerCommon.MessageObjects
{
	public struct KeyValuePairS<K, V>
	{
		[XmlElement(ElementName="Key")]
		public K Key { get; set; }

		[XmlElement(ElementName="Value")]
		public V Value { get; set; }

		public KeyValuePairS(K k, V v)
		{
			Key = k;
			Value = v;
		}
	}
}

