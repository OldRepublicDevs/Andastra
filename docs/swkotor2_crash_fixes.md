# swkotor2.exe Crash Fix Guide

## Overview

This guide provides practical solutions to fix the post-character creation crashes in `swkotor2.exe` without requiring hardware upgrades. Solutions range from simple workarounds to DLL injection patches.

## Solution 1: DLL Injection Patch (Recommended)

Create a DLL that hooks the problematic functions and adds proper error checking and synchronization.

### Implementation

Create a DLL project (`swkotor2_fix.dll`) that hooks:

1. **Hook `SwapBuffers`** - Add error checking before calling
2. **Hook `GetDC`** - Validate return value
3. **Hook `operator_new`** - Add retry logic for failed allocations
4. **Hook `FUN_00401c30`** - Add delays between rapid calls
5. **Hook resource loading functions** - Ensure proper cleanup

### Code Example (C++)

```cpp
// swkotor2_fix.cpp
#include <windows.h>
#include <gl/gl.h>

// Original function pointers
typedef BOOL (WINAPI *SwapBuffersProc)(HDC);
typedef HDC (WINAPI *GetDCProc)(HWND);
typedef BOOL (WINAPI *ReleaseDCProc)(HWND, HDC);

SwapBuffersProc OriginalSwapBuffers = nullptr;
GetDCProc OriginalGetDC = nullptr;
ReleaseDCProc OriginalReleaseDC = nullptr;

// Hooked SwapBuffers with error checking
BOOL WINAPI HookedSwapBuffers(HDC hDC)
{
    if (hDC == NULL) {
        // Log error and return safely
        return FALSE;
    }
    
    // Check if device context is valid
    if (GetPixelFormat(hDC) == 0) {
        // Invalid device context, return safely
        return FALSE;
    }
    
    return OriginalSwapBuffers(hDC);
}

// Hooked GetDC with validation
HDC WINAPI HookedGetDC(HWND hWnd)
{
    if (hWnd == NULL) {
        return NULL;
    }
    
    HDC hDC = OriginalGetDC(hWnd);
    
    if (hDC == NULL) {
        // Log error - window handle may be invalid
        return NULL;
    }
    
    return hDC;
}

// Hook FUN_00401c30 to add delays between rapid calls
// This requires finding the function address at runtime
void HookGraphicsFunction()
{
    // Use pattern scanning or signature to find FUN_00401c30
    // Add delay between calls to prevent race conditions
}

// DLL Entry Point
BOOL APIENTRY DllMain(HMODULE hModule, DWORD dwReason, LPVOID lpReserved)
{
    if (dwReason == DLL_PROCESS_ATTACH) {
        // Get original function addresses
        HMODULE hGDI32 = GetModuleHandleA("gdi32.dll");
        HMODULE hUser32 = GetModuleHandleA("user32.dll");
        
        OriginalSwapBuffers = (SwapBuffersProc)GetProcAddress(hGDI32, "SwapBuffers");
        OriginalGetDC = (GetDCProc)GetProcAddress(hUser32, "GetDC");
        OriginalReleaseDC = (ReleaseDCProc)GetProcAddress(hUser32, "ReleaseDC");
        
        // Install hooks using MinHook or similar library
        // MH_Initialize();
        // MH_CreateHook(OriginalSwapBuffers, HookedSwapBuffers, ...);
        // MH_EnableHook(OriginalSwapBuffers);
    }
    return TRUE;
}
```

### Usage

1. Compile the DLL
2. Use a DLL injector (e.g., `DLLInjector.exe`) to inject into `swkotor2.exe`
3. Or use a launcher that automatically injects the DLL

### Alternative: Use Existing Tools

- **d3d9.dll wrapper** - Some DirectX wrappers can add error checking
- **dinput8.dll wrapper** - Can intercept graphics calls
- **ENB/Reshade** - Graphics injectors that can add stability fixes

---

## Solution 2: Configuration File Workarounds

Create a configuration file that modifies game behavior through INI settings or command-line parameters.

### swkotor2.ini Modifications

```ini
[Display Options]
; Force software rendering (slower but more stable)
Disable Hardware Mouse=1
FullScreen=0

[Graphics Options]
; Reduce graphics load to prevent device context loss
Anisotropic=0
Anti Aliasing=0
Frame Buffer Effects=0
Soft Shadows=0

[Game Options]
; Add delays to prevent rapid operations
Disable Vertex Buffer Objects=1
```

### Command-Line Parameters

Create a batch file launcher:

```batch
@echo off
REM Add delay before launching to ensure system is ready
timeout /t 2 /nobreak >nul

REM Launch with compatibility settings
start "" /affinity 1 "swkotor2.exe" -nosound -windowed

REM Alternative: Use compatibility mode
REM Right-click swkotor2.exe -> Properties -> Compatibility
REM Check "Run in 640x480 screen resolution"
REM Check "Disable visual themes"
REM Check "Disable desktop composition"
```

---

## Solution 3: Memory Management Fixes

### Pre-allocate Memory

Create a memory pre-allocation script that runs before the game:

```batch
@echo off
REM Pre-allocate memory to reduce fragmentation
REM This helps with the 5616-byte allocation issue

REM Set process priority to prevent memory pressure
start "" /high "swkotor2.exe"

REM Alternative: Use a memory defragmenter before launching
```

### Windows Compatibility Settings

1. Right-click `swkotor2.exe` → Properties → Compatibility
2. Check "Run this program in compatibility mode for: Windows XP (Service Pack 3)"
3. Check "Disable fullscreen optimizations"
4. Check "Override high DPI scaling behavior" → "Application"

---

## Solution 4: Graphics Driver Workarounds

### Force Software Rendering

1. Create `swkotor2.exe.config` (if using .NET wrapper) or use DirectX control panel
2. Force software vertex processing
3. Disable hardware acceleration for the game

### Graphics Driver Settings

**NVIDIA:**
1. NVIDIA Control Panel → Manage 3D Settings → Program Settings
2. Select `swkotor2.exe`
3. Set "Power management mode" to "Prefer maximum performance"
4. Set "Threaded optimization" to "Off"
5. Set "Vertical sync" to "Off"
6. Set "Triple buffering" to "Off"

**AMD:**
1. AMD Catalyst Control Center → Gaming → 3D Application Settings
2. Select `swkotor2.exe`
3. Disable "Tessellation"
4. Set "Wait for Vertical Refresh" to "Always Off"
5. Set "Frame Rate Target Control" to "Off"

**Intel Integrated:**
1. Intel Graphics Control Panel → 3D → Application Settings
2. Select `swkotor2.exe`
3. Set "Application Optimal Mode" to "Performance"
4. Disable all advanced graphics features

---

## Solution 5: Process Priority and Affinity

Create a launcher that sets optimal process settings:

```batch
@echo off
REM Launch game
start "" "swkotor2.exe"

REM Wait for process to start
timeout /t 3 /nobreak >nul

REM Set high priority and single CPU affinity (prevents race conditions)
wmic process where name="swkotor2.exe" CALL setpriority "high priority"
wmic process where name="swkotor2.exe" CALL setaffinity "1"

REM Alternative PowerShell script:
REM $proc = Get-Process swkotor2
REM $proc.PriorityClass = "High"
REM $proc.ProcessorAffinity = 1
```

---

## Solution 6: Resource Cleanup Script

Create a script that ensures proper resource cleanup:

```batch
@echo off
REM Kill any lingering swkotor2 processes
taskkill /F /IM swkotor2.exe 2>nul

REM Clear temporary files
del /Q "%TEMP%\swkotor2_*" 2>nul

REM Clear DirectX shader cache
del /Q "%LOCALAPPDATA%\D3DSCache\*" 2>nul

REM Wait for cleanup
timeout /t 2 /nobreak >nul

REM Launch game
start "" "swkotor2.exe"
```

---

## Solution 7: Andastra Engine Fixes (Long-term)

For our Andastra reimplementation, we should implement these fixes:

### 1. Graphics Context Error Checking

```csharp
// In IGraphicsBackend implementation
public void SwapBuffers()
{
    if (_deviceContext == IntPtr.Zero)
    {
        LogError("Device context is invalid, attempting recovery...");
        RecoverDeviceContext();
        return;
    }
    
    if (!IsDeviceContextValid(_deviceContext))
    {
        LogError("Device context validation failed");
        RecoverDeviceContext();
        return;
    }
    
    try
    {
        NativeSwapBuffers(_deviceContext);
    }
    catch (Exception ex)
    {
        LogError($"SwapBuffers failed: {ex.Message}");
        RecoverDeviceContext();
    }
}

private void RecoverDeviceContext()
{
    // Implement device lost recovery
    // Recreate device context
    // Restore graphics state
}
```

### 2. Resource Cleanup Before Module Loading

```csharp
// In GameSession.StartNewGame
public void StartNewGame(CharacterCreationData characterData)
{
    // CRITICAL: Clean up character creation resources BEFORE module loading
    CleanupCharacterCreationResources();
    
    // Wait for cleanup to complete
    Thread.Sleep(100); // Small delay to ensure cleanup
    
    // Now load module
    LoadModule(startingModule);
}

private void CleanupCharacterCreationResources()
{
    // Unload character creation GUIs
    _guiManager.UnloadGui("classsel");
    _guiManager.UnloadGui("CHARGEN");
    
    // Unload character creation RIM files
    _resourceProvider.UnloadRim("RIMS:CHARGEN");
    
    // Clear character creation textures/models
    _graphicsBackend.ClearResourceCache("CHARGEN");
    
    // Force garbage collection to free memory
    GC.Collect();
    GC.WaitForPendingFinalizers();
}
```

### 3. Memory Allocation with Retry Logic

```csharp
// In ModuleLoader
private T AllocateModuleObject<T>(int size) where T : class
{
    const int maxRetries = 3;
    const int retryDelayMs = 100;
    
    for (int attempt = 0; attempt < maxRetries; attempt++)
    {
        try
        {
            // Try to allocate
            var obj = (T)Activator.CreateInstance(typeof(T));
            
            if (obj != null)
            {
                return obj;
            }
        }
        catch (OutOfMemoryException)
        {
            if (attempt < maxRetries - 1)
            {
                // Force garbage collection
                GC.Collect();
                GC.WaitForPendingFinalizers();
                
                // Wait before retry
                Thread.Sleep(retryDelayMs);
            }
        }
    }
    
    throw new OutOfMemoryException($"Failed to allocate {typeof(T).Name} after {maxRetries} attempts");
}
```

### 4. Graphics State Changes with Validation

```csharp
// In GraphicsStateManager
public void SetGraphicsState(int state, bool validate = true)
{
    if (validate && !IsDeviceReady())
    {
        LogWarning("Device not ready, waiting...");
        WaitForDeviceReady(100); // Wait up to 100ms
    }
    
    if (!IsDeviceReady())
    {
        throw new InvalidOperationException("Graphics device is not ready for state change");
    }
    
    _currentState = state;
    ApplyGraphicsState(state);
}

private void WaitForDeviceReady(int timeoutMs)
{
    var startTime = Environment.TickCount;
    while (!IsDeviceReady() && (Environment.TickCount - startTime) < timeoutMs)
    {
        Thread.Sleep(10);
    }
}
```

### 5. Synchronized Graphics Operations

```csharp
// In GraphicsRenderer
private readonly object _graphicsLock = new object();
private DateTime _lastSwapTime = DateTime.MinValue;
private const int MinSwapIntervalMs = 16; // ~60 FPS max

public void RenderFrame(float deltaTime)
{
    lock (_graphicsLock)
    {
        // Ensure minimum time between SwapBuffers calls
        var timeSinceLastSwap = (DateTime.Now - _lastSwapTime).TotalMilliseconds;
        if (timeSinceLastSwap < MinSwapIntervalMs)
        {
            Thread.Sleep((int)(MinSwapIntervalMs - timeSinceLastSwap));
        }
        
        // Validate device context before operations
        if (!ValidateDeviceContext())
        {
            RecoverDeviceContext();
            return;
        }
        
        // Perform rendering
        RenderScene();
        
        // Swap buffers with error checking
        if (!SwapBuffers())
        {
            LogError("SwapBuffers failed, recovering...");
            RecoverDeviceContext();
            return;
        }
        
        _lastSwapTime = DateTime.Now;
    }
}
```

---

## Quick Fix Checklist

For immediate relief, try these in order:

- [ ] **Set Windows compatibility mode** to Windows XP SP3
- [ ] **Disable fullscreen optimizations** in compatibility settings
- [ ] **Run as administrator** (sometimes helps with resource access)
- [ ] **Close other applications** to free memory
- [ ] **Update graphics drivers** to latest stable version (not beta)
- [ ] **Set process priority to High** using Task Manager
- [ ] **Disable antivirus** temporarily (can interfere with memory allocation)
- [ ] **Use windowed mode** instead of fullscreen
- [ ] **Reduce graphics settings** in game options
- [ ] **Add delay before launching** (2-3 seconds) to ensure system is ready

---

## Testing the Fixes

After applying fixes, test by:

1. Creating a new character
2. Completing character creation
3. Observing if the game loads the module without crashing
4. If it still crashes, try the next solution in the list

---

## Reporting Issues

If none of these solutions work, please report:
- Your graphics card model and driver version
- Windows version
- Amount of RAM
- Whether the crash happens immediately or after a delay
- Any error messages or crash logs

---

**Last Updated:** 2024
**Based on:** swkotor2_crash_analysis.md

