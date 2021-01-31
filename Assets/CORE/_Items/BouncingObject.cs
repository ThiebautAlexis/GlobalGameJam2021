// ===== Global Game Jam 2021 - https://github.com/ThiebautAlexis/GlobalGameJam2021 ===== //
//
// Notes:
//
// ====================================================================================== //

namespace GlobalGameJam2021
{
	public class BouncingObject : Trigger
    {
        #region Content
        public override bool OnEnter(Digger _digger)
        {
            _digger.Bounce(collider);
            return true;
        }
        #endregion
    }
}
