// ===== Global Game Jam 2021 - https://github.com/ThiebautAlexis/GlobalGameJam2021 ===== //
//
// Notes:
//
// ====================================================================================== //

using EnhancedEditor;
using UnityEngine;

namespace GlobalGameJam2021
{
    [CreateAssetMenu(fileName = "DAT_DiggerAttributes", menuName = "Datas/DiggerAttributes", order = 50)]
	public class DiggerAttributes : ScriptableObject
    {
        #region Content
        [HorizontalLine(1, order = 0), Section("DIGGER ATTRIBUTES", order = 1), Space(order = 2)]

        public AnimationCurve SpawnSpeed = new AnimationCurve();
        public AnimationCurve DigSpeed = new AnimationCurve();
        public AnimationCurve AboutTurnSpeed = new AnimationCurve();
        public AnimationCurve TravelingSpeed = new AnimationCurve();
        public AnimationCurve DeadSpeed = new AnimationCurve();

        [Space]

        public AnimationCurve RotationSpeed = new AnimationCurve();
        public AnimationCurve RotationLerpSpeed = new AnimationCurve();

        [HorizontalLine(1)]

        [MinMax(-45, 45)] public Vector2 BounceRange = new Vector2(-10, 10);

        [Space]

        public LayerMask LayerMask = new LayerMask();

        [HorizontalLine(1)]

        [Range(0, 1)] public float PlanetShockShake = .7f;

        [Space]

        [Range(0, 1)] public float DiggingInShake = .25f;
        [Range(0, 1)] public float DiggingOutShake = .25f;

        [HorizontalLine(1)]

        public float LightOriginalSize = 5;
        public float LightExtendedSize = 10;
        public float LightLerpSpeed = 10;

        [HorizontalLine(2, SuperColor.Raspberry)]

        public GameObject DigFX = null;

        [Space]

        public AudioClip[] DigInClips = new AudioClip[] { };
        public AudioClip[] DigLoopClips = new AudioClip[] { };
        public AudioClip[] DigOutClips = new AudioClip[] { };

        [Space]

        [Range(0, 1)] public float AudioDigVolume = 1;
        [Range(0, 1)] public float AudioDigLoopVolume = .7f;
        #endregion
    }
}
