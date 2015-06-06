using System;

namespace BaseN.Encoders
{
    public interface IDataEncoder : IDisposable
    {
        void Write(byte[] buffer);
        //void Write(byte[] buffer, int offset, int count);
    }
}