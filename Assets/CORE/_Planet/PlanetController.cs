// ===== Global Game Jam 2021 - https://github.com/ThiebautAlexis/GlobalGameJam2021 ===== //
//
// Notes:
//
// ====================================================================================== //

using EnhancedEditor;
using UnityEngine;

namespace GlobalGameJam2021
{
	public class PlanetController : Trigger
    {
        #region Fields / Properties
        [HorizontalLine(1, order = 0), Section("PLANET CONTROLLER", order = 1)]

        [SerializeField, Required] private PlanetControllerAttributes attributes = null;

        public Vector3 Center => collider.bounds.center;

        [HorizontalLine(1)]

        [SerializeField, ReadOnly] private Digger digger = null;
        [SerializeField, ReadOnly] private float speed = 1;

        // -----------------------

        private bool hasDigger = false;
        #endregion

        #region Methods

        #region Trigger
        public override bool OnEnter(Digger _digger)
        {
            hasDigger = true;
            digger = _digger;

            _digger.OnEnterPlanet(this);
            return true;
        }

        public override bool OnExit(Digger _digger)
        {
            hasDigger = false;

            _digger.OnExitPlanet(this);
            return true;
        }
        #endregion

        #region Monobehaviour
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
            float _movement = attributes.Move.ReadValue<float>();
            if (_movement != 0)
            {
                _movement *= Time.deltaTime * speed;
                transform.RotateAround(collider.bounds.center, Vector3.forward, _movement * Time.deltaTime * speed);

                if (hasDigger)
                    digger.Rotate(_movement);
            }
        }
        #endregion

        #endregion
    }
}
