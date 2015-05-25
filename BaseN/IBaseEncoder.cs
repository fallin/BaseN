using System;

namespace BaseN
{
    public interface IBaseEncoder : IDisposable
    {
        void Write(byte[] buffer);
        void Write(byte[] buffer, int offset, int count);
    }
}