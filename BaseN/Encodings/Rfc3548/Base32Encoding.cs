using System;

namespace BaseN.Encodings.Rfc3548
{
    public class Base32Encoding : BaseEncoding
    {
        static readonly char[] _alphabet =
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 
            'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
            'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 
            'Y', 'Z', '2', '3', '4', '5', '6', '7'
        };

        public Base32Encoding()
            : base(_alphabet, 32)
        {
        }
    }
}