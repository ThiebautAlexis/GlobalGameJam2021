// ===== Global Game Jam 2021 - https://github.com/ThiebautAlexis/GlobalGameJam2021 ===== //
//
// Notes:
//
// ====================================================================================== //

using EnhancedEditor;
using UnityEngine;

namespace GlobalGameJam2021
{
	public class SurfaceItem : Trigger
	{
		#region Fields / Properties
		//[HorizontalLine(1, order = 0), Section("SurfaceItem", order = 1)]
		#endregion

		#region Methods
		private void Start()
		{
			float _value = Random.Range(.3f, .65f);
			transform.localScale = new Vector3(_value, _value, 1);	
		}
		public override bool OnEnter(Digger _digger)
		{
			return false;
		}
		#endregion

	}
}
