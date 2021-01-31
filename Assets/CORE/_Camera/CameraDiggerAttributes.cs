// ===== Global Game Jam 2021 - https://github.com/ThiebautAlexis/GlobalGameJam2021 ===== //
//
// Notes:
//
// ====================================================================================== //

using EnhancedEditor;
using UnityEngine;

namespace GlobalGameJam2021
{
    [CreateAssetMenu(fileName = "DAT_CameraDiggerAttributes", menuName = "Datas/CameraDiggerAttributes", order = 50)]
	public class CameraDiggerAttributes : ScriptableObject
    {
        #region Attributes
        [HorizontalLine(1, order = 0), Section("CAMERA DIGGER ATTRIBUTES", order = 1)]

        [Min(0)] public float ShakeForce = .2f;
        [Min(0)] public float TraumaSoftening = .2f;

        [Space]

        public Vector2 ShakeMaxOffset = Vector2.one;
        [Range(0, 90)] public int ShakeMaxAngle = 5;

        [HorizontalLine(1)]

        [Range(0, 1)] public float Vibration = .01f;

        [Min(0)] public float VibrationForce = .2f;

        public Vector2 VibrationMaxOffset = Vector2.one;
        [Range(0, 90)] public int VibrationMaxAngle = 5;
        #endregion
    }
}
