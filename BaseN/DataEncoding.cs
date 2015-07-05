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
        static readonly Lazy<DataEncoding> _base64 = new Lazy<DataEncoding>(() => new Base64DataEncoding());
        public static DataEncoding Base64
        {
            get { return _base64.Value; }
        }

        static readonly Lazy<DataEncoding> _base64Url = new Lazy<DataEncoding>(() => new Base64UrlDataEncoding());
        public static DataEncoding Base64Url
        {
            get { return _base64Url.Value; }
        }

        static readonly Lazy<DataEncoding> _base32 = new Lazy<DataEncoding>(() => new Base32DataEncoding());
        public static DataEncoding Base32
        {
            get { return _base32.Value; }
        }

        static readonly Lazy<DataEncoding> _base32Hex = new Lazy<DataEncoding>(() => new Base32HexDataEncoding());
        public static DataEncoding Base32Hex
        {
            get { return _base32Hex.Value; }
        }

        static readonly Lazy<DataEncoding> _base16 = new Lazy<DataEncoding>(() => new Base16DataEncoding());
        public static DataEncoding Base16
        {
            get { return _base16.Value; }
        }

        static readonly Lazy<DataEncoding> _modhex = new Lazy<DataEncoding>(() => new ModHexDataEncoding());
        public static DataEncoding ModHex
        {
            get { return _modhex.Value; }
        }

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

        public string Encode(byte[] bytes)
        {
            using (TextWriter writer = new StringWriter())
            {
                using (var encoder = CreateEncoder(writer))
                {
                    encoder.Write(bytes);
                }
                return writer.ToString();
            }
        }

        protected virtual IDataEncoder CreateEncoder(TextWriter writer)
        {
            Ensure.That(writer, "writer").IsNotNull();
            return new DataEncoder(this, writer);
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