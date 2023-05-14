using UnityEditor;
using UnityEngine;

namespace MapImporter.Editor
{
	public class SwatchPopupWindow : PopupWindowContent
	{
		private Color[] _colors;
		
		public SwatchPopupWindow(Color[] colors)
		{
			_colors = colors;
		}
		
		public override void OnGUI(Rect rect)
		{
			GUILayout.Label("Choose a Color", EditorStyles.boldLabel);
			for (int i = 0; i < _colors.Length; i++)
			{
				// GUILayout.
			}
		}
	}
}