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
        [SerializeField] private LayoutZone[] layoutZones = new LayoutZone[] { };
        [SerializeField] private LayoutProps[] props = new LayoutProps[] { };

        [HorizontalLine(1, order = 0), Section("Options", order = 1), Space(order = 2)]
        [SerializeField] private PlanetLayoutOptions[] options = new PlanetLayoutOptions[] { };
        #endregion

        #region Methods
        public Transform GenerateLayout(Vector2 _origin)
        {
            Transform _t =Instantiate(planet, _origin, Quaternion.identity).transform;        

            // Select the options for this generation
            int _index = Random.Range(0, options.Length);
            PlanetLayoutOptions _options = options[_index];


            int _c;
            _index = Random.Range(0, props.Length);
            LayoutProps _props = props[_index];
            int _anchorIndex;
            // Instantiate the Surfaces Props
            _c = _options.SurfacePropsCount;
            _c = Mathf.Min(_c, surfacesArches.Length);
            Vector2 _pos;
            for (int i = 0; i < _c; i++)
            {
                _anchorIndex = Random.Range(0, surfacesArches.Length);
                _pos = surfacesArches[_anchorIndex].GetRandomPosition(Random.value);
                _index = Random.Range(0, _props.SurfacesProps.Length);
                Instantiate(_props.SurfacesProps[_index], _pos, Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, _pos + surfacesArches[_anchorIndex].GetMediumTangent())), _t);
            }

            // Instantiate the Surface Props 
            /*
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
            */

            List<Trigger>[] spawnedTriggers = new List<Trigger>[layoutZones.Length];
            for (int i = 0; i < spawnedTriggers.Length; i++)
            {
                spawnedTriggers[i] = new List<Trigger>();
            }

            Trigger _spawningTrigger;
            Vector2 _spawningPosition;
            bool _positionIsValid; 
            // First of all, we need to instanciate the main objects (tools, etc...)
            for (int i = 0; i < _props.Tools.Length; i++)
            {
                if (layoutZones.Length == 0)
                    break;
                _spawningTrigger = _props.Tools[i];
                for (int j = 0; j < 10; j++)
                {
                    _index = Random.Range(0, layoutZones.Length);
                    _spawningPosition = DiscSampling.GetValidPosition(_origin + layoutZones[_index].OffsetPosition, layoutZones[_index].Radius, _spawningTrigger, spawnedTriggers[_index], out _positionIsValid);
                    if (!_positionIsValid)
                    {
                        continue;
                    }
                    _spawningTrigger = Instantiate(_spawningTrigger, _spawningPosition, Quaternion.Euler(0, 0, Random.value * 360), _t);
                    spawnedTriggers[_index].Add(_spawningTrigger);
                    break;
                }
            }
            float _value; 
            for (int i = 0; i < _options.MaxLavaCount; i++)
            {
                if (layoutZones.Length == 0)
                    break;

                _value = Random.value;
                if (_value <= _options.LavaProbability)
                {
                    _spawningTrigger = _props.LavaLakes[Random.Range(0, _props.LavaLakes.Length)];
                    _index = Random.Range(0, layoutZones.Length);
                    _spawningPosition = DiscSampling.GetValidPosition(_origin + layoutZones[_index].OffsetPosition, layoutZones[_index].Radius, _spawningTrigger, spawnedTriggers[_index], out _positionIsValid);
                    if (!_positionIsValid) continue;
                    _spawningTrigger = Instantiate(_spawningTrigger, _spawningPosition, Quaternion.Euler(0, 0, Random.value * 360), _t);
                    spawnedTriggers[_index].Add(_spawningTrigger);
                }
            }
            // Then Instantiate the props
            for (int i = 0; i < _options.Count; i++)
            {
                _index = Random.Range(0, _props.Props.Length);
                _spawningTrigger = _props.Props[_index];
                _index = Random.Range(0, layoutZones.Length);
                _spawningPosition = DiscSampling.GetValidPosition(_origin + layoutZones[_index].OffsetPosition, layoutZones[_index].Radius, _spawningTrigger, spawnedTriggers[_index], out _positionIsValid);
                if (!_positionIsValid) continue;
                _spawningTrigger = Instantiate(_spawningTrigger, _spawningPosition, Quaternion.Euler(0, 0, Random.value * 360), _t);
                spawnedTriggers[_index].Add(_spawningTrigger);
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

        public Vector2 GetMediumTangent()
        {
            return (start + startTangent) + (end + endTangent).normalized; 
        }
    }
}

