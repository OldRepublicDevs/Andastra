using Andastra.Runtime.Core.Collision;
using Andastra.Runtime.Core.Interfaces;

namespace Andastra.Runtime.Games.Odyssey.Collision
{
    /// <summary>
    /// KOTOR 2: The Sith Lords (TSL) specific creature collision detection.
    /// Alias for K2CreatureCollisionDetector - both refer to the same game (swkotor2.exe).
    /// </summary>
    /// <remarks>
    /// TSL Creature Collision Detection:
    /// - Based on swkotor2.exe reverse engineering via Ghidra MCP
    /// - This is an alias for K2CreatureCollisionDetector (KOTOR 2 = TSL = The Sith Lords)
    /// - Bounding box structure pointer at offset 0x380 (different from K1's 0x340)
    /// - Reverse engineered functions:
    ///   - FUN_005479f0 @ 0x005479f0 (swkotor2.exe: main collision detection function using bounding box)
    ///   - FUN_004e17a0 @ 0x004e17a0 (swkotor2.exe: spatial query for objects in area)
    ///   - FUN_004f5290 @ 0x004f5290 (swkotor2.exe: detailed collision detection)
    ///   - FUN_0041d2c0 @ 0x0041d2c0 (swkotor2.exe: 2DA table lookup function, equivalent to FUN_00413350 in K1)
    ///   - FUN_0065a380 @ 0x0065a380 (swkotor2.exe: GetCreatureRadius wrapper, calls FUN_0041d2c0)
    /// - Bounding box structure layout (offset 0x380):
    ///   - Width at offset +0x14: `*(float *)(iVar1 + 0x14)`
    ///   - Height at offset +0xbc: `*(undefined4 *)(iVar1 + 0xbc)`
    ///   - Additional values at offset +0x30, +4
    /// - Located via string references:
    ///   - "GetCreatureRadius" @ 0x007bb128 (swkotor2.exe: function name string, referenced in code)
    ///   - "hitradius" column in appearance.2da (looked up via FUN_0041d2c0)
    /// - Cross-engine comparison:
    ///   - K1 (swkotor.exe): Bounding box at offset 0x340, radius at +8, width at +4
    ///   - TSL/K2 (swkotor2.exe): Bounding box at offset 0x380, width at +0x14, height at +0xbc
    ///   - Common: Both use appearance.2da hitradius via 2DA lookup function (FUN_00413350 in K1, FUN_0041d2c0 in TSL/K2)
    /// - Inheritance structure:
    ///   - BaseCreatureCollisionDetector (Runtime.Core.Collision): Common collision detection logic
    ///   - OdysseyCreatureCollisionDetector (Runtime.Games.Odyssey.Collision): Common Odyssey logic
    ///   - K2CreatureCollisionDetector (Runtime.Games.Odyssey.Collision): K2/TSL-specific (swkotor2.exe: 0x380)
    ///   - TSLCreatureCollisionDetector (Runtime.Games.Odyssey.Collision): Alias for K2CreatureCollisionDetector
    /// </remarks>
    public class TSLCreatureCollisionDetector : K2CreatureCollisionDetector
    {
        // This class is an alias for K2CreatureCollisionDetector
        // Both K2 and TSL refer to the same game: KOTOR 2: The Sith Lords (swkotor2.exe)
        // All functionality is inherited from K2CreatureCollisionDetector
    }
}

