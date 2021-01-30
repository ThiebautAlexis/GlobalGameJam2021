// ===== Global Game Jam 2021 - https://github.com/ThiebautAlexis/GlobalGameJam2021 ===== //
//
// Notes:
//
// ====================================================================================== //

using EnhancedEditor;
using UnityEngine;

namespace GlobalGameJam2021
{
	public class Ore : Trigger
    {
        #region Fields / Properties
        [HorizontalLine(1, order = 0), Section("Ore", order = 1)]

        [SerializeField, Min(0)] private int value = 1000;
        #endregion

        #region Methods
        public override bool OnEnter(Digger _digger)
        {
            GameManager.Instance.IncreaseScore(value);
            Destroy(gameObject);
            return false;
        }

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
