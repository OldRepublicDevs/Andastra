using System.Collections.Generic;

namespace OdyTools.Dialogs
{
    /// <summary>
    /// Provides inline help content for editors that would otherwise load from external markdown files.
    /// Content is sourced from PyKotor's wiki: https://github.com/OldRepublicDevs/PyKotor/wiki
    /// This eliminates the dependency on external wiki files bundled with the application.
    /// </summary>
    internal static class InlineEditorHelp
    {
        // Wiki filename (case-insensitive key) -> inline markdown content
        private static readonly Dictionary<string, string> InlineContent = new Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase)
        {
            ["LTR-File-Format.md"] = LtrFileFormatContent
        };

        /// <summary>
        /// Gets inline markdown content for a wiki filename, or null if no inline content is available.
        /// </summary>
        public static string GetInlineContent(string wikiFilename)
        {
            if (string.IsNullOrEmpty(wikiFilename))
                return null;

            // Normalize filename (handle with or without .md)
            string key = wikiFilename.EndsWith(".md", System.StringComparison.OrdinalIgnoreCase)
                ? wikiFilename
                : wikiFilename + ".md";

            return InlineContent.TryGetValue(key, out string content) ? content : null;
        }

        /// <summary>
        /// Returns true if inline content exists for the given wiki filename.
        /// </summary>
        public static bool HasInlineContent(string wikiFilename)
        {
            return GetInlineContent(wikiFilename) != null;
        }

        #region LTR File Format (from PyKotor wiki: https://github.com/OldRepublicDevs/PyKotor/wiki/LTR-File-Format)

        private const string LtrFileFormatContent = @"# KotOR LTR files format Documentation

LTR (Letter) resources store third-order Markov chain probability tables that the game uses to procedurally generate NPC names. The data encodes likelihoods for characters appearing at the start, middle, and end of names given zero, one, or two-character context.

## Table of Contents

- [file structure Overview](#file-structure-overview)
- [Binary format](#binary-format)
  - [header](#header)
  - [Single-Letter Block](#single-letter-block)
  - [double-Letter Blocks](#double-letter-blocks)
  - [Triple-Letter Blocks](#triple-letter-blocks)
- [Probability Blocks](#probability-blocks)
- [Name Generation Process](#name-generation-process)
- [Implementation Details](#implementation-details)

---

## file structure Overview

- KotOR always uses the **28-character alphabet** (`a–z` plus `'` and `-`). **Neverwinter Nights (NWN) used 26 characters**; the header explicitly stores the count. This is a **KotOR-specific difference** from NWN.
- LTR files are binary and consist of a short header followed by three probability tables (singles, doubles, triples) stored as contiguous float arrays.
- field offsets below trace directly to the reader implementations in [reone/ltrreader.cpp](https://github.com/th3w1zard1/reone/blob/master/src/libs/resource/format/ltrreader.cpp#L27-L74), [xoreos/ltrfile.cpp](https://github.com/th3w1zard1/xoreos/blob/master/src/aurora/ltrfile.cpp#L135-L168), and [KotOR.js/LTRObject.ts](https://github.com/th3w1zard1/KotOR.js/blob/master/src/resource/LTRObject.ts#L61-L117).

**Implementation:** [PyKotor LTR module](https://github.com/OldRepublicDevs/PyKotor/tree/master/Libraries/PyKotor/src/pykotor/resource/formats/ltr)

---

## Binary format

### header

| Name | type | offset | size | Description |
| --- | --- | --- | --- | --- |
| file type | char | 0 (0x00) | 4 | Always `""LTR ""` |
| file Version | char | 4 (0x04) | 4 | Always `""V1.0""` |
| Letter count | uint8 | 8 (0x08) | 1 | Must be 26 or 28 (KotOR uses 28) |

**Reference:** [vendor/reone/src/libs/resource/format/ltrreader.cpp:27-38](https://github.com/th3w1zard1/reone/blob/master/src/libs/resource/format/ltrreader.cpp#L27-L38)

### Single-Letter Block

Immediately after the header, the **single-letter** probabilities are stored as three arrays of `letter_count` floats (start, middle, end). For the KotOR alphabet that is `28 × 3 × 4 = 336` bytes.

| Section | Entries | Description |
| --- | --- | --- |
| Start | 28 floats | Probability of each letter starting a name |
| Middle | 28 floats | Probability of each letter appearing mid-name |
| End | 28 floats | Probability of each letter ending a name |

### double-Letter Blocks

For each character in the alphabet there is a **double-letter** block (context length 1). Each block repeats the same start/middle/end layout (28 floats each).

Total size (KotOR): `28 (letters) × 3 (position arrays) × 28 (values) × 4 bytes = 9,408 bytes`.

### Triple-Letter Blocks

The **triple-letter** section encodes 2-character context. There are `letter_count × letter_count` blocks (28 × 28 = 784 for KotOR), each with start/middle/end arrays of 28 floats.

Total size (KotOR): `28 × 28 × 3 × 28 × 4 = 73,472 bytes`.

**Reference:** [PyKotor ltr_data.py:18-44](https://github.com/OldRepublicDevs/PyKotor/blob/master/Libraries/PyKotor/src/pykotor/resource/formats/ltr/ltr_data.py#L18-L44)

---

## Probability Blocks

Each block is represented by the `LTRBlock` class in PyKotor, mirroring the `LetterSet` structs in the reverse-engineered engines. Blocks store cumulative probabilities (monotonically increasing floats) that are compared against random roll values.

- **Singles (`_singles`):** No context; used for the very first character.
- **Doubles (`_doubles`):** Indexed by the previous character; used for the second character.
- **Triples (`_triples`):** Two-dimensional array indexed by the previous two characters; used for every character after the second.

**Reference:** [vendor/reone/include/reone/resource/ltr.h:24-48](https://github.com/th3w1zard1/reone/blob/master/include/reone/resource/ltr.h#L24-L48)  
**Reference:** [vendor/xoreos/src/aurora/ltrfile.h:57-76](https://github.com/th3w1zard1/xoreos/blob/master/src/aurora/ltrfile.h#L57-L76)

---

## Name Generation Process

The runtime algorithm (implemented in PyKotor, Reone, Xoreos, KotOR.js, and Kotor.NET) follows these steps:

1. **Seed/Random Setup** – optional deterministic seed for reproducible results.
2. **First Character** – roll against `_singles.start`.
3. **Second Character** – roll against `_doubles[index(previous)], start`.
4. **Third Character** – roll against `_triples[index(prev2)][index(prev1)].start`.
5. **Subsequent Characters** – roll against `_triples` middle probabilities; termination decisions use `_triples` end probabilities plus heuristics to avoid overly short names.
6. **Post-processing** – capitalize and ensure minimum length; retries occur if probabilities fail to produce a valid sequence.

**Reference:** [PyKotor ltr_data.py:166-283](https://github.com/OldRepublicDevs/PyKotor/blob/master/Libraries/PyKotor/src/pykotor/resource/formats/ltr/ltr_data.py#L166-L283)

---

## Implementation Details

- **Binary Reader/Writer:** [PyKotor io_ltr.py](https://github.com/OldRepublicDevs/PyKotor/blob/master/Libraries/PyKotor/src/pykotor/resource/formats/ltr/io_ltr.py)
- **data model:** [PyKotor ltr_data.py](https://github.com/OldRepublicDevs/PyKotor/blob/master/Libraries/PyKotor/src/pykotor/resource/formats/ltr/ltr_data.py)
- **Reference Implementations:**
  - [vendor/reone/src/libs/resource/format/ltrreader.cpp](https://github.com/th3w1zard1/reone/blob/master/src/libs/resource/format/ltrreader.cpp)
  - [vendor/xoreos/src/aurora/ltrfile.cpp](https://github.com/th3w1zard1/xoreos/blob/master/src/aurora/ltrfile.cpp)
  - [vendor/KotOR.js/src/resource/LTRObject.ts](https://github.com/th3w1zard1/KotOR.js/blob/master/src/resource/LTRObject.ts)
  - [vendor/Kotor.NET/Kotor.NET/Formats/KotorLTR/LTR.cs](https://github.com/th3w1zard1/Kotor.NET/blob/master/Kotor.NET/Formats/KotorLTR/LTR.cs)

Because PyKotor uses the structure described in the cited implementations, LTR resources can be exchanged with those toolchains without conversion steps.

---

*Source: [PyKotor wiki - LTR File Format](https://github.com/OldRepublicDevs/PyKotor/wiki/LTR-File-Format)*";

        #endregion
    }
}
