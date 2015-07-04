using System;

namespace BaseN.Encodings
{
    /// <summary>
    /// RFC 4648 Base32 data encoding.
    /// </summary>
    public class Base32DataEncoding : DataEncoding
    {
        public Base32DataEncoding()
            : base("ABCDEFGHIJKLMNOPQRSTUVWXYZ234567", 32)
        {
        }
    }
}