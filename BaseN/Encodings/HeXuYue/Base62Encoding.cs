using System;
using System.IO;

namespace BaseN.Encodings.HeXuYue
{
    /// <summary>
    /// Base62 Encoding as described in "A Secure, Lossless, and Compressed Base62 Encoding", by
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
    public class Base62Encoding : BaseEncoding
    {
        public Base62Encoding() 
            : base("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", 64)
        {
        }

        protected override IBaseEncoder CreateEncoder(TextWriter writer)
        {
            return new Base62Encoder(this, writer);
        }
    }
}