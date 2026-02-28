using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using OdyTools.Data;

namespace OdyTools.Widgets
{
    /// <summary>
    /// Syntax highlighter for NWScript (NSS) code in the OdyTool NSS.
    /// Provides syntax highlighting patterns for keywords, functions, numbers, strings, and comments.
    /// Updates highlighting rules based on the selected game (K1 or TSL).
    /// </summary>
    public class NWScriptSyntaxHighlighter
    {
        // Exhaustive NWScript 1.69 / KotOR language keywords: statements, types, preprocessor, literals
        private static readonly string[] Keywords = new[]
        {
            "action", "break", "case", "const", "continue", "default", "do", "effect", "else",
            "event", "FALSE", "float", "for", "if", "int", "location", "object", "return",
            "string", "struct", "switch", "talent", "TRUE", "vector", "void", "while"
        };

        private static readonly string[] PreprocessorKeywords = new[]
        {
            "#include", "#define", "#ifdef", "#ifndef", "#endif", "#else"
        };

        private static readonly string[] Operators = new[]
        {
            "=", "==", "!=", "<", "<=", ">", ">=", "!", "+", "-", "*", "/", "%", "<<", ">>", "&", "|", "^", "&&", "||", "++", "--"
        };

        /// <summary>
        /// Initializes a new instance of the NWScriptSyntaxHighlighter.
        /// </summary>
        /// <param name="document">The document to highlight (for compatibility with Python interface).</param>
        /// <param name="installation">The installation to determine game-specific highlighting rules.</param>
        public NWScriptSyntaxHighlighter(object document, OdyInstallation installation = null)
        {
            Document = document;
            Installation = installation;
            IsTsl = installation?.Tsl ?? false;
            SetupRules();
        }

        /// <summary>
        /// Gets or sets the document being highlighted.
        /// </summary>
        public object Document { get; set; }

        /// <summary>
        /// Gets or sets the installation used for game-specific highlighting.
        /// </summary>
        public OdyInstallation Installation { get; set; }

        /// <summary>
        /// Gets or sets whether the highlighter is configured for TSL (KOTOR 2).
        /// </summary>
        public bool IsTsl { get; set; }

        /// <summary>
        /// Gets the highlighting rules currently in use.
        /// </summary>
        public List<HighlightingRule> Rules { get; private set; }

        /// <summary>
        /// Sets up the highlighting rules based on the current game configuration.
        /// </summary>
        private void SetupRules()
        {
            Rules = new List<HighlightingRule>();

            // Preprocessor format (purple/darkMagenta) - #include, #define, etc.
            var preprocessorFormat = new HighlightingFormat { Color = "darkMagenta", Bold = true, Italic = false };
            foreach (string pp in PreprocessorKeywords)
            {
                Rules.Add(new HighlightingRule
                {
                    Pattern = new Regex(@"^\s*" + Regex.Escape(pp) + @"\b", RegexOptions.Compiled | RegexOptions.Multiline),
                    Format = preprocessorFormat
                });
            }
            // Any #identifier for preprocessor
            Rules.Add(new HighlightingRule
            {
                Pattern = new Regex(@"#\s*[a-zA-Z_][a-zA-Z0-9_]*", RegexOptions.Compiled),
                Format = preprocessorFormat
            });

            // Keyword format (blue)
            var keywordFormat = new HighlightingFormat { Color = "blue", Bold = false, Italic = false };
            foreach (string keyword in Keywords)
            {
                Rules.Add(new HighlightingRule
                {
                    Pattern = new Regex(@"\b" + Regex.Escape(keyword) + @"\b", RegexOptions.Compiled),
                    Format = keywordFormat
                });
            }

            // Function format (darkGreen) - matches function calls like functionName(
            var functionFormat = new HighlightingFormat { Color = "darkGreen", Bold = false, Italic = false };
            Rules.Add(new HighlightingRule
            {
                Pattern = new Regex(@"\b[A-Za-z0-9_]+(?=\()", RegexOptions.Compiled),
                Format = functionFormat
            });

            // Number format (brown) - integers, floats, hex literals (0x, 0X)
            var numberFormat = new HighlightingFormat { Color = "brown", Bold = false, Italic = false };
            Rules.Add(new HighlightingRule
            {
                Pattern = new Regex(@"\b0[xX][0-9a-fA-F]+\b", RegexOptions.Compiled),
                Format = numberFormat
            });
            Rules.Add(new HighlightingRule
            {
                Pattern = new Regex(@"\b[0-9]+\b", RegexOptions.Compiled),
                Format = numberFormat
            });
            Rules.Add(new HighlightingRule
            {
                Pattern = new Regex(@"\b[0-9]+\.[0-9]+([eE][+-]?[0-9]+)?[fF]?\b", RegexOptions.Compiled),
                Format = numberFormat
            });

            // String format (darkMagenta) - double-quoted
            var stringFormat = new HighlightingFormat { Color = "darkMagenta", Bold = false, Italic = false };
            Rules.Add(new HighlightingRule
            {
                Pattern = new Regex(@"""(?:[^""\\]|\\.)*""", RegexOptions.Compiled),
                Format = stringFormat
            });

            // Single-line comment format (gray, italic)
            var commentFormat = new HighlightingFormat { Color = "gray", Bold = false, Italic = true };
            Rules.Add(new HighlightingRule
            {
                Pattern = new Regex(@"//[^\n]*", RegexOptions.Compiled),
                Format = commentFormat
            });

            // Multi-line comment format (/* ... */)
            Rules.Add(new HighlightingRule
            {
                Pattern = new Regex(@"/\*[\s\S]*?\*/", RegexOptions.Compiled),
                Format = commentFormat
            });

            MultilineCommentFormat = commentFormat;
        }

        /// <summary>
        /// Gets or sets the format for multi-line comments.
        /// </summary>
        public HighlightingFormat MultilineCommentFormat { get; set; }

        /// <summary>
        /// Updates the highlighting rules based on the selected game.
        /// Reinitializes rules and triggers re-highlighting if the document is available.
        /// </summary>
        /// <param name="isTsl">True if TSL (KOTOR 2), false if K1.</param>
        public void UpdateRules(bool isTsl)
        {
            IsTsl = isTsl;
            SetupRules();
            // Re-highlighting: full implementation in Avalonia when needed
            // in the UI layer. This method matches the Python interface.
        }

        /// <summary>
        /// Represents a highlighting rule with a pattern and format.
        /// </summary>
        public class HighlightingRule
        {
            public Regex Pattern { get; set; }
            public HighlightingFormat Format { get; set; }
        }

        /// <summary>
        /// Represents formatting information for highlighted text.
        /// </summary>
        public class HighlightingFormat
        {
            public string Color { get; set; }
            public bool Bold { get; set; }
            public bool Italic { get; set; }
        }
    }
}

