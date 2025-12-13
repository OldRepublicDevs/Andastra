// Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/analyzers.py:845-869
// Original: class DiffAnalyzerFactory: ...
using System;
using CSharpKOTOR.Diff;
using CSharpKOTOR.Mods;

namespace KotorDiff.NET.Diff
{
    // Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/analyzers.py:89-104
    // Original: class DiffAnalyzer(ABC): ...
    public abstract class DiffAnalyzer
    {
        public abstract object Analyze(byte[] leftData, byte[] rightData, string identifier);
    }

    // Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/analyzers.py:845-869
    // Original: class DiffAnalyzerFactory: ...
    public static class DiffAnalyzerFactory
    {
        // Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/analyzers.py:848-869
        // Original: def get_analyzer(resource_type: str) -> DiffAnalyzer | None: ...
        public static DiffAnalyzer GetAnalyzer(string resourceType)
        {
            if (string.IsNullOrEmpty(resourceType))
            {
                return null;
            }

            string resourceTypeLower = resourceType.ToLowerInvariant();

            // 2DA analyzer
            if (resourceTypeLower == "2da" || resourceTypeLower == "twoda")
            {
                return new TwoDADiffAnalyzerWrapper();
            }

            // GFF analyzer
            if (resourceTypeLower == "gff" || 
                resourceTypeLower == "utc" || resourceTypeLower == "uti" || resourceTypeLower == "utp" ||
                resourceTypeLower == "ute" || resourceTypeLower == "utm" || resourceTypeLower == "utd" ||
                resourceTypeLower == "utw" || resourceTypeLower == "dlg" || resourceTypeLower == "are" ||
                resourceTypeLower == "git" || resourceTypeLower == "ifo" || resourceTypeLower == "gui" ||
                resourceTypeLower == "jrl" || resourceTypeLower == "fac")
            {
                return new GFFDiffAnalyzerWrapper();
            }

            // TLK analyzer
            if (resourceTypeLower == "tlk")
            {
                return new TLKDiffAnalyzerWrapper();
            }

            // SSF analyzer
            if (resourceTypeLower == "ssf")
            {
                return new SSFDiffAnalyzerWrapper();
            }

            return null;
        }
    }

    // Wrapper for TwoDADiffAnalyzer from CSharpKOTOR
    internal class TwoDADiffAnalyzerWrapper : DiffAnalyzer
    {
        public override object Analyze(byte[] leftData, byte[] rightData, string identifier)
        {
            var analyzer = new TwoDaDiffAnalyzer();
            return analyzer.Analyze(leftData, rightData, identifier);
        }
    }

    // Wrapper for GFF analyzer (needs to be created)
    internal class GFFDiffAnalyzerWrapper : DiffAnalyzer
    {
        public override object Analyze(byte[] leftData, byte[] rightData, string identifier)
        {
            // TODO: Implement GFFDiffAnalyzer
            // This should use CSharpKOTOR.Diff.GffDiff and generate ModificationsGFF
            throw new NotImplementedException("GFFDiffAnalyzer not yet implemented");
        }
    }

    // Wrapper for TLK analyzer (needs to be created)
    internal class TLKDiffAnalyzerWrapper : DiffAnalyzer
    {
        public override object Analyze(byte[] leftData, byte[] rightData, string identifier)
        {
            // TODO: Implement TLKDiffAnalyzer
            // This should use CSharpKOTOR.Diff.TlkDiff and generate ModificationsTLK
            throw new NotImplementedException("TLKDiffAnalyzer not yet implemented");
        }
    }

    // Wrapper for SSF analyzer (needs to be created)
    internal class SSFDiffAnalyzerWrapper : DiffAnalyzer
    {
        public override object Analyze(byte[] leftData, byte[] rightData, string identifier)
        {
            // TODO: Implement SSFDiffAnalyzer
            // This should use CSharpKOTOR.Diff.SsfDiff and generate ModificationsSSF
            throw new NotImplementedException("SSFDiffAnalyzer not yet implemented");
        }
    }
}

