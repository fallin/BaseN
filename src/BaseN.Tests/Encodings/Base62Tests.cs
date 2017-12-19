using System;
using System.Text;
using BaseN.Encodings;
using FluentAssertions;
using NUnit.Framework;

namespace BaseN.Tests.Encodings
{
    [TestFixture]
    public class Base62Tests
    {
        [Test]
        public void Encode_whitepaper_use_case()
        {
            // See http://www.opitz-online.com/dl/base62_encoding.pdf, Fig. 2, pg. 763
            byte[] bytes = { 0x53, 0xFE, 0x92 };

            string encoded = DataEncoding.Base62.Encode(bytes);
            encoded.Should().Be("U98kC");
        }

        [Test]
        public void Encode_simple_string()
        {
            byte[] bytes = Encoding.UTF8.GetBytes(SimpleTestCases.SimpleString);

            string encoded = DataEncoding.Base62.Encode(bytes);
            encoded.Should().Be("dGhpcyBpcyBhIHRlc3E");
        }

        [Test]
        public void VerifyEncodingParameters()
        {
            DataEncoding.Base62.BitsPerQuantum.Should().Be(24);
            DataEncoding.Base62.EncodedCharsPerQuantum.Should().Be(4);
            DataEncoding.Base62.BitsPerEncodedChar.Should().Be(6);
        }
    }
}