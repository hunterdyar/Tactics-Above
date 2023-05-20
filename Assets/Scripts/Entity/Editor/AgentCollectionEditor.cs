using Tactics.Entities;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Entity.Editor
{
	[CustomEditor(typeof(AgentCollection))]
	public class AgentCollectionEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			var map = (AgentCollection)target;

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
				EditorGUI.DrawPreviewTexture(r,map.TerritoryMap.GetMapAsTexture(),null,ScaleMode.ScaleToFit);
			}

			base.OnInspectorGUI();
		}
		
	}
}