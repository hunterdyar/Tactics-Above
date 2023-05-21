using Tactics.Entities;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Entity.Editor
{
	[CustomEditor(typeof(Faction))]
	public class FactionEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			var map = (Faction)target;

			if (EditorApplication.isPlaying)
			{
				EditorGUILayout.HelpBox($"There are {map.Count} agents.", MessageType.Info);
			}
			else if (EditorApplication.isPaused)
			{
				EditorGUILayout.HelpBox($"There are {map.Count} agents.", MessageType.Info);
			}
			if(map.TerritoryMap != null){
				EditorGUILayout.LabelField("Territory Map");
				var r = EditorGUILayout.GetControlRect(GUILayout.Width(256), GUILayout.Height(256));
				Gradient g = new Gradient();

				EditorGUI.DrawPreviewTexture(r,map.TerritoryMap.GetMapAsTexture(),null,ScaleMode.ScaleToFit);
			}

			if (map.AttackMap != null)
			{
				EditorGUILayout.LabelField("Threat Map");
				var r = EditorGUILayout.GetControlRect(GUILayout.Width(256), GUILayout.Height(256));
				EditorGUI.DrawPreviewTexture(r, map.TerritoryMap.GetMapAsTexture(), null, ScaleMode.ScaleToFit);
			}

			base.OnInspectorGUI();
		}
		
	}
}