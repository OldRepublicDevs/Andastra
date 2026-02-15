// GFF editor roundtrip is also covered by BioWare.Tests (GFFRoundtripTests, TwoDA, SSF, LYT).
// OdyTools UI tests use [AvaloniaTest] attribute for efficient headless testing.
// note: bioware format coverage provided by tests/BioWare.Tests/cs.

using Avalonia.Headless;
using OdyTools.Tests;

[assembly: AvaloniaTestApplication(typeof(TestAppBuilder))]
