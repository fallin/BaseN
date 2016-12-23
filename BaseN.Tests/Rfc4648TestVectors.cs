using System.Text;
using NUnit.Framework;

namespace BaseN.Tests
{
    [TestFixture]
    public class Rfc4648TestVectors
    {
        [TestCase(new byte[] {0x14, 0xfb, 0x9c, 0x03, 0xd9, 0x7e}, ExpectedResult = "FPucA9l+", TestName = "Input data: 0x14fb9c03d97e")]
        [TestCase(new byte[] {0x14, 0xfb, 0x9c, 0x03, 0xd9}, ExpectedResult = "FPucA9k=", TestName = "Input data: 0x14fb9c03d9")]
        [TestCase(new byte[] {0x14, 0xfb, 0x9c, 0x03}, ExpectedResult = "FPucAw==", TestName = "Input data: 0x14fb9c03")]
        public string Base64TestVectors(byte[] input)
        {
            var encoded = DataEncoding.Base64.Encode(input);
            return encoded;
        }

        [TestCase("", ExpectedResult = "")]
        [TestCase("f", ExpectedResult = "Zg==")]
        [TestCase("fo", ExpectedResult = "Zm8=")]
        [TestCase("foo", ExpectedResult = "Zm9v")]
        [TestCase("foob", ExpectedResult = "Zm9vYg==")]
        [TestCase("fooba", ExpectedResult = "Zm9vYmE=")]
        [TestCase("foobar", ExpectedResult = "Zm9vYmFy")]
        public string Base64TestVectors(string input)
        {
            var data = Encoding.UTF8.GetBytes(input);
            var encoded = DataEncoding.Base64.Encode(data);
            return encoded;
        }

        [TestCase("", ExpectedResult = "")]
        [TestCase("f", ExpectedResult = "MY======")]
        [TestCase("fo", ExpectedResult = "MZXQ====")]
        [TestCase("foo", ExpectedResult = "MZXW6===")]
        [TestCase("foob", ExpectedResult = "MZXW6YQ=")]
        [TestCase("fooba", ExpectedResult = "MZXW6YTB")]
        [TestCase("foobar", ExpectedResult = "MZXW6YTBOI======")]
        public string Base32TestVectors(string input)
        {
            var data = Encoding.UTF8.GetBytes(input);
            var encoded = DataEncoding.Base32.Encode(data);
            return encoded;
        }

        [TestCase("", ExpectedResult = "")]
        [TestCase("f", ExpectedResult = "CO======")]
        [TestCase("fo", ExpectedResult = "CPNG====")]
        [TestCase("foo", ExpectedResult = "CPNMU===")]
        [TestCase("foob", ExpectedResult = "CPNMUOG=")]
        [TestCase("fooba", ExpectedResult = "CPNMUOJ1")]
        [TestCase("foobar", ExpectedResult = "CPNMUOJ1E8======")]
        public string Base32HexTestVectors(string input)
        {
            var data = Encoding.UTF8.GetBytes(input);
            var encoded = DataEncoding.Base32Hex.Encode(data);
            return encoded;
        }

        [TestCase("", ExpectedResult = "")]
        [TestCase("f", ExpectedResult = "66")]
        [TestCase("fo", ExpectedResult = "666F")]
        [TestCase("foo", ExpectedResult = "666F6F")]
        [TestCase("foob", ExpectedResult = "666F6F62")]
        [TestCase("fooba", ExpectedResult = "666F6F6261")]
        [TestCase("foobar", ExpectedResult = "666F6F626172")]
        public string Base16TestVectors(string input)
        {
            var data = Encoding.UTF8.GetBytes(input);
            var encoded = DataEncoding.Base16.Encode(data);
            return encoded;
        }
    }
}