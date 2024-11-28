    using UnityEngine;

    /// <summary>
    /// Modifies a camera to allows you to control the fog settings for that camera separately from the global scene fog or other cameras. 
    /// </summary>
    [RequireComponent(typeof(Camera))]
    [ExecuteInEditMode]
    public class CameraFog : MonoBehaviour
    {
        /// <summary>
        /// The enabled state weather or not fog will be visible.
        /// </summary>
        public bool Enabled;

        /// <summary>
        /// The start distance from the camera where the fog will be drawn.
        /// </summary>
        public float StartDistance;

        /// <summary>
        /// The end distance from the camera where the fog will be drawn.
        /// </summary>
        public float EndDistance;

        /// <summary>
        /// The fog mode that controls how the fog is rendered.
        /// </summary>
        public FogMode Mode;

        /// <summary>
        /// The density of the fog that is rendered.
        /// </summary>
        public float Density;

        /// <summary>
        /// The fog color.
        /// </summary>
        public Color Color;

        /// <summary>
        /// Stores the pre-render state of the start distance.
        /// </summary>
        float _startDistance;

        /// <summary>
        /// Stores the pre-render state of the end  distance.
        /// </summary>
        float _endDistance;

        /// <summary>
        /// Stores the pre-render state of the fog mode.
        /// </summary>
        FogMode _mode;

        /// <summary>
        /// Stores the pre-render state of the density.
        /// </summary>
        float _density;

        /// <summary>
        /// Stores the pre-render state of the fog color.
        /// </summary>
        Color _color;
        
        /// <summary>
        /// Stores the pre-render state wheather or not the fog is enabled.
        /// </summary>
        bool _enabled;

        /// <summary>
        /// Event that is fired before any camera starts rendering.
        /// </summary>
        void OnPreRender()
        {
            _startDistance = RenderSettings.fogStartDistance;
            _endDistance = RenderSettings.fogEndDistance;
            _mode = RenderSettings.fogMode;
            _density = RenderSettings.fogDensity;
            _color = RenderSettings.fogColor;
            _enabled = RenderSettings.fog;

            RenderSettings.fog = Enabled;
            RenderSettings.fogStartDistance = StartDistance;
            RenderSettings.fogEndDistance = EndDistance;
            RenderSettings.fogMode = Mode;
            RenderSettings.fogDensity = Density;
            RenderSettings.fogColor = Color;
        }

        /// <summary>
        /// Event that is fired after any camera finishes rendering.
        /// </summary>
        void OnPostRender()
        {
            RenderSettings.fog = _enabled;
            RenderSettings.fogStartDistance = _startDistance;
            RenderSettings.fogEndDistance = _endDistance;
            RenderSettings.fogMode = _mode;
            RenderSettings.fogDensity = _density;
            RenderSettings.fogColor = _color;
        }
    }
