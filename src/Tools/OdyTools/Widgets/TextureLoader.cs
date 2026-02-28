using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BioWare.Extract;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.TPC;
using TGAImage = BioWare.Resource.Formats.TPC.TGAImage;

namespace OdyTools.Widgets
{
    // Request tuple: (resref, restype, context, icon_size)
    public class TextureLoadRequest
    {
        public string ResRef { get; set; }
        public ResourceType ResType { get; set; }
        public object Context { get; set; }
        public int IconSize { get; set; }

        public TextureLoadRequest(string resRef, ResourceType resType, object context, int iconSize)
        {
            ResRef = resRef;
            ResType = resType;
            Context = context;
            IconSize = iconSize;
        }
    }

    // Result tuple: (context, mipmap_data, error)
    public class TextureLoadResult
    {
        public object Context { get; set; }
        public byte[] MipmapData { get; set; }
        public string Error { get; set; }

        public TextureLoadResult(object context, byte[] mipmapData, string error)
        {
            Context = context;
            MipmapData = mipmapData;
            Error = error;
        }
    }

    public class TextureLoader
    {
        private string _installationPath;
        private bool _isTsl;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _loaderTask;
        private Installation _installation;

        private readonly ConcurrentQueue<TextureLoadRequest> _requestQueue;
        private readonly ConcurrentQueue<TextureLoadResult> _resultQueue;

        // Sentinel value for shutdown (matching PyKotor SHUTDOWN_SENTINEL)
        private static readonly TextureLoadRequest ShutdownSentinel = new TextureLoadRequest(null, ResourceType.INVALID, null, 0);

        public TextureLoader(string installationPath, bool isTsl)
        {
            _installationPath = installationPath;
            _isTsl = isTsl;
            _cancellationTokenSource = new CancellationTokenSource();
            _requestQueue = new ConcurrentQueue<TextureLoadRequest>();
            _resultQueue = new ConcurrentQueue<TextureLoadResult>();
        }

        // Public method to queue a texture load request
        public void QueueTextureLoad(string resRef, ResourceType resType, object context, int iconSize = 64)
        {
            var request = new TextureLoadRequest(resRef, resType, context, iconSize);
            _requestQueue.Enqueue(request);
        }

        // Public method to retrieve results (non-blocking)
        public bool TryGetResult(out TextureLoadResult result)
        {
            return _resultQueue.TryDequeue(out result);
        }

        // Public method to request shutdown
        public void RequestShutdown()
        {
            _requestQueue.Enqueue(ShutdownSentinel);
        }

        public void Start()
        {
            _loaderTask = Task.Run(() => RunLoader(_cancellationTokenSource.Token));
        }

        private void RunLoader(CancellationToken cancellationToken)
        {
            try
            {
                // Initialize installation inside the loader task
                // (Installation objects can't be shared across threads safely, so we initialize here)
                // Note: Installation auto-detects K1 vs K2 based on game files
                // The _isTsl parameter is stored for compatibility but not used - Installation auto-detects
                _installation = new Installation(_installationPath);
                System.Console.WriteLine($"TextureLoader started for: {_installationPath}");

                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        // Use TryDequeue with a timeout equivalent (check every 500ms)
                        TextureLoadRequest request = null;
                        if (_requestQueue.TryDequeue(out request))
                        {
                            // Check for shutdown sentinel
                            if (request == ShutdownSentinel || request.ResRef == null)
                            {
                                System.Console.WriteLine("TextureLoader received shutdown signal");
                                break;
                            }

                            // Unpack request (guaranteed to be valid at this point)
                            string resref = request.ResRef;
                            ResourceType restype = request.ResType;
                            object context = request.Context;
                            int iconSize = request.IconSize;

                            // Load the texture
                            try
                            {
                                byte[] mipmapData = LoadTextureInternal(_installation, resref, restype, iconSize);
                                _resultQueue.Enqueue(new TextureLoadResult(context, mipmapData, null));
                            }
                            catch (Exception e)
                            {
                                string errorMsg = $"Error loading texture {resref}: {e}";
                                System.Console.WriteLine($"TextureLoader warning: {errorMsg}");
                                _resultQueue.Enqueue(new TextureLoadResult(context, null, errorMsg));
                                // Don't shutdown on individual texture load errors - continue processing
                            }
                        }
                        else
                        {
                            // No request available, wait a bit before checking again
                            Thread.Sleep(50); // Small delay to prevent tight loop
                        }
                    }
                    catch (Exception e)
                    {
                        // Log error but don't crash the process - continue processing other requests
                        System.Console.WriteLine($"TextureLoader error processing request: {e}");
                        // Continue the loop instead of crashing
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"TextureLoader fatal error: {ex}");
            }
            finally
            {
                _installation = null; // Clear reference on shutdown
                System.Console.WriteLine("TextureLoader shutting down");
            }
        }

        private byte[] LoadTextureInternal(Installation installation, string resref, ResourceType restype, int iconSize = 64)
        {
            // Get texture data from installation
            var textureResult = installation.Resources.LookupResource(resref, restype);
            if (textureResult == null || textureResult.Data == null)
            {
                throw new FileNotFoundException($"Texture not found: {resref}.{restype.Extension}");
            }

            byte[] textureBytes = textureResult.Data;

            TPCMipmap mipmap;
            if (restype == ResourceType.TPC)
            {
                var tpc = TPCAuto.ReadTpc(textureBytes);
                mipmap = GetBestMipmap(tpc, iconSize);
            }
            else if (restype == ResourceType.TGA)
            {
                // TGA - try to read via TPC format or use fallback
                try
                {
                    var tpc = TPCAuto.ReadTpc(textureBytes);
                    mipmap = GetBestMipmap(tpc, iconSize);
                }
                catch
                {
                    // Fall back to TGA reader
                    mipmap = LoadTgaViaTgaReader(textureBytes, iconSize);
                }
            }
            else
            {
                throw new NotSupportedException($"Unsupported texture type: {restype}");
            }

            // Serialize mipmap data for cross-thread transfer
            return SerializeMipmap(mipmap);
        }

        private TPCMipmap GetBestMipmap(TPC tpc, int targetSize)
        {
            if (tpc == null || tpc.Layers == null || tpc.Layers.Count == 0)
            {
                throw new ArgumentException("TPC has no layers");
            }

            var layer = tpc.Layers[0];
            if (layer.Mipmaps == null || layer.Mipmaps.Count == 0)
            {
                throw new ArgumentException("TPC has no mipmaps");
            }

            var mipmaps = layer.Mipmaps;

            // Find mipmap closest to target size
            TPCMipmap bestMipmap = mipmaps[0];
            int bestDiff = Math.Abs(bestMipmap.Width - targetSize);

            for (int i = 1; i < mipmaps.Count; i++)
            {
                int diff = Math.Abs(mipmaps[i].Width - targetSize);
                if (diff < bestDiff)
                {
                    bestDiff = diff;
                    bestMipmap = mipmaps[i];
                }
            }

            return bestMipmap;
        }

        private TPCMipmap LoadTgaViaTgaReader(byte[] data, int iconSize)
        {
            // Use TGA reader from BioWare
            TGAImage tga;
            using (var ms = new MemoryStream(data))
            {
                tga = TGA.ReadTga(ms);
            }

            // TGA.ReadTga already returns RGBA8888 data (see TGA.cs implementation)
            byte[] rgbaData = tga.Data; // Already RGBA8888 from TGA reader

            // Resize to icon size if needed
            if (tga.Width != iconSize || tga.Height != iconSize)
            {
                rgbaData = ResizeImage(rgbaData, tga.Width, tga.Height, iconSize, iconSize);
            }

            // Create TPCMipmap
            return new TPCMipmap(iconSize, iconSize, TPCTextureFormat.RGBA, rgbaData);
        }

        /// <summary>
        /// Resizes image data using nearest-neighbor interpolation algorithm.
        /// This method provides high-quality image resizing while maintaining pixel-perfect accuracy
        /// for cases where exact pixel mapping is required.
        /// </summary>
        /// <param name="sourceData">Source image data in RGBA8888 format (4 bytes per pixel)</param>
        /// <param name="sourceWidth">Width of the source image in pixels</param>
        /// <param name="sourceHeight">Height of the source image in pixels</param>
        /// <param name="targetWidth">Desired width of the target image in pixels</param>
        /// <param name="targetHeight">Desired height of the target image in pixels</param>
        /// <returns>Resized image data in RGBA8888 format</returns>
        /// <exception cref="ArgumentNullException">Thrown when sourceData is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when dimensions are invalid</exception>
        private byte[] ResizeImage(byte[] sourceData, int sourceWidth, int sourceHeight, int targetWidth, int targetHeight)
        {
            // Validate input parameters
            if (sourceData == null)
                throw new ArgumentNullException(nameof(sourceData));

            if (sourceWidth <= 0 || sourceHeight <= 0 || targetWidth <= 0 || targetHeight <= 0)
                throw new ArgumentOutOfRangeException("Image dimensions must be positive");

            if (sourceData.Length < sourceWidth * sourceHeight * 4)
                throw new ArgumentException("Source data buffer is too small for the specified dimensions");

            // Handle trivial case - same dimensions
            if (sourceWidth == targetWidth && sourceHeight == targetHeight)
            {
                return (byte[])sourceData.Clone();
            }

            // Allocate target buffer
            byte[] targetData = new byte[targetWidth * targetHeight * 4];

            // Calculate scaling ratios with floating point precision for accurate mapping
            double scaleX = (double)sourceWidth / targetWidth;
            double scaleY = (double)sourceHeight / targetHeight;

            // Pre-calculate source dimensions for bounds checking
            int sourcePixels = sourceWidth * sourceHeight;
            int targetPixels = targetWidth * targetHeight;

            // Perform nearest-neighbor interpolation
            for (int targetY = 0; targetY < targetHeight; targetY++)
            {
                for (int targetX = 0; targetX < targetWidth; targetX++)
                {
                    // Calculate source coordinates using floating-point arithmetic
                    // Add 0.5 to sample from the center of the target pixel
                    double sourceXFloat = (targetX + 0.5) * scaleX - 0.5;
                    double sourceYFloat = (targetY + 0.5) * scaleY - 0.5;

                    // Clamp coordinates to valid range and convert to integer
                    int sourceX = Math.Max(0, Math.Min(sourceWidth - 1, (int)Math.Round(sourceXFloat)));
                    int sourceY = Math.Max(0, Math.Min(sourceHeight - 1, (int)Math.Round(sourceYFloat)));

                    // Calculate array indices
                    int sourceIndex = (sourceY * sourceWidth + sourceX) * 4;
                    int targetIndex = (targetY * targetWidth + targetX) * 4;

                    // Ensure indices are within bounds (extra safety check)
                    if (sourceIndex >= 0 && sourceIndex + 3 < sourceData.Length &&
                        targetIndex >= 0 && targetIndex + 3 < targetData.Length)
                    {
                        // Copy RGBA pixel data
                        targetData[targetIndex] = sourceData[sourceIndex];         // R
                        targetData[targetIndex + 1] = sourceData[sourceIndex + 1]; // G
                        targetData[targetIndex + 2] = sourceData[sourceIndex + 2]; // B
                        targetData[targetIndex + 3] = sourceData[sourceIndex + 3]; // A
                    }
                }
            }

            return targetData;
        }

        private byte[] SerializeMipmap(TPCMipmap mipmap)
        {
            // Serialize a TPCMipmap for cross-thread transfer
            // Returns a bytes object containing:
            // - width (4 bytes, int)
            // - height (4 bytes, int)
            // - format (4 bytes, int - TPCTextureFormat value)
            // - data_length (4 bytes, int)
            // - data (variable length bytes)

            using (var ms = new MemoryStream())
            using (var writer = new System.IO.BinaryWriter(ms))
            {
                writer.Write(mipmap.Width);
                writer.Write(mipmap.Height);
                writer.Write((int)mipmap.TpcFormat);
                writer.Write(mipmap.Data != null ? mipmap.Data.Length : 0);

                if (mipmap.Data != null && mipmap.Data.Length > 0)
                {
                    writer.Write(mipmap.Data);
                }

                return ms.ToArray();
            }
        }

        // Public method for deserializing mipmap (can be used by consumers)
        public static TPCMipmap DeserializeMipmap(byte[] data)
        {
            if (data == null || data.Length < 16)
            {
                throw new ArgumentException("Invalid mipmap data: insufficient length");
            }

            using (var ms = new MemoryStream(data))
            using (var reader = new System.IO.BinaryReader(ms))
            {
                int width = reader.ReadInt32();
                int height = reader.ReadInt32();
                int formatValue = reader.ReadInt32();
                int dataLength = reader.ReadInt32();

                if (dataLength < 0 || dataLength > data.Length - 16)
                {
                    throw new ArgumentException("Invalid mipmap data: invalid data length");
                }

                byte[] mipmapData = reader.ReadBytes(dataLength);

                return new TPCMipmap(width, height, (TPCTextureFormat)formatValue, mipmapData);
            }
        }

        // Legacy public method for backward compatibility (deprecated - use queue system instead)
        [Obsolete("Use QueueTextureLoad and TryGetResult instead")]
        public byte[] LoadTexture(object installation, string resref, ResourceType restype, int iconSize = 64)
        {
            try
            {
                if (installation is Installation inst)
                {
                    return LoadTextureInternal(inst, resref, restype, iconSize);
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error loading texture {resref}: {ex}");
                return null;
            }
        }

        public void Stop()
        {
            _cancellationTokenSource?.Cancel();
            _loaderTask?.Wait(TimeSpan.FromSeconds(5));
        }
    }
}
