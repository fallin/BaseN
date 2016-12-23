using System;
using System.IO;
using BaseN.Encoders;

namespace BaseN.Encodings
{
    /// <summary>
    /// Base62 data encoding as described in "A Secure, Lossless, and Compressed Base62 Encoding", by
    /// Kejing He, Xiancheng Xu and Qiang Yue.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This implementation only implements the binary-to-text encoding technique described in the
    /// paper. It does not implement the Run Length Encoding (RLE), Burrows-Wheeler
    /// Transform (BWT), Move-To-Front (MTF), Zero-Length Encoding (ZLE), Range Encoding, or
    /// the encryption workflow.
    /// </para>
    /// <para>
    /// This encoding is compatible with the online Base62 encoder/decoder
    /// found here: http://www.opitz-online.com/base62/title/base62-encoder-decoder
    /// </para>
    /// <para>
    /// The research paper, "A Secure, Lossless, and Compressed Base62 Encoding" is available
    /// here: http://www.opitz-online.com/dl/base62_encoding.pdf
    /// </para>
    /// </remarks>
    public class Base62DataEncoding : DataEncoding
    {
        public Base62DataEncoding() 
            : base("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", 64)
        {
        }

        protected override DataEncoder CreateEncoder(Stream outputStream)
        {
            return new Base62DataEncoder(this, outputStream);
        }
    }
}