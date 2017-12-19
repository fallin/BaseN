using System;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace BaseN.Tests.Encodings
{
    [TestFixture]
    public class Base16Tests
    {
        [Test]
        public void Encode_guid()
        {
            string encoded = DataEncoding.Base16.Encode(SimpleTestCases.Uuid.ToByteArray());
            encoded.Should().Be("FEE4F2C4D7FD634D8A30D2C44D443034");
        }

        [Test]
        public void Encode_simple_string()
        {
            byte[] ascii = Encoding.ASCII.GetBytes(SimpleTestCases.SimpleString);
            string encoded = DataEncoding.Base16.Encode(ascii);
            encoded.Should().Be("7468697320697320612074657374");
        }

        [Test]
        public void VerifyEncodingParameters()
        {
            DataEncoding.Base16.BitsPerQuantum.Should().Be(8);
            DataEncoding.Base16.EncodedCharsPerQuantum.Should().Be(2);
            DataEncoding.Base16.BitsPerEncodedChar.Should().Be(4);
        }
    }
}