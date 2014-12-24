using System;

namespace BaseN.Encodings.Rfc3548
{
    public class Base64UrlSafeEncoding : BaseEncoding
    {
        public Base64UrlSafeEncoding()
            : base("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_", 64)
        {
        }
    }
}