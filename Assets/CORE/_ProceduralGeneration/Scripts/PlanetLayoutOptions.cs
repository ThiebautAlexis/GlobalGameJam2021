// ===== Global Game Jam 2021 - https://github.com/ThiebautAlexis/GlobalGameJam2021 ===== //
//
// Notes:
//
// ====================================================================================== //

using EnhancedEditor;
using UnityEngine;

namespace GlobalGameJam2021
{
    [CreateAssetMenu(fileName = "DAT_PlanetLayoutOptions", menuName = "Datas/Layout/PlanetLayoutOptions", order = 50)]
	public class PlanetLayoutOptions : ScriptableObject
    {
        #region Content
        [HorizontalLine(1, order = 0), Section("Planet Underground Layout Options", order = 1), Space(order = 2)]
        [SerializeField, MinMax(1, 100)] private Vector2Int propsCount = new Vector2Int(5, 10);
        public int Count => Random.Range(propsCount.x, propsCount.y);
        [SerializeField, Range(.5f, 10.0f)] private float globalSpacing = 1.0f;
        [SerializeField, Range(0.0f,1.0f)] private float chunkProbability = 0.0f;
        [SerializeField, Range(0.0f, 1.0f)] private float lavaProbability = 0.0f;
        public float LavaProbability => lavaProbability; 
        [SerializeField, Range(0, 10)] private int maxLavaCount = 0;
        public int MaxLavaCount => maxLavaCount;

        [HorizontalLine(1, order = 0), Section("Planet Surface Layout Options", order = 1), Space(order = 2)]
        [SerializeField, MinMax(1, 100)] private Vector2Int surfacePropsCount = new Vector2Int(5, 10);
        public int SurfacePropsCount => Random.Range(surfacePropsCount.x, surfacePropsCount.y);

        #endregion

        #region Methods
        #endregion
    }
}
