# OdyTools.Tests

Editor tests for OdyTools, mirroring the Python editor tests under `PyKotor/Tools/OdyTools/tests/gui/editors/`.

## Editor coverage

- **TPC, TXT, UTC, UTD, UTE, UTI, UTM, UTP, UTS, UTT, UTW** – TPC/TXT/UT*/WAV/2DA/… via dedicated or GFF-based tests  
- **WAV, 2DA, ARE, BWM, DLG, ERF, GFF, GIT, IFO, JRL, LIP, LTR, MDL, NSS, PTH, save, SSF, TLK** – covered by `*EditorTests.cs` and `GFFBasedEditorTests.cs`

**Test classes:** `OdyToolBWMTests`, `OdyToolDLGTests`, `OdyToolERFTests`, `OdyToolFACTests`, `OdyToolGFFTests`, `OdyToolGFFBasedTests` (UTC, UTD, UTE, UTI, UTM, UTP, UTS, UTT, UTW, ARE, GIT, IFO, JRL, PTH), `OdyToolLIPTests`, `OdyToolLTRTests`, `OdyToolMDLTests`, `OdyToolNSSTests`, `OdyToolSAVTests`, `OdyToolSSFTests`, `OdyToolTLKTests`, `OdyToolTPCTests`, `OdyTool2DATests`, `OdyToolTXTTests`, `OdyToolWAVTests`, `ReferenceFinderTests`, `ScriptsDisassemblyTests`, `IndoorMapBuildTests`, `IndoorMapBuildWalkmeshTests`, `IndoorMapIoTests`, `IndoorMapWindowFileOpsTests`.

Tests use **Avalonia headless** (`HeadlessUnitTestSession.StartNew(typeof(TestApp))`) so no display is required.

### Test counts and timeouts

- **39 tests** run by default. **4 tests** are marked **Explicit** and skipped unless run by name or with explicit filter.
- **Full default run** can take **about 15–25 minutes** (NSS and SSF editors are slow in headless). Do not set an overly low run timeout.
- Per-test timeouts:
  - **NSS:** 90 s (two tests), 120 s (one test).
  - **SSF:** 180 s (LoadAndBuild), 120 s (New), 90 s (LoadEmpty).
  - **GFF-based:** 60 s most; 120 s for IFO, UTD, UTP (those three are Explicit).
  - **JRL** (Explicit): 60 s.

### Explicit tests (skipped by default)

| Test | Reason |
|------|--------|
| **OdyToolIFO_LoadMinimalGff_BuildsValidGff** | Editor init exceeds ~2 min in headless. |
| **OdyToolUTD_LoadMinimalGff_BuildsValidGff** | Same. |
| **OdyToolUTP_LoadMinimalGff_BuildsValidGff** | Same. |
| **OdyToolJRL_LoadMinimalGff_BuildsValidGff** | `HeadlessUnitTestSession.Dispose()` throws NRE after this editor. |

To run an explicit test from the command line, use the test name in the filter and enable explicit runs, e.g.:

```bash
dotnet test "tests\OdyTools.Tests\OdyTools.Tests.csproj" --filter "FullyQualifiedName~OdyToolIFO_LoadMinimalGff" -c Debug --no-build -- NUnit.RunConfiguration.ExplicitlyRunExplicitTests=true
```

(Or run them from Test Explorer by selecting the test.)

## Running tests

**Option A – script (recommended):** From the test project directory, run `.\Run-Tests.ps1`. It stops any running `testhost` processes, builds, then runs all editor tests (default 39 tests).

**Option B – manual:**

1. **Close any other test runs or IDE test runners** so that `testhost` does not lock DLLs.
2. **Close OdyTools (GUI)** if you need to rebuild; the build copies into `OdyTools\bin\Debug\net9.0\` and will fail if the app is running.
3. Build and run:

   ```bash
   dotnet test "tests\OdyTools.Tests\OdyTools.Tests.csproj" --filter "FullyQualifiedName~OdyTools.Tests" -c Debug
   ```

   Or build once then run without rebuilding:

   ```bash
   dotnet build "tests\OdyTools.Tests\OdyTools.Tests.csproj" -c Debug
   dotnet test "tests\OdyTools.Tests\OdyTools.Tests.csproj" --filter "FullyQualifiedName~OdyTools.Tests" -c Debug --no-build
   ```

### Troubleshooting

- **File in use (testhost):** If you see "The process cannot access the file ... because it is being used by another process (testhost)", close any running test sessions (e.g. Test Explorer in the IDE), then run the commands again. You can also end the `testhost` processes from Task Manager if they remain.
- **File in use (OdyTools.NET):** If the build fails copying DLLs into `OdyTools\bin\Debug\net9.0\`, close the OdyTools GUI application and rebuild.
- **hostpolicy / testhost.runtimeconfig.json:** If tests fail with "hostpolicy.dll not found" or "testhost.runtimeconfig.json was not found", run a full build (without `--no-build`) once so the test host is generated, then run tests (with or without `--no-build`).
