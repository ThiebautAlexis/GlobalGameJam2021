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
		[HorizontalLine(1, order = 0), Section("DEBUG", order = 1)]

		[SerializeField] private PlanetLayout debuggedLayout = null; 
		[SerializeField] private SpriteRenderer debugRenderer; 
		#endregion

		#region Methods
		private void Start()
		{
			Transform _t = debuggedLayout.GenerateLayout(transform.position);
		}

		private void GenerateRandomLayout()
		{
			int _layoutIndex = Random.Range(0, layouts.Length);

			Transform _t = layouts[_layoutIndex].GenerateLayout(transform.position); 
		}

		private void OnDrawGizmos()
		{
			
			if (debuggedLayout == null) return;
			debuggedLayout.Draw(transform.position);
			debugRenderer.sprite = debuggedLayout.PlanetSprite; 
		}
		#endregion
	}
}
