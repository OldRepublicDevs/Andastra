# Performance Testing Guide

This document describes the performance testing infrastructure that ensures all tests complete within 2 minutes and generates profiling reports for bottleneck analysis.

## Overview

All tests in the project are configured with:
- **2-minute timeout enforcement**: Tests automatically fail if they exceed 120 seconds
- **Performance profiling**: Detailed reports are generated similar to Python's cProfile
- **Bottleneck identification**: Reports highlight the slowest operations

## Test Framework Support

### xUnit (TSLPatcher.Tests)

Tests use the `[Fact(Timeout = 120000)]` attribute to enforce 2-minute timeouts:

```csharp
[Fact(Timeout = 120000)] // 2 minutes timeout - test will fail if exceeded
public void MyTest()
{
    using (var perfHelper = new Performance.PerformanceTestHelper(
        nameof(MyTest),
        output, // ITestOutputHelper
        maxSeconds: 120,
        enableProfiling: true))
    {
        // Your test code here
        perfHelper.CheckTimeout(); // Call periodically in long-running tests
    }
}
```

### NUnit (CSharpKOTOR.Tests)

Tests use the `[PerformanceTest]` attribute:

```csharp
[Test]
[PerformanceTest(maxSeconds: 120, enableProfiling: true)]
public void MyTest()
{
    // Your test code here
}
```

## Running Tests with Profiling

### Using the Test Runner Script

```powershell
# Run all tests with profiling
.\scripts\RunAllTestsWithProfiling.ps1

# Run specific test project
.\scripts\RunAllTestsWithProfiling.ps1 -Projects @("src\TSLPatcher.Tests\TSLPatcher.Tests.csproj")

# Run with filter
.\scripts\RunAllTestsWithProfiling.ps1 -Filter "FullyQualifiedName~NCSRoundtripTests"
```

### Using dotnet test directly

```powershell
dotnet test src/TSLPatcher.Tests/TSLPatcher.Tests.csproj --logger "console;verbosity=detailed"
```

All tests automatically have timeout enforcement - no additional configuration needed.

## Profiling Reports

Profiling reports are saved to the `profiles/` directory with the format:
- `{TestClassName}_{TestMethodName}.profile.txt`

### Report Contents

Each profile report contains:
- **Execution time**: Total time in seconds and milliseconds
- **Memory usage**: Initial, final, and delta memory consumption
- **CPU time**: User and total CPU time
- **Thread count**: Number of threads used
- **Handle count**: System handles used

### Example Report

```
=== Performance Profile: TestRoundTripSuite ===
Start Time: 2025-01-15 10:30:45.123
Initial Memory: 150 MB
Process ID: 12345

End Time: 2025-01-15 10:32:30.456
Elapsed Time: 105.333 seconds (105333 ms)
Final Memory: 280 MB
Memory Delta: 130 MB
Peak Memory: 320 MB
CPU Time (User): 98.500 seconds
CPU Time (Total): 105.200 seconds
Threads: 8
Handles: 450
```

## Identifying Bottlenecks

### Using the Test Runner

The test runner automatically identifies the slowest tests:

```powershell
.\scripts\RunAllTestsWithProfiling.ps1
```

Output includes:
```
Top 10 slowest tests (by profile file size):
───────────────────────────────────────────────────────────
1. TestRoundTripSuite: 105.333s
2. TestLargeFileProcessing: 95.123s
...
```

### Manual Analysis

1. Review profile files in `profiles/` directory
2. Look for tests with high "Elapsed Time"
3. Check "Memory Delta" for memory leaks
4. Compare "CPU Time" vs "Elapsed Time" to identify I/O bottlenecks

## Performance Optimization Guidelines

### Common Bottlenecks

1. **File I/O**: Minimize `File.ReadAllText`, `File.WriteAllText` calls
   - Cache file contents when possible
   - Use streaming for large files

2. **Process Execution**: External compiler calls
   - Batch operations when possible
   - Use async/parallel processing

3. **Directory Operations**: `Directory.CreateDirectory`, `Directory.Delete`
   - Reuse directories when possible
   - Clean up asynchronously

4. **Memory Allocations**: Large object creation
   - Use object pooling for frequently created objects
   - Dispose resources promptly

### Adding Timeout Checks

In long-running tests, add periodic timeout checks:

```csharp
foreach (var item in largeCollection)
{
    perfHelper.CheckTimeout(); // Check every iteration
    // Process item
}
```

## Troubleshooting

### Test Exceeds Timeout

If a test exceeds 2 minutes:

1. Check the profile report in `profiles/` directory
2. Identify the slowest operations
3. Optimize:
   - Reduce file I/O
   - Parallelize operations
   - Cache expensive computations
   - Optimize algorithms

### Profile Reports Not Generated

Ensure:
- `enableProfiling: true` is set
- Test completes (even if it fails)
- `profiles/` directory is writable

### False Timeout Failures

If a test fails due to timeout but completes quickly:
- Check for blocking operations
- Verify timeout is set correctly (120000 ms = 2 minutes)
- Check system load during test execution

## Best Practices

1. **Always use timeout attributes**: All tests should have `[Fact(Timeout = 120000)]` or `[PerformanceTest]`
2. **Add periodic checks**: Call `CheckTimeout()` in loops and long-running operations
3. **Review profiles regularly**: Check for performance regressions
4. **Optimize incrementally**: Fix one bottleneck at a time
5. **Document optimizations**: Note what was optimized and why

## Integration with CI/CD

The test runner can be integrated into CI/CD pipelines:

```yaml
- name: Run Tests with Profiling
  run: |
    pwsh -File scripts/RunAllTestsWithProfiling.ps1
  continue-on-error: true

- name: Upload Profile Reports
  uses: actions/upload-artifact@v3
  if: always()
  with:
    name: performance-profiles
    path: profiles/
```
