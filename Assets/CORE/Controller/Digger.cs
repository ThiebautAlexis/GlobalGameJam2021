// ===== Global Game Jam 2021 - https://github.com/ThiebautAlexis/GlobalGameJam2021 ===== //
//
// Notes:
//
// ====================================================================================== //

using EnhancedEditor;
using UnityEngine;

namespace GlobalGameJam2021
{
    public enum DiggerState
    {
        Spawn = 0,
        GettingIn,
        Digging,
        GettingOut,
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

        [HorizontalLine(1)]

        [SerializeField, ReadOnly] private DiggerState state = DiggerState.Spawn;
        [SerializeField, ReadOnly] private float speed = 1;

        [SerializeField] private Vector2 movement = new Vector2();

        // -----------------------

        private ContactFilter2D contactFiler = new ContactFilter2D();
        #endregion

        #region Methods

        #region Core Movements
        private float speedVar = 0;

        // -----------------------

        private void SpawnMove()
        {
            speedVar = Mathf.Min(speedVar + Time.deltaTime, attributes.SpawnSpeed[attributes.SpawnSpeed.length - 1].time);
            speed = attributes.SpawnSpeed.Evaluate(speedVar);

            int _amount = Move();
            for (int _i = 0; _i < _amount; _i++)
            {
                if (castBuffer[_i].collider.CompareTag("Border"))
                {
                    speedVar = 0;
                    state = DiggerState.GettingIn;
                    rigidbody.position += movement * castBuffer[_i].distance;
                    transform.position = rigidbody.position;
                    return;
                }
            }

            rigidbody.position += movement * speed * Time.deltaTime;
            transform.position = rigidbody.position;
        }

        private void GetInMove()
        {
            speedVar = Mathf.Min(speedVar + Time.deltaTime, attributes.DigSpeed[attributes.DigSpeed.length - 1].time);
            speed = attributes.DigSpeed.Evaluate(speedVar);

            int _amount = Move();
            for (int _i = 0; _i < _amount; _i++)
            {
                if (castBuffer[_i].collider.CompareTag("Border"))
                {
                    rigidbody.position += movement * speed * Time.deltaTime;
                    transform.position = rigidbody.position;
                    return;
                }
            }

            state = DiggerState.Digging;
            rigidbody.position += movement * speed * Time.deltaTime;
            transform.position = rigidbody.position;
        }

        private void DigMove()
        {
            speedVar = Mathf.Min(speedVar + Time.deltaTime, attributes.DigSpeed[attributes.DigSpeed.length - 1].time);
            speed = attributes.DigSpeed.Evaluate(speedVar);

            int _amount = Move();
            for (int _i = 0; _i < _amount; _i++)
            {
                if (castBuffer[_i].collider.CompareTag("Border"))
                {
                    state = DiggerState.GettingOut;
                    rigidbody.position += movement * castBuffer[_i].distance;
                    transform.position = rigidbody.position;
                    return;
                }
            }

            rigidbody.position += movement * speed * Time.deltaTime;
            transform.position = rigidbody.position;
        }

        private void GetOutMove()
        {
            speedVar = Mathf.Min(speedVar + Time.deltaTime, attributes.DigSpeed[attributes.DigSpeed.length - 1].time);
            speed = attributes.DigSpeed.Evaluate(speedVar);

            int _amount = Move();
            for (int _i = 0; _i < _amount; _i++)
            {
                if (castBuffer[_i].collider.CompareTag("Border"))
                {
                    rigidbody.position += movement * speed * Time.deltaTime;
                    transform.position = rigidbody.position;
                    return;
                }
            }

            speedVar = 0;
            state = DiggerState.AboutTurn;
            rigidbody.position += movement * speed * Time.deltaTime;
            transform.position = rigidbody.position;
        }

        private void AboutTurnMove()
        {
            speedVar = Mathf.Min(speedVar + Time.deltaTime, attributes.AboutTurnSpeed[attributes.AboutTurnSpeed.length - 1].time);
            speed = attributes.AboutTurnSpeed.Evaluate(speedVar);

            int _amount = Move();
            for (int _i = 0; _i < _amount; _i++)
            {
                if (castBuffer[_i].collider.CompareTag("Border"))
                {
                    speedVar = 0;
                    movement *= -1;
                    state = DiggerState.GettingIn;
                    rigidbody.position += movement * castBuffer[_i].distance;
                    transform.position = rigidbody.position;
                    return;
                }
            }

            rigidbody.position += movement * speed * Time.deltaTime;
            transform.position = rigidbody.position;
        }

        private void DieMove()
        {

        }
        #endregion

        #region Collisions & Calculs
        private readonly RaycastHit2D[] castBuffer = new RaycastHit2D[8];

        // -----------------------

        private int Move()
        {
            float _speed = speed * Time.deltaTime;
            return collider.Cast(movement, contactFiler, castBuffer, _speed, true);
        }
        #endregion

        #region Monobehaviour
        private void Start()
        {
            // Initialize contact filter.
            contactFiler.layerMask = attributes.layerMask;
            contactFiler.useLayerMask = true;
            contactFiler.useTriggers = true;
        }

        private void Update()
        {
            switch (state)
            {
                case DiggerState.Spawn:
                    SpawnMove();
                    break;

                case DiggerState.GettingIn:
                    GetInMove();
                    break;

                case DiggerState.Digging:
                    DigMove();
                    break;

                case DiggerState.GettingOut:
                    GetOutMove();
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
