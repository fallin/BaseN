using System;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace BaseN.Tests.Encodings
{
    [TestFixture]
    public class ModHexTests
    {
        [Test]
        public void Encode_simple_string()
        {
            byte[] ascii = Encoding.ASCII.GetBytes(SimpleTestCases.SimpleString);
            string encoded = DataEncoding.ModHex.Encode(ascii);
            encoded.Should().Be("ifhjhkiedchkiedchbdcifhgieif");
        }

        [Test]
        public void VerifyEncodingParameters()
        {
            DataEncoding.ModHex.BitsPerQuantum.Should().Be(8);
            DataEncoding.ModHex.EncodedCharsPerQuantum.Should().Be(2);
            DataEncoding.ModHex.BitsPerEncodedChar.Should().Be(4);
        }
    }
}