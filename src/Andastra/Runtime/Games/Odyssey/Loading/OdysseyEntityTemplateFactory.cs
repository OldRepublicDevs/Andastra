using System.Numerics;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Parsing.Common;

namespace Andastra.Runtime.Engines.Odyssey.Loading
{
    /// <summary>
    /// Odyssey-specific implementation of IEntityTemplateFactory.
    /// Creates entities from UTC templates using EntityFactory.
    /// </summary>
    /// <remarks>
    /// Entity Template Factory (Odyssey):
    /// - Based on swkotor2.exe entity creation system
    /// - Located via string references: "TemplateResRef" @ 0x007bd00c
    /// - Template loading: FUN_005fb0f0 @ 0x005fb0f0 loads creature templates from GFF
    /// - Original implementation: Creates runtime entities from UTC GFF templates
    /// - This implementation wraps EntityFactory to provide Core-compatible interface
    /// - Module is required for template resource loading (UTC files from module archives)
    /// </remarks>
    public class OdysseyEntityTemplateFactory : IEntityTemplateFactory
    {
        private readonly EntityFactory _entityFactory;
        private readonly Module _module;

        /// <summary>
        /// Creates a new OdysseyEntityTemplateFactory.
        /// </summary>
        /// <param name="entityFactory">The EntityFactory to use for creating entities.</param>
        /// <param name="module">The module to load templates from.</param>
        public OdysseyEntityTemplateFactory(EntityFactory entityFactory, Module module)
        {
            _entityFactory = entityFactory ?? throw new System.ArgumentNullException("entityFactory");
            _module = module ?? throw new System.ArgumentNullException("module");
        }

        /// <summary>
        /// Creates a creature entity from a template ResRef at the specified position.
        /// </summary>
        /// <param name="templateResRef">The template resource reference (e.g., "n_darthmalak").</param>
        /// <param name="position">The spawn position.</param>
        /// <param name="facing">The facing direction in radians.</param>
        /// <returns>The created entity, or null if template not found or creation failed.</returns>
        public IEntity CreateCreatureFromTemplate(string templateResRef, Vector3 position, float facing)
        {
            if (string.IsNullOrEmpty(templateResRef))
            {
                return null;
            }

            // Use EntityFactory to create creature from template
            // Based on swkotor2.exe: EntityFactory.CreateCreatureFromTemplate loads UTC GFF and creates entity
            // Located via string references: "TemplateResRef" @ 0x007bd00c
            // Original implementation: Loads UTC GFF, reads creature properties, creates entity with components
            return _entityFactory.CreateCreatureFromTemplate(_module, templateResRef, position, facing);
        }
    }
}

