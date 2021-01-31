// ===== Global Game Jam 2021 - https://github.com/ThiebautAlexis/GlobalGameJam2021 ===== //
//
// Notes:
//
// ====================================================================================== //

using EnhancedEditor;
using UnityEngine;
using System.Collections.Generic;

namespace GlobalGameJam2021
{
	public class PlanetGenerator : MonoBehaviour
    {
		#region Fields / Properties
		[HorizontalLine(1, order = 0), Section("PlanetGenerator", order = 1)]
		[SerializeField] private PlanetLayout[] layouts = new PlanetLayout[] { };
		#endregion

		#region Methods
		private void Start()
		{
			GenerateRandomLayout();
		}

		private void GenerateRandomLayout()
		{
			int _layoutIndex = Random.Range(0, layouts.Length);

			Transform _t = layouts[_layoutIndex].GenerateLayout(transform.position); 
		}
		#endregion
	}
}
