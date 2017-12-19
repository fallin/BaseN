using System;
using System.Text;
using BaseN.Encodings;
using FluentAssertions;
using NUnit.Framework;

namespace BaseN.Tests.Encodings
{
    [TestFixture]
    public class Base32Tests
    {
        [Test]
        public void Encode_guid()
        {
            string encoded = DataEncoding.Base32.Encode(SimpleTestCases.Uuid.ToByteArray());
            encoded.Should().Be("73SPFRGX7VRU3CRQ2LCE2RBQGQ======");
        }

        [Test]
        public void Encode_simple_string()
        {
            byte[] ascii = Encoding.ASCII.GetBytes(SimpleTestCases.SimpleString);
            string encoded = DataEncoding.Base32.Encode(ascii);
            encoded.Should().Be("ORUGS4ZANFZSAYJAORSXG5A=");
        }

        [Test]
        public void VerifyEncodingParameters()
        {
            DataEncoding.Base32.BitsPerQuantum.Should().Be(40);
            DataEncoding.Base32.EncodedCharsPerQuantum.Should().Be(8);
            DataEncoding.Base32.BitsPerEncodedChar.Should().Be(5);
        }

        [Test]
        public void Encode_quantum_minus4bytes()
        {
            byte[] bytes = { 0x11 };
            string encoded = DataEncoding.Base32.Encode(bytes);
            encoded.Should().Be("CE======");
        }

        [Test]
        public void Encode_quantum_minus3bytes()
        {
            byte[] bytes = { 0x11, 0x11 };
            string encoded = DataEncoding.Base32.Encode(bytes);
            encoded.Should().Be("CEIQ====");
        }

        [Test]
        public void Encode_quantum_minus2bytes()
        {
            byte[] bytes = { 0x11, 0x11, 0x11 };
            string encoded = DataEncoding.Base32.Encode(bytes);
            encoded.Should().Be("CEIRC===");
        }

        [Test]
        public void Encode_quantum_minus1byte()
        {
            byte[] bytes = { 0x11, 0x11, 0x11, 0x11 };
            string encoded = DataEncoding.Base32.Encode(bytes);
            encoded.Should().Be("CEIRCEI=");
        }

        [Test]
        public void Encode_quantum()
        {
            byte[] bytes = { 0x11, 0x11, 0x11, 0x11, 0x11 };
            string encoded = DataEncoding.Base32.Encode(bytes);
            encoded.Should().Be("CEIRCEIR");
        }

        [Test]
        public void Encode_quantum_plus1byte()
        {
            byte[] bytes = { 0x11, 0x11, 0x11, 0x11, 0x11, 0x11 };
            string encoded = DataEncoding.Base32.Encode(bytes);
            encoded.Should().Be("CEIRCEIRCE======");
        }

        [Test]
        public void Encode_quantum_plus2bytes()
        {
            byte[] bytes = { 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11 };
            string encoded = DataEncoding.Base32.Encode(bytes);
            encoded.Should().Be("CEIRCEIRCEIQ====");
        }

        [Test]
        public void Encode_quantum_plus3bytes()
        {
            byte[] bytes = { 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11 };
            string encoded = DataEncoding.Base32.Encode(bytes);
            encoded.Should().Be("CEIRCEIRCEIRC===");
        }

        [Test]
        public void Encode_quantum_plus4bytes()
        {
            byte[] bytes = { 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11 };
            string encoded = DataEncoding.Base32.Encode(bytes);
            encoded.Should().Be("CEIRCEIRCEIRCEI=");
        }
    }
}