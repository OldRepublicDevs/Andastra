using System;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Core.Interfaces.Components;

namespace Andastra.Runtime.Core.Animation
{
    /// <summary>
    /// System that updates appear animation fade-in effects for entities.
    /// </summary>
    /// <remarks>
    /// Appear Animation Fade System:
    /// - Based on swkotor.exe and swkotor2.exe appear animation system
    /// - Located via string references: "FadeTime" @ 0x007c60ec (fade duration in swkotor2.exe)
    /// - Original implementation: Entities with appear animation fade in from opacity 0.0 to 1.0 over fade duration
    /// - Fade duration: Typically 0.75 seconds (matches ActionDestroyObject fade-out duration)
    /// - Updates IRenderableComponent.Opacity property over time for smooth fade-in effect
    /// - Clears appear animation flag when fade-in completes (Opacity >= 1.0)
    /// - Rendering system uses IRenderableComponent.Opacity for alpha blending during rendering
    /// - Similar pattern to ActionDestroyObject fade-out, but in reverse (fade-in instead of fade-out)
    /// </remarks>
    public class AppearAnimationFadeSystem
    {
        private readonly IWorld _world;

        public AppearAnimationFadeSystem(IWorld world)
        {
            _world = world ?? throw new ArgumentNullException("world");
        }

        /// <summary>
        /// Updates all appear animation fade-in effects in the world.
        /// </summary>
        /// <param name="deltaTime">Time elapsed since last update.</param>
        public void Update(float deltaTime)
        {
            if (_world == null)
            {
                return;
            }

            // Get current simulation time for fade calculations
            float currentTime = 0.0f;
            if (_world.TimeManager != null)
            {
                currentTime = _world.TimeManager.SimulationTime;
            }

            // Iterate over all entities to find those with appear animation active
            foreach (IEntity entity in _world.GetAllEntities())
            {
                // Check if entity has appear animation flag set
                if (entity is Core.Entities.Entity entityImpl)
                {
                    bool hasAppearAnimation = entityImpl.GetData<bool>("AppearAnimation", false);
                    if (!hasAppearAnimation)
                    {
                        continue;
                    }

                    // Get fade timing data
                    float startTime = entityImpl.GetData<float>("AppearAnimationStartTime", 0.0f);
                    float duration = entityImpl.GetData<float>("AppearAnimationDuration", 0.75f);

                    // Calculate elapsed time since fade started
                    float elapsedTime = currentTime - startTime;

                    // Get renderable component to update opacity
                    IRenderableComponent renderable = entity.GetComponent<IRenderableComponent>();
                    if (renderable == null)
                    {
                        // No renderable component, clear flag and continue
                        entityImpl.SetData("AppearAnimation", false);
                        continue;
                    }

                    // Calculate fade progress (0.0 to 1.0)
                    float fadeProgress = 0.0f;
                    if (duration > 0.0f)
                    {
                        fadeProgress = Math.Min(1.0f, elapsedTime / duration);
                    }
                    else
                    {
                        // Duration is 0 or negative, complete immediately
                        fadeProgress = 1.0f;
                    }

                    // Update opacity: fade from 0.0 to 1.0
                    renderable.Opacity = fadeProgress;

                    // If fade is complete, clear the appear animation flag
                    if (fadeProgress >= 1.0f)
                    {
                        entityImpl.SetData("AppearAnimation", false);
                        // Opacity is already at 1.0, no need to update it
                    }
                }
            }
        }
    }
}

