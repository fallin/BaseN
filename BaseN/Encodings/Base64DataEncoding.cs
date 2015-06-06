using System;

namespace BaseN.Encodings
{
    /// <summary>
    /// RFC 3548 Base64 data encoding.
    /// </summary>
    public class Base64DataEncoding : DataEncoding
    {
        public Base64DataEncoding()
            : base("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/", 64)
        {
        }
    }
}