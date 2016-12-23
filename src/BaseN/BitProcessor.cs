using System;
using System.IO;
using EnsureThat;

namespace BaseN
{
    public abstract class BitProcessor : IDisposable
    {
        public const int EndOfStream = -1;
        readonly Stream _stream;
        readonly bool _leaveOpen;
        bool _disposed;

        protected BitProcessor(Stream stream) : this(stream, false)
        {
        }

        protected BitProcessor(Stream stream, bool leaveOpen)
        {
            _stream = stream;
            _leaveOpen = leaveOpen;
        }

        public Stream BaseStream => _stream;
        protected int RelativePosition { get; set; }
        protected int CurrentByte { get; set; }

        /// <summary>
        /// Gets the absolute position in the stream (in bits).
        /// </summary>
        /// <value>The current position within the stream (in bits).</value>
        public long Position
        {
            get
            {
                var adjustedPosition = CurrentByte == EndOfStream
                    ? _stream.Position
                    : Math.Max((long)(int)(_stream.Position - 1), 0);
                return adjustedPosition * 8 + RelativePosition;
            }
        }

        /// <summary>
        /// Gets the length of the stream (in bits).
        /// </summary>
        /// <value>The length.</value>
        public long Length => _stream.Length * 8;

        /// <summary>
        /// Sets the position within the stream.
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
                    throw new ArgumentOutOfRangeException(nameof(origin));
            }

            long positionBits;
            long positionBytes = Math.DivRem(absolutePosition, 8, out positionBits);

            _stream.Seek(positionBytes, SeekOrigin.Begin);
            CurrentByte = _stream.ReadByte();

            RelativePosition = (int)positionBits;

            return Position;
        }

        /// <summary>
        /// Creates a mask with the specified number of 1 bits (from the least-significant bits)
        /// </summary>
        /// <param name="bits">A count of the number of bits.</param>
        /// <returns>System.Byte.</returns>
        protected static byte CreateMask(int bits)
        {
            switch (bits)
            {
                case 0:
                    return 0x00;
                case 1:
                    return 0x01;
                case 2:
                    return 0x03;
                case 3:
                    return 0x07;
                case 4:
                    return 0x0F;
                case 5:
                    return 0x1F;
                case 6:
                    return 0x3F;
                case 7:
                    return 0x7F;
                case 8:
                    return 0xFF;
                default:
                    throw new ArgumentOutOfRangeException(nameof(bits), bits, "Must be between 0 and 8 (inclusive).");
            }
        }

        /// <summary>
        /// Closes the stream.
        /// </summary>
        public void Close()
        {
            Dispose();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or
        /// resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="BitProcessor"/> class.
        /// </summary>
        ~BitProcessor()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases unmanaged and managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
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