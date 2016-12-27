using System;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace BaseN.Tests.Encodings
{
    [TestFixture]
    public class Base64DataEncodingTests
    {
        [Test]
        public void Encode_guid()
        {
            string encoded = DataEncoding.Base64.Encode(SimpleTestCases.Uuid.ToByteArray());
            encoded.Should().Be("/uTyxNf9Y02KMNLETUQwNA==");
        }

        [Test]
        public void Encode_simple_string()
        {
            byte[] ascii = Encoding.ASCII.GetBytes(SimpleTestCases.SimpleString);
            string encoded = DataEncoding.Base64.Encode(ascii);
            encoded.Should().Be("dGhpcyBpcyBhIHRlc3Q=");
        }

        [Test]
        public void Encode_quantum()
        {
            byte[] ascii = Encoding.ASCII.GetBytes("ABC");
            string encoded = DataEncoding.Base64.Encode(ascii);
            encoded.Should().Be("QUJD");
        }

        [Test]
        public void Encode_quantum_minus1()
        {
            byte[] ascii = Encoding.ASCII.GetBytes("AB");
            string encoded = DataEncoding.Base64.Encode(ascii);
            encoded.Should().Be("QUI=");
        }

        [Test]
        public void Encode_quantum_minus2()
        {
            byte[] ascii = Encoding.ASCII.GetBytes("A");
            string encoded = DataEncoding.Base64.Encode(ascii);
            encoded.Should().Be("QQ==");
        }

        [Test]
        public void Encode_empty_string()
        {
            byte[] ascii = Encoding.ASCII.GetBytes("");
            string encoded = DataEncoding.Base64.Encode(ascii);
            encoded.Should().Be("");
        }

        [Test]
        public void Encode_should_throw_when_null_string()
        {
            Action action = () => DataEncoding.Base64.Encode(null);
            action.ShouldThrow<ArgumentNullException>();
        }
    }
}