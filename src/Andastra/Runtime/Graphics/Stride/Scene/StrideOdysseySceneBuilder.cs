using System;
using JetBrains.Annotations;

namespace Andastra.Runtime.Stride.Scene
{
    /// <summary>
    /// [OBSOLETE] This class has been replaced with graphics-agnostic scene builders.
    /// Use Andastra.Runtime.Games.Odyssey.Scene.OdysseySceneBuilder instead.
    /// </summary>
    /// <remarks>
    /// Migration:
    /// - Old: StrideOdysseySceneBuilder (graphics-backend specific)
    /// - New: OdysseySceneBuilder (graphics-backend agnostic, works with MonoGame and Stride)
    /// - Location: Runtime.Games.Odyssey.Scene.OdysseySceneBuilder
    ///
    /// Architecture Change:
    /// - Scene builders are now engine-specific (not graphics-backend specific)
    /// - Graphics backends (MonoGame, Stride) consume abstract scene data structures
    /// - This allows any engine to work with any graphics backend
    /// </remarks>
    [Obsolete("Use Andastra.Runtime.Games.Odyssey.Scene.OdysseySceneBuilder instead. Scene builders are now graphics-backend agnostic.")]
    public class StrideOdysseySceneBuilder
    {
        // This class is obsolete - use OdysseySceneBuilder from Runtime.Games.Odyssey.Scene
    }
}
