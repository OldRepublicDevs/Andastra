// Matching PyKotor implementation at vendor/PyKotor/Tools/KotorDiff/src/kotordiff/formatters.py:78-94
// Original: class FormatterFactory: ...
using System;
using KotorDiff.NET.Diff.Objects;

namespace KotorDiff.NET.Formatters
{
    /// <summary>
    /// Factory for creating diff formatters with kotordiff logger integration.
    /// 1:1 port of FormatterFactory from vendor/PyKotor/Tools/KotorDiff/src/kotordiff/formatters.py:78-94
    /// </summary>
    public static class FormatterFactory
    {
        /// <summary>
        /// Create a formatter of the specified type.
        /// </summary>
        public static DiffFormatter CreateFormatter(DiffFormat formatType, Action<string> outputFunc = null, int width = 80)
        {
            switch (formatType)
            {
                case DiffFormat.Default:
                    return new DefaultFormatter(outputFunc);
                case DiffFormat.Unified:
                    return new UnifiedFormatter(outputFunc);
                case DiffFormat.Context:
                    return new ContextFormatter(outputFunc);
                case DiffFormat.SideBySide:
                    return new SideBySideFormatter(width, outputFunc);
                default:
                    throw new ArgumentException($"Unknown diff format: {formatType}");
            }
        }
    }
}

