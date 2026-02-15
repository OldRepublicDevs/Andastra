using Stride.Games;
using Stride.Graphics;
using Stride.Core.Serialization.Contents;
using System;

namespace StrideGameFPS.Scenes
{
    /// <summary>
    /// Abstract base class for game scenes (screens).
    /// Equivalent to MonoGameFPS.Scenes.Scene using Stride APIs.
    /// </summary>
    public abstract class Scene : IDisposable
    {
        protected GraphicsDevice GraphicsDevice { get; private set; }
        protected ContentManager Content { get; private set; }
        protected SpriteBatch SpriteBatch { get; private set; }
        protected GraphicsContext GraphicsContext { get; private set; }

        public bool IsInitialized { get; private set; }

        public void InternalInitialize(GraphicsDevice graphicsDevice, ContentManager content, SpriteBatch spriteBatch, GraphicsContext graphicsContext)
        {
            GraphicsDevice = graphicsDevice;
            Content = content;
            SpriteBatch = spriteBatch;
            GraphicsContext = graphicsContext;
            Initialize();
            LoadContent();
            IsInitialized = true;
        }

        public virtual void Initialize() { }

        public virtual void LoadContent() { }

        public virtual void UnloadContent() { }

        public abstract void Update(GameTime gameTime, Stride.Input.InputManager input);

        public abstract void Draw(GameTime gameTime);

        public virtual void Dispose()
        {
            UnloadContent();
        }
    }
}
