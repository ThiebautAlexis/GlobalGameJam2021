// ===== Global Game Jam 2021 - https://github.com/ThiebautAlexis/GlobalGameJam2021 ===== //
//
// Notes:
//
// ====================================================================================== //

using EnhancedEditor;
using UnityEngine;

namespace GlobalGameJam2021
{
    public enum KeyItemType
    {
        Unknown,
        Pickaxe,
        Lantern
    }

	public abstract class KeyItem : Trigger
    {
        #region Content
        [SerializeField, Required] private GameObject fx = null;

        public virtual KeyItemType ItemType => KeyItemType.Unknown;

        public override bool OnEnter(Digger _digger)
        {
            GameManager.Instance.PickupKeyItem();
            _digger.PickupKeyItem(ItemType);

            Instantiate(fx, transform.position, Quaternion.identity);
            Destroy(gameObject);
            return false;
        }

        private void Awake() => GameManager.Instance.IncreaseKeyItemCount();
        #endregion
    }
}
