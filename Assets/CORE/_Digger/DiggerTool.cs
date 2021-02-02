// ===== Global Game Jam 2021 - https://github.com/ThiebautAlexis/GlobalGameJam2021 ===== //
//
// Notes:
//
// ====================================================================================== //

using EnhancedEditor;
using UnityEngine;

namespace GlobalGameJam2021
{
	public class DiggerTool : MonoBehaviour
    {
        #region Content
        private Vector3 direction = new Vector3();
        private float maxDistance = 0;
        private float speed = 0;

        [SerializeField, ReadOnly] private Vector3[] bezierCurve = new Vector3[16];
        [SerializeField, ReadOnly] private int bezierCurveIndex = 0;

        // -----------------------

        public void Initialized(Vector3 _direction, Vector3 _arc, float _maxDistance, float _speed)
        {
            direction = _direction;
            maxDistance = _maxDistance;
            speed = _speed;

            Vector3 _p0 = transform.position;
            Vector3 _p2 = _direction * _maxDistance;
            Vector3 _p1 = _p0 + (_arc * ((_p2 - _p0).magnitude / 1.25f) * (1 + (_speed * Time.deltaTime * .5f)));
            float _t;

            bezierCurve[0] = _p0;
            bezierCurve[bezierCurve.Length - 1] = _p2;
            bezierCurveIndex = 1;

            for (int _i = 1; _i < (bezierCurve.Length - 1); _i++)
            {
                _t = _i / (float)(bezierCurve.Length - 1);
                bezierCurve[_i] = (Mathf.Pow(1f - _t, 2f) * _p0) + (2f * (1f - _t) * _t * _p1) + ((_t * _t) * _p2);
            }
        }

        private void Update()
        {
            Vector3 _position = transform.position;
            float _speed = speed * Time.deltaTime;
            float _distance;

            while (true)
            {
                _distance = (bezierCurve[bezierCurveIndex] - _position).magnitude;
                if (_distance < _speed)
                {
                    if (bezierCurveIndex < (bezierCurve.Length - 1))
                    {
                        _speed -= _distance;
                        _position = bezierCurve[bezierCurveIndex];

                        bezierCurveIndex++;
                    }
                    else
                    {
                        Destroy(gameObject);
                        return;
                    }
                }
                else
                {
                    _position += (bezierCurve[bezierCurveIndex] - _position).normalized * _speed;
                    break;
                }
            }

            transform.position = _position;
        }
        #endregion
    }
}
