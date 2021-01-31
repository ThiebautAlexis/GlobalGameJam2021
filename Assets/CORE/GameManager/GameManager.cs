// ===== Global Game Jam 2021 - https://github.com/ThiebautAlexis/GlobalGameJam2021 ===== //
//
// Notes:
//
// ====================================================================================== //

using EnhancedEditor;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GlobalGameJam2021
{
	public class GameManager : MonoBehaviour
    {
        #region Fields / Properties
        public static GameManager Instance = null;

        [HorizontalLine(1, order = 0), Section("GAME MANAGER", order = 1)]

        [SerializeField, Required] private GameManagerAttributes attributes = null;
        [SerializeField, Required] private Animator title = null;
        [SerializeField, Required] private TextMeshProUGUI scoreText = null;

        [Space]

        [SerializeField, Required] private AudioSource audioSource = null;

        [HorizontalLine(1)]

        [SerializeField, ReadOnly] private int score = 0;
        [SerializeField, ReadOnly] private int keyItemCount = 0;
        #endregion

        #region Animation
        private readonly int switchTitle_Hash = Animator.StringToHash("Switch");

        // -----------------------

        private void PlaySwitchTitle() => title.SetTrigger(switchTitle_Hash);
        #endregion

        #region Methods

        #region Level Management
        /// <summary>
        /// Reloads the whole game.
        /// </summary>
        public void ResetGame() => SceneManager.LoadScene(0, LoadSceneMode.Single);
        #endregion

        #region Score
        /// <summary>
        /// Increases player score.
        /// </summary>
        /// <param name="_increase"></param>
        public void IncreaseScore(int _increase)
        {
            score += _increase;
            scoreText.text = score.ToString("### ### ### ###");
        }

        public void PickupKeyItem()
        {
            keyItemCount--;
        }

        public void IncreaseKeyItemCount() => keyItemCount++;
        #endregion

        #region Feedbacks
        public void PlaySound(LootSound _sound, Vector3 _position)
        {
            AudioClip _clip;
            switch (_sound)
            {
                case LootSound.Bone:
                    _clip = attributes.LootBoneClips[Random.Range(0, attributes.LootBoneClips.Length)];
                    break;

                case LootSound.Ore:
                    _clip = attributes.LootOreClips[Random.Range(0, attributes.LootOreClips.Length)];
                    break;

                case LootSound.Amber:
                    _clip = attributes.LootAmberClip;
                    break;

                default:
                    _clip = attributes.LootBoneClips[Random.Range(0, attributes.LootBoneClips.Length)];
                    break;
            }

            AudioSource.PlayClipAtPoint(_clip, _position, attributes.AudioEffectsVolume);
        }

        public void InstantiateFX(LootFX _fx, Vector3 _position)
        {
            GameObject _instance;
            switch (_fx)
            {
                case LootFX.Generic:
                    _instance = attributes.LootGenericFX;
                    break;

                case LootFX.Amber:
                    _instance = attributes.LootAmberFX;
                    break;

                case LootFX.CyanOre:
                    _instance = attributes.LootCyanOreFX;
                    break;

                case LootFX.DarkBlueOre:
                    _instance = attributes.LootDarkBlueOreFX;
                    break;

                case LootFX.DarkGreenOre:
                    _instance = attributes.LootDarkGreenOreFX;
                    break;

                case LootFX.LightBlueOre:
                    _instance = attributes.LootLightBlueOreFX;
                    break;

                case LootFX.LightGreenOre:
                    _instance = attributes.LootLightGreenOreFX;
                    break;

                case LootFX.OrangeOre:
                    _instance = attributes.LootOrangeOreFX;
                    break;

                case LootFX.PinkOre:
                    _instance = attributes.LootPinkOreFX;
                    break;

                case LootFX.RedOre:
                    _instance = attributes.LootRedOreFX;
                    break;

                default:
                    _instance = attributes.LootGenericFX;
                    break;
            }

            Instantiate(_instance, _position, Quaternion.identity);
        }
        #endregion

        #region Monobehaviour
        private void Awake() => Instance = this;

        private IEnumerator Start()
        {
            // Start the game.
            Time.timeScale = 0;
            scoreText.text = string.Empty;
            while (!attributes.ActionInput.triggered && !attributes.ActionAltInput.triggered)
                yield return null;

            Time.timeScale = 1;
            PlaySwitchTitle();
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
            // Quit button.
            if (attributes.QuitInput.triggered)
                Application.Quit();
        }
        #endregion

        #endregion
    }
}
