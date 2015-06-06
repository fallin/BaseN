using System;
using System.Text;
using BaseN.Encodings;
using FluentAssertions;
using NUnit.Framework;

namespace BaseN.Tests.Encodings.HeXuYue
{
    [TestFixture]
    public class Base62EncoderTests
    {
        const string SimpleString = "this is a test";

        [Test]
        public void Base62Encode_whitepaper_use_case()
        {
            // See http://www.opitz-online.com/dl/base62_encoding.pdf, Fig. 2, pg. 763
            byte[] bytes = { 0x53, 0xFE, 0x92 };

            var encoder = new Base62Encoding();
            string encoded = encoder.Encode(bytes);
            encoded.Should().Be("U98kC");
        }

        [Test]
        public void Base62Encode_with_simple_string()
        {
            byte[] bytes = Encoding.UTF8.GetBytes(SimpleString);

            var encoder = new Base62Encoding();
            string encoded = encoder.Encode(bytes);
            encoded.Should().Be("dGhpcyBpcyBhIHRlc3E");
        }

//        [Test]
//        public void Base62Encode_with_simple_string()
//        {
//            byte[] ascii = Encoding.ASCII.GetBytes(SimpleString);
//
//            var encoder = new Base62Encoding();
//            string encoded = encoder.Encode(ascii);
//
//            // http://www.opitz-online.com/base62/title/base62-encoder-decoder
//            //encoded.Should().Be("dGhpcyBpcyBhIHRlc3Ef");
//
//            // http://encode-base62.nichabi.com/?input=this+is+a+test
//            //encoded.Should().Be("9zdRwc5uxcmUK8GlIF2");
//        }
    }
}