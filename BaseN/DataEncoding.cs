using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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

        protected DataEncoding(string alphabet, int encodingBase)
        {
            Alphabet = Encoding.ASCII.GetBytes(alphabet);

            Padding = (byte) '=';
            BitsPerChar = Log2(encodingBase);
            BitsPerQuantum = Lcm(8, BitsPerChar);
        }

        public byte Padding { get; protected set; }
        public IReadOnlyList<byte> Alphabet { get; private set; }
        public int BitsPerChar { get; }
        public int BitsPerQuantum { get; }

        public int CharsPerQuantum => BitsPerQuantum / BitsPerChar;

        public string Encode(byte[] bytes)
        {
            using (var outputStream = new MemoryStream())
            {
                using (var encoder = CreateEncoder(outputStream))
                {
                    encoder.Encode(bytes);
                }

                // The encoded text is always ASCII
                string encoded = outputStream.ReadAllText();
                return encoded;
            }
        }

        //public byte[] Decode(string encoded)
        //{
        //    using (MemoryStream outputStream = new MemoryStream())
        //    {
        //        using (var decoder = CreateDecoder(outputStream))
        //        {
        //            decoder.Decode(encoded);
        //        }

        //        outputStream.Seek(0, SeekOrigin.Begin);
        //        StreamReader reader = new StreamReader(outputStream);
        //        string decoded = reader.ReadToEnd();

        //        return decoded;
        //    }
        //}

        protected virtual DataEncoder CreateEncoder(Stream outputStream)
        {
            Ensure.That(outputStream, "outputStream").IsNotNull();
            return new DefaultDataEncoder(this, outputStream);
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
            return a / Gcf(a, b) * b;
        }

        static int Log2(int value)
        {
            return (int) Math.Log(value, 2);
        }
    }
}