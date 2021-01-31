// ===== Global Game Jam 2021 - https://github.com/ThiebautAlexis/GlobalGameJam2021 ===== //
//
// Notes:
//
// ====================================================================================== //

namespace GlobalGameJam2021
{
	public class Trap : BouncingObject
	{
        #region Content
        public override bool OnEnter(Digger _digger)
        {
            GameManager.Instance.EmptyOxygenTank();
            return base.OnEnter(_digger);
        }
        #endregion
    }
}
