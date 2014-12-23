using System;
using System.Collections.Generic;
using System.IO;
using BaseN.Encodings.Rfc3548;
using BaseN.Encodings.Yubico;

namespace BaseN
{
    public abstract class BaseEncoding
    {
        static readonly Lazy<BaseEncoding> _base64 = new Lazy<BaseEncoding>(() => new Base64Encoding());
        public static BaseEncoding Base64
        {
            get { return _base64.Value; }
        }

        static readonly Lazy<BaseEncoding> _base64urlsafe = new Lazy<BaseEncoding>(() => new Base64UrlSafeEncoding());
        public static BaseEncoding Base64UrlSafe
        {
            get { return _base64urlsafe.Value; }
        }

        static readonly Lazy<BaseEncoding> _base32 = new Lazy<BaseEncoding>(() => new Base32Encoding());
        public static BaseEncoding Base32
        {
            get { return _base32.Value; }
        }

        static readonly Lazy<BaseEncoding> _base16 = new Lazy<BaseEncoding>(() => new Base16Encoding());
        public static BaseEncoding Base16
        {
            get { return _base16.Value; }
        }

        static readonly Lazy<BaseEncoding> _modhex = new Lazy<BaseEncoding>(() => new ModHexEncoding());
        public static BaseEncoding ModHex
        {
            get { return _modhex.Value; }
        }

        readonly IReadOnlyList<char> _alphabet;
        readonly int _bitsPerChar;
        readonly int _bitsPerQuantum;

        protected BaseEncoding(IReadOnlyList<char> alphabet, int @encodingBase)
        {
            _alphabet = alphabet;

            Padding = '=';
            _bitsPerChar = Log2(encodingBase);
            _bitsPerQuantum = Lcm(8, _bitsPerChar);
        }

        public char Padding { get; protected set; }

        public IReadOnlyList<char> Alphabet
        {
            get { return _alphabet; }
        }

        public int BitsPerChar
        {
            get { return _bitsPerChar; }
        }

        public int BitsPerQuantum
        {
            get { return _bitsPerQuantum; }
        }

        public string Encode(byte[] bytes)
        {
            using (var writer = new StringWriter())
            {
                using (var encoder = new BaseEncoder(this, writer))
                {
                    encoder.Write(bytes);
                }
                return writer.ToString();
            }
        }

        static int Gcf(int a, int b)
        {
            while (b != 0)
            {
                int t = b;
                b = a % b;
                a = t;
            }
            return a;
        }

        static int Lcm(int a, int b)
        {
            return (a / Gcf(a, b)) * b;
        }

        static int Log2(int value)
        {
            return (int) Math.Log(value, 2);
        }
    }
}