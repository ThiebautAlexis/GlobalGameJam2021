// ===== Global Game Jam 2021 - https://github.com/ThiebautAlexis/GlobalGameJam2021 ===== //
//
// Notes:
//
// ====================================================================================== //

namespace GlobalGameJam2021
{
    public enum KeyItemType
    {
        Unknown,
        Pickaxe,
        Lantern
    }

	public abstract class KeyItem : LootItem
    {
        #region Content
        public virtual KeyItemType ItemType => KeyItemType.Unknown;

        public override bool OnEnter(Digger _digger)
        {
            GameManager.Instance.PickupKeyItem(transform.position);
            _digger.PickupKeyItem(ItemType);

            return base.OnEnter(_digger);
        }

        private void Awake() => GameManager.Instance.IncreaseKeyItemCount();
        #endregion
    }
}
