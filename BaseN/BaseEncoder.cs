using System;
using System.Collections.Generic;
using System.IO;
using EnsureThat;

namespace BaseN
{
    public class BaseEncoder : IDisposable
    {
        readonly BaseEncoding _encoding;
        readonly TextWriter _writer;
        readonly BitReader _reader;
        int _charsWritten;
        bool _disposed;

        public BaseEncoder(BaseEncoding encoding, TextWriter writer)
        {
            Ensure.That(encoding, "encoding").IsNotNull();
            Ensure.That(writer, "writer").IsNotNull();

            _encoding = encoding;
            _writer = writer;

            MemoryStream inputStream = new MemoryStream();
            _reader = new BitReader(inputStream);
        }

        char Padding
        {
            get { return _encoding.Padding; }
        }

        IReadOnlyList<char> Alphabet
        {
            get { return _encoding.Alphabet; }
        }

        int BitsPerChar
        {
            get { return _encoding.BitsPerChar; }
        }

        int BitsPerQuantum
        {
            get { return _encoding.BitsPerQuantum; }
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
                char c = Alphabet[index];
                _writer.Write(c);
                _charsWritten++;
            }
        }

        void AppendToStream(Stream stream, ArraySegment<byte> buffer)
        {
            long originalPosition = stream.Position;
            stream.Write(buffer.Array, buffer.Offset, buffer.Count);
            stream.Position = originalPosition;
        }

        public void Flush()
        {
            _writer.Flush();
        }

        public void Close()
        {
            Dispose(true);
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~BaseEncoder()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // free managed resources that implement IDisposable
                    FinalizeEncoding();
                }

                // free native resources if there are any 
                // and set object references to null

                _disposed = true;
            }
        }

        void FinalizeEncoding()
        {
            byte index;
            int readBits = _reader.ReadChunk(BitsPerChar, out index);
            if (readBits > 0)
            {
                char c = Alphabet[index];
                _writer.Write(c);
                _charsWritten++;
            }

            int charsPerQuantum = BitsPerQuantum/BitsPerChar;
            int charsInFinalGroup = _charsWritten % charsPerQuantum;
            if (charsInFinalGroup > 0)
            {
                int missingCharsInFinalGroup = charsPerQuantum - charsInFinalGroup;
                _writer.Write(new string(Padding, missingCharsInFinalGroup));
            }
        }
    }
}