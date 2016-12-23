using System;
using System.IO;
using System.Text;
using EnsureThat;

namespace BaseN
{
    static class MemoryStreamExtensions
    {
        public static string ReadAllText(this MemoryStream stream)
        {
            // In most contexts you'd choose UTF-8 as the default encoding, but for RFC4648 (and similar)
            // encoders, the text is always ASCII.
            return ReadAllText(stream, Encoding.ASCII);
        }

        public static string ReadAllText(this MemoryStream stream, Encoding encoding)
        {
            Ensure.That(stream, "stream").IsNotNull();
            Ensure.That(encoding, "encoding").IsNotNull();

            stream.Seek(0, SeekOrigin.Begin);

            // We do not want to dispose the stream (that's the caller's responsibiliy, so we must
            // pass leaveOpen: true

            string value;
            using (StreamReader reader = new StreamReader(stream, Encoding.ASCII, false, 256, leaveOpen: true))
            {
                value = reader.ReadToEnd();
            }
            return value;
        }
    }
}