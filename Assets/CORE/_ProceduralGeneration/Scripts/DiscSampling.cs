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
	public static class DiscSampling
    {
		#region Methods
		public static List<Vector2> GeneratePoints(Vector2 _center, float _radius, float _spacing, int _count, int _samplesBeforeRejections = 50)
		{
			List<Vector2> _points = new List<Vector2>();
			List<Vector2> _spawnPoints = new List<Vector2>(); 
			float _angle = Random.value * Mathf.PI * 2;
			Vector2 _dir = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)); 
			Vector2 _candidate = _center + _dir * Random.Range(_spacing, _spacing * 2);
			if (!IsValid(_candidate, _center, _radius, _points, _spacing)) return _points;
			_points.Add(_candidate);
			_spawnPoints.Add(_center);
			_spawnPoints.Add(_candidate);
			int _index;
			Vector2 _base; 
			for (int i = 0; i < _count-1; i++)
			{
				_index = Random.Range(0, _spawnPoints.Count);
				_base = _spawnPoints[_index];
				for (int j = 0; j < _samplesBeforeRejections; j++)
				{
					_angle = Random.value * Mathf.PI * 2;
					_dir = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle));
					_candidate = _base + _dir * Random.Range(_spacing, _spacing * 2);
					if (IsValid(_candidate, _center, _radius, _points, _spacing))
					{
						_points.Add(_candidate);
						_spawnPoints.RemoveAt(_index);
						_spawnPoints.Add(_candidate);
						break;
					}
				}
			}
			return _points; 
		}
	
		private static bool IsValid(Vector2 _point, Vector2 _center, float _radius, List<Vector2> _otherPoints, float _spacing)
		{
			if (Vector2.Distance(_point, _center) > _radius) return false;
			for (int i = 0; i < _otherPoints.Count; i++)
			{
				if (Vector2.Distance(_point, _otherPoints[i]) < _spacing) return false; 
			}
			return true; 
		}
		#endregion
    }
}
