using System;

namespace BaseN.Encoders
{
    public interface IBaseEncoder : IDisposable
    {
        void Write(byte[] buffer);
        void Write(byte[] buffer, int offset, int count);
    }
}