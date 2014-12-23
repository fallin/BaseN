using System;

namespace BaseN.Encodings.Rfc3548
{
    public class Base16Encoding : BaseEncoding
    {
        static readonly char[] _alphabet =
        {
            '0', '1', '2', '3', '4', '5', '6', '7', 
            '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'
        };

        public Base16Encoding()
            : base(_alphabet, 16)
        {
        }
    }
}