// ===== Global Game Jam 2021 - https://github.com/ThiebautAlexis/GlobalGameJam2021 ===== //
//
// Notes:
//
// ====================================================================================== //

using EnhancedEditor;
using UnityEngine;

namespace GlobalGameJam2021
{
	public class LavaLake : Trigger
	{
		#region Fields / Properties
		//[HorizontalLine(1, order = 0), Section("LavaLake", order = 1)]

		#endregion

		#region Methods
		public override bool OnEnter(Digger _digger)
		{
			// Take Damages here; 
			return false; 
		}
		#endregion

	}
}
