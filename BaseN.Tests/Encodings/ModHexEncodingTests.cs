using System;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace BaseN.Tests.Encodings
{
    [TestFixture]
    public class ModHexEncodingTests
    {
        [Test]
        public void Encode_with_simple_string()
        {
            byte[] ascii = Encoding.ASCII.GetBytes(SimpleTestCases.SimpleString);
            string encoded = BaseEncoding.ModHex.Encode(ascii);
            encoded.Should().Be("ifhjhkiedchkiedchbdcifhgieif");
        }
    }
}