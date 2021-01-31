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

        [Space]

        public AnimationCurve RotationSpeed = new AnimationCurve();
        public AnimationCurve RotationLerpSpeed = new AnimationCurve();

        [HorizontalLine(1)]

        public LayerMask LayerMask = new LayerMask();

        [HorizontalLine(1)]

        [Range(0, 1)] public float DiggingInShake = .25f;
        [Range(0, 1)] public float DiggingOutShake = .25f;

        [HorizontalLine(1)]

        public float LightOriginalSize = 5;
        public float LightExtendedSize = 10;

        [HorizontalLine(2, SuperColor.Raspberry)]

        public AudioClip[] DigInClips = new AudioClip[] { };
        public AudioClip[] DigLoopClips = new AudioClip[] { };
        public AudioClip[] DigOutClips = new AudioClip[] { };

        [Space]

        [Range(0, 1)] public float AudioDigVolume = 1;
        [Range(0, 1)] public float AudioDigLoopVolume = .7f;
        #endregion
    }
}
