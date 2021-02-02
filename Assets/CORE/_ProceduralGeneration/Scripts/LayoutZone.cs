// ===== Global Game Jam 2021 - https://github.com/ThiebautAlexis/GlobalGameJam2021 ===== //
//
// Notes:
//
// ====================================================================================== //

using EnhancedEditor;
using UnityEngine;

namespace GlobalGameJam2021
{
    [CreateAssetMenu(fileName = "DAT_LayoutZone", menuName = "Datas/Layout/LayoutZone", order = 50)]
	public class LayoutZone : ScriptableObject
    {
        #region Content
        [HorizontalLine(1, order = 0), Section("Layout Zone", order = 1), Space(order = 2)]
        [SerializeField] private Vector2 offsetPosition = Vector2.zero;
        public Vector2 OffsetPosition => offsetPosition;
        [SerializeField] private float radius = 1.0f;
        public float Radius => radius;

        [HorizontalLine(1, order = 0), Section("Zone options", order = 1), Space(order = 2)]
        [SerializeField, MinMax(1, 20)] private Vector2Int count = new Vector2Int(1, 5);
        public int Count => Random.Range(count.x, count.y);
        #endregion
    }
}
