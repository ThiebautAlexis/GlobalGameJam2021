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

        #region Travel
        private bool isTraveling = false;
        private float travelVar = 0;
        private Vector3 travelDestination = new Vector3();

        // -----------------------

        public void StartTravel(Vector3 _destination)
        {
            isTraveling = true;
            travelVar = 0;
            travelDestination = new Vector3(_destination.x, _destination.y, transform.position.z);
        }

        public void StopTravel(Vector3 _distance)
        {
            travelDestination -= _distance;
            transform.parent.position -= _distance;
        }
        #endregion

        #region Screenshake
        private bool isScreenshaking = false;

        private float force = 0;
        private int maxAngle = 0;
        private Vector2 maxOffset = new Vector2();

        // -----------------------

        /// <summary>
        /// Use this to shake the camera.
        /// </summary>
        /// <param name="_trauma">Shake related value, kept between 0 and 1.</param>
        public void Shake(float _trauma, bool _forceReset = false)
        {
            trauma = Mathf.Clamp01((_forceReset ? 0 : trauma) + _trauma);
            isScreenshaking = true;

            force = attributes.ShakeForce;
            maxAngle = attributes.ShakeMaxAngle;
            maxOffset = attributes.ShakeMaxOffset;
        }

        public void Vibrate()
        {
            isScreenshaking = true;

            if (trauma < attributes.Vibration)
                trauma = attributes.Vibration;

            force = attributes.VibrationForce;
            maxAngle = attributes.VibrationMaxAngle;
            maxOffset = attributes.VibrationMaxOffset;
        }
        #endregion

        #region Monobehaviour
        private void Awake() => Instance = this;

        private void Update()
        {
            // Travel in space.
            if (isTraveling)
            {
                travelVar = Mathf.Min(travelVar + Time.deltaTime, attributes.TravelSpeed[attributes.TravelSpeed.length - 1].time);
                float _speed = attributes.TravelSpeed.Evaluate(travelVar);

                transform.parent.position = Vector3.MoveTowards(transform.parent.position, travelDestination, Time.deltaTime * _speed);
                if (transform.parent.position == travelDestination)
                    isTraveling = false;
            }

            // Screenshake thing.
            if (isScreenshaking)
            {
                float _trauma = Mathf.Pow(trauma, 2);

                float _angle = maxAngle * _trauma * ((Mathf.PerlinNoise(0, Time.time * attributes.ShakeForce) * 2) - 1);
                float _offsetX = maxOffset.x * _trauma * ((Mathf.PerlinNoise(1, Time.time * attributes.ShakeForce) * 2) - 1);
                float _offsetY = maxOffset.y * _trauma * ((Mathf.PerlinNoise(2, Time.time * attributes.ShakeForce) * 2) - 1);

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
