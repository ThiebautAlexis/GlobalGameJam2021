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
using UnityEngine.UI;

namespace GlobalGameJam2021
{
	public class GameManager : MonoBehaviour
    {
        #region Fields / Properties
        public static GameManager Instance = null;
        private static bool isMusicInitialized = false;

        [HorizontalLine(1, order = 0), Section("GAME MANAGER", order = 1)]

        [SerializeField, Required] private GameManagerAttributes attributes = null;
        [SerializeField, Required] private new CameraDigger camera = null;
        [SerializeField, Required] private PlanetGenerator generator = null;
        [SerializeField, Required] private Digger digger = null;

        [Space]

        [SerializeField, Required] private Animator titleAnimator = null;
        [SerializeField, Required] private Animator scoreAnimator = null;
        [SerializeField, Required] private Animator oxygenAnimator = null;

        [Space]

        [SerializeField, Required] private TextMeshProUGUI scoreText = null;
        [SerializeField, Required] private TextMeshProUGUI oxygenText = null;
        [SerializeField, Required] private TextMeshProUGUI endScreenScore = null;
        [SerializeField, Required] private Image oxygenGauge = null;
        [SerializeField, Required] private Image deathScreen = null;
        [SerializeField, Required] private CanvasGroup endScreen = null;
        [SerializeField, Required] private GameObject tuto = null;

        [Space]

        [SerializeField, Required] private AudioSource audioSource = null;

        [HorizontalLine(1)]

        [SerializeField, ProgressBar("oxygenTank", SuperColor.Sapphire)] private float oxygen = 0;
        [SerializeField, ReadOnly] private float oxygenTank = 0;
        [SerializeField, ReadOnly] private bool isDrainingOxygen = false;

        [Space]

        [SerializeField, ReadOnly] private int score = 0;
        [SerializeField, ReadOnly] private int keyItemCount = 0;
        #endregion

        #region Animation
        private readonly int switchTitle_Hash = Animator.StringToHash("Switch");
        private readonly int scoreIncrease_Hash = Animator.StringToHash("Increase");

        private readonly int oxygenFill_Hash = Animator.StringToHash("Fill");
        private readonly int oxygenDecrease_Hash = Animator.StringToHash("Decrease");
        private readonly int oxyenEmpty_Hash = Animator.StringToHash("IsEmpty");

        // -----------------------

        private void PlaySwitchTitle() => titleAnimator.SetTrigger(switchTitle_Hash);
        private void PlayScoreIncrease() => scoreAnimator.SetTrigger(scoreIncrease_Hash);

        private void PlayOxygenFill() => oxygenAnimator.SetTrigger(oxygenFill_Hash);
        private void PlayOxygenDecrease() => oxygenAnimator.SetTrigger(oxygenDecrease_Hash);
        private void PlayOxgenEmpty(bool _isEmpty) => oxygenAnimator.SetBool(oxyenEmpty_Hash, _isEmpty);
        #endregion

        #region Methods

        #region Level Management
        private Transform actualPlanet = null;
        private GameObject planetToDestroy = null;
        private bool isLevelCompleted = false;

        private bool isOxygenEmpty = false;
        private float oxygenValue = 1;

        // -----------------------

        public void CompleteLevel()
        {
            isLevelCompleted = true;
            digger.CompleteLevel();
            tuto.SetActive(false);
        }

        public void OnLeaveEarth(Vector2 _direction)
        {
            planetToDestroy = actualPlanet.gameObject;

            actualPlanet = generator.GenerateRandomLayout();
            actualPlanet.position = digger.transform.position + (Vector3)(_direction * attributes.TravelDistance);

            camera.StartTravel(actualPlanet.position);
        }

        public void DestroyPreviousPlanet()
        {
            isLevelCompleted = false;
            Destroy(planetToDestroy);
        }

        // -----------------------

        public void FillOxygenTank(float _value)
        {
            isDrainingOxygen = true;
            oxygenTank = _value;
            oxygenValue = _value;
            oxygenText.text = "100%";

            if (isOxygenEmpty)
            {
                isOxygenEmpty = false;
                PlayOxgenEmpty(false);
                PlayOxygenFill();
            }

            // Update UI.
            oxygenGauge.fillAmount = 1;
        }

        public void EmptyOxygenTank()
        {
            PlayOxygenDecrease();
            EmptyOxygenTank(attributes.TrapOxgyenDecrease);
        }

        public void EmptyOxygenTank(float _value)
        {
            if (isLevelCompleted)
                return;

            oxygenValue -= _value;
            if (!isOxygenEmpty && ((oxygenValue / oxygenTank) < attributes.OxygenEmptyTreshold))
            {
                isOxygenEmpty = true;
                PlayOxgenEmpty(true);
            }

            if (oxygenValue < 0)
            {
                isDrainingOxygen = false;
                oxygenValue = 0;

                // Restart game.
                digger.Kill();
                StartCoroutine(DoResetGame());
            }
        }

        private IEnumerator DoResetGame()
        {
            float _var = 0;
            while (true)
            {
                _var += Time.deltaTime;
                deathScreen.color = new Color(0, 0, 0, _var / attributes.ResetGameTime);
                if (_var > attributes.ResetGameTime)
                    break;

                yield return null;
            }

            // Show end screen title.
            _var = 0;
            endScreen.gameObject.SetActive(true);
            endScreenScore.text = scoreText.text;
            while (true)
            {
                _var += Time.deltaTime;
                endScreen.alpha = _var / attributes.ResetGameTime;
                if (_var > attributes.ResetGameTime)
                    break;

                yield return null;
            }

            while (!attributes.ActionInput.triggered && !attributes.ActionAltInput.triggered)
                yield return null;

            yield return new WaitForSeconds(.25f);
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
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

            PlayScoreIncrease();
        }

        public void PickupKeyItem(Vector3 _position)
        {
            Instantiate(attributes.LootSparkleFX, _position, Quaternion.identity);

            keyItemCount--;
            if (keyItemCount < 1)
                CompleteLevel();
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

                case LootSound.Rock:
                    _clip = attributes.destroyRockAudioClips[Random.Range(0, attributes.destroyRockAudioClips.Length)];
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

                case LootFX.Rock:
                    _instance = attributes.destroyRockFX;
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
            actualPlanet = generator.GenerateRandomLayout();
            if (!isMusicInitialized)
            {
                isMusicInitialized = true;
                DontDestroyOnLoad(audioSource.gameObject);
            }
            else
                Destroy(audioSource.gameObject);

            scoreText.text = string.Empty;
            
            Time.timeScale = 0;
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
            // Oxygen UI update.
            if (isDrainingOxygen)
                EmptyOxygenTank(Time.deltaTime);

            oxygen = Mathf.MoveTowards(oxygen, oxygenValue, Time.deltaTime * attributes.OxygenGaugeSpeed);
            oxygenGauge.fillAmount = oxygen / oxygenTank;
            oxygenText.text = (int)((oxygen / oxygenTank) * 100) + "%";

            // Quit button.
            if (attributes.QuitInput.triggered)
                Application.Quit();
        }
        #endregion

        #endregion
    }
}
