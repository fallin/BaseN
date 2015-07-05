using System;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace BaseN.Tests.Encodings
{
    [TestFixture]
    public class Base64UrlDataEncodingTests
    {
        [Test]
        public void Encode_with_guid()
        {
            string encoded = DataEncoding.Base64Url.Encode(SimpleTestCases.Uuid.ToByteArray());
            encoded.Should().Be("_uTyxNf9Y02KMNLETUQwNA==");
        }

        [Test]
        public void Encode_with_simple_string()
        {
            byte[] ascii = Encoding.ASCII.GetBytes(SimpleTestCases.SimpleString);
            string encoded = DataEncoding.Base64Url.Encode(ascii);
            encoded.Should().Be("dGhpcyBpcyBhIHRlc3Q=");
        }
    }
}