// ===== Global Game Jam 2021 - https://github.com/ThiebautAlexis/GlobalGameJam2021 ===== //
//
// Notes:
//
// ====================================================================================== //

using EnhancedEditor;
using UnityEngine;

namespace GlobalGameJam2021
{
	public class LootItem : Trigger
    {
        #region Content
        [HorizontalLine(1, order = 0), Section("LOOT ITEM", order = 1)]

        [SerializeField] private LootFX fx = LootFX.Generic;
        [SerializeField] private LootSound sound = LootSound.Amber;

        [Space]

        [SerializeField, Min(0)] private int value = 1000;

        public override bool OnEnter(Digger _digger)
        {
            GameManager.Instance.InstantiateFX(fx, transform.position);
            GameManager.Instance.PlaySound(sound, transform.position);
            GameManager.Instance.IncreaseScore(value);

            Destroy(gameObject);
            return false;
        }
        #endregion
    }
}
