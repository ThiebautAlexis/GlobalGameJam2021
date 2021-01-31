using UnityEngine;

namespace GlobalGameJam2021
{
    public static class BezierUtility
    {
        public static Vector2 EvaluateCubicCurve(Vector2 _start, Vector2 _end, Vector2 _startTangent, Vector2 _endTangent, float _t)
        {
            Vector2 _v1 = EvaluateQuadraticCurve(_start, _endTangent, _startTangent, _t);
            Vector2 _v2 = EvaluateQuadraticCurve(_startTangent, _end, _endTangent, _t);
            return Vector2.Lerp(_v1, _v2, _t);
        }

        public static Vector2 EvaluateQuadraticCurve(Vector2 _start, Vector2 _end, Vector2 _tangent, float _t)
        {
            Vector2 _v1 = Vector2.Lerp(_start, _tangent, _t);
            Vector2 _v2 = Vector2.Lerp(_tangent, _end, _t);
            return Vector2.Lerp(_v1, _v2, _t);
        }
    }
}

