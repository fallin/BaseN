using System;

namespace BaseN.Encodings.Rfc3548
{
    public class Base32Encoding : BaseEncoding
    {
        public Base32Encoding()
            : base("ABCDEFGHIJKLMNOPQRSTUVWXYZ234567", 32)
        {
        }
    }
}