using System;
using System.Text;
using BaseN.Encodings;
using FluentAssertions;
using NUnit.Framework;

namespace BaseN.Tests.Encodings
{
    [TestFixture]
    public class Base62EncodingTests
    {
        [Test]
        public void Encode_whitepaper_use_case()
        {
            // See http://www.opitz-online.com/dl/base62_encoding.pdf, Fig. 2, pg. 763
            byte[] bytes = { 0x53, 0xFE, 0x92 };

            var encoder = new Base62Encoding();
            string encoded = encoder.Encode(bytes);
            encoded.Should().Be("U98kC");
        }

        [Test]
        public void Encode_with_simple_string()
        {
            byte[] bytes = Encoding.UTF8.GetBytes(SimpleTestCases.SimpleString);

            var encoder = new Base62Encoding();
            string encoded = encoder.Encode(bytes);
            encoded.Should().Be("dGhpcyBpcyBhIHRlc3E");
        }
    }
}