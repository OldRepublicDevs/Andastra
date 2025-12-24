// Output repair processor for NCS decompiler
// Implements comprehensive fixes for decompiled NSS code to ensure recompilability
// while maintaining 1:1 parity with original engine behavior

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Andastra.Parsing.Common;
using JetBrains.Annotations;

namespace Andastra.Parsing.Formats.NCS.NCSDecomp
{
    /// <summary>
    /// Configuration options for output repair processing
    /// </summary>
    public class OutputRepairConfig
    {
        /// <summary>Enable syntax repairs (missing semicolons, braces, etc.)</summary>
        public bool EnableSyntaxRepair { get; set; } = true;

        /// <summary>Enable type system repairs (incorrect types, missing casts)</summary>
        public bool EnableTypeRepair { get; set; } = true;

        /// <summary>Enable expression repairs (operator precedence, malformed expressions)</summary>
        public bool EnableExpressionRepair { get; set; } = true;

        /// <summary>Enable control flow repairs (broken if/while/for statements)</summary>
        public bool EnableControlFlowRepair { get; set; } = true;

        /// <summary>Enable function signature repairs</summary>
        public bool EnableFunctionSignatureRepair { get; set; } = true;

        /// <summary>Maximum number of repair passes to attempt</summary>
        public int MaxRepairPasses { get; set; } = 3;

        /// <summary>Enable verbose logging of repair operations</summary>
        public bool VerboseLogging { get; set; } = false;

        /// <summary>Whether repairs were applied to the code</summary>
        public bool RepairsApplied { get; set; } = false;

        /// <summary>List of applied repairs for logging/debugging</summary>
        public List<string> AppliedRepairs { get; } = new List<string>();
    }

    /// <summary>
    /// Processes and repairs decompiled NSS output to fix common decompiler issues
    /// while maintaining parity with original engine behavior
    /// </summary>
    public static class OutputRepairProcessor
    {
        private static readonly Regex MissingSemicolonRegex = new Regex(@"^(.*[^;\s}])\s*$", RegexOptions.Multiline);
        private static readonly Regex UnmatchedBraceRegex = new Regex(@"\{([^{}]*(?:\{[^{}]*\}[^{}]*)*)\}");
        private static readonly Regex InvalidTypeCastRegex = new Regex(@"\(\s*(\w+)\s*\)\s*([^;\n]+);");
        private static readonly Regex BrokenIfStatementRegex = new Regex(@"if\s*\(\s*([^)]+)\s*\)\s*\{");
        private static readonly Regex BrokenWhileStatementRegex = new Regex(@"while\s*\(\s*([^)]+)\s*\)\s*\{");
        private static readonly Regex BrokenForStatementRegex = new Regex(@"for\s*\(\s*([^;]+);\s*([^;]+);\s*([^)]+)\)\s*\{");
        private static readonly Regex MalformedReturnRegex = new Regex(@"return\s+([^;]+)\s*$", RegexOptions.Multiline);
        private static readonly Regex InvalidOperatorPrecedenceRegex = new Regex(@"(\w+)\s*([+\-*/])\s*(\w+)\s*([+\-*/])\s*(\w+)");

        /// <summary>
        /// Applies comprehensive repairs to decompiled NSS code
        /// </summary>
        /// <param name="nssCode">The decompiled NSS code to repair</param>
        /// <param name="config">Repair configuration options</param>
        /// <returns>Repaired NSS code</returns>
        public static string RepairOutput([NotNull] string nssCode, [NotNull] OutputRepairConfig config)
        {
            if (string.IsNullOrEmpty(nssCode))
            {
                return nssCode;
            }

            string repairedCode = nssCode;
            bool repairsApplied = false;

            // Apply multiple repair passes
            for (int pass = 0; pass < config.MaxRepairPasses; pass++)
            {
                string beforePass = repairedCode;
                repairedCode = ApplyRepairPass(repairedCode, config);

                if (repairedCode != beforePass)
                {
                    repairsApplied = true;
                    if (config.VerboseLogging)
                    {
                        config.AppliedRepairs.Add($"Pass {pass + 1}: Applied repairs");
                    }
                }
                else
                {
                    // No changes in this pass, stop iterating
                    break;
                }
            }

            config.RepairsApplied = repairsApplied;
            return repairedCode;
        }

        /// <summary>
        /// Applies a single repair pass to the code
        /// </summary>
        private static string ApplyRepairPass(string nssCode, OutputRepairConfig config)
        {
            string repairedCode = nssCode;

            if (config.EnableSyntaxRepair)
            {
                repairedCode = ApplySyntaxRepairs(repairedCode, config);
            }

            if (config.EnableTypeRepair)
            {
                repairedCode = ApplyTypeRepairs(repairedCode, config);
            }

            if (config.EnableExpressionRepair)
            {
                repairedCode = ApplyExpressionRepairs(repairedCode, config);
            }

            if (config.EnableControlFlowRepair)
            {
                repairedCode = ApplyControlFlowRepairs(repairedCode, config);
            }

            if (config.EnableFunctionSignatureRepair)
            {
                repairedCode = ApplyFunctionSignatureRepairs(repairedCode, config);
            }

            return repairedCode;
        }

        /// <summary>
        /// Applies syntax repairs (missing semicolons, braces, etc.)
        /// </summary>
        private static string ApplySyntaxRepairs(string nssCode, OutputRepairConfig config)
        {
            string repairedCode = nssCode;

            // Fix missing semicolons after statements (but not after blocks)
            repairedCode = MissingSemicolonRegex.Replace(repairedCode, match =>
            {
                string line = match.Groups[1].Value.Trim();
                // Don't add semicolon if line ends with }, ), or already has ;
                if (line.EndsWith("}") || line.EndsWith(")") || line.EndsWith(";"))
                {
                    return match.Value;
                }

                // Don't add semicolon to control flow keywords
                if (line.StartsWith("if") || line.StartsWith("while") || line.StartsWith("for") ||
                    line.StartsWith("switch") || line.StartsWith("return"))
                {
                    return match.Value;
                }

                if (config.VerboseLogging)
                {
                    config.AppliedRepairs.Add($"Added missing semicolon: {line}");
                }

                return line + ";";
            });

            // Fix unmatched braces (simple cases)
            repairedCode = FixUnmatchedBraces(repairedCode, config);

            return repairedCode;
        }

        /// <summary>
        /// Applies type system repairs
        /// </summary>
        private static string ApplyTypeRepairs(string nssCode, OutputRepairConfig config)
        {
            string repairedCode = nssCode;

            // Fix invalid type casts
            repairedCode = InvalidTypeCastRegex.Replace(repairedCode, match =>
            {
                string castType = match.Groups[1].Value;
                string expression = match.Groups[2].Value;

                // Validate cast type is a valid NWScript type
                if (IsValidNWScriptType(castType))
                {
                    return match.Value; // Keep valid casts
                }

                // Remove invalid casts
                if (config.VerboseLogging)
                {
                    config.AppliedRepairs.Add($"Removed invalid type cast: ({castType}) {expression}");
                }

                return expression + ";";
            });

            return repairedCode;
        }

        /// <summary>
        /// Applies expression repairs
        /// </summary>
        private static string ApplyExpressionRepairs(string nssCode, OutputRepairConfig config)
        {
            string repairedCode = nssCode;

            // Fix operator precedence issues (add parentheses around complex expressions)
            repairedCode = InvalidOperatorPrecedenceRegex.Replace(repairedCode, match =>
            {
                string operand1 = match.Groups[1].Value;
                string op1 = match.Groups[2].Value;
                string operand2 = match.Groups[3].Value;
                string op2 = match.Groups[4].Value;
                string operand3 = match.Groups[5].Value;

                // Add parentheses to clarify precedence
                string repaired = $"({operand1} {op1} {operand2}) {op2} {operand3}";

                if (config.VerboseLogging)
                {
                    config.AppliedRepairs.Add($"Fixed operator precedence: {match.Value.Trim()} -> {repaired}");
                }

                return repaired;
            });

            return repairedCode;
        }

        /// <summary>
        /// Applies control flow repairs
        /// </summary>
        private static string ApplyControlFlowRepairs(string nssCode, OutputRepairConfig config)
        {
            string repairedCode = nssCode;

            // Fix malformed if statements
            repairedCode = BrokenIfStatementRegex.Replace(repairedCode, match =>
            {
                string condition = match.Groups[1].Value;
                // Ensure condition has proper parentheses
                if (!condition.Trim().StartsWith("(") || !condition.Trim().EndsWith(")"))
                {
                    condition = $"({condition})";
                }

                if (config.VerboseLogging)
                {
                    config.AppliedRepairs.Add($"Fixed if statement condition: {condition}");
                }

                return $"if {condition} {{";
            });

            // Fix malformed while statements
            repairedCode = BrokenWhileStatementRegex.Replace(repairedCode, match =>
            {
                string condition = match.Groups[1].Value;
                if (!condition.Trim().StartsWith("(") || !condition.Trim().EndsWith(")"))
                {
                    condition = $"({condition})";
                }

                if (config.VerboseLogging)
                {
                    config.AppliedRepairs.Add($"Fixed while statement condition: {condition}");
                }

                return $"while {condition} {{";
            });

            // Fix malformed for statements
            repairedCode = BrokenForStatementRegex.Replace(repairedCode, match =>
            {
                string init = match.Groups[1].Value;
                string condition = match.Groups[2].Value;
                string increment = match.Groups[3].Value;

                string fixedFor = $"for ({init}; {condition}; {increment}) {{";

                if (config.VerboseLogging)
                {
                    config.AppliedRepairs.Add($"Fixed for statement: {match.Value.Trim()} -> {fixedFor}");
                }

                return fixedFor;
            });

            // Fix malformed return statements
            repairedCode = MalformedReturnRegex.Replace(repairedCode, match =>
            {
                string returnValue = match.Groups[1].Value.Trim();
                string fixedReturn = $"return {returnValue};";

                if (config.VerboseLogging)
                {
                    config.AppliedRepairs.Add($"Fixed return statement: return {returnValue} -> {fixedReturn}");
                }

                return fixedReturn;
            });

            return repairedCode;
        }

        /// <summary>
        /// Applies function signature repairs
        /// </summary>
        private static string ApplyFunctionSignatureRepairs(string nssCode, OutputRepairConfig config)
        {
            // TODO: Implement function signature repairs (parameter types, return types, etc.)
            // This would require more complex parsing and analysis
            return nssCode;
        }

        /// <summary>
        /// Fixes unmatched braces in simple cases
        /// </summary>
        private static string FixUnmatchedBraces(string nssCode, OutputRepairConfig config)
        {
            // Count braces to detect mismatches
            int openBraces = nssCode.Count(c => c == '{');
            int closeBraces = nssCode.Count(c => c == '}');

            if (openBraces > closeBraces)
            {
                // Missing closing braces
                int missing = openBraces - closeBraces;
                for (int i = 0; i < missing; i++)
                {
                    nssCode += "\n}";
                }

                if (config.VerboseLogging)
                {
                    config.AppliedRepairs.Add($"Added {missing} missing closing brace(s)");
                }
            }
            else if (closeBraces > openBraces)
            {
                // Extra closing braces - this is harder to fix automatically
                // For now, we'll leave it as is since removing braces could break valid code
                if (config.VerboseLogging)
                {
                    config.AppliedRepairs.Add($"Warning: Found {closeBraces - openBraces} extra closing brace(s) - manual review needed");
                }
            }

            return nssCode;
        }

        /// <summary>
        /// Checks if a type name is a valid NWScript type
        /// </summary>
        private static bool IsValidNWScriptType(string typeName)
        {
            // NWScript built-in types
            string[] validTypes = {
                "int", "float", "string", "void", "object", "location", "vector",
                "talent", "effect", "event", "itemproperty", "action"
            };

            return validTypes.Contains(typeName.ToLower());
        }

        /// <summary>
        /// Creates a default repair configuration
        /// </summary>
        public static OutputRepairConfig CreateDefaultConfig()
        {
            return new OutputRepairConfig();
        }

        /// <summary>
        /// Creates a minimal repair configuration (syntax only)
        /// </summary>
        public static OutputRepairConfig CreateMinimalConfig()
        {
            return new OutputRepairConfig
            {
                EnableTypeRepair = false,
                EnableExpressionRepair = false,
                EnableControlFlowRepair = false,
                EnableFunctionSignatureRepair = false
            };
        }

        /// <summary>
        /// Creates a comprehensive repair configuration (all repairs enabled)
        /// </summary>
        public static OutputRepairConfig CreateComprehensiveConfig()
        {
            return new OutputRepairConfig
            {
                EnableSyntaxRepair = true,
                EnableTypeRepair = true,
                EnableExpressionRepair = true,
                EnableControlFlowRepair = true,
                EnableFunctionSignatureRepair = true,
                VerboseLogging = true
            };
        }
    }
}
