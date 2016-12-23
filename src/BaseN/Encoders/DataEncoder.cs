using System;
using System.IO;
using EnsureThat;

namespace BaseN.Encoders
{
    public abstract class DataEncoder : IDisposable
    {
        readonly DataEncoding _encoding;
        readonly Stream _outputStream;
        readonly BitReader _reader;
        bool _disposed;

        protected DataEncoder(DataEncoding encoding, Stream outputStream)
        {
            Ensure.That(encoding, "encoding").IsNotNull();
            Ensure.That(outputStream, "outputStream").IsNotNull();

            _encoding = encoding;
            _outputStream = outputStream;

            MemoryStream inputStream = new MemoryStream();
            _reader = new BitReader(inputStream);
        }

        protected DataEncoding Encoding => _encoding;

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
            Encode(_reader, _outputStream);
        }

        public void Flush()
        {
            _outputStream.Flush();
        }

        public void Close()
        {
            Dispose(true);
        }

        protected abstract void Encode(BitReader reader, Stream outputStream);

        protected abstract void FinalizeEncoding(BitReader reader, Stream outputStream);

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
                    FinalizeEncoding(_reader, _outputStream);
                }

                // free native resources if there are any 
                // and set object references to null

                _disposed = true;
            }
        }

        #endregion
    }
}