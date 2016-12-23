using System;
using System.IO;
using BaseN.Encodings;

namespace BaseN.Encoders
{
    public class Base62DataEncoder : DataEncoder
    {
        public Base62DataEncoder(Base62DataEncoding encoding, Stream outputStream)
            : base(encoding, outputStream)
        {
        }

        protected override void Encode(BitReader reader, Stream outputStream)
        {
            byte index;
            while ((reader.ReadCompleteChunk(Encoding.BitsPerChar, out index)) > 0)
            {
                if (index < 60)
                {
                    outputStream.WriteByte(Encoding.Alphabet[index]);
                }
                else if (index < 62)
                {
                    reader.Seek(-1, SeekOrigin.Current);
                    outputStream.WriteByte(Encoding.Alphabet[60]);
                }
                else
                {
                    reader.Seek(-1, SeekOrigin.Current);
                    outputStream.WriteByte(Encoding.Alphabet[61]);
                }
            }
        }

        protected override void FinalizeEncoding(BitReader reader, Stream outputStream)
        {
            byte index;
            int readBits = reader.ReadChunk(Encoding.BitsPerChar, out index);
            if (readBits > 0)
            {
                if (readBits < Encoding.BitsPerChar)
                {
                    index >>= (Encoding.BitsPerChar - readBits);
                }
                outputStream.WriteByte(Encoding.Alphabet[index]);
            }
        }
    }
}