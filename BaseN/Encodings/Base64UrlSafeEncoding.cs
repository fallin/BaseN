using System;

namespace BaseN.Encodings
{
    /// <summary>
    /// RFC 3548 Base64 data encoding with URL and Filename Safe Alphabet.
    /// </summary>
    public class Base64UrlSafeEncoding : BaseEncoding
    {
        public Base64UrlSafeEncoding()
            : base("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_", 64)
        {
        }
    }
}