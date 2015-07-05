using System;

namespace BaseN.Encodings
{
    /// <summary>
    /// RFC 4648 Base32 data encoding with Extended Hex Alphabet
    /// </summary>
    public class Base32HexDataEncoding : DataEncoding
    {
        public Base32HexDataEncoding()
            : base("0123456789ABCDEFGHIJKLMNOPQRSTUV", 32)
        {
        }
    }
}