using HDyar.MapImporter;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using PopupWindow = UnityEditor.PopupWindow;

namespace MapImporter.Editor
{
	[CustomPropertyDrawer(typeof(ColorToPrefab))]
	public class ColorToPrefabDrawer : PropertyDrawer
	{
		//AssetImportEditor doesn't support UIToolkit, only IMGUI right now. (You cant have UIToolkit inside of IMGUI). Took me way too long to figure that out.
		
		// public override VisualElement CreatePropertyGUI(SerializedProperty property)
		// {
		// 	var container = new VisualElement();
		// 	var prefabField = new PropertyField(property.FindPropertyRelative("Prefab"));
		// 	container.Add(prefabField);
		// 	return container;
		// }
		
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			// Using BeginProperty / EndProperty on the parent property means that
			// prefab override logic works on the entire property.
			EditorGUI.BeginProperty(position, label, property);
			
			// Draw label
			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
		
			// Don't make child fields be indented
			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
		
			// Calculate rects
			var prefabRect = new Rect(position.x, position.y, 120, position.height);
			var unitRect = new Rect(position.x + 125, position.y, 100, position.height);
		
			// Draw fields - pass GUIContent.none to each so they are drawn without labels
			EditorGUI.PropertyField(prefabRect, property.FindPropertyRelative("Prefab"), GUIContent.none);
			// EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("color"), GUIContent.none);
		
			//create list of icons
			var options = property.FindPropertyRelative("ColorOptions");
			var displayedOptions = new GUIContent[options.arraySize];
			var colorOptions = new Color[options.arraySize];//also make a list of all colors while we are at it.
			for (var i = 0; i < displayedOptions.Length; i++)
			{
				//create a texture.
				var color = options.GetArrayElementAtIndex(i).colorValue;
				var tex = new Texture2D(32,32);
				for (int y = 0; y < tex.height; y++)
				{
					for (int x = 0; x < tex.width; x++)
					{
						tex.SetPixel(x, y, color);
					}
				}
				tex.Apply();//i always forget to apply.
				
				//create the option.
				displayedOptions[i] = new GUIContent()
				{
					text = "Color " + i,
					image = tex,
					tooltip = color.ToString()
				};
				
				//also create our colorOptions array
				colorOptions[i] = color;
			}
		
			GUIStyle style = new GUIStyle()
			{
				imagePosition = ImagePosition.ImageOnly
			};
			var currentselected = 0;
			//Set color by current selected.
			var currentColor = property.FindPropertyRelative("color");
			for (int i = 0; i < options.arraySize; i++)
			{
				if (options.GetArrayElementAtIndex(i).colorValue == currentColor.colorValue)
				{
					currentselected = i;
					break;
				}
			}
			//So Popup will only display text, not image icons for it's elements..
			
			int selected = EditorGUI.Popup(unitRect, currentselected, displayedOptions, style);
			//turn the options into selected color
			
			if (selected != currentselected)
			{
				Debug.Log($"change color from {currentselected}-{currentColor.colorValue} to {selected}-{options.GetArrayElementAtIndex(selected).colorValue}");
				currentColor.colorValue = options.GetArrayElementAtIndex(selected).colorValue;
			}

			var rect = new Rect(position.x+350, position.y, 50, position.height);
			//Popup Try Two
			if (GUILayout.Button("Color Options", GUILayout.Width(200)))
			{
				PopupWindow.Show(rect,new SwatchPopupWindow(colorOptions));
			}

			// Set indent back to what it was
			EditorGUI.indentLevel = indent;
		
			EditorGUI.EndProperty();
		}
	}
}