// ===== Global Game Jam 2021 - https://github.com/ThiebautAlexis/GlobalGameJam2021 ===== //
//
// Notes:
//
// ====================================================================================== //

using System.Collections;
using EnhancedEditor;
using UnityEngine;

namespace GlobalGameJam2021
{
	public class GameManager : MonoBehaviour
    {
        #region Fields / Properties
        public static GameManager Instance = null;

        [HorizontalLine(1, order = 0), Section("GAME MANAGER", order = 1)]

        [SerializeField, Required] private GameManagerAttributes attributes = null;
        #endregion

        #region Methods

        #region Monobehaviour
        private void Awake() => Instance = this;

        private IEnumerator Start()
        {
            while (!attributes.ActionInput.triggered)
                yield return null;

            // Start the game.
        }

        private void OnEnable()
        {
            attributes.EnableInputs();
        }

        private void OnDisable()
        {
            attributes.DisableInputs();
        }

        private void Update()
        {
            
        }
        #endregion

        #endregion
    }
}
