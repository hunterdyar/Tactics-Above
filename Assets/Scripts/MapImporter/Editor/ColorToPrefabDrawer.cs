using System;
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

			float colorWidth = 40;
			float goWidth = 120;
			float margin = 5;
			// property rects
			var unitRect = new Rect(position.x, position.y, colorWidth, position.height);
			var prefabRect1 = new Rect(position.x+colorWidth+ margin, position.y, goWidth, position.height);
			var prefabRect2 = new Rect(position.x+colorWidth+goWidth+margin*2, position.y, goWidth, position.height);

		
			// Draw fields - pass GUIContent.none to each so they are drawn without labels
			EditorGUI.PropertyField(prefabRect1, property.FindPropertyRelative("PrefabUpper"), GUIContent.none);
			EditorGUI.PropertyField(prefabRect2, property.FindPropertyRelative("PrefabLower"), GUIContent.none);

			// EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("color"), GUIContent.none);

			var path = AssetDatabase.GetAssetPath(property.serializedObject.targetObject);
			AssetDatabase.GUIDFromAssetPath(path);
			var imageToPrefabMap = AssetDatabase.LoadAssetAtPath(path, typeof(ImageToPrefabMap)) as ImageToPrefabMap;
			Color[] options = Array.Empty<Color>();
			if (imageToPrefabMap != null)
			{
				options = imageToPrefabMap.AllColorsInTexture;
			}
			//create list of icons
			var displayedOptions = new GUIContent[options.Length];
			var colorOptions = new Color[options.Length];//also make a list of all colors while we are at it.
			for (var i = 0; i < options.Length; i++)
			{
				//create a texture.
				var color = options[i];
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
			var currentSelected = 0;
			//Set color by current selected.
			var currentColor = property.FindPropertyRelative("color");
			for (int i = 0; i < options.Length; i++)
			{
				if (options[i] == currentColor.colorValue)
				{
					currentSelected = i;
					break;
				}
			}
			//So Popup will only display text, not image icons for it's elements..
			
			// int selected = EditorGUI.Popup(unitRect, currentselected, displayedOptions, style);
			// //turn the options into selected color
			//
			// if (selected != currentselected)
			// {
			// 	Debug.Log($"change color from {currentselected}-{currentColor.colorValue} to {selected}-{options.GetArrayElementAtIndex(selected).colorValue}");
			// 	currentColor.colorValue = options.GetArrayElementAtIndex(selected).colorValue;
			// }

			
			if (options.Length > 0)
			{
				if (GUI.Button(unitRect, displayedOptions[currentSelected].image, new GUIStyle()
				    {
					    fixedWidth = 50,
				    }))
				{
					PopupWindow.Show(unitRect, new SwatchPopupWindow(currentSelected, colorOptions, color =>
					{
						//currentColor.colorValue = options.GetArrayElementAtIndex(selected).colorValue; //by index or pass color back?
						currentColor.colorValue = color;
						property.serializedObject.ApplyModifiedProperties();

					}));
				}
			}
			else
			{
				//so heres the current issue':
				//While importing the asset, we make a list of all of the colors in the texture, and save it as a serialized property in the scriptedimporter. But this change won't take effect until we hit apply, and do another re-import.
				//So the first time you import an asset, it doesn't work, then you have to hit apply.
				//We can work around it by saving this list-of-color-data somewhere else that does get saved, like the scriptableObject we are creating.
			}

			// Set indent back to what it was
			EditorGUI.indentLevel = indent;
		
			EditorGUI.EndProperty();
		}

		public void OnColorSelected(Color color)
		{
			
		}
	}
}