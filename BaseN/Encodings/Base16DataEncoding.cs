using System;

namespace BaseN.Encodings
{
    /// <summary>
    /// RFC 4648 Base16 data encoding.
    /// </summary>
    public class Base16DataEncoding : DataEncoding
    {
        public Base16DataEncoding()
            : base("0123456789ABCDEF", 16)
        {
        }
    }
}