// ===== Global Game Jam 2021 - https://github.com/ThiebautAlexis/GlobalGameJam2021 ===== //
//
// Notes:
//
// ====================================================================================== //

using UnityEngine;
using UnityEditor; 


namespace GlobalGameJam2021
{
	[CustomEditor(typeof(LayoutZone))]
	public class LayoutZoneEditor : Editor
    {
		#region Fields / Properties
		private SerializedProperty offsetPosition; 
		private SerializedProperty radius; 
		private SerializedProperty spacing; 
		
		#endregion

		#region Methods
		private void OnEnable()
		{
			offsetPosition = serializedObject.FindProperty("offsetPosition");
			radius = serializedObject.FindProperty("radius");
			spacing = serializedObject.FindProperty("spacing");
			SceneView.duringSceneGui += OnSceneGUI;
		}

		private void OnDisable()
		{
			SceneView.duringSceneGui -= OnSceneGUI;
		}

		private void OnSceneGUI(SceneView _view)
		{
			Handles.color = Color.green; 
			Handles.DrawWireDisc(offsetPosition.vector2Value, Vector3.forward, radius.floatValue);
			Handles.color = Color.red; 
			Handles.DrawWireDisc(offsetPosition.vector2Value, Vector3.forward, spacing.floatValue);

		}
		#endregion
	}
}
