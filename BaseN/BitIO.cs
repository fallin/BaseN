using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using EnsureThat;
using JetBrains.Annotations;

namespace BaseN
{
    /// <summary>
    /// A base class for bit-wise stream readers and writers.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public abstract class BitIO : IDisposable
    {
        readonly Stream _stream;
        readonly bool _leaveOpen;
        bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="BitIO"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        protected BitIO([NotNull] Stream stream)
            : this(stream, false)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitIO"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="leaveOpen">if set to <c>true</c> [leave open].</param>
        protected BitIO([NotNull] Stream stream, bool leaveOpen)
        {
            Ensure.That(stream, "stream").IsNotNull();
            _stream = stream;
            _leaveOpen = leaveOpen;

            RelativePosition = 0;
            CurrentByte = -1;
        }

        /// <summary>
        /// The end of stream.
        /// </summary>
        public const int EndOfStream = -1;

        /// <summary>
        /// Gets or sets the relative position within the current byte.
        /// </summary>
        /// <value>The relative position.</value>
        protected int RelativePosition { get; set; }

        /// <summary>
        /// Gets or sets the current byte from the stream.
        /// </summary>
        /// <value>The current byte.</value>
        protected int CurrentByte { get; set; }

        /// <summary>
        /// Gets the underlying stream.
        /// </summary>
        /// <value>The base stream.</value>
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
                var adjustedPosition = CurrentByte == EndOfStream 
                    ? _stream.Position 
                    : Math.Max((long) (int) (_stream.Position - 1), 0);
                return (adjustedPosition * 8) + RelativePosition;
            }
        }

        /// <summary>
        /// Gets the length of the stream (in bits).
        /// </summary>
        /// <value>The length.</value>
        public long Length
        {
            get { return _stream.Length*8; }
        }

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
                    throw new ArgumentOutOfRangeException("origin");
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
            byte mask = 0;
            for (int i = 0; i < bits; i++)
            {
                mask <<= 1;
                mask |= 1;
            }
            return mask;
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
        /// Finalizes an instance of the <see cref="BitIO"/> class.
        /// </summary>
        ~BitIO()
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