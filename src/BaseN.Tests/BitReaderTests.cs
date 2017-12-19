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
        public void Readbits_should_return_zero_when_stream_is_empty()
        {
            byte[] bytes = { };

            using (var reader = new BitReader(bytes))
            {
                byte value;
                reader.ReadBits(8, out value).Should().Be(0);
                value.Should().Be(0);
            }
        }

        [Test]
        public void ReadBits_should_return_actual_bits_read_when_reaching_endofstream()
        {
            byte[] bytes = { 0x55 };
            using (var reader = new BitReader(bytes))
            {
                byte b1;
                int bitsRead1 = reader.ReadBits(5, out b1);
                // 01010101 --------
                // ^^^^^    ~~> 00001010 = 0x0A
                b1.Should().Be(0x0A);
                bitsRead1.Should().Be(5);

                byte b2;
                int bitsRead2 = reader.ReadBits(5, out b2);
                // 01010101 --------
                //      ^^^ ^^
                // Possible alternatives:
                // 1. 00000101 = 0x05 (right-aligned)
                //         ^^^
                // 2. 00010100 = 0x14 (zero padded)
                //       ^^^^^
                // Using option #2
                b2.Should().Be(0x14);
                bitsRead2.Should().Be(3);
            }
        }

        [Test]
        public void ReadBits_should_return_zero_when_reading_beyond_endofstream()
        {
            byte[] bytes = { 0x55 };
            using (var reader = new BitReader(bytes))
            {
                byte b1;
                int bitsRead1 = reader.ReadBits(5, out b1);
                // 01010101 --------
                // ^^^^^    ~~> 00001010 = 0x0A
                b1.Should().Be(0x0A);
                bitsRead1.Should().Be(5);

                byte b2;
                int bitsRead2 = reader.ReadBits(5, out b2);
                // 01010101 -------- (reach end of stream)
                //      ^^^ ^^ ~~> 00010100 = 0x14
                b2.Should().Be(0x14);
                bitsRead2.Should().Be(3);

                byte b3;
                int bitsRead3 = reader.ReadBits(5, out b3);
                // 01010101 -------- (beyond end of stream)
                //            ^^^^^^ ~~> 0x00
                b3.Should().Be(0x00);
                bitsRead3.Should().Be(0);
            }
        }

        [Test]
        public void ReadBits_should_return_actual_bits_read()
        {
            byte[] bytes = { 0x55, 0xAA };
            using (var reader = new BitReader(bytes))
            {
                byte b1;
                int bitsRead1 = reader.ReadBits(5, out b1);
                bitsRead1.Should().Be(5);

                byte b2;
                int bitsRead2 = reader.ReadBits(4, out b2);
                bitsRead2.Should().Be(4);

                byte b3;
                int bitsRead3 = reader.ReadBits(2, out b3);
                bitsRead3.Should().Be(2);
            }
        }

        [Test]
        public void ReadBits_can_read_0bits()
        {
            byte[] bytes = { 0x55 };

            using (var reader = new BitReader(bytes))
            {
                byte value;
                reader.ReadBits(0, out value).Should().Be(0);
                value.Should().Be(0x00);
            }
        }

        [Test]
        public void ReadBits_can_read_6bits()
        {
            byte[] bytes = { 0x55 };

            using (var reader = new BitReader(bytes))
            {
                byte value;
                reader.ReadBits(6, out value).Should().Be(6);
                value.Should().Be(0x15);
            }
        }

        [Test]
        public void ReadBits_can_read_8bits()
        {
            byte[] bytes = { 0x55 };

            using (var reader = new BitReader(bytes))
            {
                byte value;
                reader.ReadBits(8, out value).Should().Be(8);
                value.Should().Be(0x55);
            }
        }

        [Test]
        public void ReadBits_can_read_1byte_in_3bit_chunks()
        {
            //  octet = 01000001 = 0x41
            //  chucked by 3 bits: 010 000 01-

            byte[] bytes = { 0x41 };

            using (var reader = new BitReader(bytes))
            {
                byte value;
                reader.ReadBits(3, out value).Should().Be(3);
                value.Should().Be(0x02);

                reader.ReadBits(3, out value).Should().Be(3);
                value.Should().Be(0x00);

                reader.ReadBits(3, out value).Should().Be(2);
                value.Should().Be(0x02);
            }
        }

        [Test]
        public void ReadBits_can_read_2bytes_in_3bit_chunks()
        {
            //  octet = 01000001 01000001 = 0x4141
            //  chucked by 3 bits: 010 000 010 100 000 1--

            byte[] bytes = { 0x41, 0x41 };

            using (var reader = new BitReader(bytes))
            {
                byte value;
                reader.ReadBits(3, out value).Should().Be(3);
                value.Should().Be(0x02);

                reader.ReadBits(3, out value).Should().Be(3);
                value.Should().Be(0x00);

                reader.ReadBits(3, out value).Should().Be(3);
                value.Should().Be(0x02);

                reader.ReadBits(3, out value).Should().Be(3);
                value.Should().Be(0x04);

                reader.ReadBits(3, out value).Should().Be(3);
                value.Should().Be(0x00);

                reader.ReadBits(3, out value).Should().Be(1);
                value.Should().Be(0x04);
            }
        }

        [Test]
        public void ReadBits_can_read_2bytes_in_8bit_chunks()
        {
            //  octet = 01000001 01000001 = 0x4141
            //  chucked by 8 bits: 01000001 01000001

            byte[] bytes = { 0x41, 0x41 };

            using (var reader = new BitReader(bytes))
            {
                byte value;
                reader.ReadBits(8, out value).Should().Be(8);
                value.Should().Be(0x41);

                reader.ReadBits(8, out value).Should().Be(8);
                value.Should().Be(0x41);

                reader.ReadBits(8, out value).Should().Be(0);
                value.Should().Be(0x00);
            }
        }

        [Test]
        public void ReadBits_can_read_successive_5bits()
        {
            byte[] bytes = { 0x55, 0xAA };
            using (var reader = new BitReader(bytes))
            {
                byte b1;
                reader.ReadBits(5, out b1);
                // 01010101 10101010
                // ^^^^^             ~~> 00001010 = 0x0A
                b1.Should().Be(0x0A);

                byte b2;
                reader.ReadBits(5, out b2);
                // 01010101 10101010
                //      ^^^ ^^       ~~> 00010110 = 0x16
                b2.Should().Be(0x16);
            }
        }

        [Test]
        public void ReadBits_can_read_successive_8bits()
        {
            byte[] bytes = { 0x55, 0xAA };
            using (var reader = new BitReader(bytes))
            {
                byte b1;
                reader.ReadBits(8, out b1);
                // 01010101 10101010
                // ^^^^^^^^          ~~> 01010101 = 0x55
                b1.Should().Be(0x55);

                byte b2;
                reader.ReadBits(8, out b2);
                // 01010101 10101010
                //          ^^^^^^^^ ~~> 10101010 = 0xAA
                b2.Should().Be(0xAA);
            }
        }

        [Test]
        public void ReadBits_can_read_successive_5bits_4bits_2bits()
        {
            byte[] bytes = { 0x55, 0xAA };
            using (var reader = new BitReader(bytes))
            {
                byte b1;
                reader.ReadBits(5, out b1);
                // 01010101 10101010
                // ^^^^^             ~~> 00001010 = 0x0A
                b1.Should().Be(0x0A);

                byte b2;
                reader.ReadBits(4, out b2);
                // 01010101 10101010
                //      ^^^ ^        ~~> 00001011 = 0x0B
                b2.Should().Be(0x0B);

                byte b3;
                reader.ReadBits(2, out b3);
                // 01010101 10101010
                //           ^^      ~~> 00000001 = 0x01
                b3.Should().Be(0x01);
            }
        }

        [Test]
        public void Seek_backwards_within_current_byte()
        {
            //  octet = 01000001 01000001 = 0x4141

            byte[] bytes = { 0x41, 0x41 };

            using (var reader = new BitReader(bytes))
            {
                byte value;
                reader.ReadBits(4, out value).Should().Be(4);
                value.Should().Be(0x04);
                //          ++++
                //  octet = 01000001 01000001 = 0x4141
                //              ^ position
                reader.Position.Should().Be(4);

                reader.Seek(-3, SeekOrigin.Current).Should().Be(1).And.Be(reader.Position);
                //  octet = 01000001 01000001 = 0x4141
                //           ^ position

                reader.ReadBits(4, out value).Should().Be(4);
                value.Should().Be(0x08);
                //           ++++
                //  octet = 01000001 01000001 = 0x4141
                //               ^ position
            }
        }

        [Test]
        public void Seek_forwards_within_current_byte()
        {
            //  octet = 01000001 01000001 = 0x4141

            byte[] bytes = { 0x41, 0x41 };

            using (var reader = new BitReader(bytes))
            {
                byte value;
                reader.ReadBits(4, out value).Should().Be(4);
                value.Should().Be(0x04);
                //          ++++
                //  octet = 01000001 01000001 = 0x4141
                //              ^ position
                reader.Position.Should().Be(4);

                reader.Seek(3, SeekOrigin.Current).Should().Be(7).And.Be(reader.Position);
                //  octet = 01000001 01000001 = 0x4141
                //                 ^ position

                reader.ReadBits(4, out value).Should().Be(4);
                value.Should().Be(0x0A);
                //                 + +++
                //  octet = 01000001 01000001 = 0x4141
                //                      ^ position
            }
        }

        [Test]
        public void Seek_backwards_beyond_current_byte()
        {
            //  octet = 01000001 01000001 = 0x4141

            byte[] bytes = { 0x41, 0x41 };

            using (var reader = new BitReader(bytes))
            {
                byte value;
                reader.ReadBits(4, out value).Should().Be(4);
                //          ++++
                //  octet = 01000001 01000001 = 0x4141
                //              ^ position
                reader.ReadBits(4, out value).Should().Be(4);
                //              ++++
                //  octet = 01000001 01000001 = 0x4141
                //                   ^ position
                reader.ReadBits(4, out value).Should().Be(4);
                //                   ++++
                //  octet = 01000001 01000001 = 0x4141
                //                       ^ position
                reader.Position.Should().Be(12);

                reader.Seek(-7, SeekOrigin.Current).Should().Be(5).And.Be(reader.Position);
                //  octet = 01000001 01000001 = 0x4141
                //               ^ position

                // NOTE: Not only does the position need to be adjusted, but the bit-reader
                // must also make sure the "current" byte is correct to reflect the current
                // position
                reader.ReadBits(4, out value).Should().Be(4);
                value.Should().Be(0x02);
                //               +++ +
                //  octet = 01000001 01000001 = 0x4141
                //                    ^ position
            }
        }

        [Test]
        public void Seek_forwards_beyond_current_byte()
        {
            //  octet = 01000001 01000001 = 0x4141

            byte[] bytes = { 0x41, 0x41 };

            using (var reader = new BitReader(bytes))
            {
                byte value;
                reader.ReadBits(4, out value).Should().Be(4);
                //          ++++
                //  octet = 01000001 01000001 = 0x4141
                //              ^ position

                reader.Seek(+7, SeekOrigin.Current).Should().Be(11).And.Be(reader.Position);
                //  octet = 01000001 01000001 = 0x4141
                //                      ^ position

                // NOTE: Not only does the position need to be adjusted, but the bit-reader
                // must also make sure the "current" byte is correct to reflect the current
                // position
                reader.ReadBits(5, out value).Should().Be(5);
                value.Should().Be(0x01);
                //                      +++++
                //  octet = 01000001 01000001 = 0x4141
                //                           ^ position
            }
        }

        [Test]
        public void Seek_to_beginning()
        {
            //  octet = 01001001 01001101 = 0x494D

            byte[] bytes = { 0x49, 0x4D };

            using (var reader = new BitReader(bytes))
            {
                byte value;
                reader.ReadBits(4, out value).Should().Be(4);
                //          ++++
                //  octet = 01001001 01001101
                //              ^ position

                reader.Seek(0, SeekOrigin.Begin).Should().Be(0).And.Be(reader.Position);
                //  octet = 01001001 01001101 = 0x4141
                //          ^ position

                // NOTE: Not only does the position need to be adjusted, but the bit-reader
                // must also make sure the "current" byte is correct to reflect the current
                // position
                reader.ReadBits(3, out value).Should().Be(3);
                value.Should().Be(0x02);
                //          +++
                //  octet = 01001001 01001101
                //             ^ position
            }
        }

        [Test]
        public void Seek_relative_to_beginning_within_first_byte()
        {
            //  octet = 01001001 01001101 = 0x494D

            byte[] bytes = { 0x49, 0x4D };

            using (var reader = new BitReader(bytes))
            {
                byte value;
                reader.ReadBits(4, out value).Should().Be(4);
                //          ++++
                //  octet = 01001001 01001101
                //              ^ position

                reader.Seek(1, SeekOrigin.Begin).Should().Be(1).And.Be(reader.Position);
                //  octet = 01001001 01001101 = 0x4141
                //           ^ position

                // NOTE: Not only does the position need to be adjusted, but the bit-reader
                // must also make sure the "current" byte is correct to reflect the current
                // position
                reader.ReadBits(3, out value).Should().Be(3);
                value.Should().Be(0x04);
                //           +++
                //  octet = 01001001 01001101
                //              ^ position
            }
        }

        [Test]
        public void Seek_relative_to_beginning_within_second_byte()
        {
            //  octet = 01001001 01001101 = 0x494D

            byte[] bytes = { 0x49, 0x4D };

            using (var reader = new BitReader(bytes))
            {
                byte value;
                reader.ReadBits(4, out value).Should().Be(4);
                //          ++++
                //  octet = 01001001 01001101
                //              ^ position

                reader.Seek(12, SeekOrigin.Begin).Should().Be(12).And.Be(reader.Position);
                //  octet = 01001001 01001101 = 0x4141
                //                       ^ position

                // NOTE: Not only does the position need to be adjusted, but the bit-reader
                // must also make sure the "current" byte is correct to reflect the current
                // position
                reader.ReadBits(3, out value).Should().Be(3);
                value.Should().Be(0x06);
                //                       +++
                //  octet = 01001001 01001101
                //                          ^ position
            }
        }

        [Test]
        public void Seek_relative_to_end_within_last_byte()
        {
            //  octet = 01001001 01001101 = 0x494D

            byte[] bytes = { 0x49, 0x4D };

            using (var reader = new BitReader(bytes))
            {
                byte value;
                reader.ReadBits(4, out value).Should().Be(4);
                //          ++++
                //  octet = 01001001 01001101
                //              ^ position

                reader.Seek(-3, SeekOrigin.End).Should().Be(12).And.Be(reader.Position);
                //  octet = 01001001 01001101 = 0x4141
                //                       ^ position

                // NOTE: Not only does the position need to be adjusted, but the bit-reader
                // must also make sure the "current" byte is correct to reflect the current
                // position
                reader.ReadBits(3, out value).Should().Be(3);
                value.Should().Be(0x06);
                //                       +++
                //  octet = 01001001 01001101
                //                          ^ position
            }
        }

        [Test]
        public void Seek_relative_to_end_within_second_to_last_byte()
        {
            //  octet = 01001001 01001101 = 0x494D

            byte[] bytes = { 0x49, 0x4D };

            using (var reader = new BitReader(bytes))
            {
                byte value;
                reader.ReadBits(4, out value).Should().Be(4);
                //          ++++
                //  octet = 01001001 01001101
                //              ^ position

                reader.Seek(-11, SeekOrigin.End).Should().Be(4).And.Be(reader.Position);
                //  octet = 01001001 01001101 = 0x4141
                //              ^ position

                // NOTE: Not only does the position need to be adjusted, but the bit-reader
                // must also make sure the "current" byte is correct to reflect the current
                // position
                reader.ReadBits(3, out value).Should().Be(3);
                value.Should().Be(0x04);
                //              +++
                //  octet = 01001001 01001101
                //              ^ position
            }
        }

        [Test]
        public void Seek_to_end()
        {
            //  octet = 01001001 01001101 = 0x494D

            byte[] bytes = { 0x49, 0x4D };

            using (var reader = new BitReader(bytes))
            {
                byte value;
                reader.ReadBits(4, out value).Should().Be(4);
                //          ++++
                //  octet = 01001001 01001101
                //              ^ position

                reader.Seek(0, SeekOrigin.End).Should().Be(15).And.Be(reader.Position);
                //  octet = 01001001 01001101 = 0x4141
                //                          ^ position

                // NOTE: Not only does the position need to be adjusted, but the bit-reader
                // must also make sure the "current" byte is correct to reflect the current
                // position
                reader.ReadBits(3, out value).Should().Be(1);
                value.Should().Be(0x04);
                //                          +++
                //  octet = 01001001 01001101
                //                             ^ position
            }
        }

        [Test]
        public void Length_should_return_zero_bits_for_empty_array()
        {
            byte[] bytes = { };

            using (var reader = new BitReader(bytes))
            {
                reader.Length.Should().Be(0);
            }
        }

        [Test]
        public void Length_should_return_8bits_when_1byte_array()
        {
            byte[] bytes = { 0x00 };

            using (var reader = new BitReader(bytes))
            {
                reader.Length.Should().Be(8);
            }
        }
    }
}