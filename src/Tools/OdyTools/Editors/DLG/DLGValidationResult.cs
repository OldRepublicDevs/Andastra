using BioWare.Resource.Formats.GFF.Generics.DLG;

namespace OdyTools.Editors.DLG
{
    public enum DLGValidationSeverity
    {
        Info,
        Warning,
        Error
    }

    public sealed class DLGValidationResult
    {
        public DLGValidationSeverity Severity { get; set; }
        public string RuleId { get; set; }
        public string Message { get; set; }
        public DLGNode NodeReference { get; set; }
    }
}
