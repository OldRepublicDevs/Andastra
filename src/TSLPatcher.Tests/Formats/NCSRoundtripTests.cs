using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using TSLPatcher.Core.Common;
using Xunit;

namespace TSLPatcher.Tests.Formats;

/// <summary>
/// Tests for NCS roundtrip compilation/decompilation.
/// 1:1 port of test_ncs_roundtrip.py from tests/resource/formats/test_ncs_roundtrip.py
/// 
/// NOTE: These tests require NSS compilation and NCS decompilation functionality.
/// They may be skipped if the required functionality is not yet implemented.
/// </summary>
public class NCSRoundtripTests
{
    private const int DefaultSampleSize = 10;
    private static readonly int SampleLimit = int.TryParse(
        Environment.GetEnvironmentVariable("PYKOTOR_NCS_ROUNDTRIP_SAMPLE") ?? DefaultSampleSize.ToString(),
        out int limit) ? limit : DefaultSampleSize;

    /// <summary>
    /// Python: test_nss_roundtrip
    /// Tests that NSS -> NCS -> NSS -> NCS roundtrip is stable.
    /// </summary>
    [Fact(Skip = "Requires compile_nss and decompile_ncs functionality")]
    public void TestNssRoundtrip()
    {
        // Python: if not self.roundtrip_cases:
        //     self.skipTest("Vanilla_KOTOR_Script_Source submodule not available or no scripts collected")
        
        // Python: for game, script_path, library_lookup in self.roundtrip_cases:
        //     with self.subTest(f"{game.name}_{script_path.relative_to(self.vanilla_root)}"):
        //         source = script_path.read_text(encoding="windows-1252", errors="ignore")
        //         original_ncs = compile_nss(source, game, library_lookup=library_lookup)
        //
        //         decompiled_source = decompile_ncs(original_ncs, game)
        //         roundtrip_ncs = compile_nss(decompiled_source, game, library_lookup=library_lookup)
        //         roundtrip_source = decompile_ncs(roundtrip_ncs, game)
        //         roundtrip_ncs_second = compile_nss(roundtrip_source, game, library_lookup=library_lookup)
        //
        //         self.assertEqual(
        //             roundtrip_ncs,
        //             roundtrip_ncs_second,
        //             f"Roundtrip compilation not stable for {script_path}",
        //         )

        // TODO: Implement when compile_nss and decompile_ncs are available
        // This test requires:
        // 1. compile_nss(source, game, library_lookup) -> NCS
        // 2. decompile_ncs(ncs, game) -> string
        // 3. Access to Vanilla_KOTOR_Script_Source submodule or test scripts
    }
}

