using System;

namespace BaseN.Encodings
{
    /// <summary>
    /// RFC 3548 Base32 data encoding.
    /// </summary>
    public class Base32Encoding : BaseEncoding
    {
        public Base32Encoding()
            : base("ABCDEFGHIJKLMNOPQRSTUVWXYZ234567", 32)
        {
        }
    }
}