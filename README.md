# BaseN #
A binary-to-text (Base) encoder

This project currently implements encoders for:
- Base64
- Base64 (URL-safe)
- Base32
- Base16
- ModHex (a Yubico encoding)

Notes:

This project implements the encodings described in [RFC3548](http://tools.ietf.org/html/rfc3548).

TODO:

- Upgrade encoders to [RFC4648](http://tools.ietf.org/html/rfc4648)
- Support stream-based encoder (for files and larger inputs)
- Implement decoders
- Consider other encodings, like Base62, Base85, Z85?