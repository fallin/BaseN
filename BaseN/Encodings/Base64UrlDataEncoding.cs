using System;

namespace BaseN.Encodings
{
    /// <summary>
    /// RFC 4648 Base64 data encoding with URL and Filename Safe Alphabet.
    /// </summary>
    public class Base64UrlDataEncoding : DataEncoding
    {
        public Base64UrlDataEncoding()
            : base("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_", 64)
        {
        }
    }
}