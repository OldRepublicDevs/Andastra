using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Andastra.Parsing.Installation;
using Andastra.Parsing.Resource;
using Andastra.Runtime.Graphics;
using Andastra.Runtime.MonoGame.Graphics;
using Moq;

namespace Andastra.Tests.Runtime.TestHelpers
{
    /// <summary>
    /// Helper class for creating test graphics devices and installations.
    /// </summary>
    public static class GraphicsTestHelper
    {
        /// <summary>
        /// Creates a test MonoGame GraphicsDevice using a headless Game instance.
        /// </summary>
        public static GraphicsDevice CreateTestGraphicsDevice()
        {
            // Create a minimal Game instance for testing
            var game = new Game();
            game.Initialize();
            return game.GraphicsDevice;
        }

        /// <summary>
        /// Creates a test IGraphicsDevice wrapper.
        /// </summary>
        public static IGraphicsDevice CreateTestIGraphicsDevice()
        {
            var mgDevice = CreateTestGraphicsDevice();
            return new MonoGameGraphicsDevice(mgDevice);
        }

        /// <summary>
        /// Creates a mock Installation with resource lookup capabilities.
        /// </summary>
        public static Installation CreateMockInstallation()
        {
            // For testing, we'll create a real Installation pointing to a test directory
            // In a real scenario, you'd use Moq to mock the Installation
            string testPath = Path.Combine(Path.GetTempPath(), "AndastraTestInstallation");
            if (!Directory.Exists(testPath))
            {
                Directory.CreateDirectory(testPath);
            }

            // Create a minimal installation structure
            // Note: This is a simplified version - real tests would need proper game files
            try
            {
                return new Installation(testPath);
            }
            catch
            {
                // If installation creation fails, create a mock
                var mockInstallation = new Mock<Installation>(MockBehavior.Strict);
                var mockResources = new Mock<IResourceLookup>(MockBehavior.Strict);
                
                mockInstallation.Setup(i => i.Resources).Returns(mockResources.Object);
                
                // Setup default resource lookup to return null (resource not found)
                mockResources.Setup(r => r.LookupResource(
                    It.IsAny<string>(),
                    It.IsAny<ResourceType>(),
                    It.IsAny<string[]>(),
                    It.IsAny<string[]>()))
                    .Returns((ResourceResult)null);

                return mockInstallation.Object;
            }
        }

        /// <summary>
        /// Creates a mock Installation with specific resource data.
        /// </summary>
        public static Installation CreateMockInstallationWithResource(string resRef, ResourceType resourceType, byte[] data)
        {
            var mockInstallation = new Mock<Installation>(MockBehavior.Strict);
            var mockResources = new Mock<IResourceLookup>(MockBehavior.Strict);
            
            mockInstallation.Setup(i => i.Resources).Returns(mockResources.Object);
            
            // Setup resource lookup for specific resource
            mockResources.Setup(r => r.LookupResource(
                resRef,
                resourceType,
                It.IsAny<string[]>(),
                It.IsAny<string[]>()))
                .Returns(new ResourceResult { Data = data });

            // Setup default lookup to return null
            mockResources.Setup(r => r.LookupResource(
                It.Is<string>(s => s != resRef),
                It.IsAny<ResourceType>(),
                It.IsAny<string[]>(),
                It.IsAny<string[]>()))
                .Returns((ResourceResult)null);

            return mockInstallation.Object;
        }

        /// <summary>
        /// Cleans up test resources.
        /// </summary>
        public static void CleanupTestGraphicsDevice(GraphicsDevice device)
        {
            if (device != null)
            {
                try
                {
                    device.Dispose();
                }
                catch
                {
                    // Ignore disposal errors
                }
            }
        }
    }
}

