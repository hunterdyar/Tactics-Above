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
			var faction = (Faction)target;

			if (EditorApplication.isPlaying)
			{
				EditorGUILayout.HelpBox($"There are {faction.Count} agents.", MessageType.Info);
			}
			else if (EditorApplication.isPaused)
			{
				EditorGUILayout.HelpBox($"There are {faction.Count} agents.", MessageType.Info);
			}
			if(faction.AIContext != null){
				EditorGUILayout.LabelField("Territory Map");
				var r = EditorGUILayout.GetControlRect(GUILayout.Width(256), GUILayout.Height(256));
				Gradient g = new Gradient();

				EditorGUI.DrawPreviewTexture(r,faction.AIContext.TerritoryMap.GetMapAsTexture(faction.TerritoryGradient),null,ScaleMode.ScaleToFit);
			}

			if (faction.AIContext != null)
			{
				EditorGUILayout.LabelField("Attack Map");
				var r = EditorGUILayout.GetControlRect(GUILayout.Width(256), GUILayout.Height(256));
				Gradient g = new Gradient();

				EditorGUI.DrawPreviewTexture(r, faction.AIContext.AttackMap.GetMapAsTexture(), null, ScaleMode.ScaleToFit);
			}

			if (faction.AIContext != null)
			{
				EditorGUILayout.LabelField("Battlefront Map");
				var r = EditorGUILayout.GetControlRect(GUILayout.Width(256), GUILayout.Height(256));
				Gradient g = new Gradient();

				EditorGUI.DrawPreviewTexture(r, faction.AIContext.BattleMap.GetMapAsTexture(), null, ScaleMode.ScaleToFit);
			}

			base.OnInspectorGUI();
		}
		
	}
}