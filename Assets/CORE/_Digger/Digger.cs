// ===== Global Game Jam 2021 - https://github.com/ThiebautAlexis/GlobalGameJam2021 ===== //
//
// Notes:
//
// ====================================================================================== //

using System.Collections.Generic;
using EnhancedEditor;
using UnityEngine;

namespace GlobalGameJam2021
{
    public enum DiggerState
    {
        Spawn = 0,
        Digging,
        AboutTurn,
        Traveling,
        Dead = 999
    }

	public class Digger : MonoBehaviour
    {
		#region Fields / Properties
		[HorizontalLine(1, order = 0), Section("DIGGER", order = 1)]

		[SerializeField, Required] private DiggerAttributes attributes = null;
		[SerializeField, Required] private new Rigidbody2D rigidbody = null;
		[SerializeField, Required] private new Collider2D collider = null;
        [SerializeField, Required] private Animator animator = null;

        [SerializeField, Required] private AudioSource audioSource = null;
        [SerializeField, Required] private ParticleSystem dirtFX = null;
        [SerializeField, Required] private SpriteMask lightMask = null;

        public Collider2D Collider => collider;

        [HorizontalLine(1)]

        [SerializeField, ReadOnly] private DiggerState state = DiggerState.Spawn;
        [SerializeField, ReadOnly] private float speed = 1;
        [SerializeField, ReadOnly] private float rotationSpeed = 1;
        [SerializeField, ReadOnly] private float rotationLerpSpeed = 1;

        [Space]

        [SerializeField, ReadOnly] private bool isLevelCompleted = false;

        [Space]

        [SerializeField, ReadOnly] private bool hasPickaxe = false;
        public bool HasPickaxe => hasPickaxe;

        [SerializeField] private Vector2 movement = new Vector2();

        // -----------------------

        private ContactFilter2D contactFiler = new ContactFilter2D();
        #endregion

        #region Animation
        public readonly int state_Hash = Animator.StringToHash("State");

        // -----------------------

        private void PlayDead() => animator.SetInteger(state_Hash, -1);

        private void PlayFloating() => animator.SetInteger(state_Hash, 0);
        public void PlayDigging() => animator.SetInteger(state_Hash, hasPickaxe ? 2 : 1);
        #endregion

        #region Methods

        #region Overlap Callbacks

        #region Planet
        private float rotationSpeedVar = 0;

        private Quaternion rotationLerpTo = new Quaternion();
        private float rotationLerpVar = 0;
        private bool isLerping = false;

        // -----------------------

        /// <summary>
        /// Diggers rotates when the planet itself rotate.
        /// </summary>
        public void Rotate(float _speed)
        {
            if (_speed == 0)
            {
                rotationSpeedVar = 0;
                rotationSpeed = 0;
                return;
            }

            rotationSpeedVar = Mathf.Min(rotationSpeedVar + Time.deltaTime, attributes.RotationSpeed[attributes.RotationSpeed.length - 1].time);
            rotationSpeed = attributes.RotationSpeed.Evaluate(rotationSpeedVar) * -_speed;

            transform.Rotate(Vector3.forward, rotationSpeed);

            Vector2 _previousMovement = movement;
            movement = transform.rotation * Vector3.up;

            // If lerping, rotate target lerp rotation.
            if (isLerping)
            {
                float _angle = Vector2.SignedAngle(movement, _previousMovement);
                rotationLerpTo = (Quaternion.AngleAxis(_angle, Vector3.forward) * rotationLerpTo).normalized;
            }
        }

        /// <summary>
        /// Diggers enter into the planet and starts digging.
        /// </summary>
        public void OnEnterPlanet(PlanetController _planet)
        {
            if (state == DiggerState.Spawn)
            {
                isLightLerping = true;
                lightLerpValue = attributes.LightOriginalSize;

                _planet.Activate();
            }
            else if (state == DiggerState.Traveling)
            {
                isLightLerping = true;
                lightLerpValue = attributes.LightOriginalSize;

                isLevelCompleted = false;
                _planet.Activate();

                CameraDigger.Instance.Shake(attributes.DiggingInShake);
                GameManager.Instance.DestroyPreviousPlanet();

                Vector3 _distance = _planet.transform.position;
                CameraDigger.Instance.StopTravel(_distance);

                transform.position -= _distance;
                _planet.transform.position = Vector3.zero;
            }
            else
                CameraDigger.Instance.Shake(attributes.DiggingInShake);

            speedVar = 0;

            isLerping = false;
            state = DiggerState.Digging;

            PlayDigging();

            dirtFX.Play();
            Instantiate(attributes.DigFX, transform.position, Quaternion.identity);

            audioSource.volume = attributes.AudioDigLoopVolume;
            audioSource.clip = attributes.DigLoopClips[Random.Range(0, attributes.DigLoopClips.Length)];
            audioSource.Play();
            audioSource.PlayOneShot(attributes.DigInClips[Random.Range(0, attributes.DigInClips.Length)], attributes.AudioDigVolume);
        }

        /// <summary>
        /// Diggers exits the planet and make an about turn to get in back.
        /// </summary>
        public void OnExitPlanet(PlanetController _planet)
        {
            if (isLevelCompleted)
            {
                speedVar = 0;
                rotationSpeedVar = 0;
                state = DiggerState.Traveling;

                PlayFloating();
                dirtFX.Stop();
                audioSource.Stop();

                GameManager.Instance.OnLeaveEarth(movement);
                _planet.DisablePlanet();
                return;
            }

            speedVar = 0;
            rotationSpeedVar = 0;
            state = DiggerState.AboutTurn;

            PlayFloating();
            CameraDigger.Instance.Shake(attributes.DiggingOutShake, true);

            dirtFX.Stop();
            Instantiate(attributes.DigFX, transform.position, Quaternion.identity);

            audioSource.Stop();
            audioSource.PlayOneShot(attributes.DigOutClips[Random.Range(0, attributes.DigOutClips.Length)], attributes.AudioDigVolume);

            // Start lerping rotation.
            isLerping = true;
            rotationLerpVar = 0;

            Vector2 _direction = -transform.position.normalized;
            float _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;

            rotationLerpTo = Quaternion.Euler(new Vector3(0, 0, _angle - 90));
        }
        #endregion

        #region Key Items
        private float lightLerpValue = 0;
        private bool isLightLerping = false;

        // -----------------------

        public void PickupKeyItem(KeyItemType _type)
        {
            switch (_type)
            {
                case KeyItemType.Unknown:
                    break;

                case KeyItemType.Pickaxe:
                    hasPickaxe = true;
                    break;

                case KeyItemType.Lantern:
                    isLightLerping = true;
                    lightLerpValue = attributes.LightExtendedSize;
                    break;

                default:
                    break;
            }
        }
        #endregion

        #region Others
        public void CompleteLevel() => isLevelCompleted = true;

        public void Kill()
        {
            speedVar = 0;
            isLerping = false;
            rotationSpeedVar = 0;
            state = DiggerState.Dead;

            collider.enabled = false;
            PlayDead();
        }

        public void Bounce(Collider2D _collider)
        {
            if (isLevelCompleted)
                return;

            ColliderDistance2D _distance = collider.Distance(_collider);
            float _angle = Random.Range(attributes.BounceRange.x, attributes.BounceRange.y);
            //movement = Quaternion.AngleAxis(_angle, Vector3.forward) * _distance.normal;
            movement *= -1;
            movement += new Vector2(Mathf.Cos(Mathf.Deg2Rad * _angle), Mathf.Sin(Mathf.Deg2Rad * _angle));
            movement = movement.normalized;
            _angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, _angle - 90));
        }
        #endregion

        #endregion

        #region Core Movements
        private float speedVar = 0;

        // -----------------------

        private void SpawnMove()
        {
            speedVar = Mathf.Min(speedVar + Time.deltaTime, attributes.SpawnSpeed[attributes.SpawnSpeed.length - 1].time);
            speed = attributes.SpawnSpeed.Evaluate(speedVar);

            Move();
        }

        private void DigMove()
        {
            speedVar = Mathf.Min(speedVar + Time.deltaTime, attributes.DigSpeed[attributes.DigSpeed.length - 1].time);
            speed = attributes.DigSpeed.Evaluate(speedVar);

            CameraDigger.Instance.Vibrate();
            Move();
        }

        private void AboutTurnMove()
        {
            speedVar = Mathf.Min(speedVar + Time.deltaTime, attributes.AboutTurnSpeed[attributes.AboutTurnSpeed.length - 1].time);
            speed = attributes.AboutTurnSpeed.Evaluate(speedVar);

            Move();
        }

        private void TravelMove()
        {
            speedVar = Mathf.Min(speedVar + Time.deltaTime, attributes.TravelingSpeed[attributes.TravelingSpeed.length - 1].time);
            speed = attributes.TravelingSpeed.Evaluate(speedVar);

            Move();
        }

        private void DieMove()
        {
            speedVar = Mathf.Min(speedVar + Time.deltaTime, attributes.DeadSpeed[attributes.DeadSpeed.length - 1].time);
            speed = attributes.DeadSpeed.Evaluate(speedVar);

            // Update position without triggering anything...
            rigidbody.position += movement * speed * Time.deltaTime;
            transform.position = rigidbody.position;
        }
        #endregion

        #region Collisions & Calculs
        private readonly RaycastHit2D[] castBuffer = new RaycastHit2D[8];
        private readonly Trigger[] castTriggers = new Trigger[8];

        private readonly List<Trigger> overlappingTriggers = new List<Trigger>();

        // -----------------------

        private int Move()
        {
            float _speed = speed * Time.deltaTime;
            int _amount = collider.Cast(movement, contactFiler, castBuffer, _speed, true);

            int _castTriggersAmount = 0;
            for (int _i = 0; _i < _amount; _i++)
            {
                Trigger _trigger;
                if (!castBuffer[_i].collider.TryGetComponent(out _trigger))
                    continue;

                // Store trigger.
                castTriggers[_castTriggersAmount] = _trigger;
                _castTriggersAmount++;

                // Starts overlapping a new trigger.
                if (DoEnterTrigger(_trigger))
                {
                    overlappingTriggers.Add(_trigger);
                    if (_trigger.OnEnter(this))
                    {
                        _speed = castBuffer[_i].distance;
                        break;
                    }
                }
            }

            // Update position.
            rigidbody.position += movement * _speed;
            transform.position = rigidbody.position;

            // Exit from no more overlapping triggers.
            for (int _i = 0; _i < overlappingTriggers.Count; _i++)
            {
                Trigger _trigger = overlappingTriggers[_i];
                if (HasExitedTrigger(_trigger))
                {
                    overlappingTriggers.Remove(_trigger);
                    _trigger.OnExit(this);
                    _i--;
                }
                else
                    _trigger.OnUpdate(this);
            }

            return _amount;

            // ----- Local Methods ----- //
            bool DoEnterTrigger(Trigger _trigger)
            {
                for (int _i = 0; _i < overlappingTriggers.Count; _i++)
                {
                    if (_trigger.Compare(overlappingTriggers[_i]))
                        return false;
                }
                return true;
            }

            bool HasExitedTrigger(Trigger _trigger)
            {
                for (int _i = 0; _i < _castTriggersAmount; _i++)
                {
                    if (_trigger.Compare(castTriggers[_i]))
                        return false;
                }
                return true;
            }
        }
        #endregion

        #region Monobehaviour
        private void Start()
        {
            // Initialize contact filter.
            contactFiler.layerMask = attributes.LayerMask;
            contactFiler.useLayerMask = true;
            contactFiler.useTriggers = true;
        }

        private void Update()
        {
            if (isLerping)
            {
                rotationLerpVar = Mathf.Min(rotationLerpVar + Time.deltaTime, attributes.RotationLerpSpeed[attributes.RotationLerpSpeed.length - 1].time);
                rotationLerpSpeed = attributes.RotationLerpSpeed.Evaluate(rotationLerpVar);

                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationLerpTo, rotationLerpSpeed * Time.deltaTime);
                if (transform.rotation == rotationLerpTo)
                    isLerping = false;

                movement = transform.rotation * Vector3.up;
            }

            if (isLightLerping)
            {
                float _value = Mathf.MoveTowards(lightMask.transform.localScale.x, lightLerpValue, Time.deltaTime * attributes.LightLerpSpeed);
                lightMask.transform.localScale = new Vector3(_value, _value, 1);
                if (_value == lightLerpValue)
                    isLightLerping = false;
            }

            switch (state)
            {
                case DiggerState.Spawn:
                    SpawnMove();
                    break;

                case DiggerState.Digging:
                    DigMove();
                    break;

                case DiggerState.AboutTurn:
                    AboutTurnMove();
                    break;

                case DiggerState.Traveling:
                    TravelMove();
                    break;

                case DiggerState.Dead:
                    DieMove();
                    break;

                default:
                    // This never happen.
                    break;
            }
        }
        #endregion

        #endregion
    }
}
