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
        Died = 999
    }

	public class Digger : MonoBehaviour
    {
		#region Fields / Properties
		[HorizontalLine(1, order = 0), Section("DIGGER", order = 1)]

		[SerializeField, Required] private DiggerAttributes attributes = null;
		[SerializeField, Required] private new Rigidbody2D rigidbody = null;
		[SerializeField, Required] private new Collider2D collider = null;

		[SerializeField, Required] private SpriteMask lightMask = null;

        [HorizontalLine(1)]

        [SerializeField, ReadOnly] private DiggerState state = DiggerState.Spawn;
        [SerializeField, ReadOnly] private float speed = 1;
        [SerializeField, ReadOnly] private float rotationSpeed = 1;

        [Space]

        [SerializeField, ReadOnly] private bool hasPickaxe = false;

        [SerializeField] private Vector2 movement = new Vector2();

        // -----------------------

        private ContactFilter2D contactFiler = new ContactFilter2D();
        #endregion

        #region Methods

        #region Overlap Callbacks

        #region Planet
        private float rotationSpeedVar = 0;

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
            rotationSpeed = attributes.RotationSpeed.Evaluate(rotationSpeedVar) * _speed;

            transform.Rotate(Vector3.forward, rotationSpeed);
            movement = transform.rotation * Vector3.up;
        }

        /// <summary>
        /// Diggers enter into the planet and starts digging.
        /// </summary>
        public void OnEnterPlanet(PlanetController _planet)
        {
            speedVar = 0;
            if (state != DiggerState.Digging)
            {
                transform.Rotate(Vector3.forward, 180);
                movement = transform.rotation * Vector3.up;
            }

            state = DiggerState.Digging;
        }

        /// <summary>
        /// Diggers exits the planet and make an about turn to get in back.
        /// </summary>
        public void OnExitPlanet(PlanetController _planet)
        {
            speedVar = 0;
            state = DiggerState.AboutTurn;

            movement = (transform.position - _planet.Center).normalized;

            float _angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, _angle - 90));
        }
        #endregion

        #region Key Items
        public void PickupLantern()
        {
            // Lerp.
            lightMask.transform.localScale = new Vector3(attributes.LightExtendedSize, attributes.LightExtendedSize, 1);
        }

        public void PickupPickaxe() => hasPickaxe = true;
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

            Move();
        }

        private void AboutTurnMove()
        {
            speedVar = Mathf.Min(speedVar + Time.deltaTime, attributes.AboutTurnSpeed[attributes.AboutTurnSpeed.length - 1].time);
            speed = attributes.AboutTurnSpeed.Evaluate(speedVar);

            Move();
        }

        private void DieMove()
        {

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

            // Initialize objects.
            lightMask.transform.localScale = new Vector3(attributes.LightOriginalSize, attributes.LightOriginalSize, 1);
        }

        private void Update()
        {
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

                case DiggerState.Died:
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
