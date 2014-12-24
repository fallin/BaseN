using System;

namespace BaseN.Encodings.Rfc3548
{
    public class Base16Encoding : BaseEncoding
    {
        public Base16Encoding()
            : base("0123456789ABCDEF", 16)
        {
        }
    }
}