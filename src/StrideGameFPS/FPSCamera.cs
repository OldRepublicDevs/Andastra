using Stride.Core.Mathematics;
using Stride.Graphics;
using System;

namespace StrideGameFPS
{
    /// <summary>
    /// First-person camera with free look and movement.
    /// Equivalent to MonoGameFPS.FPSCamera using Stride.Core.Mathematics.
    /// </summary>
    public class FPSCamera
    {
        private readonly GraphicsDevice _graphicsDevice;

        public Vector3 Position { get; set; }
        public Vector3 LookAtDirection { get; set; }
        public Vector3 Up { get; private set; }
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public float MovementSpeed { get; set; } = 50f;
        public float MouseSensitivity { get; set; } = 0.003f;
        public float FieldOfView { get; set; } = (float)Math.PI / 4f;
        public float NearPlane { get; set; } = 0.1f;
        public float FarPlane { get; set; } = 1000f;
        public Matrix View { get; private set; }
        public Matrix Projection { get; private set; }
        public BoundingFrustum Frustum { get; private set; }

        public FPSCamera(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            Position = Vector3.Zero;
            LookAtDirection = Vector3.UnitZ;
            Up = Vector3.UnitY;
            Yaw = 0f;
            Pitch = 0f;
            UpdateProjectionMatrix();
            UpdateViewMatrix();
        }

        public void UpdateProjectionMatrix()
        {
            var desc = _graphicsDevice.Presenter?.Description;
            int w = desc?.BackBufferWidth ?? 1920, h = desc?.BackBufferHeight ?? 1080;
            float aspectRatio = (float)w / h;
            Projection = Matrix.PerspectiveFovRH(FieldOfView, aspectRatio, NearPlane, FarPlane);
            var viewProj = View * Projection;
            Frustum = new BoundingFrustum(in viewProj);
        }

        public void UpdateViewMatrix()
        {
            var rotationMatrix = Matrix.RotationX(Pitch) * Matrix.RotationY(Yaw);
            var v4 = Vector4.Transform(new Vector4(Vector3.UnitZ, 0), rotationMatrix);
            LookAtDirection = new Vector3(v4.X, v4.Y, v4.Z);
            LookAtDirection.Normalize();
            Vector3 right = Vector3.Cross(Vector3.UnitY, LookAtDirection);
            right.Normalize();
            Up = Vector3.Cross(LookAtDirection, right);
            Up.Normalize();
            View = Matrix.LookAtRH(Position, Position + LookAtDirection, Up);
            var viewProj2 = View * Projection;
            Frustum = new BoundingFrustum(in viewProj2);
        }

        public void MoveForward(float amount) => Position += LookAtDirection * amount;
        public void MoveBackward(float amount) => Position -= LookAtDirection * amount;
        public void Strafe(float amount)
        {
            var right = Vector3.Cross(Vector3.UnitY, LookAtDirection);
            right.Normalize();
            Position += right * amount;
        }
        public void MoveVertical(float amount) => Position += Vector3.UnitY * amount;

        public void Rotate(float deltaX, float deltaY)
        {
            Yaw -= deltaX * MouseSensitivity;
            Pitch -= deltaY * MouseSensitivity;
            Pitch = MathUtil.Clamp(Pitch, -(float)Math.PI / 2 + 0.1f, (float)Math.PI / 2 - 0.1f);
        }

        public Vector3 GetRightVector()
        {
            var right = Vector3.Cross(Vector3.UnitY, LookAtDirection);
            right.Normalize();
            return right;
        }

        public Vector3 GetForwardXZ()
        {
            var forward = LookAtDirection;
            forward.Y = 0;
            if (forward.LengthSquared() > 0) forward.Normalize();
            else forward = Vector3.UnitZ;
            return forward;
        }

        public Vector3 GetRightXZ()
        {
            var right = GetRightVector();
            right.Y = 0;
            if (right.LengthSquared() > 0) right.Normalize();
            else right = Vector3.UnitX;
            return right;
        }
    }
}
