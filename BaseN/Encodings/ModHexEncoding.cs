using System;

namespace BaseN.Encodings
{
    /// <summary>
    /// Yubico ModHex (Modified Hexadecimal) Base32 data encoding.
    /// </summary>
    /// <remarks>
    /// See https://www.yubico.com/modhex-calculator/
    /// </remarks>
    public class ModHexEncoding : BaseEncoding
    {
        public ModHexEncoding()
            : base("cbdefghijklnrtuv", 16)
        {
        }
    }
}