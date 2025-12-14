using System;
using Avalonia;
using Avalonia.Headless;
using Xunit;

namespace HolocronToolset.NET.Tests.TestHelpers
{
    // Test fixture to initialize Avalonia for unit tests
    public class AvaloniaTestFixture : IDisposable
    {
        private static bool _initialized = false;
        private static readonly object _lock = new object();

        public AvaloniaTestFixture()
        {
            lock (_lock)
            {
                if (!_initialized)
                {
                    try
                    {
                        BuildAvaloniaApp()
                            .UseHeadless(new AvaloniaHeadlessPlatformOptions
                            {
                                UseHeadlessDrawing = true
                            })
                            .SetupWithoutStarting();
                        _initialized = true;
                    }
                    catch
                    {
                        // If initialization fails, continue anyway - some tests may not need Avalonia
                    }
                }
            }
        }

        public void Dispose()
        {
            // Cleanup if needed
        }

        private static AppBuilder BuildAvaloniaApp()
        {
            return AppBuilder.Configure<HolocronToolset.NET.App>()
                .UsePlatformDetect()
                .LogToTrace();
        }
    }

    // XUnit collection fixture for Avalonia initialization
    [CollectionDefinition("Avalonia Test Collection")]
    public class AvaloniaTestCollection : ICollectionFixture<AvaloniaTestFixture>
    {
    }
}
