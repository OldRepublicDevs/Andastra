// Public domain LZMA implementation based on 7-Zip SDK (LZMA SDK 19.00)
// Minimal wrapper for raw LZMA1 compress/decompress (no headers).
// This matches PyKotor bzf.py usage: lzma.FORMAT_RAW with FILTER_LZMA1 defaults.

using System;
using System.IO;

namespace CSharpKOTOR.Common.LZMA
{
    internal static class LzmaHelper
    {
        // Default LZMA properties: lc=3, lp=0, pb=2, dict=1<<23 (8 MB)
        private const int DefaultLc = 3;
        private const int DefaultLp = 0;
        private const int DefaultPb = 2;
        private const int DefaultDictionary = 1 << 23;

        private static readonly byte[] DefaultProperties = BuildProperties(DefaultLc, DefaultLp, DefaultPb, DefaultDictionary);

        public static byte[] Decompress(byte[] compressed, int uncompressedSize)
        {
            using (var inStream = new MemoryStream(compressed))
            using (var outStream = new MemoryStream(uncompressedSize))
            {
                var decoder = new LzmaDecoder();
                decoder.SetDecoderProperties(DefaultProperties);
                decoder.Code(inStream, outStream, compressed.Length, uncompressedSize, null);
                return outStream.ToArray();
            }
        }

        public static byte[] Compress(byte[] data)
        {
            using (var inStream = new MemoryStream(data))
            using (var outStream = new MemoryStream())
            {
                var encoder = new LzmaEncoderAdapter(DefaultLc, DefaultLp, DefaultPb, DefaultDictionary);
                encoder.Encode(inStream, outStream);
                return outStream.ToArray();
            }
        }

        private static byte[] BuildProperties(int lc, int lp, int pb, int dictSize)
        {
            byte[] props = new byte[5];
            props[0] = (byte)((pb * 5 + lp) * 9 + lc);
            for (int i = 0; i < 4; i++)
            {
                props[1 + i] = (byte)(dictSize >> (8 * i));
            }
            return props;
        }
    }

    internal interface ICoderProgress
    {
        void SetProgress(long inSize, long outSize);
    }

    // Range coder and base classes (decoder side)
    internal static class LzmaBase
    {
        public const uint TopValue = 1 << 24;
    }

    internal struct BitDecoder
    {
        private const int NumBitModelTotalBits = 11;
        private const uint BitModelTotal = 1 << NumBitModelTotalBits;
        private const int NumMoveBits = 5;
        private uint _prob;

        public void Init() { _prob = BitModelTotal >> 1; }

        public uint Decode(RangeDecoder rangeDecoder)
        {
            uint bound = (rangeDecoder.Range >> NumBitModelTotalBits) * _prob;
            if (rangeDecoder.Code < bound)
            {
                rangeDecoder.Range = bound;
                _prob += (BitModelTotal - _prob) >> NumMoveBits;
                if (rangeDecoder.Range < LzmaBase.TopValue)
                {
                    rangeDecoder.Code = (rangeDecoder.Code << 8) | (byte)rangeDecoder.Stream.ReadByte();
                    rangeDecoder.Range <<= 8;
                }
                return 0;
            }
            rangeDecoder.Range -= bound;
            rangeDecoder.Code -= bound;
            _prob -= _prob >> NumMoveBits;
            if (rangeDecoder.Range < LzmaBase.TopValue)
            {
                rangeDecoder.Code = (rangeDecoder.Code << 8) | (byte)rangeDecoder.Stream.ReadByte();
                rangeDecoder.Range <<= 8;
            }
            return 1;
        }
    }

    internal struct BitTreeDecoder
    {
        private readonly BitDecoder[] _models;
        private readonly int _numBitLevels;

        public BitTreeDecoder(int numBitLevels)
        {
            _numBitLevels = numBitLevels;
            _models = new BitDecoder[1 << numBitLevels];
        }

        public void Init()
        {
            for (uint i = 1; i < _models.Length; i++)
            {
                _models[i].Init();
            }
        }

        public uint Decode(RangeDecoder rangeDecoder)
        {
            uint m = 1;
            for (int bitIndex = _numBitLevels; bitIndex > 0; bitIndex--)
            {
                m = (m << 1) + _models[m].Decode(rangeDecoder);
            }
            return m - ((uint)1 << _numBitLevels);
        }

        public uint ReverseDecode(RangeDecoder rangeDecoder)
        {
            uint m = 1;
            uint symbol = 0;
            for (int bitIndex = 0; bitIndex < _numBitLevels; bitIndex++)
            {
                uint bit = _models[m].Decode(rangeDecoder);
                m = (m << 1) + bit;
                symbol |= bit << bitIndex;
            }
            return symbol;
        }

        public static uint ReverseDecode(BitDecoder[] models, uint startIndex, RangeDecoder rangeDecoder, int numBitLevels)
        {
            uint m = 1;
            uint symbol = 0;
            for (int bitIndex = 0; bitIndex < numBitLevels; bitIndex++)
            {
                uint bit = models[startIndex + m].Decode(rangeDecoder);
                m = (m << 1) + bit;
                symbol |= bit << bitIndex;
            }
            return symbol;
        }
    }

    internal sealed class RangeDecoder
    {
        public Stream Stream;
        public uint Range;
        public uint Code;

        public void Init(Stream stream)
        {
            Stream = stream;
            Code = 0;
            Range = uint.MaxValue;
            for (int i = 0; i < 5; i++)
            {
                Code = (Code << 8) | (byte)stream.ReadByte();
            }
        }

        public uint DecodeDirectBits(int numTotalBits)
        {
            uint result = 0;
            for (int i = numTotalBits; i > 0; i--)
            {
                Range >>= 1;
                uint t = (Code - Range) >> 31;
                Code -= Range & (t - 1);
                result = (result << 1) | (1 - t);
                if (Range < LzmaBase.TopValue)
                {
                    Code = (Code << 8) | (byte)Stream.ReadByte();
                    Range <<= 8;
                }
            }
            return result;
        }

        public void ReleaseStream()
        {
            Stream = null;
        }
    }

    // Adapter over LZMA SDK encoder for raw LZMA1 (no headers), matching decoder properties.
    internal sealed class LzmaEncoderAdapter
    {
        private readonly Encoder _encoder;
        private readonly int _lc;
        private readonly int _lp;
        private readonly int _pb;
        private readonly int _dictSize;

        public LzmaEncoderAdapter(int lc, int lp, int pb, int dictSize)
        {
            _lc = lc;
            _lp = lp;
            _pb = pb;
            _dictSize = dictSize;
            _encoder = new Encoder();
        }

        public void Encode(Stream input, Stream output)
        {
            _encoder.SetCoderProperties(_lc, _lp, _pb, _dictSize, 32);
            _encoder.Code(input, output, input.Length, -1, null);
        }
    }

    internal const int LiteralCoderSize = 0x300;

    // Minimal LZMA encoder (raw, no headers) adapted from LZMA SDK.
    internal sealed class Encoder
    {
        private const int PosStateBits = 2;
        private const int NumLiteralPosStateBits = 0;
        private const int NumLiteralContextBits = 3;
        private const int NumFastBytes = 32;
        private const uint DictionarySize = 1 << 23;
        private const int NumMoveBits = 5;
        private const int NumMoveReducingBits = 2;
        private const int NumBitPriceShiftBits = 6;

        private static readonly uint[] ProbPrices = new uint[BitModelTotal >> NumMoveReducingBits];
        private const int BitModelTotal = 1 << 11;
        private const int MatchMinLen = 2;
        private const int NumStates = 12;
        private const int NumPosSlotBits = 6;
        private const int NumLenToPosStates = 4;
        private const int NumAlignBits = 4;
        private const uint AlignTableSize = 1 << NumAlignBits;
        private const uint StartPosModelIndex = 4;
        private const uint EndPosModelIndex = 14;
        private const uint NumFullDistances = 1 << ((int)EndPosModelIndex >> 1);

        private readonly RangeEncoder _rangeEncoder = new RangeEncoder();
        private readonly BitEncoder[] _isMatch = new BitEncoder[NumStates << PosStateBits];
        private readonly BitEncoder[] _isRep = new BitEncoder[NumStates];
        private readonly BitEncoder[] _isRepG0 = new BitEncoder[NumStates];
        private readonly BitEncoder[] _isRepG1 = new BitEncoder[NumStates];
        private readonly BitEncoder[] _isRepG2 = new BitEncoder[NumStates];
        private readonly BitEncoder[] _isRep0Long = new BitEncoder[NumStates << PosStateBits];
        private readonly BitTreeEncoder[] _posSlotEncoder = new BitTreeEncoder[NumLenToPosStates];
        private readonly BitEncoder[] _posEncoders = new BitEncoder[1 + (int)EndPosModelIndex - (int)StartPosModelIndex];
        private readonly BitTreeEncoder _posAlignEncoder = new BitTreeEncoder(NumAlignBits);
        private readonly LenPriceTableEncoder _lenEncoder = new LenPriceTableEncoder();
        private readonly LenPriceTableEncoder _repLenEncoder = new LenPriceTableEncoder();
        private readonly LiteralEncoder _literalEncoder = new LiteralEncoder();

        static Encoder()
        {
            for (int i = 0; i < ProbPrices.Length; i++)
            {
                uint prob = (uint)(i << NumMoveReducingBits) + (1U << (NumMoveReducingBits - 1));
                ProbPrices[i] = (uint)((1 << NumBitPriceShiftBits) - Log2(prob));
            }
        }

        private static uint Log2(uint value)
        {
            uint result = 0;
            while (value > 1)
            {
                value >>= 1;
                result++;
            }
            return result;
        }

        public Encoder()
        {
            for (int i = 0; i < NumLenToPosStates; i++)
            {
                _posSlotEncoder[i] = new BitTreeEncoder(NumPosSlotBits);
            }
        }

        public void SetCoderProperties(int lc, int lp, int pb, int dictSize, int numFastBytes)
        {
            _literalEncoder.Create(lp, lc);
            _lenEncoder.Create(1U << pb);
            _repLenEncoder.Create(1U << pb);
        }

        public void Code(Stream inStream, Stream outStream, long inSize, long outSize, ICoderProgress progress)
        {
            var state = new State();
            state.Init();
            _rangeEncoder.SetStream(outStream);
            _rangeEncoder.Init();
            uint nowPos = 0;
            byte prevByte = 0;
            uint posStateMask = (1U << PosStateBits) - 1;

            // Initialize probabilities
            for (int i = 0; i < _isMatch.Length; i++) _isMatch[i].Init();
            for (int i = 0; i < _isRep.Length; i++) _isRep[i].Init();
            for (int i = 0; i < _isRepG0.Length; i++) _isRepG0[i].Init();
            for (int i = 0; i < _isRepG1.Length; i++) _isRepG1[i].Init();
            for (int i = 0; i < _isRepG2.Length; i++) _isRepG2[i].Init();
            for (int i = 0; i < _isRep0Long.Length; i++) _isRep0Long[i].Init();
            for (int i = 0; i < _posEncoders.Length; i++) _posEncoders[i].Init();
            for (int i = 0; i < NumLenToPosStates; i++) _posSlotEncoder[i].Init();
            _posAlignEncoder.Init();
            _lenEncoder.Init(1U << PosStateBits);
            _repLenEncoder.Init(1U << PosStateBits);
            _literalEncoder.Init();

            // Naive literal-only encoding (no matches) for simplicity; still valid LZMA stream.
            while (true)
            {
                int readByte = inStream.ReadByte();
                if (readByte < 0)
                {
                    break;
                }
                byte b = (byte)readByte;
                uint posState = nowPos & posStateMask;
                _isMatch[(state.Index << PosStateBits) + posState].Encode(_rangeEncoder, 0);
                _literalEncoder.Encode(_rangeEncoder, nowPos, prevByte, b);
                prevByte = b;
                state.UpdateChar();
                nowPos++;
            }

            // Write end marker
            uint posStateEnd = nowPos & posStateMask;
            _isMatch[(state.Index << PosStateBits) + posStateEnd].Encode(_rangeEncoder, 1);
            _isRep[state.Index].Encode(_rangeEncoder, 0);
            state.UpdateMatch();
            _lenEncoder.Encode(_rangeEncoder, posStateEnd, MatchMinLen);
            _posSlotEncoder[GetLenToPosState(MatchMinLen)].Encode(_rangeEncoder, 0);
            _posAlignEncoder.Encode(_rangeEncoder, 0);

            _rangeEncoder.FlushData();
            _rangeEncoder.FlushStream();
        }

        private static uint GetLenToPosState(uint len)
        {
            len -= MatchMinLen;
            if (len < NumLenToPosStates)
            {
                return len;
            }
            return NumLenToPosStates - 1;
        }
    }

    internal sealed class RangeEncoder
    {
        private Stream _stream;
        internal ulong Low;
        internal uint Range;
        private byte _cache;
        private uint _cacheSize;

        internal const uint TopValue = 1 << 24;

        public void SetStream(Stream stream)
        {
            _stream = stream;
        }

        public void Init()
        {
            Low = 0;
            Range = uint.MaxValue;
            _cache = 0;
            _cacheSize = 1;
        }

        public void FlushData()
        {
            for (int i = 0; i < 5; i++)
            {
                ShiftLow();
            }
        }

        public void FlushStream()
        {
            _stream.Flush();
        }

        public void EncodeDirectBits(uint v, int numTotalBits)
        {
            for (int i = numTotalBits - 1; i >= 0; i--)
            {
                Range >>= 1;
                if (((v >> i) & 1) == 1)
                {
                    Low += Range;
                }
                if (Range < TopValue)
                {
                    Range <<= 8;
                    ShiftLow();
                }
            }
        }

        public void Encode(BitEncoder model, uint symbol)
        {
            model.Encode(this, symbol);
        }

        public void ShiftLow()
        {
            if ((uint)Low < 0xFF000000 || (uint)(Low >> 32) == 1)
            {
                byte temp = (byte)(_cache + (Low >> 32));
                _stream.WriteByte(temp);
                temp = 0xFF;
                for (; _cacheSize > 1; _cacheSize--)
                {
                    _stream.WriteByte(temp);
                }
                _cache = (byte)(((uint)Low) >> 24);
            }
            _cacheSize++;
            Low = (Low & 0x00FFFFFF) << 8;
        }
    }

    internal struct BitEncoder
    {
        private const int NumBitModelTotalBits = 11;
        private const uint BitModelTotal = 1 << NumBitModelTotalBits;
        private const int NumMoveBits = 5;
        private uint _prob;

        public void Init() { _prob = BitModelTotal >> 1; }

        public void Encode(RangeEncoder encoder, uint symbol)
        {
            uint newBound = (encoder.Range >> NumBitModelTotalBits) * _prob;
            if (symbol == 0)
            {
                encoder.Range = newBound;
                _prob += (BitModelTotal - _prob) >> NumMoveBits;
            }
            else
            {
                encoder.Low += newBound;
                encoder.Range -= newBound;
                _prob -= _prob >> NumMoveBits;
            }
            if (encoder.Range < RangeEncoder.TopValue)
            {
                encoder.Range <<= 8;
                encoder.ShiftLow();
            }
        }
    }

    internal struct BitTreeEncoder
    {
        private readonly BitEncoder[] _models;
        private readonly int _numBitLevels;

        public BitTreeEncoder(int numBitLevels)
        {
            _numBitLevels = numBitLevels;
            _models = new BitEncoder[1 << numBitLevels];
        }

        public void Init()
        {
            for (uint i = 1; i < (1U << _numBitLevels); i++)
            {
                _models[i].Init();
            }
        }

        public void Encode(RangeEncoder rangeEncoder, uint symbol)
        {
            uint m = 1;
            for (int bitIndex = _numBitLevels; bitIndex > 0; bitIndex--)
            {
                uint bit = (symbol >> (bitIndex - 1)) & 1;
                _models[m].Encode(rangeEncoder, bit);
                m = (m << 1) + bit;
            }
        }
    }

    internal sealed class LenPriceTableEncoder
    {
        private const int MaxPosStates = 1 << 4; // support up to pb=4
        private BitTreeEncoder _highCoder = new BitTreeEncoder(8);
        private BitTreeEncoder[] _lowCoder = new BitTreeEncoder[MaxPosStates];
        private BitTreeEncoder[] _midCoder = new BitTreeEncoder[MaxPosStates];
        private BitEncoder _choice;
        private BitEncoder _choice2;

        public LenPriceTableEncoder()
        {
            for (int posState = 0; posState < MaxPosStates; posState++)
            {
                _lowCoder[posState] = new BitTreeEncoder(3);
                _midCoder[posState] = new BitTreeEncoder(3);
            }
        }

        public void Init(uint numPosStates)
        {
            _choice.Init();
            _choice2.Init();
            for (uint posState = 0; posState < numPosStates; posState++)
            {
                _lowCoder[posState].Init();
                _midCoder[posState].Init();
            }
            _highCoder.Init();
        }

        public void Encode(RangeEncoder rangeEncoder, uint posState, uint symbol)
        {
            if (symbol < 8)
            {
                _choice.Encode(rangeEncoder, 0);
                _lowCoder[posState].Encode(rangeEncoder, symbol);
            }
            else
            {
                symbol -= 8;
                _choice.Encode(rangeEncoder, 1);
                if (symbol < 8)
                {
                    _choice2.Encode(rangeEncoder, 0);
                    _midCoder[posState].Encode(rangeEncoder, symbol);
                }
                else
                {
                    _choice2.Encode(rangeEncoder, 1);
                    _highCoder.Encode(rangeEncoder, symbol - 8);
                }
            }
        }
    }

    internal sealed class LiteralEncoder
    {
        private struct Encoder2
        {
            private readonly BitEncoder[] _encoders;

            public Encoder2(bool dummy)
            {
                _encoders = new BitEncoder[LiteralCoderSize];
            }

            public void Init()
            {
                for (int i = 0; i < LiteralCoderSize; i++) _encoders[i].Init();
            }

            public void Encode(RangeEncoder rangeEncoder, byte symbol)
            {
                uint context = 1;
                for (int i = 7; i >= 0; i--)
                {
                    uint bit = (uint)((symbol >> i) & 1);
                    _encoders[context].Encode(rangeEncoder, bit);
                    context = (context << 1) | bit;
                }
            }
        }

        private Encoder2[] _coders;
        private int _numPrevBits;
        private int _numPosBits;
        private uint _posMask;

        public void Create(int numPosBits, int numPrevBits)
        {
            if (_coders != null && _numPrevBits == numPrevBits && _numPosBits == numPosBits) return;
            _numPosBits = numPosBits;
            _posMask = (uint)((1 << numPosBits) - 1);
            _numPrevBits = numPrevBits;
            uint numStates = (uint)1 << (_numPrevBits + _numPosBits);
            _coders = new Encoder2[numStates];
            for (uint i = 0; i < numStates; i++)
            {
                _coders[i] = new Encoder2(true);
            }
        }

        public void Init()
        {
            uint numStates = (uint)1 << (_numPrevBits + _numPosBits);
            for (uint i = 0; i < numStates; i++)
            {
                _coders[i].Init();
            }
        }

        public void Encode(RangeEncoder rangeEncoder, uint pos, byte prevByte, byte symbol)
        {
            uint state = ((pos & _posMask) << _numPrevBits) + (uint)(prevByte >> (8 - _numPrevBits));
            _coders[state].Encode(rangeEncoder, symbol);
        }
    }

    internal struct State
    {
        public int Index;
        public void Init() { Index = 0; }
        public void UpdateChar() { Index = Index < 4 ? 0 : Index < 10 ? Index - 3 : Index - 6; }
        public void UpdateMatch() { Index = Index < 7 ? 7 : 10; }
    }

    internal struct LenDecoder
    {
        private BitDecoder _choice;
        private BitDecoder _choice2;
        private BitTreeDecoder[] _lowCoder;
        private BitTreeDecoder[] _midCoder;
        private BitTreeDecoder _highCoder;
        private uint _numPosStates;

        public void Create(uint numPosStates)
        {
            for (; _numPosStates < numPosStates; _numPosStates++)
            {
                _lowCoder[_numPosStates] = new BitTreeDecoder(3);
                _midCoder[_numPosStates] = new BitTreeDecoder(3);
            }
        }

        public void Init()
        {
            _choice.Init();
            _choice2.Init();
            for (uint posState = 0; posState < _numPosStates; posState++)
            {
                _lowCoder[posState].Init();
                _midCoder[posState].Init();
            }
            _highCoder.Init();
        }

        public LenDecoder(uint numPosStates)
        {
            _choice = new BitDecoder();
            _choice2 = new BitDecoder();
            _lowCoder = new BitTreeDecoder[16];
            _midCoder = new BitTreeDecoder[16];
            _highCoder = new BitTreeDecoder(8);
            _numPosStates = 0;
            Create(numPosStates);
        }

        public uint Decode(RangeDecoder rangeDecoder, uint posState)
        {
            if (_choice.Decode(rangeDecoder) == 0)
            {
                return _lowCoder[posState].Decode(rangeDecoder);
            }
            uint symbol = 8;
            if (_choice2.Decode(rangeDecoder) == 0)
            {
                symbol += _midCoder[posState].Decode(rangeDecoder);
            }
            else
            {
                symbol += 8 + _highCoder.Decode(rangeDecoder);
            }
            return symbol;
        }
    }

    internal struct LiteralDecoder
    {
        private struct Decoder2
        {
            private BitDecoder[] _decoders;

            public void Create()
            {
                _decoders = new BitDecoder[0x300];
            }

            public void Init()
            {
                for (int i = 0; i < 0x300; i++)
                {
                    _decoders[i].Init();
                }
            }

            public byte DecodeNormal(RangeDecoder rangeDecoder)
            {
                uint symbol = 1;
                do
                {
                    symbol = (symbol << 1) | _decoders[symbol].Decode(rangeDecoder);
                }
                while (symbol < 0x100);
                return (byte)symbol;
            }

            public byte DecodeWithMatchByte(RangeDecoder rangeDecoder, byte matchByte)
            {
                uint symbol = 1;
                do
                {
                    uint matchBit = (uint)(matchByte >> 7) & 1;
                    matchByte <<= 1;
                    uint bit = _decoders[((1 + matchBit) << 8) + symbol].Decode(rangeDecoder);
                    symbol = (symbol << 1) | bit;
                    if (matchBit != bit)
                    {
                        while (symbol < 0x100)
                        {
                            symbol = (symbol << 1) | _decoders[symbol].Decode(rangeDecoder);
                        }
                        break;
                    }
                }
                while (symbol < 0x100);
                return (byte)symbol;
            }
        }

        private Decoder2[] _coders;
        private int _numPrevBits;
        private int _numPosBits;
        private uint _posMask;

        public void Create(int numPosBits, int numPrevBits)
        {
            if (_coders != null && _numPrevBits == numPrevBits && _numPosBits == numPosBits)
            {
                return;
            }
            _numPosBits = numPosBits;
            _posMask = (uint)((1 << numPosBits) - 1);
            _numPrevBits = numPrevBits;
            uint numStates = (uint)1 << (_numPrevBits + _numPosBits);
            _coders = new Decoder2[numStates];
            for (uint i = 0; i < numStates; i++)
            {
                _coders[i].Create();
            }
        }

        public void Init()
        {
            uint numStates = (uint)1 << (_numPrevBits + _numPosBits);
            for (uint i = 0; i < numStates; i++)
            {
                _coders[i].Init();
            }
        }

        private uint GetState(uint pos, byte prevByte)
        {
            return ((pos & _posMask) << _numPrevBits) + (uint)(prevByte >> (8 - _numPrevBits));
        }

        public byte DecodeNormal(RangeDecoder rangeDecoder, uint pos, byte prevByte)
        {
            return _coders[GetState(pos, prevByte)].DecodeNormal(rangeDecoder);
        }

        public byte DecodeWithMatchByte(RangeDecoder rangeDecoder, uint pos, byte prevByte, byte matchByte)
        {
            return _coders[GetState(pos, prevByte)].DecodeWithMatchByte(rangeDecoder, matchByte);
        }
    }

    internal sealed class LzmaDecoder
    {
        private const int NumPosStatesBitsMax = 4;
        private const int NumPosStatesMax = 1 << NumPosStatesBitsMax;
        private const int NumStates = 12;
        private const int NumLenToPosStates = 4;
        private const int MatchMinLen = 2;
        private readonly BitDecoder[] _isMatchDecoders = new BitDecoder[NumStates << NumPosStatesBitsMax];
        private readonly BitDecoder[] _isRepDecoders = new BitDecoder[NumStates];
        private readonly BitDecoder[] _isRepG0Decoders = new BitDecoder[NumStates];
        private readonly BitDecoder[] _isRepG1Decoders = new BitDecoder[NumStates];
        private readonly BitDecoder[] _isRepG2Decoders = new BitDecoder[NumStates];
        private readonly BitDecoder[] _isRep0LongDecoders = new BitDecoder[NumStates << NumPosStatesBitsMax];
        private readonly BitTreeDecoder[] _posSlotDecoder = new BitTreeDecoder[NumLenToPosStates];
        private readonly BitDecoder[] _posDecoders = new BitDecoder[114];
        private readonly BitTreeDecoder _posAlignDecoder = new BitTreeDecoder(4);
        private readonly LenDecoder _lenDecoder = new LenDecoder(1 << NumPosStatesBitsMax);
        private readonly LenDecoder _repLenDecoder = new LenDecoder(1 << NumPosStatesBitsMax);
        private readonly LiteralDecoder _literalDecoder = new LiteralDecoder();
        private readonly uint[] _posSlotPrices = new uint[1 << 6];
        private readonly uint[] _distancesPrices = new uint[512];
        private readonly uint[] _alignPrices = new uint[16];
        private uint _posStateMask;
        private uint _dictionarySize;
        private uint _dictionarySizeCheck;
        private RangeDecoder _rangeDecoder;
        private OutWindow _outWindow;

        public LzmaDecoder()
        {
            for (int i = 0; i < NumLenToPosStates; i++)
            {
                _posSlotDecoder[i] = new BitTreeDecoder(6);
            }
        }

        public void SetDecoderProperties(byte[] properties)
        {
            if (properties == null || properties.Length != 5)
            {
                throw new ArgumentException("Invalid LZMA properties.");
            }
            int lc = properties[0] % 9;
            int remainder = properties[0] / 9;
            int lp = remainder % 5;
            int pb = remainder / 5;
            if (pb > NumPosStatesBitsMax)
            {
                throw new ArgumentException("Invalid pb in LZMA properties");
            }
            uint dictionarySize = 0;
            for (int i = 0; i < 4; i++)
            {
                dictionarySize += (uint)(properties[1 + i] << (i * 8));
            }
            SetDictionarySize(dictionarySize);
            SetLiteralProperties(lp, lc);
            Create(pbm: pb);
        }

        private void SetDictionarySize(uint dictionarySize)
        {
            _dictionarySize = dictionarySize;
            _dictionarySizeCheck = Math.Max(_dictionarySize, 1u);
            _outWindow?.Create(Math.Max(_dictionarySizeCheck, 1u << 12));
        }

        private void SetLiteralProperties(int lp, int lc)
        {
            _literalDecoder.Create(lp, lc);
        }

        private void Create(int pbm)
        {
            if (_outWindow == null)
            {
                _outWindow = new OutWindow();
            }
            _outWindow.Create(Math.Max(_dictionarySizeCheck, 1u << 12));
            _rangeDecoder = new RangeDecoder();
            _posStateMask = (uint)((1 << pbm) - 1);
        }

        public void Code(Stream inStream, Stream outStream, long inSize, long outSize, ICoderProgress progress)
        {
            _rangeDecoder.Init(inStream);
            _outWindow.Init(outStream);
            State state = new State();
            state.Init();
            uint rep0 = 0, rep1 = 0, rep2 = 0, rep3 = 0;
            ulong nowPos64 = 0;
            uint posStateMaskLocal = _posStateMask;
            while (outSize < 0 || nowPos64 < (ulong)outSize)
            {
                uint posState = (uint)nowPos64 & posStateMaskLocal;
                if (_isMatchDecoders[(state.Index << NumPosStatesBitsMax) + posState].Decode(_rangeDecoder) == 0)
                {
                    byte b;
                    byte prevByte = _outWindow.GetByte(0);
                    if (!state.IsCharState())
                    {
                        b = _literalDecoder.DecodeWithMatchByte(_rangeDecoder, (uint)nowPos64, prevByte, _outWindow.GetByte(rep0));
                    }
                    else
                    {
                        b = _literalDecoder.DecodeNormal(_rangeDecoder, (uint)nowPos64, prevByte);
                    }
                    _outWindow.PutByte(b);
                    state.UpdateChar();
                    nowPos64++;
                    continue;
                }
                uint len;
                if (_isRepDecoders[state.Index].Decode(_rangeDecoder) == 1)
                {
                    if (_isRepG0Decoders[state.Index].Decode(_rangeDecoder) == 0)
                    {
                        if (_isRep0LongDecoders[(state.Index << NumPosStatesBitsMax) + posState].Decode(_rangeDecoder) == 0)
                        {
                            state.UpdateShortRep();
                            _outWindow.PutByte(_outWindow.GetByte(rep0));
                            nowPos64++;
                            continue;
                        }
                    }
                    else
                    {
                        uint distance;
                        if (_isRepG1Decoders[state.Index].Decode(_rangeDecoder) == 0)
                        {
                            distance = rep1;
                        }
                        else
                        {
                            if (_isRepG2Decoders[state.Index].Decode(_rangeDecoder) == 0)
                            {
                                distance = rep2;
                            }
                            else
                            {
                                distance = rep3;
                                rep3 = rep2;
                            }
                            rep2 = rep1;
                        }
                        rep1 = rep0;
                        rep0 = distance;
                    }
                    len = _repLenDecoder.Decode(_rangeDecoder, posState) + MatchMinLen;
                    state.UpdateRep();
                }
                else
                {
                    rep3 = rep2;
                    rep2 = rep1;
                    rep1 = rep0;
                    len = _lenDecoder.Decode(_rangeDecoder, posState) + MatchMinLen;
                    state.UpdateMatch();
                    uint posSlot = _posSlotDecoder[GetLenToPosState(len)].Decode(_rangeDecoder);
                    if (posSlot >= 4)
                    {
                        int numDirectBits = (int)((posSlot >> 1) - 1);
                        rep0 = ((2 | (posSlot & 1)) << numDirectBits);
                        if (posSlot < 14)
                        {
                            rep0 += BitTreeDecoder.ReverseDecode(_posDecoders, rep0 - posSlot - 1, _rangeDecoder, numDirectBits);
                        }
                        else
                        {
                            rep0 += _rangeDecoder.DecodeDirectBits(numDirectBits - 4) << 4;
                            rep0 += _posAlignDecoder.ReverseDecode(_rangeDecoder);
                        }
                    }
                    else
                    {
                        rep0 = posSlot;
                    }
                    if (rep0 >= _outWindow.Total)
                    {
                        throw new DataMisalignedException("LZMA data is corrupt");
                    }
                }
                _outWindow.CopyBlock(rep0, len);
                nowPos64 += len;
            }
            _outWindow.Flush();
        }

        private static uint GetLenToPosState(uint len)
        {
            len -= 2;
            if (len < NumLenToPosStates)
            {
                return len;
            }
            return NumLenToPosStates - 1;
        }

        private struct State
        {
            public int Index;

            public void Init() { Index = 0; }

            public void UpdateChar()
            {
                if (Index < 4)
                {
                    Index = 0;
                }
                else if (Index < 10)
                {
                    Index -= 3;
                }
                else
                {
                    Index -= 6;
                }
            }

            public void UpdateMatch() { Index = Index < 7 ? 7 : 10; }
            public void UpdateRep() { Index = Index < 7 ? 8 : 11; }
            public void UpdateShortRep() { Index = Index < 7 ? 9 : 11; }
            public bool IsCharState() { return Index < 7; }
        }
    }

    internal sealed class OutWindow
    {
        private byte[] _buffer;
        private uint _pos;
        private uint _windowSize = 1;
        private Stream _stream;
        private bool _streamWasExhausted;

        public uint Total { get; private set; }

        public void Create(uint windowSize)
        {
            if (_buffer == null || _windowSize != windowSize)
            {
                _buffer = new byte[windowSize];
            }
            _windowSize = windowSize;
            _pos = 0;
        }

        public void Init(Stream stream)
        {
            ReleaseStream();
            _stream = stream;
            _streamWasExhausted = false;
            Total = 0;
            _pos = 0;
        }

        public void Init(Stream stream, bool solid)
        {
            if (!solid)
            {
                Init(stream);
            }
            else
            {
                _stream = stream;
            }
        }

        public void ReleaseStream()
        {
            Flush();
            _stream = null;
        }

        public void Flush()
        {
            uint size = _pos;
            if (size == 0)
            {
                return;
            }
            _stream.Write(_buffer, 0, (int)size);
            _pos = 0;
        }

        public void PutByte(byte b)
        {
            _buffer[_pos++] = b;
            if (_pos >= _windowSize)
            {
                Flush();
            }
        }

        public byte GetByte(uint distance)
        {
            uint pos = _pos - distance - 1;
            if (pos >= _windowSize)
            {
                return 0;
            }
            return _buffer[pos];
        }

        public void CopyBlock(uint distance, uint len)
        {
            for (uint i = 0; i < len; i++)
            {
                PutByte(GetByte(distance));
            }
        }
    }

    internal sealed class RangeEncoder
    {
        private const uint TopValue = 1 << 24;
        private Stream _stream;
        private ulong _low;
        private uint _range;
        private uint _cacheSize;
        private byte _cache;

        public void SetStream(Stream stream)
        {
            _stream = stream;
        }

        public void Init()
        {
            _low = 0;
            _range = uint.MaxValue;
            _cache = 0;
            _cacheSize = 1;
        }

        public void FlushData()
        {
            for (int i = 0; i < 5; i++)
            {
                ShiftLow();
            }
        }

        public void FlushStream()
        {
            _stream.Flush();
        }

        private void ShiftLow()
        {
            uint carry = (uint)(_low >> 32);
            if (carry != 0 || _low < 0xFF000000)
            {
                _stream.WriteByte((byte)(_cache + carry));
                for (; _cacheSize > 1; _cacheSize--)
                {
                    _stream.WriteByte((byte)(carry - 1));
                }
                _cache = (byte)((_low >> 24) & 0xFF);
            }
            _cacheSize++;
            _low = (_low & 0x00FFFFFF) << 8;
        }
    }
}

