using System;
using System.Collections.Generic;
using System.IO;
using BaseN.Encoders;
using BaseN.Encodings;
using EnsureThat;

namespace BaseN
{
    public abstract class DataEncoding
    {
        // ReSharper disable once InconsistentNaming
        static readonly Lazy<Base64DataEncoding> _base64 = new Lazy<Base64DataEncoding>(() => new Base64DataEncoding());
        public static Base64DataEncoding Base64 => _base64.Value;

        // ReSharper disable once InconsistentNaming
        static readonly Lazy<Base64UrlDataEncoding> _base64Url = new Lazy<Base64UrlDataEncoding>(() => new Base64UrlDataEncoding());
        public static Base64UrlDataEncoding Base64Url => _base64Url.Value;

        // ReSharper disable once InconsistentNaming
        static readonly Lazy<Base62DataEncoding> _base62 = new Lazy<Base62DataEncoding>(() => new Base62DataEncoding());
        public static Base62DataEncoding Base62 => _base62.Value;

        // ReSharper disable once InconsistentNaming
        static readonly Lazy<Base32DataEncoding> _base32 = new Lazy<Base32DataEncoding>(() => new Base32DataEncoding());
        public static Base32DataEncoding Base32 => _base32.Value;

        // ReSharper disable once InconsistentNaming
        static readonly Lazy<Base32HexDataEncoding> _base32Hex = new Lazy<Base32HexDataEncoding>(() => new Base32HexDataEncoding());
        public static Base32HexDataEncoding Base32Hex => _base32Hex.Value;

        // ReSharper disable once InconsistentNaming
        static readonly Lazy<Base16DataEncoding> _base16 = new Lazy<Base16DataEncoding>(() => new Base16DataEncoding());
        public static Base16DataEncoding Base16 => _base16.Value;

        // ReSharper disable once InconsistentNaming
        static readonly Lazy<ModHexDataEncoding> _modhex = new Lazy<ModHexDataEncoding>(() => new ModHexDataEncoding());
        public static ModHexDataEncoding ModHex => _modhex.Value;

        protected DataEncoding(string alphabet, int @encodingBase)
        {
            Alphabet = alphabet.ToCharArray();

            Padding = '=';
            BitsPerChar = Log2(encodingBase);
            BitsPerQuantum = Lcm(8, BitsPerChar);
        }

        public char Padding { get; protected set; }
        public IReadOnlyList<char> Alphabet { get; private set; }
        public int BitsPerChar { get; private set; }
        public int BitsPerQuantum { get; private set; }

        public int CharsPerQuantum
        {
            get { return BitsPerQuantum / BitsPerChar; }
        }

        public string Encode(byte[] bytes)
        {
            using (TextWriter writer = new StringWriter())
            {
                using (var encoder = CreateEncoder(writer))
                {
                    encoder.Encode(bytes);
                }
                return writer.ToString();
            }
        }

        protected virtual DataEncoder CreateEncoder(TextWriter writer)
        {
            Ensure.That(writer, "writer").IsNotNull();
            return new DefaultDataEncoder(this, writer);
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