using System;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace BaseN.Tests
{
    [TestFixture]
    public class BaseEncodingTests
    {
        const string SimpleString = "secretpassword";
        readonly Guid _uuid = new Guid("c4f2e4fefdd74d638a30d2c44d443034");

        const string B64 = "64 bit";
        const string B64UrlSafe = "64 bit UrlSafe";
        const string B32 = "32 bit";
        const string B16 = "16 bit";

        [Test, Category(B64)]
        public void Base64Encode_with_guid()
        {
            string encoded = BaseEncoding.Base64.Encode(_uuid.ToByteArray());
            encoded.Should().Be("/uTyxNf9Y02KMNLETUQwNA==");
        }

        [Test, Category(B64)]
        public void Base64Encode_with_simple_string()
        {
            byte[] ascii = Encoding.ASCII.GetBytes(SimpleString);
            string encoded = BaseEncoding.Base64.Encode(ascii);
            encoded.Should().Be("c2VjcmV0cGFzc3dvcmQ=");
        }

        [Test, Category(B64)]
        public void Base64Encode_with_quantum()
        {
            byte[] ascii = Encoding.ASCII.GetBytes("ABC");
            string encoded = BaseEncoding.Base64.Encode(ascii);
            encoded.Should().Be("QUJD");
        }

        [Test, Category(B64)]
        public void Base64Encode_with_quantum_minus1()
        {
            byte[] ascii = Encoding.ASCII.GetBytes("AB");
            string encoded = BaseEncoding.Base64.Encode(ascii);
            encoded.Should().Be("QUI=");
        }

        [Test, Category(B64)]
        public void Base64Encode_with_quantum_minus2()
        {
            byte[] ascii = Encoding.ASCII.GetBytes("A");
            string encoded = BaseEncoding.Base64.Encode(ascii);
            encoded.Should().Be("QQ==");
        }

        [Test, Category(B64)]
        public void Base64Encode_with_empty_string()
        {
            byte[] ascii = Encoding.ASCII.GetBytes("");
            string encoded = BaseEncoding.Base64.Encode(ascii);
            encoded.Should().Be("");
        }

        [Test, Category(B64)]
        public void Base64Encode_with_null_string()
        {
            Action action = () => BaseEncoding.Base64.Encode(null);
            action.ShouldThrow<ArgumentNullException>();
        }

        [Test, Category(B64UrlSafe)]
        public void Base64UrlSafeEncode_with_guid()
        {
            string encoded = BaseEncoding.Base64UrlSafe.Encode(_uuid.ToByteArray());
            encoded.Should().Be("_uTyxNf9Y02KMNLETUQwNA==");
        }

        [Test, Category(B64UrlSafe)]
        public void Base64UrlSafeEncode_with_simple_string()
        {
            byte[] ascii = Encoding.ASCII.GetBytes(SimpleString);
            string encoded = BaseEncoding.Base64UrlSafe.Encode(ascii);
            encoded.Should().Be("c2VjcmV0cGFzc3dvcmQ=");
        }

        [Test, Category(B32)]
        public void Base32Encode_with_guid()
        {
            string encoded = BaseEncoding.Base32.Encode(_uuid.ToByteArray());
            encoded.Should().Be("73SPFRGX7VRU3CRQ2LCE2RBQGQ======");
        }

        [Test, Category(B32)]
        public void Base32Encode_with_simple_string()
        {
            byte[] ascii = Encoding.ASCII.GetBytes(SimpleString);
            string encoded = BaseEncoding.Base32.Encode(ascii);
            encoded.Should().Be("ONSWG4TFORYGC43TO5XXEZA=");
        }

        [Test, Category(B32)]
        public void Base32Encode_with_1Byte()
        {
            byte[] bytes = { 0x11 };
            string encoded = BaseEncoding.Base32.Encode(bytes);
            encoded.Should().Be("CE======");
        }

        [Test, Category(B32)]
        public void Base32Encode_with_2bytes()
        {
            byte[] bytes = { 0x11, 0x11 };
            string encoded = BaseEncoding.Base32.Encode(bytes);
            encoded.Should().Be("CEIQ====");
        }

        [Test, Category(B32)]
        public void Base32Encode_with_3bytes()
        {
            byte[] bytes = { 0x11, 0x11, 0x11 };
            string encoded = BaseEncoding.Base32.Encode(bytes);
            encoded.Should().Be("CEIRC===");
        }

        [Test, Category(B32)]
        public void Base32Encode_with_4bytes()
        {
            byte[] bytes = { 0x11, 0x11, 0x11, 0x11 };
            string encoded = BaseEncoding.Base32.Encode(bytes);
            encoded.Should().Be("CEIRCEI=");
        }

        [Test, Category(B32)]
        public void Base32Encode_with_5byte_quantum()
        {
            byte[] bytes = { 0x11, 0x11, 0x11, 0x11, 0x11 };
            string encoded = BaseEncoding.Base32.Encode(bytes);
            encoded.Should().Be("CEIRCEIR");
        }

        [Test, Category(B32)]
        public void Base32Encode_with_6byte_quantum_plus1()
        {
            byte[] bytes = { 0x11, 0x11, 0x11, 0x11, 0x11, 0x11 };
            string encoded = BaseEncoding.Base32.Encode(bytes);
            encoded.Should().Be("CEIRCEIRCE======");
        }

        [Test, Category(B32)]
        public void Base32Encode_with_7byte_quantum_plus2()
        {
            byte[] bytes = { 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11 };
            string encoded = BaseEncoding.Base32.Encode(bytes);
            encoded.Should().Be("CEIRCEIRCEIQ====");
        }

        [Test, Category(B32)]
        public void Base32Encode_with_8byte_quantum_plus3()
        {
            byte[] bytes = { 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11 };
            string encoded = BaseEncoding.Base32.Encode(bytes);
            encoded.Should().Be("CEIRCEIRCEIRC===");
        }

        [Test, Category(B32)]
        public void Base32Encode_with_9byte_quantum_plus4()
        {
            byte[] bytes = { 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11 };
            string encoded = BaseEncoding.Base32.Encode(bytes);
            encoded.Should().Be("CEIRCEIRCEIRCEI=");
        }

        [Test, Category(B16)]
        public void Base16Encode_with_guid()
        {
            string encoded = BaseEncoding.Base16.Encode(_uuid.ToByteArray());
            encoded.Should().Be("FEE4F2C4D7FD634D8A30D2C44D443034");
        }

        [Test, Category(B16)]
        public void Base16Encode_with_simple_string()
        {
            byte[] ascii = Encoding.ASCII.GetBytes(SimpleString);
            string encoded = BaseEncoding.Base16.Encode(ascii);
            encoded.Should().Be("73656372657470617373776F7264");
        }

        [Test, Category("Modhex")]
        public void ModhexEncode_with_simple_string()
        {
            byte[] ascii = Encoding.ASCII.GetBytes(SimpleString);
            string encoded = BaseEncoding.ModHex.Encode(ascii);
            encoded.Should().Be("iehgheidhgifichbieieiihvidhf");
        }
    }
}