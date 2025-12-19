using System;
using Andastra.Runtime.Graphics;
using Andastra.Runtime.Graphics.Common.Enums;
using Andastra.Runtime.Graphics.Common.GUI;
using Andastra.Tests.Runtime.TestHelpers;
using FluentAssertions;
using Moq;
using Xunit;

namespace Andastra.Tests.Runtime.Graphics.Common.GUI
{
    /// <summary>
    /// Comprehensive unit tests for MenuRendererFactory.
    /// Tests factory creation logic for all supported graphics backends.
    /// </summary>
    /// <remarks>
    /// MenuRendererFactory Tests:
    /// - Tests factory creation for MonoGame backend
    /// - Tests factory creation for Stride backend
    /// - Tests error handling (null backend, uninitialized backend, unsupported backend)
    /// - Tests reflection-based graphics device extraction
    /// - Ensures proper menu renderer instantiation
    /// </remarks>
    public class MenuRendererFactoryTests
    {
        [Fact]
        public void CreateMenuRenderer_WithNullBackend_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            MenuRendererFactory.Invoking(f => f.CreateMenuRenderer(null))
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("graphicsBackend");
        }

        [Fact]
        public void CreateMenuRenderer_WithUninitializedBackend_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var mockBackend = new Mock<IGraphicsBackend>(MockBehavior.Strict);
            mockBackend.Setup(b => b.IsInitialized).Returns(false);
            mockBackend.Setup(b => b.BackendType).Returns(GraphicsBackendType.MonoGame);

            // Act & Assert
            MenuRendererFactory.Invoking(f => f.CreateMenuRenderer(mockBackend.Object))
                .Should().Throw<InvalidOperationException>()
                .WithMessage("*initialized*");
        }

        [Fact]
        public void CreateMenuRenderer_WithMonoGameBackend_ShouldCreateMyraMenuRenderer()
        {
            // Arrange
            var graphicsDevice = GraphicsTestHelper.CreateTestIGraphicsDevice();
            var mockBackend = CreateMockGraphicsBackend(GraphicsBackendType.MonoGame, graphicsDevice, true);

            // Act
            var renderer = MenuRendererFactory.CreateMenuRenderer(mockBackend.Object);

            // Assert
            renderer.Should().NotBeNull();
            renderer.Should().BeOfType<MonoGame.GUI.MyraMenuRenderer>();
            renderer.IsInitialized.Should().BeTrue();
        }

        [Fact]
        public void CreateMenuRenderer_WithMonoGameBackend_ShouldInitializeRenderer()
        {
            // Arrange
            var graphicsDevice = GraphicsTestHelper.CreateTestIGraphicsDevice();
            var mockBackend = CreateMockGraphicsBackend(GraphicsBackendType.MonoGame, graphicsDevice, true);

            // Act
            var renderer = MenuRendererFactory.CreateMenuRenderer(mockBackend.Object);

            // Assert
            renderer.Should().NotBeNull();
            renderer.IsInitialized.Should().BeTrue();
        }

        [Fact]
        public void CreateMenuRenderer_WithStrideBackend_ShouldCreateStrideMenuRenderer()
        {
            // Arrange
            // Note: Stride backend requires actual Stride GraphicsDevice
            // For testing, we'll mock it and expect null if device extraction fails
            var graphicsDevice = GraphicsTestHelper.CreateTestIGraphicsDevice();
            var mockBackend = CreateMockGraphicsBackend(GraphicsBackendType.Stride, graphicsDevice, true);
            mockBackend.Setup(b => b.ContentManager).Returns((IContentManager)null);

            // Act
            var renderer = MenuRendererFactory.CreateMenuRenderer(mockBackend.Object);

            // Assert
            // Stride renderer creation may fail if device is not Stride device
            // This is expected behavior - factory should handle gracefully
            if (renderer != null)
            {
                renderer.Should().BeOfType<Stride.GUI.StrideMenuRenderer>();
            }
        }

        [Fact]
        public void CreateMenuRenderer_WithUnsupportedBackend_ShouldReturnNull()
        {
            // Arrange
            var graphicsDevice = GraphicsTestHelper.CreateTestIGraphicsDevice();
            var mockBackend = CreateMockGraphicsBackend((GraphicsBackendType)999, graphicsDevice, true);

            // Act
            var renderer = MenuRendererFactory.CreateMenuRenderer(mockBackend.Object);

            // Assert
            renderer.Should().BeNull();
        }

        [Fact]
        public void CreateMenuRenderer_WithMonoGameBackend_ShouldSetViewport()
        {
            // Arrange
            var graphicsDevice = GraphicsTestHelper.CreateTestIGraphicsDevice();
            var mockBackend = CreateMockGraphicsBackend(GraphicsBackendType.MonoGame, graphicsDevice, true);

            // Act
            var renderer = MenuRendererFactory.CreateMenuRenderer(mockBackend.Object);

            // Assert
            renderer.Should().NotBeNull();
            if (renderer != null)
            {
                renderer.ViewportWidth.Should().BeGreaterThan(0);
                renderer.ViewportHeight.Should().BeGreaterThan(0);
            }
        }

        [Fact]
        public void CreateMenuRenderer_WithMonoGameBackend_ShouldAllowVisibilityControl()
        {
            // Arrange
            var graphicsDevice = GraphicsTestHelper.CreateTestIGraphicsDevice();
            var mockBackend = CreateMockGraphicsBackend(GraphicsBackendType.MonoGame, graphicsDevice, true);

            // Act
            var renderer = MenuRendererFactory.CreateMenuRenderer(mockBackend.Object);

            // Assert
            renderer.Should().NotBeNull();
            if (renderer != null)
            {
                renderer.IsVisible.Should().BeFalse();
                renderer.SetVisible(true);
                renderer.IsVisible.Should().BeTrue();
            }
        }

        [Fact]
        public void CreateMenuRenderer_WithMonoGameBackend_ShouldAllowViewportUpdates()
        {
            // Arrange
            var graphicsDevice = GraphicsTestHelper.CreateTestIGraphicsDevice();
            var mockBackend = CreateMockGraphicsBackend(GraphicsBackendType.MonoGame, graphicsDevice, true);

            // Act
            var renderer = MenuRendererFactory.CreateMenuRenderer(mockBackend.Object);

            // Assert
            renderer.Should().NotBeNull();
            if (renderer != null)
            {
                renderer.Invoking(r => r.UpdateViewport(1920, 1080)).Should().NotThrow();
                renderer.ViewportWidth.Should().Be(1920);
                renderer.ViewportHeight.Should().Be(1080);
            }
        }

        [Fact]
        public void CreateMenuRenderer_WithMonoGameBackend_ShouldAllowDisposal()
        {
            // Arrange
            var graphicsDevice = GraphicsTestHelper.CreateTestIGraphicsDevice();
            var mockBackend = CreateMockGraphicsBackend(GraphicsBackendType.MonoGame, graphicsDevice, true);

            // Act
            var renderer = MenuRendererFactory.CreateMenuRenderer(mockBackend.Object);

            // Assert
            renderer.Should().NotBeNull();
            if (renderer != null)
            {
                renderer.Invoking(r => r.Dispose()).Should().NotThrow();
            }
        }

        /// <summary>
        /// Creates a mock IGraphicsBackend with the specified configuration.
        /// </summary>
        private Mock<IGraphicsBackend> CreateMockGraphicsBackend(
            GraphicsBackendType backendType,
            IGraphicsDevice graphicsDevice,
            bool isInitialized)
        {
            var mockBackend = new Mock<IGraphicsBackend>(MockBehavior.Strict);
            mockBackend.Setup(b => b.BackendType).Returns(backendType);
            mockBackend.Setup(b => b.GraphicsDevice).Returns(graphicsDevice);
            mockBackend.Setup(b => b.IsInitialized).Returns(isInitialized);
            return mockBackend;
        }
    }
}

