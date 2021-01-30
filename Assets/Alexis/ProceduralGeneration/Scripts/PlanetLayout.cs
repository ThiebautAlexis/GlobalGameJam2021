// ===== Global Game Jam 2021 - https://github.com/ThiebautAlexis/GlobalGameJam2021 ===== //
//
// Notes:
//
// ====================================================================================== //

using EnhancedEditor;
using UnityEngine;
using System.Collections.Generic; 

namespace GlobalGameJam2021
{
    [CreateAssetMenu(fileName = "DAT_PlanetLayout", menuName = "Datas/PlanetLayout", order = 50)]
    public class PlanetLayout : ScriptableObject
    {
        #region Content
        [HorizontalLine(1, order = 0), Section("PlanetLayout", order = 1), Space(order = 2)]
        [SerializeField] private Sprite planetSprite;
        public Sprite PlanetSprite => planetSprite; 
        [SerializeField] private LayoutZone[] layoutZones = new LayoutZone[] { }; 
        [SerializeField] private PlanetLayoutOptions[] options = new PlanetLayoutOptions[] { };
        

        #endregion


        #region Methods
        public List<Vector2> GenerateLayout(Vector2 _origin)
        {
            // Select the options for this generation
            int _optionsIndex = Random.Range(0, options.Length);
            PlanetLayoutOptions _options = options[_optionsIndex];
            // Get the number of props instancied 
            int[] _zonesCount = new int[layoutZones.Length];
            int _globalCount = _options.Count;
            int _c; 
            for (int i = 0; i < _zonesCount.Length; i++)
            {
                _c = layoutZones[i].Count;
                _c = Mathf.Min(_c, _globalCount); 
                _zonesCount[i] = _c;
                _globalCount -= _c;
                if (_globalCount <= 0) break; 
            }
            List<Vector2> positions = new List<Vector2>();
            for (int i = 0; i < layoutZones.Length; i++)
            {
                positions.AddRange(DiscSampling.GeneratePoints(_origin + layoutZones[i].OffsetPosition, layoutZones[i].Radius, layoutZones[i].Spacing, _zonesCount[i]));
            }
            
            // First of all, we need to instanciate the main objects (tools, etc...)

            // Then Instantiate the props and the bonus


            return positions; 
        }

        public void Draw(Vector3 _origin)
        {
            for (int i = 0; i < layoutZones.Length; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(_origin + (Vector3)layoutZones[i].OffsetPosition, layoutZones[i].Radius);
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(_origin + (Vector3)layoutZones[i].OffsetPosition, layoutZones[i].Spacing);
            }
        }
        #endregion
    }

}

