using System.Collections.Generic;

namespace ConvertKotorGame.Models
{
    public sealed class ConversionSummary
    {
        public int ConvertedCount { get; set; }
        public int CopiedCount { get; set; }
        public int FailedCount { get; set; }
        public int ContainersProcessed { get; set; }
        public Dictionary<string, int> ConvertedByType { get; private set; } = new Dictionary<string, int>();
        public Dictionary<string, int> SeenByType { get; private set; } = new Dictionary<string, int>();
        public Dictionary<string, int> ContainersByType { get; private set; } = new Dictionary<string, int>();
        public Dictionary<string, int> FailedByType { get; private set; } = new Dictionary<string, int>();
        /// <summary>
        /// Files where conversion was blocked (e.g. unmappable TSL→K1 NCS). Fallback bytes were written; entries are recorded in conversion_blocked_report.
        /// </summary>
        public List<(string RelativePath, string Reason)> BlockedFiles { get; } = new List<(string RelativePath, string Reason)>();
        public string OutputPath { get; set; }
    }
}
