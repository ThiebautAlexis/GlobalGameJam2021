// ===== Global Game Jam 2021 - https://github.com/ThiebautAlexis/GlobalGameJam2021 ===== //
//
// Notes:
//
// ====================================================================================== //

using EnhancedEditor;
using UnityEngine;

namespace GlobalGameJam2021
{
	public class SurfaceItem : BouncingObject
	{
		#region Methods
		private void Start()
		{
			float _value = Random.Range(.3f, .65f);
			transform.localScale = new Vector3(_value, _value, 1);	
		}
		public override bool OnEnter(Digger _digger) => base.OnEnter(_digger);
		#endregion

	}
}
