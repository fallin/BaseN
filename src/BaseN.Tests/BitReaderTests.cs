using System;
using System.IO;
using FluentAssertions;
using NUnit.Framework;

namespace BaseN.Tests
{
    [TestFixture]
    public class BitReaderTests
    {
        [Test]
        public void ReadBits_returns_negative_one_when_end_of_stream()
        {
            byte[] input = { };
            Stream inStream = new MemoryStream(input);

            var bitio = new BitReader(inStream);

            byte value;
            bitio.ReadBits(8, out value).Should().Be(0);
            value.Should().Be(0);
        }

        [Test]
        public void ReadBits_can_read_0bits()
        {
            byte[] input = { 0x55 };
            Stream inStream = new MemoryStream(input);

            var bitio = new BitReader(inStream);

            byte value;
            bitio.ReadBits(0, out value).Should().Be(0);
            value.Should().Be(0x00);
        }

        [Test]
        public void ReadBits_can_read_6bits()
        {
            byte[] input = { 0x55 };
            Stream inStream = new MemoryStream(input);

            var bitio = new BitReader(inStream);

            byte value;
            bitio.ReadBits(6, out value).Should().Be(6);
            value.Should().Be(0x15);
        }

        [Test]
        public void ReadBits_can_read_8bits()
        {
            byte[] input = { 0x55 };
            Stream inStream = new MemoryStream(input);

            var bitio = new BitReader(inStream);

            byte value;
            bitio.ReadBits(8, out value).Should().Be(8);
            value.Should().Be(0x55);
        }

        [Test]
        public void ReadBits_one_byte_read_in_3bit_chunks()
        {
            //  octet = 01000001 = 0x41
            //  chucked by 3 bits: 010 000 01-

            byte[] input = { 0x41 };
            Stream inStream = new MemoryStream(input);

            var bitio = new BitReader(inStream);

            byte value;
            bitio.ReadBits(3, out value).Should().Be(3);
            value.Should().Be(0x02);

            bitio.ReadBits(3, out value).Should().Be(3);
            value.Should().Be(0x00);

            bitio.ReadBits(3, out value).Should().Be(2);
            value.Should().Be(0x02);
        }

        [Test]
        public void ReadBits_two_bytes_read_in_3bit_chunks()
        {
            //  octet = 01000001 01000001 = 0x4141
            //  chucked by 3 bits: 010 000 010 100 000 1--

            byte[] input = { 0x41, 0x41 };
            Stream inStream = new MemoryStream(input);

            var bitio = new BitReader(inStream);

            byte value;
            bitio.ReadBits(3, out value).Should().Be(3);
            value.Should().Be(0x02);

            bitio.ReadBits(3, out value).Should().Be(3);
            value.Should().Be(0x00);

            bitio.ReadBits(3, out value).Should().Be(3);
            value.Should().Be(0x02);

            bitio.ReadBits(3, out value).Should().Be(3);
            value.Should().Be(0x04);

            bitio.ReadBits(3, out value).Should().Be(3);
            value.Should().Be(0x00);

            bitio.ReadBits(3, out value).Should().Be(1);
            value.Should().Be(0x04);
        }

        [Test]
        public void ReadBits_two_bytes_read_in_8bit_chunks()
        {
            //  octet = 01000001 01000001 = 0x4141
            //  chucked by 8 bits: 01000001 01000001

            byte[] input = { 0x41, 0x41 };
            Stream inStream = new MemoryStream(input);

            var bitio = new BitReader(inStream);

            byte value;
            bitio.ReadBits(8, out value).Should().Be(8);
            value.Should().Be(0x41);

            bitio.ReadBits(8, out value).Should().Be(8);
            value.Should().Be(0x41);

            bitio.ReadBits(8, out value).Should().Be(0);
            value.Should().Be(0x00);
        }

        [Test]
        public void Seek_backwards_within_current_byte()
        {
            //  octet = 01000001 01000001 = 0x4141

            byte[] input = { 0x41, 0x41 };
            Stream inStream = new MemoryStream(input);

            var bitio = new BitReader(inStream);

            byte value;
            bitio.ReadBits(4, out value).Should().Be(4);
            value.Should().Be(0x04);
            //          ++++
            //  octet = 01000001 01000001 = 0x4141
            //              ^ position
            bitio.Position.Should().Be(4);

            bitio.Seek(-3, SeekOrigin.Current).Should().Be(1).And.Be(bitio.Position);
            //  octet = 01000001 01000001 = 0x4141
            //           ^ position

            bitio.ReadBits(4, out value).Should().Be(4);
            value.Should().Be(0x08);
            //           ++++
            //  octet = 01000001 01000001 = 0x4141
            //               ^ position
        }

        [Test]
        public void Seek_forwards_within_current_byte()
        {
            //  octet = 01000001 01000001 = 0x4141

            byte[] input = { 0x41, 0x41 };
            Stream inStream = new MemoryStream(input);

            var bitio = new BitReader(inStream);

            byte value;
            bitio.ReadBits(4, out value).Should().Be(4);
            value.Should().Be(0x04);
            //          ++++
            //  octet = 01000001 01000001 = 0x4141
            //              ^ position
            bitio.Position.Should().Be(4);

            bitio.Seek(3, SeekOrigin.Current).Should().Be(7).And.Be(bitio.Position);
            //  octet = 01000001 01000001 = 0x4141
            //                 ^ position

            bitio.ReadBits(4, out value).Should().Be(4);
            value.Should().Be(0x0A);
            //                 + +++
            //  octet = 01000001 01000001 = 0x4141
            //                      ^ position
        }

        [Test]
        public void Seek_backwards_beyond_current_byte()
        {
            //  octet = 01000001 01000001 = 0x4141

            byte[] input = { 0x41, 0x41 };
            Stream inStream = new MemoryStream(input);

            var bitio = new BitReader(inStream);

            byte value;
            bitio.ReadBits(4, out value).Should().Be(4);
            //          ++++
            //  octet = 01000001 01000001 = 0x4141
            //              ^ position
            bitio.ReadBits(4, out value).Should().Be(4);
            //              ++++
            //  octet = 01000001 01000001 = 0x4141
            //                   ^ position
            bitio.ReadBits(4, out value).Should().Be(4);
            //                   ++++
            //  octet = 01000001 01000001 = 0x4141
            //                       ^ position
            bitio.Position.Should().Be(12);

            bitio.Seek(-7, SeekOrigin.Current).Should().Be(5).And.Be(bitio.Position);
            //  octet = 01000001 01000001 = 0x4141
            //               ^ position

            // NOTE: Not only does the position need to be adjusted, but the bit-reader
            // must also make sure the "current" byte is correct to reflect the current
            // position
            bitio.ReadBits(4, out value).Should().Be(4);
            value.Should().Be(0x02);
            //               +++ +
            //  octet = 01000001 01000001 = 0x4141
            //                    ^ position
        }

        [Test]
        public void Seek_forwards_beyond_current_byte()
        {
            //  octet = 01000001 01000001 = 0x4141

            byte[] input = { 0x41, 0x41 };
            Stream inStream = new MemoryStream(input);

            var bitio = new BitReader(inStream);

            byte value;
            bitio.ReadBits(4, out value).Should().Be(4);
            //          ++++
            //  octet = 01000001 01000001 = 0x4141
            //              ^ position

            bitio.Seek(+7, SeekOrigin.Current).Should().Be(11).And.Be(bitio.Position);
            //  octet = 01000001 01000001 = 0x4141
            //                      ^ position

            // NOTE: Not only does the position need to be adjusted, but the bit-reader
            // must also make sure the "current" byte is correct to reflect the current
            // position
            bitio.ReadBits(5, out value).Should().Be(5);
            value.Should().Be(0x01);
            //                      +++++
            //  octet = 01000001 01000001 = 0x4141
            //                           ^ position
        }

        [Test]
        public void Seek_to_beginning()
        {
            //  octet = 01001001 01001101 = 0x494D

            byte[] input = { 0x49, 0x4D };
            Stream inStream = new MemoryStream(input);

            var bitio = new BitReader(inStream);

            byte value;
            bitio.ReadBits(4, out value).Should().Be(4);
            //          ++++
            //  octet = 01001001 01001101
            //              ^ position

            bitio.Seek(0, SeekOrigin.Begin).Should().Be(0).And.Be(bitio.Position);
            //  octet = 01001001 01001101 = 0x4141
            //          ^ position

            // NOTE: Not only does the position need to be adjusted, but the bit-reader
            // must also make sure the "current" byte is correct to reflect the current
            // position
            bitio.ReadBits(3, out value).Should().Be(3);
            value.Should().Be(0x02);
            //          +++
            //  octet = 01001001 01001101
            //             ^ position
        }

        [Test]
        public void Seek_relative_to_beginning_within_first_byte()
        {
            //  octet = 01001001 01001101 = 0x494D

            byte[] input = { 0x49, 0x4D };
            Stream inStream = new MemoryStream(input);

            var bitio = new BitReader(inStream);

            byte value;
            bitio.ReadBits(4, out value).Should().Be(4);
            //          ++++
            //  octet = 01001001 01001101
            //              ^ position

            bitio.Seek(1, SeekOrigin.Begin).Should().Be(1).And.Be(bitio.Position);
            //  octet = 01001001 01001101 = 0x4141
            //           ^ position

            // NOTE: Not only does the position need to be adjusted, but the bit-reader
            // must also make sure the "current" byte is correct to reflect the current
            // position
            bitio.ReadBits(3, out value).Should().Be(3);
            value.Should().Be(0x04);
            //           +++
            //  octet = 01001001 01001101
            //              ^ position
        }

        [Test]
        public void Seek_relative_to_beginning_within_second_byte()
        {
            //  octet = 01001001 01001101 = 0x494D

            byte[] input = { 0x49, 0x4D };
            Stream inStream = new MemoryStream(input);

            var bitio = new BitReader(inStream);

            byte value;
            bitio.ReadBits(4, out value).Should().Be(4);
            //          ++++
            //  octet = 01001001 01001101
            //              ^ position

            bitio.Seek(12, SeekOrigin.Begin).Should().Be(12).And.Be(bitio.Position);
            //  octet = 01001001 01001101 = 0x4141
            //                       ^ position

            // NOTE: Not only does the position need to be adjusted, but the bit-reader
            // must also make sure the "current" byte is correct to reflect the current
            // position
            bitio.ReadBits(3, out value).Should().Be(3);
            value.Should().Be(0x06);
            //                       +++
            //  octet = 01001001 01001101
            //                          ^ position
        }

        [Test]
        public void Seek_relative_to_end_within_last_byte()
        {
            //  octet = 01001001 01001101 = 0x494D

            byte[] input = { 0x49, 0x4D };
            Stream inStream = new MemoryStream(input);

            var bitio = new BitReader(inStream);

            byte value;
            bitio.ReadBits(4, out value).Should().Be(4);
            //          ++++
            //  octet = 01001001 01001101
            //              ^ position

            bitio.Seek(-3, SeekOrigin.End).Should().Be(12).And.Be(bitio.Position);
            //  octet = 01001001 01001101 = 0x4141
            //                       ^ position

            // NOTE: Not only does the position need to be adjusted, but the bit-reader
            // must also make sure the "current" byte is correct to reflect the current
            // position
            bitio.ReadBits(3, out value).Should().Be(3);
            value.Should().Be(0x06);
            //                       +++
            //  octet = 01001001 01001101
            //                          ^ position
        }

        [Test]
        public void Seek_relative_to_end_within_second_to_last_byte()
        {
            //  octet = 01001001 01001101 = 0x494D

            byte[] input = { 0x49, 0x4D };
            Stream inStream = new MemoryStream(input);

            var bitio = new BitReader(inStream);

            byte value;
            bitio.ReadBits(4, out value).Should().Be(4);
            //          ++++
            //  octet = 01001001 01001101
            //              ^ position

            bitio.Seek(-11, SeekOrigin.End).Should().Be(4).And.Be(bitio.Position);
            //  octet = 01001001 01001101 = 0x4141
            //              ^ position

            // NOTE: Not only does the position need to be adjusted, but the bit-reader
            // must also make sure the "current" byte is correct to reflect the current
            // position
            bitio.ReadBits(3, out value).Should().Be(3);
            value.Should().Be(0x04);
            //              +++
            //  octet = 01001001 01001101
            //              ^ position
        }

        [Test]
        public void Seek_to_end()
        {
            //  octet = 01001001 01001101 = 0x494D

            byte[] input = { 0x49, 0x4D };
            Stream inStream = new MemoryStream(input);

            var bitio = new BitReader(inStream);

            byte value;
            bitio.ReadBits(4, out value).Should().Be(4);
            //          ++++
            //  octet = 01001001 01001101
            //              ^ position

            bitio.Seek(0, SeekOrigin.End).Should().Be(15).And.Be(bitio.Position);
            //  octet = 01001001 01001101 = 0x4141
            //                          ^ position

            // NOTE: Not only does the position need to be adjusted, but the bit-reader
            // must also make sure the "current" byte is correct to reflect the current
            // position
            bitio.ReadBits(3, out value).Should().Be(1);
            value.Should().Be(0x04);
            //                          +++
            //  octet = 01001001 01001101
            //                             ^ position
        }

        [Test]
        public void Length_with_zero_bytes_is_zero_bits()
        {
            byte[] input = { };
            Stream inStream = new MemoryStream(input);

            var bitio = new BitReader(inStream);
            bitio.Length.Should().Be(0);
        }

        [Test]
        public void Length_with_one_byte_is_8bits()
        {
            byte[] input = { 0x00 };
            Stream inStream = new MemoryStream(input);

            var bitio = new BitReader(inStream);
            bitio.Length.Should().Be(8);
        }
    }
}