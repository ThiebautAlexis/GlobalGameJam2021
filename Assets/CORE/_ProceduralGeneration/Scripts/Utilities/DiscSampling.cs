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
		private static bool IsValid(Vector2 _point, Vector2 _center, float _radius, List<Trigger> _otherTrigger, float _spacing)
		{
			if (Vector2.Distance(_point, _center) > _radius - _spacing)
			{
				return false;
			}
			float _maxSpacing; 
			for (int i = 0; i < _otherTrigger.Count; i++)
			{
				_maxSpacing = _spacing + _otherTrigger[i].SpacingRange; 
				if (Vector2.Distance(_point, _otherTrigger[i].transform.position) < _maxSpacing) return false;
			}
			return true;
		}

		public static Vector2 GetValidPosition(Vector2 _center, float _radius, Trigger _spawnedTrigger, List<Trigger> _previousTrigger, out bool _positionIsValid, int _samplesCount = 10,  int _samplesBeforeRejections = 50)
		{
			_positionIsValid = true; 
			float _angle;
			Vector2 _dir;
			Vector2 _candidate;
			if(_previousTrigger.Count == 0)
			{
				for (int j = 0; j < _samplesBeforeRejections; j++)
				{
					_angle = Random.value * Mathf.PI * 2;
					_dir = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle));
					_candidate = _center + _dir * Random.Range(0, _spawnedTrigger.SpacingRange * 2.0f);
					if (IsValid(_candidate, _center, _radius, _previousTrigger, _spawnedTrigger.SpacingRange))
					{
						return _candidate;
					}
				}			
			}
			else
			{
				int _index;
				Trigger _t; 
				for (int i = 0; i < _samplesCount; i++)
				{
					_index = Random.Range(0, _previousTrigger.Count);
					_t = _previousTrigger[_index];
					for (int j = 0; j < _samplesBeforeRejections; j++)
					{
						_angle = Random.value * Mathf.PI * 2;
						_dir = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle));
						_candidate = (Vector2)_t.transform.position + _dir * Random.Range(_t.SpacingRange,_radius);
						if (IsValid(_candidate, _center, _radius, _previousTrigger, _spawnedTrigger.SpacingRange))
						{
							return _candidate; 
						}
					}
				}
			}
			_positionIsValid = false; 
			return Vector2.zero; 
		}
		#endregion
    }
}
