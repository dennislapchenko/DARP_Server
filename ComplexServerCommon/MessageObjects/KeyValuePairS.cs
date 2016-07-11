//using System.Xml.Serialization;


namespace ComplexServerCommon.MessageObjects
{
    [System.Serializable]
    public struct KeyValuePairS<K, V>
    {
        //[XmlElement(ElementName = "Key")]
        public K Key;

        //[XmlElement(ElementName = "Value")]
        public V Value;

		public KeyValuePairS(K k, V v)
		{
			Key = k;
			Value = v;
		}
	}
}

