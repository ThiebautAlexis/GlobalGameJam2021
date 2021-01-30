// ===== Global Game Jam 2021 - https://github.com/ThiebautAlexis/GlobalGameJam2021 ===== //
//
// Notes:
//
// ====================================================================================== //

namespace GlobalGameJam2021
{
    public class Pickaxe : Trigger
    {
        #region Methods
        public override bool OnEnter(Digger _digger)
        {
            _digger.PickupPickaxe();
            Destroy(gameObject);
            return false;
        }
        #endregion
    }
}
