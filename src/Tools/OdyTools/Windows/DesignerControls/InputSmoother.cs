using System;

namespace OdyTools.Windows.DesignerControls
{
    /// <summary>
    /// Provides input smoothing for mouse movements.
    ///
    /// Uses exponential moving average to smooth out jerky mouse input
    /// while maintaining responsiveness.
    /// </summary>
    public class InputSmoother
    {
        private readonly float _smoothingFactor;
        private float _prevX;
        private float _prevY;
        private bool _initialized;

        /// <summary>
        /// Initialize the smoother.
        /// </summary>
        /// <param name="smoothingFactor">Value between 0 and 1. Higher = more smoothing.</param>
        public InputSmoother(float smoothingFactor = 0.3f)
        {
            _smoothingFactor = smoothingFactor;
            _prevX = 0.0f;
            _prevY = 0.0f;
            _initialized = false;
        }

        /// <summary>
        /// Gets the smoothing factor value.
        /// </summary>
        public float SmoothingFactor => _smoothingFactor;

        /// <summary>
        /// Apply smoothing to input values.
        /// </summary>
        /// <param name="x">Raw X input.</param>
        /// <param name="y">Raw Y input.</param>
        /// <returns>Smoothed (x, y) tuple.</returns>
        public Tuple<float, float> Smooth(float x, float y)
        {
            if (!_initialized)
            {
                _prevX = x;
                _prevY = y;
                _initialized = true;
                return Tuple.Create(x, y);
            }

            // Exponential moving average
            float smoothedX = _prevX * _smoothingFactor + x * (1.0f - _smoothingFactor);
            float smoothedY = _prevY * _smoothingFactor + y * (1.0f - _smoothingFactor);

            _prevX = smoothedX;
            _prevY = smoothedY;

            return Tuple.Create(smoothedX, smoothedY);
        }

        /// <summary>
        /// Reset the smoother state.
        /// </summary>
        public void Reset()
        {
            _initialized = false;
        }

        /// <summary>
        /// Gets whether the smoother has been initialized.
        /// </summary>
        public bool IsInitialized => _initialized;
    }
}

