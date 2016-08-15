namespace RegionServer.DataHelperObjects
{
    public struct CappedByte
    {
        private readonly byte _cap;
        private byte value;

        public CappedByte(byte initValue, byte cap)
        {
            value = (initValue < cap) ? initValue : cap;
            _cap = cap;
        }
        public byte Value
        {
            get { return value; }
        }
        public bool SetValue(byte newValue)
        {
            bool success = newValue <= _cap;
            this.value = (success) ? value : _cap;
            return success;
        }
        public byte PutOne()
        {
            IncrValue();
            return value;
        }

        public byte GetOne()
        {
            DecrValue();
            return value;
        }
        public bool IncrValue(byte factor = 1)
        {
            bool success = (value+=factor) <= _cap;
            this.value = success ? value : _cap;
            return success;
        }
        public bool DecrValue(byte factor = 1)
        {
            bool success = (value -= factor) > 0;
            this.value = success ? value : (byte)0;
            return success;
        }
        public byte GetCap()
        {
            return _cap;
        }

        public static CappedByte Init(byte initValue, byte cap)
        {
            return new CappedByte(initValue, cap);
        }
    }
}
