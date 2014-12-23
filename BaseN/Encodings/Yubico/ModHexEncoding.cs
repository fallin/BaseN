using System;

namespace BaseN.Encodings.Yubico
{
    public class ModHexEncoding : BaseEncoding
    {
        static readonly char[] _alphabet =
        {
            'c','b','d','e','f','g','h','i',
            'j','k','l','n','r','t','u','v'
        };

        public ModHexEncoding()
            : base(_alphabet, 16)
        {
        }
    }
}