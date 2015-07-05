using System.Text;
using NUnit.Framework;

namespace BaseN.Tests
{
    [TestFixture]
    public class Rfc4648TestVectors
    {
        [TestCase(new byte[] { 0x14, 0xfb, 0x9c, 0x03, 0xd9, 0x7e }, Result = "FPucA9l+")]
        [TestCase(new byte[] { 0x14, 0xfb, 0x9c, 0x03, 0xd9 }, Result = "FPucA9k=")]
        [TestCase(new byte[] { 0x14, 0xfb, 0x9c, 0x03 }, Result = "FPucAw==")]
        public string Base64TestVectors(byte[] input)
        {
            string encoded = DataEncoding.Base64.Encode(input);
            return encoded;
        }

        [TestCase("", Result="")]
        [TestCase("f", Result = "Zg==")]
        [TestCase("fo", Result = "Zm8=")]
        [TestCase("foo", Result = "Zm9v")]
        [TestCase("foob", Result = "Zm9vYg==")]
        [TestCase("fooba", Result = "Zm9vYmE=")]
        [TestCase("foobar", Result = "Zm9vYmFy")]
        public string Base64TestVectors(string input)
        {
            byte[] data = Encoding.UTF8.GetBytes(input);
            string encoded = DataEncoding.Base64.Encode(data);
            return encoded;
        }

        [TestCase("", Result = "")]
        [TestCase("f", Result = "MY======")]
        [TestCase("fo", Result = "MZXQ====")]
        [TestCase("foo", Result = "MZXW6===")]
        [TestCase("foob", Result = "MZXW6YQ=")]
        [TestCase("fooba", Result = "MZXW6YTB")]
        [TestCase("foobar", Result = "MZXW6YTBOI======")]
        public string Base32TestVectors(string input)
        {
            byte[] data = Encoding.UTF8.GetBytes(input);
            string encoded = DataEncoding.Base32.Encode(data);
            return encoded;
        }

        [TestCase("", Result = "")]
        [TestCase("f", Result = "CO======")]
        [TestCase("fo", Result = "CPNG====")]
        [TestCase("foo", Result = "CPNMU===")]
        [TestCase("foob", Result = "CPNMUOG=")]
        [TestCase("fooba", Result = "CPNMUOJ1")]
        [TestCase("foobar", Result = "CPNMUOJ1E8======")]
        public string Base32HexTestVectors(string input)
        {
            byte[] data = Encoding.UTF8.GetBytes(input);
            string encoded = DataEncoding.Base32Hex.Encode(data);
            return encoded;
        }

        [TestCase("", Result = "")]
        [TestCase("f", Result = "66")]
        [TestCase("fo", Result = "666F")]
        [TestCase("foo", Result = "666F6F")]
        [TestCase("foob", Result = "666F6F62")]
        [TestCase("fooba", Result = "666F6F6261")]
        [TestCase("foobar", Result = "666F6F626172")]
        public string Base16TestVectors(string input)
        {
            byte[] data = Encoding.UTF8.GetBytes(input);
            string encoded = DataEncoding.Base16.Encode(data);
            return encoded;
        }
    }
}