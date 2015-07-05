# BaseN
A .NET (C#) implementation of various binary-to-text data encodings.

This project currently implements encoders for:
- Base64
- Base64 URL and filename safe alphabet
- Base62
- Base32
- Base32 extended hex alphabet
- Base16
- ModHex (a Yubico encoding)

Notes:

This project implements the encodings described in [RFC4648](http://tools.ietf.org/html/rfc4648).

The Base62 encoding is implemented as described in Section F of *A Secure, Lossless, and Compressed Base62 Encoding*, by Kejing He, Xiancheng Xu and Qiang Yue. That is, it provides special handling of the missing last two (6 bit) character combinations as described in section F. Note, the RLE, BWT, MTF, ZLE, and range encoding steps of the Base62 workflow are not implemented.

TODO:

- Support stream-based encoder (for files and larger inputs)
- Implement decoders
- Consider other encodings, like Base85, Z85?
- Implement Crockford's Base32 alphabet & checksum