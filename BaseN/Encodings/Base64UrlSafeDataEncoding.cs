using System;

namespace BaseN.Encodings
{
    /// <summary>
    /// RFC 3548 Base64 data encoding with URL and Filename Safe Alphabet.
    /// </summary>
    public class Base64UrlSafeDataEncoding : DataEncoding
    {
        public Base64UrlSafeDataEncoding()
            : base("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_", 64)
        {
        }
    }
}