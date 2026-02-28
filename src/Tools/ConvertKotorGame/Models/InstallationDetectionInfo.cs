using BioWare.Common;

namespace ConvertKotorGame.Models
{
    public sealed class InstallationDetectionInfo
    {
        public string Path { get; set; }
        public BioWareGame? Game { get; set; }
        public string Distribution { get; set; }
        public string PlatformSummary { get; set; }
    }
}
