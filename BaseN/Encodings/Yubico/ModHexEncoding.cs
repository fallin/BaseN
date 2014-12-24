using System;

namespace BaseN.Encodings.Yubico
{
    public class ModHexEncoding : BaseEncoding
    {
        public ModHexEncoding()
            : base("cbdefghijklnrtuv", 16)
        {
        }
    }
}