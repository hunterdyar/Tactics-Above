using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace MapImporter.Editor
{
	public class SwatchPopupWindow : PopupWindowContent
	{
		private Color[] _colors;
		private int _selected;
		public delegate void OnColorSelected(Color color);
		private OnColorSelected _selectedCallback;
		public SwatchPopupWindow(int selected, Color[] colors, OnColorSelected selectedCallback)
		{
			_selected = selected;
			_colors = colors;
			_selectedCallback = selectedCallback;
		}

		public override void OnGUI(Rect rect)
		{
			GUILayout.Label("Choose a Color", EditorStyles.boldLabel);

			Texture[] textures = new Texture[_colors.Length]; 
			for (int i = 0; i < _colors.Length; i++)
			{
				//create a texture. todo: abstract
				var tex = new Texture2D(32, 32);
				for (int y = 0; y < tex.height; y++)
				{
					for (int x = 0; x < tex.width; x++)
					{
						tex.SetPixel(x, y, _colors[i]);
					}
				}
				tex.Apply(); //i always forget to apply.
				textures[i] = tex;
			}
			int selected = GUILayout.SelectionGrid(_selected, textures, 4, GUIStyle.none);
			if (_selected != selected)
			{
				_selected = selected;
				_selectedCallback?.Invoke(_colors[_selected]);
				editorWindow.Close();
			}
		}
	
	}
}