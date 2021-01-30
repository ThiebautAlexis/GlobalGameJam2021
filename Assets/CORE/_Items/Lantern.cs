// ===== Global Game Jam 2021 - https://github.com/ThiebautAlexis/GlobalGameJam2021 ===== //
//
// Notes:
//
// ====================================================================================== //

namespace GlobalGameJam2021
{
	public class Lantern : Trigger
    {
		#region Methods
		public override bool OnEnter(Digger _digger)
        {
			_digger.PickupLantern();
			Destroy(gameObject);
			return false;
        }
		#endregion
	}
}
