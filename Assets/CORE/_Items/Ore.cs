// ===== Global Game Jam 2021 - https://github.com/ThiebautAlexis/GlobalGameJam2021 ===== //
//
// Notes:
//
// ====================================================================================== //

using UnityEngine;

namespace GlobalGameJam2021
{
	public class Ore : LootItem
    {
        #region Content
        private void Start()
        {
            transform.localRotation = Quaternion.Euler(0, 0, Random.value * 360);
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).localRotation = Quaternion.Euler(0, 0, Random.value * 360);
            }
        }
        #endregion
    }
}
