using Stride.Games;
using Stride.Graphics;
using Stride.Core.Serialization.Contents;
using Stride.Input;
using System;

namespace StrideGameFPS.Scenes
{
    /// <summary>
    /// Manages scene transitions and lifecycle.
    /// Equivalent to MonoGameFPS.Scenes.SceneManager using Stride APIs.
    /// </summary>
    public class SceneManager
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly ContentManager _globalContent;
        private readonly SpriteBatch _spriteBatch;
        private readonly GraphicsContext _graphicsContext;
        private readonly InputManager _input;

        private Scene _currentScene;
        private Scene _nextScene;

        public SceneManager(GraphicsDevice graphicsDevice, ContentManager globalContent, SpriteBatch spriteBatch, GraphicsContext graphicsContext, InputManager input)
        {
            _graphicsDevice = graphicsDevice;
            _globalContent = globalContent;
            _spriteBatch = spriteBatch;
            _graphicsContext = graphicsContext;
            _input = input;
        }

        public void ChangeScene(Scene newScene)
        {
            _nextScene = newScene;
        }

        public void Update(GameTime gameTime)
        {
            if (_nextScene != null)
                TransitionToNextScene();
            _currentScene?.Update(gameTime, _input);
        }

        public void Draw(GameTime gameTime)
        {
            _currentScene?.Draw(gameTime);
        }

        private void TransitionToNextScene()
        {
            _currentScene?.Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            _currentScene = _nextScene;
            _nextScene = null;
            if (_currentScene != null && !_currentScene.IsInitialized)
                _currentScene.InternalInitialize(_graphicsDevice, _globalContent, _spriteBatch, _graphicsContext);
        }
    }
}
