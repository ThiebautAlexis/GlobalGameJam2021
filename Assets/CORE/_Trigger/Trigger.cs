// ===== Global Game Jam 2021 - https://github.com/ThiebautAlexis/GlobalGameJam2021 ===== //
//
// Notes:
//
// ====================================================================================== //

using EnhancedEditor;
using UnityEngine;

namespace GlobalGameJam2021
{
	public abstract class Trigger : MonoBehaviour
    {
        #region Fields / Properties
        [HorizontalLine(1, order = 0), Section("TRIGGER", order = 1)]

        [SerializeField] protected new Collider2D collider = null;
        #endregion

        #region Methods
        public abstract bool OnEnter(Digger _digger);

        public virtual void OnUpdate(Digger _digger) { }

        public virtual bool OnExit(Digger _digger) => false;

        public bool Compare(Trigger _trigger) => GetInstanceID() == _trigger.GetInstanceID();

        protected virtual void Awake()
        {
            if (!collider)
                collider = GetComponent<Collider2D>();
        }
        #endregion
    }
}
