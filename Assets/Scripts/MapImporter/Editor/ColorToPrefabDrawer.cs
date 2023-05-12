using HDyar.MapImporter;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace MapImporter.Editor
{
	[CustomPropertyDrawer(typeof(ColorToPrefab))]
	public class ColorToPrefabDrawer : PropertyDrawer
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var container = new VisualElement();

			var prefabField = new PropertyField(property.FindPropertyRelative("Prefab"));
			container.Add(prefabField);
			return container;
		}
		
		// Draw the property inside the given rect
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
			for (var i = 0; i < displayedOptions.Length; i++)
			{
				var color = options.GetArrayElementAtIndex(i).colorValue;
				var tex = new Texture2D(100,100);
				//create an array of colors for every pixel.
				var colorArray = tex.GetPixels();
				for (int j = 0; j < colorArray.Length; j++)
				{
					colorArray[i] = color;
				}
				
				tex.SetPixels(0,0,10,10,colorArray);
				displayedOptions[i] = new GUIContent()
				{
					text = "color" + i,
					image = tex,
					tooltip = color.ToString()
				};
			}

			GUIStyle style = new GUIStyle()
			{
				imagePosition = ImagePosition.ImageOnly
			};
			var currentselected = 0;
			var currentColor = property.FindPropertyRelative("color");
			for (int i = 0; i < options.arraySize; i++)
			{
				if (options.GetArrayElementAtIndex(i).colorValue == currentColor.colorValue)
				{
					currentselected = i;
					break;
				}
			}
			int selected = EditorGUI.Popup(unitRect, currentselected, displayedOptions, style);
			//turn the options into selected color
			
			if (selected != currentselected)
			{
				Debug.Log($"change color from {currentselected}-{currentColor.colorValue} to {selected}-{options.GetArrayElementAtIndex(selected).colorValue}");
				currentColor.colorValue = options.GetArrayElementAtIndex(selected).colorValue;
			}
			// Set indent back to what it was
			EditorGUI.indentLevel = indent;

			EditorGUI.EndProperty();
		}
	}
}