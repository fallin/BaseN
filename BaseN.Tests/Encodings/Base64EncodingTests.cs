using System;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace BaseN.Tests.Encodings
{
    [TestFixture]
    public class Base64EncodingTests
    {
        [Test]
        public void Encode_with_guid()
        {
            string encoded = BaseEncoding.Base64.Encode(SimpleTestCases.Uuid.ToByteArray());
            encoded.Should().Be("/uTyxNf9Y02KMNLETUQwNA==");
        }

        [Test]
        public void Encode_with_simple_string()
        {
            byte[] ascii = Encoding.ASCII.GetBytes(SimpleTestCases.SimpleString);
            string encoded = BaseEncoding.Base64.Encode(ascii);
            encoded.Should().Be("dGhpcyBpcyBhIHRlc3Q=");
        }

        [Test]
        public void Encode_with_quantum()
        {
            byte[] ascii = Encoding.ASCII.GetBytes("ABC");
            string encoded = BaseEncoding.Base64.Encode(ascii);
            encoded.Should().Be("QUJD");
        }

        [Test]
        public void Encode_with_quantum_minus1()
        {
            byte[] ascii = Encoding.ASCII.GetBytes("AB");
            string encoded = BaseEncoding.Base64.Encode(ascii);
            encoded.Should().Be("QUI=");
        }

        [Test]
        public void Encode_with_quantum_minus2()
        {
            byte[] ascii = Encoding.ASCII.GetBytes("A");
            string encoded = BaseEncoding.Base64.Encode(ascii);
            encoded.Should().Be("QQ==");
        }

        [Test]
        public void Encode_with_empty_string()
        {
            byte[] ascii = Encoding.ASCII.GetBytes("");
            string encoded = BaseEncoding.Base64.Encode(ascii);
            encoded.Should().Be("");
        }

        [Test]
        public void Encode_with_null_string()
        {
            Action action = () => BaseEncoding.Base64.Encode(null);
            action.ShouldThrow<ArgumentNullException>();
        }
    }
}