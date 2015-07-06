using System;
using System.IO;

namespace BaseN.Encoders
{
    public class DefaultDataEncoder : DataEncoder
    {
        int _charsWritten;

        public DefaultDataEncoder(DataEncoding encoding, TextWriter writer)
            : base(encoding, writer)
        {
        }

        protected override void Encode(BitReader reader, TextWriter writer)
        {
            byte index;
            while ((reader.ReadCompleteChunk(Encoding.BitsPerChar, out index)) > 0)
            {
                char c = Encoding.Alphabet[index];
                writer.Write(c);
                _charsWritten++;
            }
        }

        protected override void FinalizeEncoding(BitReader reader, TextWriter writer)
        {
            byte index;
            int readBits = reader.ReadChunk(Encoding.BitsPerChar, out index);
            if (readBits > 0)
            {
                char c = Encoding.Alphabet[index];
                writer.Write(c);
                _charsWritten++;
            }

            int charsInFinalGroup = _charsWritten % Encoding.CharsPerQuantum;
            if (charsInFinalGroup > 0)
            {
                int missingCharsInFinalGroup = Encoding.CharsPerQuantum - charsInFinalGroup;
                writer.Write(new string(Encoding.Padding, missingCharsInFinalGroup));
            }
        }
    }
}