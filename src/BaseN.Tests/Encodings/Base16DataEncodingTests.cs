using System;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace BaseN.Tests.Encodings
{
    [TestFixture]
    public class Base16DataEncodingTests
    {
        [Test]
        public void Encode_with_guid()
        {
            string encoded = DataEncoding.Base16.Encode(SimpleTestCases.Uuid.ToByteArray());
            encoded.Should().Be("FEE4F2C4D7FD634D8A30D2C44D443034");
        }

        [Test]
        public void Encode_with_simple_string()
        {
            byte[] ascii = Encoding.ASCII.GetBytes(SimpleTestCases.SimpleString);
            string encoded = DataEncoding.Base16.Encode(ascii);
            encoded.Should().Be("7468697320697320612074657374");
        }
    }
}