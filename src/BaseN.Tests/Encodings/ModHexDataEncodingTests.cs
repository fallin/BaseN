using System;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace BaseN.Tests.Encodings
{
    [TestFixture]
    public class ModHexDataEncodingTests
    {
        [Test]
        public void Encode_with_simple_string()
        {
            byte[] ascii = Encoding.ASCII.GetBytes(SimpleTestCases.SimpleString);
            string encoded = DataEncoding.ModHex.Encode(ascii);
            encoded.Should().Be("ifhjhkiedchkiedchbdcifhgieif");
        }
    }
}