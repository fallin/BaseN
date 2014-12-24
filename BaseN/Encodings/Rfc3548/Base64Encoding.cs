using System;

namespace BaseN.Encodings.Rfc3548
{
    /// <summary>
    /// http://tools.ietf.org/html/rfc3548
    /// </summary>
    public class Base64Encoding : BaseEncoding
    {
        public Base64Encoding()
            : base("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/", 64)
        {
        }
    }
}