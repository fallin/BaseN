using System;
using System.IO;
using EnsureThat;

namespace BaseN.Encoders
{
    public abstract class DataEncoder : IDisposable
    {
        readonly DataEncoding _encoding;
        readonly TextWriter _writer;
        readonly BitReader _reader;
        bool _disposed;

        protected DataEncoder(DataEncoding encoding, TextWriter writer)
        {
            Ensure.That(encoding, "encoding").IsNotNull();
            Ensure.That(writer, "writer").IsNotNull();

            _encoding = encoding;
            _writer = writer;

            MemoryStream inputStream = new MemoryStream();
            _reader = new BitReader(inputStream);
        }

        protected DataEncoding Encoding
        {
            get { return _encoding; }
        }

        public void Encode(byte[] buffer)
        {
            Encode(new ArraySegment<byte>(buffer));
        }

        public void Encode(byte[] buffer, int offset, int count)
        {
            Encode(new ArraySegment<byte>(buffer, offset, count));
        }

        public void Encode(ArraySegment<byte> buffer)
        {
            AppendToStream(_reader.BaseStream, buffer);
            Encode(_reader, _writer);
        }

        public void Flush()
        {
            _writer.Flush();
        }

        public void Close()
        {
            Dispose(true);
        }

        protected abstract void Encode(BitReader reader, TextWriter writer);

        protected abstract void FinalizeEncoding(BitReader reader, TextWriter writer);

        void AppendToStream(Stream stream, ArraySegment<byte> buffer)
        {
            long originalPosition = stream.Position;
            stream.Write(buffer.Array, buffer.Offset, buffer.Count);
            stream.Position = originalPosition;
        }

        #region IDisposable Implementation

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DataEncoder()
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
                    FinalizeEncoding(_reader, _writer);
                }

                // free native resources if there are any 
                // and set object references to null

                _disposed = true;
            }
        }

        #endregion
    }
}