// ===== Global Game Jam 2021 - https://github.com/ThiebautAlexis/GlobalGameJam2021 ===== //
//
// Notes:
//
// ====================================================================================== //

using EnhancedEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GlobalGameJam2021
{
    [CreateAssetMenu(fileName = "DAT_GameManagerAttributes", menuName = "Datas/GameManagerAttributes", order = 50)]
	public class GameManagerAttributes : ScriptableObject
    {
        #region Content
        [HorizontalLine(1, order = 0), Section("GAME MANAGE ATTRIBUTES", order = 1), Space(order = 2)]

        public InputAction ActionInput = new InputAction();
        public InputAction QuitInput = new InputAction();
        #endregion

        #region Methods
        public void EnableInputs()
        {
            ActionInput.Enable();
            QuitInput.Enable();
        }

        public void DisableInputs()
        {
            ActionInput.Disable();
            QuitInput.Disable();
        }
        #endregion
    }
}
