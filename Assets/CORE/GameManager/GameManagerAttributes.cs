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
    public enum LootSound
    {
        Bone,
        Ore,
        Amber
    }

    public enum LootFX
    {
        Generic,
        Amber,
        CyanOre,
        DarkBlueOre,
        DarkGreenOre,
        LightBlueOre,
        LightGreenOre,
        OrangeOre,
        PinkOre,
        RedOre
    }

    [CreateAssetMenu(fileName = "DAT_GameManagerAttributes", menuName = "Datas/GameManagerAttributes", order = 50)]
	public class GameManagerAttributes : ScriptableObject
    {
        #region Content
        [HorizontalLine(1, order = 0), Section("GAME MANAGE ATTRIBUTES", order = 1), Space(order = 2)]

        public float ResetGameTime = 2;
        public float TrapOxgyenDecrease = 5;

        [Space]

        [Min(0)] public float TravelDistance = 200;

        [HorizontalLine(1)]

        public AudioClip[] LootBoneClips = new AudioClip[] { };
        public AudioClip[] LootOreClips = new AudioClip[] { };
        public AudioClip LootAmberClip = null;

        [HorizontalLine(1)]

        public GameObject LootGenericFX = null;
        public GameObject LootAmberFX = null;
        public GameObject LootSparkleFX = null;

        [Space]

        public GameObject LootCyanOreFX = null;
        public GameObject LootDarkBlueOreFX = null;
        public GameObject LootDarkGreenOreFX = null;
        public GameObject LootLightBlueOreFX = null;
        public GameObject LootLightGreenOreFX = null;
        public GameObject LootOrangeOreFX = null;
        public GameObject LootPinkOreFX = null;
        public GameObject LootRedOreFX = null;

        [Space]

        [Range(0, 1)] public float AudioEffectsVolume = 1;

        [HorizontalLine(2, SuperColor.Raspberry)]

        [HideInInspector] public InputAction ActionAltInput = new InputAction();
        public InputAction ActionInput = new InputAction();
        public InputAction QuitInput = new InputAction();
        #endregion

        #region Methods
        public void EnableInputs()
        {
            ActionAltInput = new InputAction(binding: "/*/<button>");

            ActionAltInput.Enable();
            ActionInput.Enable();
            QuitInput.Enable();
        }

        public void DisableInputs()
        {
            ActionAltInput.Disable();
            ActionInput.Disable();
            QuitInput.Disable();
        }
        #endregion
    }
}
