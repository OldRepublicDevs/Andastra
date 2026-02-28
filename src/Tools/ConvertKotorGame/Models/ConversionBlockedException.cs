using System;

namespace ConvertKotorGame.Models
{
    /// <summary>
    /// Raised when a resource cannot be converted without breaking semantics for the target game.
    /// Caller should record the reason and, when provided, write FallbackData instead of the original.
    /// </summary>
    public sealed class ConversionBlockedException : Exception
    {
        public byte[] FallbackData { get; }

        public ConversionBlockedException(string message, byte[] fallbackData = null, Exception innerException = null)
            : base(message, innerException)
        {
            FallbackData = fallbackData;
        }
    }
}

