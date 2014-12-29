using System;
using System.IO;
using EnsureThat;
using JetBrains.Annotations;

namespace BaseN
{
    class BitReader : IDisposable
    {
        public const int EndOfStream = -1;
        readonly Stream _stream;
        readonly bool _leaveOpen;
        bool _disposed;

        int _relativePosition;
        int _currentByte; // accumulator

        public BitReader([NotNull] Stream stream)
            : this(stream, false)
        {
            
        }

        public BitReader([NotNull] Stream stream, bool leaveOpen)
        {
            Ensure.That(stream, "stream").IsNotNull();
            _stream = stream;
            _leaveOpen = leaveOpen;

            _relativePosition = 0;
            _currentByte = -1;
        }

        public Stream BaseStream
        {
            get { return _stream; }
        }

        /// <summary>
        /// Gets the absolute position in the stream (in bits).
        /// </summary>
        /// <value>The current position within the stream (in bits).</value>
        public long Position
        {
            get
            {
                var adjustedPosition = _currentByte == EndOfStream 
                    ? _stream.Position 
                    : Math.Max(_stream.Position - 1, 0);
                return (adjustedPosition * 8) + _relativePosition;
            }
        }

        public long Length
        {
            get { return _stream.Length*8; }
        }

        /// <summary>
        /// Sets the position within the current stream.
        /// </summary>
        /// <param name="offsetBits">A offset (in bits!) relative to the origin parameter.</param>
        /// <param name="origin">A value of type SeekOrigin indicating the reference point used to obtain the new position.</param>
        /// <returns>The new position within the current stream.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">origin</exception>
        public long Seek(long offsetBits, SeekOrigin origin)
        {
            Ensure.That(offsetBits, "offsetBits").IsLte(int.MaxValue);

            long absolutePosition;
            switch (origin)
            {
                case SeekOrigin.Begin:
                    absolutePosition = offsetBits;
                    break;
                case SeekOrigin.Current:
                    absolutePosition = Position + offsetBits;
                    break;
                case SeekOrigin.End:
                    absolutePosition = Length + offsetBits - 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("origin");
            }

            long positionBits;
            long positionBytes = Math.DivRem(absolutePosition, 8, out positionBits);

            _stream.Seek(positionBytes, SeekOrigin.Begin);
            _currentByte = _stream.ReadByte();

            _relativePosition = (int)positionBits;

            return Position;
        }

        public int ReadCompleteChunk(int countBits, out byte value)
        {
            int readBits = ReadChunk(countBits, out value);
            if (readBits == countBits || readBits == EndOfStream) // complete chunk
            {
                return readBits;
            }

            Seek(-countBits, SeekOrigin.Current);
            value = 0x00;
            return 0;
        }

        public int ReadChunk(int countBits, out byte value)
        {
            Ensure.That(countBits, "countBits").IsInRange(0, 8);

            // Attempt to read the next byte to prime the _currentByte
            if (_currentByte == EndOfStream)
            {
                _currentByte = _stream.ReadByte();
            }

            int readBits;
            int currentBits = 0x00;
            
            if (_currentByte != EndOfStream)
            {
                _relativePosition += countBits;
                int shift = 8 - _relativePosition;
                currentBits |= shift > 0 ? (byte)(_currentByte >> shift) : (byte)(_currentByte << Math.Abs(shift));
                readBits = countBits;

                // Adjust when moving beyond byte boundary
                if (_relativePosition > 7)
                {
                    _relativePosition %= 8;
                    _currentByte = _stream.ReadByte();
                    if (_currentByte != EndOfStream)
                    {
                        shift = 8 - _relativePosition;
                        currentBits |= shift > 0 ? (byte)(_currentByte >> shift) : (byte)(_currentByte << Math.Abs(shift));
                    }
                    else
                    {
                        readBits -= _relativePosition;
                    }
                }

                byte mask = CreateMask(countBits);
                currentBits &= mask;
            }
            else
            {
                readBits = EndOfStream;
            }

            value = (byte)currentBits;
            return readBits;
        }

        static byte CreateMask(int bits)
        {
            byte mask = 0;
            for (int i = 0; i < bits; i++)
            {
                mask <<= 1;
                mask |= 1;
            }
            return mask;
        }

        public void Close()
        {
            Dispose(true);
        }

        //public void Flush()
        //{
        //    _stream.Flush();
        //}

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~BitReader()
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
                    if (_leaveOpen)
                    {
                        _stream.Flush();
                    }
                    else
                    {
                        _stream.Dispose();
                    }
                }

                // release any unmanaged objects
                // set the object references to null

                _disposed = true;
            }
        }
    }
}