using System;

namespace BaseN.Encodings.Rfc3548
{
    /// <summary>
    /// http://tools.ietf.org/html/rfc3548
    /// </summary>
    public class Base64Encoding : BaseEncoding
    {
        static readonly char[] _alphabet =
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 
            'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
            'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 
            'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f',
            'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
            'o', 'p', 'q', 'r', 's', 't', 'u', 'v',
            'w', 'x', 'y', 'z', '0', '1', '2', '3',
            '4', '5', '6', '7', '8', '9', '+', '/'
        };

        public Base64Encoding()
            : base(_alphabet, 64)
        {
        }
    }
}