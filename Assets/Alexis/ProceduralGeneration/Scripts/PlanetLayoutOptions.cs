// ===== Global Game Jam 2021 - https://github.com/ThiebautAlexis/GlobalGameJam2021 ===== //
//
// Notes:
//
// ====================================================================================== //

using EnhancedEditor;
using UnityEngine;

namespace GlobalGameJam2021
{
    [CreateAssetMenu(fileName = "DAT_PlanetLayoutOptions", menuName = "Datas/PlanetLayoutOptions", order = 50)]
	public class PlanetLayoutOptions : ScriptableObject
    {
        #region Content
        [HorizontalLine(1, order = 0), Section("PlanetLayoutOptions", order = 1), Space(order = 2)]
        [SerializeField, MinMax(1, 100)] private Vector2Int propsCount = new Vector2Int(5, 10);
        public int Count => Random.Range(propsCount.x, propsCount.y);
        [SerializeField, Range(.5f, 10.0f)] private float globalSpacing = 1.0f;
        [SerializeField, Range(0.0f,1.0f)] private float chunkProbability = 0.0f;
        [SerializeField, Range(0.0f, 1.0f)] private float lavaProbability = 0.0f;  
        [SerializeField, Range(0, 10)] private float maxLavaCount = 0.0f;  
        #endregion

        #region Methods
        #endregion
    }
}
