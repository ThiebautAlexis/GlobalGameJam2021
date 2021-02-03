// ===== Global Game Jam 2021 - https://github.com/ThiebautAlexis/GlobalGameJam2021 ===== //
//
// Notes:
//
// ====================================================================================== //

using EnhancedEditor;
using UnityEngine;

namespace GlobalGameJam2021
{
	public class BouncingObject : Trigger
    {
        #region Content
        [HorizontalLine(1, order = 0), Section("BOUCNING ITEM", order = 1)]

        [SerializeField] protected bool doBounce = false;

        public override bool OnEnter(Digger _digger)
        {
            if (doBounce)
                _digger.Bounce(collider);
            return true;
        }

        private void OnDrawGizmos()
        {
            if(doBounce)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, spacingRange);
            }
        }
        #endregion
    }
}
