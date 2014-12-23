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
        readonly byte _charMask;
        int _sourcePosition;
        byte _accumulator;
        int _charsWritten;
        bool _disposed;

        public BaseEncoder(BaseEncoding encoding, TextWriter writer)
        {
            Ensure.That(encoding, "encoding").IsNotNull();
            Ensure.That(writer, "writer").IsNotNull();

            _encoding = encoding;
            _writer = writer;

            _charMask = CreateMask(BitsPerChar);
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
            foreach (byte index in RealignByteSizeBoundaries(buffer))
            {
                char c = Alphabet[index];
                _writer.Write(c);
                _charsWritten++;
            }
        }

        public void Flush()
        {
            _writer.Flush();
        }

        public void Close()
        {
            Dispose(true);
        }

        IEnumerable<byte> RealignByteSizeBoundaries(IEnumerable<byte> bytes)
        {
            foreach (byte octet in bytes)
            {
                while (true)
                {
                    if (_sourcePosition < 8)
                    {
                        _sourcePosition += BitsPerChar;
                    }
                    else // is incomplete, resume
                    {
                        _sourcePosition %= 8;
                        if (_sourcePosition == 0) break;
                    }

                    int shift = 8 - _sourcePosition;
                    byte x = shift > 0 ? (byte)(octet >> shift) : (byte)(octet << Math.Abs(shift));
                    _accumulator |= x;

                    if (_sourcePosition > 8) break; // incomplete, get next byte

                    _accumulator &= _charMask;
                    yield return _accumulator;
                    _accumulator = 0;
                }
            }
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
            if (_sourcePosition > 8)
            {
                _sourcePosition = 0; // flush can be called more than once!
                _accumulator &= _charMask;
                char c = Alphabet[_accumulator];
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