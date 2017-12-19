using System;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace BaseN.Tests.Encodings
{
    [TestFixture]
    public class Base64UrlTests
    {
        [Test]
        public void Encode_guid()
        {
            string encoded = DataEncoding.Base64Url.Encode(SimpleTestCases.Uuid.ToByteArray());
            encoded.Should().Be("_uTyxNf9Y02KMNLETUQwNA==");
        }

        [Test]
        public void Encode_simple_string()
        {
            byte[] ascii = Encoding.ASCII.GetBytes(SimpleTestCases.SimpleString);
            string encoded = DataEncoding.Base64Url.Encode(ascii);
            encoded.Should().Be("dGhpcyBpcyBhIHRlc3Q=");
        }

        [Test]
        public void VerifyEncodingParameters()
        {
            DataEncoding.Base64Url.BitsPerQuantum.Should().Be(24);
            DataEncoding.Base64Url.EncodedCharsPerQuantum.Should().Be(4);
            DataEncoding.Base64Url.BitsPerEncodedChar.Should().Be(6);
        }
    }
}