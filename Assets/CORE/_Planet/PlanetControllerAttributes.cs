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
    [CreateAssetMenu(fileName = "DAT_PlanetControllerAttributes", menuName = "Datas/PlanetControllerAttributes", order = 50)]
	public class PlanetControllerAttributes : ScriptableObject
    {
        #region Content
        [HorizontalLine(1, order = 0), Section("PLANET CONTROLLER ATTRIBUTES", order = 1), Space(order = 2)]

        public AnimationCurve RotationSpeed = new AnimationCurve();

        [HorizontalLine(1)]

        public InputAction Move = new InputAction();
        #endregion

        #region Methods
        public void EnableInputs() => Move.Enable();

        public void DisableInputs() => Move.Disable();
        #endregion
    }
}
