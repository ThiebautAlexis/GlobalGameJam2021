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

        [HorizontalLine(1)]

        public Vector2 ShakeMaxOffset = Vector2.one;
        [Range(0, 90)] public int ShakeMaxAngle = 5;
        #endregion
    }
}
