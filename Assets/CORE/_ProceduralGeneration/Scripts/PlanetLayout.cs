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
    [CreateAssetMenu(fileName = "DAT_PlanetLayout", menuName = "Datas/Layout/PlanetLayout", order = 50)]
    public class PlanetLayout : ScriptableObject
    {
        #region Content
        [HorizontalLine(1, order = 0), Section("PlanetLayout", order = 1), Space(order = 2)]
        [SerializeField] private PlanetController planet;
        public PlanetController Planet => planet;

        [SerializeField] private BezierArc[] surfacesArches = new BezierArc[] { };

        [HorizontalLine(1, order = 0), Section("Settings and Props", order = 1), Space(order = 2)]
        [SerializeField] private LayoutZone[] layoutZones = new LayoutZone[] { };
        [SerializeField] private LayoutProps[] props = new LayoutProps[] { };
        [SerializeField] private Sprite[] surfaceProps = new Sprite[] { }; 
        [SerializeField] private PlanetLayoutOptions[] options = new PlanetLayoutOptions[] { };
        #endregion

        #region Methods
        public Transform GenerateLayout(Vector2 _origin)
        {
            Transform _t =Instantiate(planet, _origin, Quaternion.identity).transform;        

            // Select the options for this generation
            int _index = Random.Range(0, options.Length);
            PlanetLayoutOptions _options = options[_index];

            // Instantiate the Surface Props 
            int _c = _options.SurfacePropsCount;
            _c = Mathf.Min(_c, surfacesArches.Length); 
            Vector2 _pos;
            float _value; 
            for (int i = 0; i < _c; i++)
            {
                _value = Random.value; 
                _index = Random.Range(0, surfacesArches.Length);
                _pos = surfacesArches[_index].GetRandomPosition(_value);
                SpriteRenderer _renderer = new GameObject().AddComponent<SpriteRenderer>();
                _renderer.transform.SetParent(_t); 
                _renderer.sprite = surfaceProps[Random.Range(0, surfaceProps.Length)];
                _renderer.transform.localPosition = _pos;
                _renderer.transform.localScale = new Vector3(.25f, .25f, 1);
                _renderer.transform.localRotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, _pos + surfacesArches[_index].GetMediumTangent(_value)));
            }

            // Get the number of props instancied 
            int[] _zonesCount = new int[layoutZones.Length];
            int _globalCount = _options.Count;
            for (int i = 0; i < _zonesCount.Length; i++)
            {
                _c = layoutZones[i].Count;
                _c = Mathf.Min(_c, _globalCount); 
                _zonesCount[i] = _c;
                _globalCount -= _c;
                if (_globalCount <= 0) break; 
            }

            List<Vector2> _positions = new List<Vector2>();
            for (int i = 0; i < layoutZones.Length; i++)
            {
                _positions.AddRange(DiscSampling.GeneratePoints(_origin + layoutZones[i].OffsetPosition, layoutZones[i].Radius, layoutZones[i].Spacing, _zonesCount[i]));
            }

            _index = Random.Range(0, props.Length);
            LayoutProps _props = props[_index];
            GameObject _go = new GameObject(); 
            // First of all, we need to instanciate the main objects (tools, etc...)
            for (int i = 0; i < _props.Tools.Length; i++)
            {
                _index = Random.Range(0,_positions.Count);
                Instantiate(_props.Tools[i], _positions[_index], Quaternion.Euler(0, 0, Random.value * 360), _t);
                _positions.RemoveAt(_index); 
            }
            // Then Instantiate the props and the bonus
            for (int i = 0; i < _positions.Count; i++)
            {
                _index = Random.Range(0, _props.Props.Length);
                Instantiate(_props.Props[_index], _positions[i], Quaternion.identity, _t);
            }

            return _t; 
        }        
        #endregion
    }

    [System.Serializable]
    public class BezierArc
    {
        [SerializeField] private Vector2 start = new Vector2(-1, 0); 
        [SerializeField] private Vector2 end = new Vector2(1, 0);
        [SerializeField] private Vector2 startTangent = new Vector2(.5f, .5f); 
        [SerializeField] private Vector2 endTangent = new Vector2(.5f, .5f);

        public Vector2 GetRandomPosition(float _value) => BezierUtility.EvaluateCubicCurve(start, end, start + startTangent, end + endTangent, _value);
        public Vector2 GetCenterPosition(float _value) => Vector2.Lerp(start, end, _value);

        public Vector2 GetMediumTangent(float _value)
        {
            Vector2 _v = (start + startTangent) + (end + endTangent).normalized; 
            return _v;
        }
    }
}

