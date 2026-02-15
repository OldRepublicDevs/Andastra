# StrideGameFPS

Stride 3D equivalent of **MonoGameFPS**: same structure and behavior, using Stride APIs.

- **Entry:** `Program.cs` → `FPSGame` → `Run()`
- **Scenes:** `SceneManager`, `Scene` base, `MainMenuScene`, `OptionsScene`, `GameScene`
- **Gameplay:** `FPSCamera`, `PlayerController`, procedural terrain (`TerrainGenerator`, `TerrainRenderer`), `PerlinNoise`, `ProceduralTextureGenerator`
- **UI:** `Button` (Stride SpriteBatch + fonts)

## Requirements

- .NET 8.0 (Windows)
- Stride 4.2.1 (NuGet: Stride.Engine, Stride.Graphics, Stride.Input)

## Run

```bash
dotnet run
```

## Differences from MonoGameFPS

- **Math:** `Stride.Core.Mathematics` (Vector3.UnitZ/UnitY/UnitX, Matrix, etc.) instead of XNA/MonoGame.
- **Window/viewport:** Size from `GraphicsDevice.Presenter.Description`; no `GraphicsDevice.Viewport`.
- **Content:** Stride `ContentManager`; font paths (e.g. `Fonts/TitleFont`) require Stride assets or use fallback UI.
- **Terrain 3D:** Chunks and buffers are built; actual drawing is stubbed until a Stride effect (BasicEffect-style) is hooked up.

## 1:1 layout vs MonoGameFPS

| MonoGameFPS        | StrideGameFPS        |
|--------------------|----------------------|
| `Game`             | `Game` (Stride.Engine) |
| `GraphicsDeviceManager` | `GraphicsDeviceManager` (Stride.Games) |
| `SpriteBatch`      | `Stride.Graphics.SpriteBatch` |
| `Scene` / `SceneManager` | Same roles, Stride types |
| `FPSCamera`        | Same logic, Stride math |
| `PlayerController` | Same logic, Stride Input |
| `TerrainGenerator` / `TerrainRenderer` | Same design, Stride `Buffer`; draw path TODO |
| `PerlinNoise`      | Unchanged (no framework) |
| `Button`           | Same behavior, Stride SpriteBatch + Input |
