// ===== Global Game Jam 2021 - https://github.com/ThiebautAlexis/GlobalGameJam2021 ===== //
//
// Notes:
//
// ====================================================================================== //

using EnhancedEditor;
using UnityEngine;

namespace GlobalGameJam2021
{
	public class DestructibleBouncingObject : BouncingObject
    {
		#region Fields / Properties
		[HorizontalLine(1, order = 0), Section("DestructibleBouncingObject", order = 1)]
		[SerializeField] private LootFX fx = LootFX.Generic;
        [SerializeField] private LootSound sound = LootSound.Amber;

		#endregion

		#region Methods
		public override bool OnEnter(Digger _digger)
		{
			if(doBounce && _digger.HasPickaxe)
			{
				GameManager.Instance.InstantiateFX(fx, transform.position);
				GameManager.Instance.PlaySound(sound, transform.position);

				Destroy(gameObject);
				return true;
			}
			return base.OnEnter(_digger);
		}
		#endregion
	}
}
