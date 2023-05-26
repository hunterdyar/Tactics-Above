using System.Collections.Generic;
using Tactics.AI.Blackboard;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;


[CustomPropertyDrawer(typeof(BlackboardProperty))]
public class BlackboardPropertyEditor : PropertyDrawer
{
	public override VisualElement CreatePropertyGUI(SerializedProperty property)
	{
		var container = new VisualElement();
		var targetObject = property.serializedObject.targetObject;//this will be the SELECTED object, not the blackboard property
			
		var objectField = new PropertyField(property.FindPropertyRelative("blackboard"));
		container.Add(objectField);
		
		
		var names = property.FindPropertyRelative("elementNames");
		
		var elementNames = new List<string>();
		for (int i = 0; i < names.arraySize; i++)
		{
			elementNames.Add(names.GetArrayElementAtIndex(i).stringValue);
		}

		var dropdown = new PopupField<string>("Property", elementNames, 0);
		container.Add(new Label("Data path")) ;
		container.Add(dropdown);

		return container;
		return base.CreatePropertyGUI(property);
	}
}


