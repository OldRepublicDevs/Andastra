using System.Threading;
using System.Threading.Tasks;
using CSharpKOTOR.Resources;

namespace Odyssey.Content.Interfaces
{
    /// <summary>
    /// Interface for content converters that transform KOTOR assets to runtime format.
    /// </summary>
    public interface IContentConverter<TInput, TOutput>
    {
        /// <summary>
        /// Converts an input asset to the output format.
        /// </summary>
        Task<TOutput> ConvertAsync(TInput input, ConversionContext ctx, CancellationToken ct);
        
        /// <summary>
        /// Checks if this converter can handle the given resource.
        /// </summary>
        bool CanConvert(ResourceIdentifier id);
        
        /// <summary>
        /// The resource types this converter handles.
        /// </summary>
        ResourceType[] SupportedTypes { get; }
    }
    
    /// <summary>
    /// Context for content conversion.
    /// </summary>
    public class ConversionContext
    {
        /// <summary>
        /// The resource provider for loading dependencies.
        /// </summary>
        public IGameResourceProvider ResourceProvider { get; set; }
        
        /// <summary>
        /// The content cache for storing converted assets.
        /// </summary>
        public IContentCache ContentCache { get; set; }
        
        /// <summary>
        /// The game type (K1/K2).
        /// </summary>
        public GameType GameType { get; set; }
        
        /// <summary>
        /// The current module name (if applicable).
        /// </summary>
        public string ModuleName { get; set; }
        
        /// <summary>
        /// Converter version for cache invalidation.
        /// </summary>
        public int ConverterVersion { get; set; }
    }
}

