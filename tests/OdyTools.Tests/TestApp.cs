using Avalonia;
using Avalonia.Headless;
using Avalonia.Themes.Fluent;

namespace OdyTools.Tests
{
    /// <summary>
    /// Minimal Application for headless tests. Does not load XAML so that
    /// headless testing works without precompiled App.axaml.
    /// </summary>
    public class TestApp : Application
    {
        public override void Initialize()
        {
            Styles.Add(new FluentTheme());
        }
    }

    /// <summary>
    /// AppBuilder factory for [AvaloniaTest] attribute.
    /// Referenced by [assembly: AvaloniaTestApplication] in AssemblyInfo.cs.
    /// Creates a single shared headless application instance for all tests.
    /// </summary>
    public class TestAppBuilder
    {
        public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<TestApp>()
            .UseHeadless(new AvaloniaHeadlessPlatformOptions());
    }
}
