using System;
using System.IO;
using System.Text;
using BaseN.Encoders;
using FluentAssertions;
using NUnit.Framework;

namespace BaseN.Tests.Encoders
{
    [TestFixture]
    public class DefaultDataEncoderTests
    {
        [Test]
        public void Encode_with_quantum_single_write()
        {
            var writer = new StringWriter();
            using (var encoder = new DefaultDataEncoder(DataEncoding.Base64, writer))
            {
                byte[] input1 = Encoding.UTF8.GetBytes("ABC");
                encoder.Encode(new ArraySegment<byte>(input1));
            }

            writer.ToString().Should().Be("QUJD");
        }

        [Test]
        public void Encode_with_quantum_across_multiple_writes()
        {
            var writer = new StringWriter();
            using (var encoder = new DefaultDataEncoder(DataEncoding.Base64, writer))
            {
                byte[] input1 = Encoding.UTF8.GetBytes("AB");
                encoder.Encode(new ArraySegment<byte>(input1));

                byte[] input2 = Encoding.UTF8.GetBytes("C");
                encoder.Encode(new ArraySegment<byte>(input2));
            }

            writer.ToString().Should().Be("QUJD");
        }

        [Test]
        public void Encode_with_quantum_minus1_single_write()
        {
            var writer = new StringWriter();
            using (var encoder = new DefaultDataEncoder(DataEncoding.Base64, writer))
            {
                byte[] input1 = Encoding.UTF8.GetBytes("AB");
                encoder.Encode(new ArraySegment<byte>(input1));
            }

            writer.ToString().Should().Be("QUI=");
        }

        [Test]
        public void Encode_with_quantum_minus2_across_multiple_writes()
        {
            var writer = new StringWriter();
            using (var encoder = new DefaultDataEncoder(DataEncoding.Base64, writer))
            {
                byte[] input1 = Encoding.UTF8.GetBytes("A");
                encoder.Encode(new ArraySegment<byte>(input1));

                byte[] input2 = Encoding.UTF8.GetBytes("B");
                encoder.Encode(new ArraySegment<byte>(input2));
            }

            writer.ToString().Should().Be("QUI=");
        }

        [Test]
        public void Flush_should_handle_when_called_before_dispose()
        {
            var writer = new StringWriter();
            using (var encoder = new DefaultDataEncoder(DataEncoding.Base64, writer))
            {
                byte[] input1 = Encoding.UTF8.GetBytes("abcdefghijk");
                encoder.Encode(new ArraySegment<byte>(input1));

                encoder.Flush();
            }

            writer.ToString().Should().Be("YWJjZGVmZ2hpams=");
        }

        [Test]
        public void Encode_should_resume_correctly_after_flush()
        {
            var writer = new StringWriter();
            using (var encoder = new DefaultDataEncoder(DataEncoding.Base64, writer))
            {
                byte[] input1 = Encoding.UTF8.GetBytes("abcde");
                encoder.Encode(new ArraySegment<byte>(input1));

                encoder.Flush();

                byte[] input2 = Encoding.UTF8.GetBytes("fghijk");
                encoder.Encode(new ArraySegment<byte>(input2));
            }

            writer.ToString().Should().Be("YWJjZGVmZ2hpams=");
        }
    }
}

