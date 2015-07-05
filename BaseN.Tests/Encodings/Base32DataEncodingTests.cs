using System;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace BaseN.Tests.Encodings
{
    [TestFixture]
    public class Base32DataEncodingTests
    {
        [Test]
        public void Encode_with_guid()
        {
            string encoded = DataEncoding.Base32.Encode(SimpleTestCases.Uuid.ToByteArray());
            encoded.Should().Be("73SPFRGX7VRU3CRQ2LCE2RBQGQ======");
        }

        [Test]
        public void Encode_with_simple_string()
        {
            byte[] ascii = Encoding.ASCII.GetBytes(SimpleTestCases.SimpleString);
            string encoded = DataEncoding.Base32.Encode(ascii);
            encoded.Should().Be("ORUGS4ZANFZSAYJAORSXG5A=");
        }

        [Test]
        public void Encode_with_1Byte()
        {
            byte[] bytes = { 0x11 };
            string encoded = DataEncoding.Base32.Encode(bytes);
            encoded.Should().Be("CE======");
        }

        [Test]
        public void Encode_with_2bytes()
        {
            byte[] bytes = { 0x11, 0x11 };
            string encoded = DataEncoding.Base32.Encode(bytes);
            encoded.Should().Be("CEIQ====");
        }

        [Test]
        public void Encode_with_3bytes()
        {
            byte[] bytes = { 0x11, 0x11, 0x11 };
            string encoded = DataEncoding.Base32.Encode(bytes);
            encoded.Should().Be("CEIRC===");
        }

        [Test]
        public void Encode_with_4bytes()
        {
            byte[] bytes = { 0x11, 0x11, 0x11, 0x11 };
            string encoded = DataEncoding.Base32.Encode(bytes);
            encoded.Should().Be("CEIRCEI=");
        }

        [Test]
        public void Encode_with_5byte_quantum()
        {
            byte[] bytes = { 0x11, 0x11, 0x11, 0x11, 0x11 };
            string encoded = DataEncoding.Base32.Encode(bytes);
            encoded.Should().Be("CEIRCEIR");
        }

        [Test]
        public void Encode_with_6byte_quantum_plus1()
        {
            byte[] bytes = { 0x11, 0x11, 0x11, 0x11, 0x11, 0x11 };
            string encoded = DataEncoding.Base32.Encode(bytes);
            encoded.Should().Be("CEIRCEIRCE======");
        }

        [Test]
        public void Encode_with_7byte_quantum_plus2()
        {
            byte[] bytes = { 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11 };
            string encoded = DataEncoding.Base32.Encode(bytes);
            encoded.Should().Be("CEIRCEIRCEIQ====");
        }

        [Test]
        public void Encode_with_8byte_quantum_plus3()
        {
            byte[] bytes = { 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11 };
            string encoded = DataEncoding.Base32.Encode(bytes);
            encoded.Should().Be("CEIRCEIRCEIRC===");
        }

        [Test]
        public void Encode_with_9byte_quantum_plus4()
        {
            byte[] bytes = { 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11 };
            string encoded = DataEncoding.Base32.Encode(bytes);
            encoded.Should().Be("CEIRCEIRCEIRCEI=");
        }
    }
}