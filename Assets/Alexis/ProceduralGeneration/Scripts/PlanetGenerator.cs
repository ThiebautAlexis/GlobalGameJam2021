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
		[SerializeField] private List<Vector2> positions = new List<Vector2>();
		[SerializeField] private SpriteRenderer debugRenderer; 
		#endregion

		#region Methods
		private void Start()
		{
			positions = debuggedLayout.GenerateLayout(transform.position);
		}

		private void GenerateRandomLayout()
		{
			int _layoutIndex = Random.Range(0, layouts.Length);

			positions = layouts[_layoutIndex].GenerateLayout(transform.position); 
		}

		private void OnDrawGizmos()
		{
			for (int i = 0; i < positions.Count; i++)
			{
				Gizmos.DrawSphere(positions[i], .1f);
			}
			if (debuggedLayout == null) return;
			debuggedLayout.Draw(transform.position);
			debugRenderer.sprite = debuggedLayout.PlanetSprite; 
		}
		#endregion
	}
}
