// ===== Global Game Jam 2021 - https://github.com/ThiebautAlexis/GlobalGameJam2021 ===== //
//
// Notes:
//
// ====================================================================================== //

using EnhancedEditor;
using UnityEngine;

namespace GlobalGameJam2021
{
    [CreateAssetMenu(fileName = "DAT_LayoutProps", menuName = "Datas/Layout/LayoutProps", order = 50)]
    public class LayoutProps : ScriptableObject
    {
        #region Content
        [HorizontalLine(1, order = 0), Section("Player Layout Props", order = 1), Space(order = 2)]
        [SerializeField] private Trigger[] tools = new Trigger[] { };
        [SerializeField] private Trigger[] props = new Trigger[] { };
        [SerializeField] private Trigger[] lavaLakes = new Trigger[] { };

        public Trigger[] Tools => tools;
        public Trigger[] Props => props;
        public Trigger[] LavaLakes => lavaLakes; 
        #endregion
    }
}
