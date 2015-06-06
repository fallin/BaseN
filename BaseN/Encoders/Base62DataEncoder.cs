using System;
using System.Collections.Generic;
using System.IO;
using BaseN.Encodings;
using EnsureThat;

namespace BaseN.Encoders
{
    public class Base62DataEncoder : IDataEncoder
    {
        readonly DataEncoding _encoding;
        readonly TextWriter _writer;
        readonly BitReader _reader;
        bool _disposed;

        IReadOnlyList<char> Alphabet
        {
            get { return _encoding.Alphabet; }
        }

        int BitsPerChar
        {
            get { return _encoding.BitsPerChar; }
        }

        public void Write(byte[] buffer)
        {
            Write(new ArraySegment<byte>(buffer));
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            Write(new ArraySegment<byte>(buffer, offset, count));
        }

        public void Write(ArraySegment<byte> buffer)
        {
            AppendToStream(_reader.BaseStream, buffer);

            byte index;
            while ((_reader.ReadCompleteChunk(BitsPerChar, out index)) > 0)
            {
                if (index < 60)
                {
                    _writer.Write(Alphabet[index]);
                }
                else if (index < 62)
                {
                    _reader.Seek(-1, SeekOrigin.Current);
                    _writer.Write(Alphabet[60]);
                }
                else
                {
                    _reader.Seek(-1, SeekOrigin.Current);
                    _writer.Write(Alphabet[61]);
                }
            }
        }

        void AppendToStream(Stream stream, ArraySegment<byte> buffer)
        {
            long originalPosition = stream.Position;
            stream.Write(buffer.Array, buffer.Offset, buffer.Count);
            stream.Position = originalPosition;
        }

        public Base62DataEncoder(Base62DataEncoding encoding, TextWriter writer)
        {
            Ensure.That(encoding, "encoding").IsNotNull();
            Ensure.That(writer, "writer").IsNotNull();

            _encoding = encoding;
            _writer = writer;

            MemoryStream inputStream = new MemoryStream();
            _reader = new BitReader(inputStream);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Base62DataEncoder()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // free other managed objects that implement
                    // IDisposable only
                    FinalizeEncoding();
                }

                // release any unmanaged objects
                // set the object references to null

                _disposed = true;
            }
        }

        void FinalizeEncoding()
        {
            byte index;
            int readBits = _reader.ReadChunk(BitsPerChar, out index);
            if (readBits > 0)
            {
                if (readBits < BitsPerChar)
                {
                    index >>= (BitsPerChar - readBits);
                }
                _writer.Write(Alphabet[index]);
            }
        }
    }
}