// ===== Global Game Jam 2021 - https://github.com/ThiebautAlexis/GlobalGameJam2021 ===== //
//
// Notes:
//
// ====================================================================================== //

using UnityEngine;
using UnityEditor;

namespace GlobalGameJam2021
{
	[CustomEditor(typeof(PlanetLayout))]
	public class PlanetLayoutEditor : Editor
    {
		#region Fields / Properties
		private SerializedProperty surfacesAnchor = null;
		#endregion

		#region Methods
		private void OnEnable()
		{
			surfacesAnchor = serializedObject.FindProperty("surfacesArches");
			SceneView.duringSceneGui += OnSceneGUI; 
		}

		private void OnDisable()
		{
			SceneView.duringSceneGui -= OnSceneGUI;
		}

		private void OnSceneGUI(SceneView _view)
		{	
			Vector2 _start, _end; 
			for (int i = 0; i < surfacesAnchor.arraySize; i++)
			{
				Handles.color = Color.green;
				surfacesAnchor.GetArrayElementAtIndex(i).FindPropertyRelative("start").vector2Value = Handles.FreeMoveHandle(surfacesAnchor.GetArrayElementAtIndex(i).FindPropertyRelative("start").vector2Value, Quaternion.identity, .25f, Vector2.zero, Handles.CylinderHandleCap);
				Handles.color = Color.red;
				surfacesAnchor.GetArrayElementAtIndex(i).FindPropertyRelative("end").vector2Value = Handles.FreeMoveHandle(surfacesAnchor.GetArrayElementAtIndex(i).FindPropertyRelative("end").vector2Value, Quaternion.identity, .25f, Vector2.zero, Handles.CylinderHandleCap);
				
				_start = surfacesAnchor.GetArrayElementAtIndex(i).FindPropertyRelative("start").vector2Value;
				_end = surfacesAnchor.GetArrayElementAtIndex(i).FindPropertyRelative("end").vector2Value;
				Handles.color = Color.blue;
				surfacesAnchor.GetArrayElementAtIndex(i).FindPropertyRelative("startTangent").vector2Value = (Vector2)Handles.FreeMoveHandle(_start + surfacesAnchor.GetArrayElementAtIndex(i).FindPropertyRelative("startTangent").vector2Value, Quaternion.identity, .25f, Vector2.zero, Handles.CylinderHandleCap) - _start ;
				surfacesAnchor.GetArrayElementAtIndex(i).FindPropertyRelative("endTangent").vector2Value = (Vector2)Handles.FreeMoveHandle(_end + surfacesAnchor.GetArrayElementAtIndex(i).FindPropertyRelative("endTangent").vector2Value, Quaternion.identity, .25f, Vector2.zero, Handles.CylinderHandleCap) - _end;

				Handles.DrawBezier(_start,
								   _end,
								   _start + surfacesAnchor.GetArrayElementAtIndex(i).FindPropertyRelative("startTangent").vector2Value,
								   _end + surfacesAnchor.GetArrayElementAtIndex(i).FindPropertyRelative("endTangent").vector2Value,
								   Color.magenta, null, 2.5f);

				Handles.color = Color.white; 
				Handles.DrawLine(_start, _start + surfacesAnchor.GetArrayElementAtIndex(i).FindPropertyRelative("startTangent").vector2Value);
				Handles.DrawLine(_end, _end + surfacesAnchor.GetArrayElementAtIndex(i).FindPropertyRelative("endTangent").vector2Value);
			}
			serializedObject.ApplyModifiedProperties();
		}
		#endregion
	}
}
