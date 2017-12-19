using System;
using System.IO;
using System.Linq;

namespace BaseN.Encoders
{
    public class DefaultDataEncoder : DataEncoder
    {
        int _charsWritten;

        public DefaultDataEncoder(DataEncoding encoding, Stream outputStream)
            : base(encoding, outputStream)
        {
        }

        protected override void Encode(BitReader reader, Stream outputStream)
        {
            byte index;
            while ((reader.ReadCompleteChunk(Encoding.BitsPerEncodedChar, out index)) > 0)
            {
                byte c = Encoding.Alphabet[index];
                outputStream.WriteByte(c);
                _charsWritten++;
            }
        }

        protected override void FinalizeEncoding(BitReader reader, Stream outputStream)
        {
            byte index;
            int readBits = reader.ReadBits(Encoding.BitsPerEncodedChar, out index);
            if (readBits > 0)
            {
                byte c = Encoding.Alphabet[index];
                outputStream.WriteByte(c);
                _charsWritten++;
            }

            int charsInFinalGroup = _charsWritten % Encoding.EncodedCharsPerQuantum;
            if (charsInFinalGroup > 0)
            {
                int missingCharsInFinalGroup = Encoding.EncodedCharsPerQuantum - charsInFinalGroup;

                byte[] paddingBuffer = Enumerable.Repeat(Encoding.Padding, missingCharsInFinalGroup).ToArray();
                outputStream.Write(paddingBuffer, 0, missingCharsInFinalGroup);
            }
        }
    }
}