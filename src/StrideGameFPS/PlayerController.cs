using Stride.Core.Mathematics;
using Stride.Games;
using Stride.Graphics;
using Stride.Input;
using System;

namespace StrideGameFPS
{
    /// <summary>
    /// Handles player input and movement with collision detection.
    /// Equivalent to MonoGameFPS.PlayerController using Stride APIs.
    /// </summary>
    public class PlayerController
    {
        private readonly FPSCamera _camera;
        private readonly TerrainGenerator _terrain;
        private readonly GraphicsDevice _graphicsDevice;
        private Vector3 _velocity;
        private bool _isOnGround;
        private bool _isFlying;
        private Vector2 _lastMousePosition;
        private bool _firstUpdate = true;

        public float WalkSpeed { get; set; } = 30f;
        public float SprintSpeed { get; set; } = 60f;
        public float JumpForce { get; set; } = 15f;
        public float Gravity { get; set; } = -40f;
        public float PlayerHeight { get; set; } = 3.5f;
        public float PlayerRadius { get; set; } = 1f;

        public PlayerController(FPSCamera camera, TerrainGenerator terrain, GraphicsDevice graphicsDevice)
        {
            _camera = camera;
            _terrain = terrain;
            _graphicsDevice = graphicsDevice;
            _velocity = Vector3.Zero;
        }

        public void Update(GameTime gameTime, InputManager input, bool isMouseLocked)
        {
            float deltaTime = (float)gameTime.Elapsed.TotalSeconds;
            var mousePos = input.MousePosition;

            if (isMouseLocked)
            {
                if (_firstUpdate)
                {
                    _lastMousePosition = mousePos;
                    _firstUpdate = false;
                }
                else
                {
                    float deltaX = mousePos.X - _lastMousePosition.X;
                    float deltaY = mousePos.Y - _lastMousePosition.Y;
                    _camera.Rotate(deltaX, deltaY);
                    _lastMousePosition = mousePos;
                }
            }
            else
                _firstUpdate = true;

            if (input.IsKeyDown(Keys.F)) { _isFlying = true; _velocity.Y = 0; }
            if (input.IsKeyDown(Keys.G)) _isFlying = false;

            Vector3 moveDirection = Vector3.Zero;
            float currentSpeed = input.IsKeyDown(Keys.LeftShift) ? SprintSpeed : WalkSpeed;
            if (input.IsKeyDown(Keys.W)) moveDirection += _camera.GetForwardXZ();
            if (input.IsKeyDown(Keys.S)) moveDirection -= _camera.GetForwardXZ();
            if (input.IsKeyDown(Keys.A)) moveDirection -= _camera.GetRightXZ();
            if (input.IsKeyDown(Keys.D)) moveDirection += _camera.GetRightXZ();

            if (moveDirection.LengthSquared() > 0)
            {
                moveDirection.Normalize();
                moveDirection *= currentSpeed;
            }

            if (_isFlying)
            {
                if (input.IsKeyDown(Keys.Space)) _camera.MoveVertical(currentSpeed * deltaTime);
                if (input.IsKeyDown(Keys.LeftCtrl)) _camera.MoveVertical(-currentSpeed * deltaTime);
                _camera.Position += moveDirection * deltaTime;
            }
            else
            {
                _velocity.Y += Gravity * deltaTime;
                if (_isOnGround && input.IsKeyDown(Keys.Space))
                {
                    _velocity.Y = JumpForce;
                    _isOnGround = false;
                }
                Vector3 newPosition = _camera.Position + moveDirection * deltaTime;
                newPosition.Y = _camera.Position.Y + _velocity.Y * deltaTime;
                float terrainHeight = _terrain.GetHeightAt(newPosition.X, newPosition.Z);
                float playerBottom = newPosition.Y;
                if (playerBottom <= terrainHeight + PlayerHeight)
                {
                    newPosition.Y = terrainHeight + PlayerHeight;
                    _velocity.Y = 0;
                    _isOnGround = true;
                }
                else
                    _isOnGround = false;
                float currentTerrainHeight = _terrain.GetHeightAt(newPosition.X, newPosition.Z);
                if (newPosition.Y < currentTerrainHeight + PlayerHeight)
                {
                    newPosition.Y = currentTerrainHeight + PlayerHeight;
                    _velocity.Y = 0;
                }
                _camera.Position = newPosition;
            }
            _camera.UpdateViewMatrix();
        }
    }
}
