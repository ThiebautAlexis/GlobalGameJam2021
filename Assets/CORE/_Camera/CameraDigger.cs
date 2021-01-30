// ===== Global Game Jam 2021 - https://github.com/ThiebautAlexis/GlobalGameJam2021 ===== //
//
// Notes:
//
// ====================================================================================== //

using EnhancedEditor;
using UnityEngine;

namespace GlobalGameJam2021
{
	public class CameraDigger : MonoBehaviour
    {
        #region Fields / Properties
        public static CameraDigger Instance = null;

        [HorizontalLine(1, order = 0), Section("CAMERA DIGGER", order = 1)]

        [SerializeField, Required] private CameraDiggerAttributes attributes = null;
        [SerializeField, Required] private new Camera camera = null;

        // -----------------------

        [HorizontalLine(1)]

        [SerializeField, ReadOnly] private float trauma = 0;
        #endregion

        #region Methods

        #region Screenshake
        private bool isScreenshaking = false;

        // -----------------------

        /// <summary>
        /// Use this to shake the camera.
        /// </summary>
        /// <param name="_trauma">Shake related value, kept between 0 and 1.</param>
        public void Shake(float _trauma)
        {
            trauma = Mathf.Clamp01(trauma + _trauma);
            isScreenshaking = true;
        }
        #endregion

        #region Monobehaviour
        private void Awake() => Instance = this;

        private void Update()
        {
            // Screenshake thing.
            if (isScreenshaking)
            {
                float _trauma = Mathf.Pow(trauma, 2);

                float _angle = attributes.ShakeMaxAngle * _trauma * ((Mathf.PerlinNoise(0, Time.time * attributes.ShakeForce) * 2) - 1);
                float _offsetX = attributes.ShakeMaxOffset.x * _trauma * ((Mathf.PerlinNoise(1, Time.time * attributes.ShakeForce) * 2) - 1);
                float _offsetY = attributes.ShakeMaxOffset.y * _trauma * ((Mathf.PerlinNoise(2, Time.time * attributes.ShakeForce) * 2) - 1);

                camera.transform.localEulerAngles = new Vector3(0, 0, _angle);
                camera.transform.localPosition = new Vector3(_offsetX, _offsetY, 0);

                trauma = Mathf.Max(0, trauma - (Time.deltaTime * attributes.TraumaSoftening));
                if (trauma == 0)
                {
                    isScreenshaking = false;
                    camera.transform.localPosition = Vector3.zero;
                    camera.transform.localEulerAngles = Vector3.zero;
                }
            }
        }
        #endregion

        #endregion
    }
}
