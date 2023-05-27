using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Tactics.AI.Blackboard;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


[CustomPropertyDrawer(typeof(BlackboardProperty))]
public class BlackboardPropertyEditor : PropertyDrawer
{
	private SerializedProperty _property;
	public override VisualElement CreatePropertyGUI(SerializedProperty property)
	{
		_property = property;
		var container = new VisualElement();
		var targetObject = property.serializedObject.targetObject;//this will be the SELECTED object, not the blackboard property
		var blackboard = GetValue(targetObject, property.name) as BlackboardProperty;
		
		//add object assignment field. to be replaced with parent object later.
		var objectField = new PropertyField(property.FindPropertyRelative("blackboard"));
		container.Add(objectField);
		
		//selected object for debugging.
		var selectedField = new PropertyField(property.FindPropertyRelative("selectedElement"));
		selectedField.SetEnabled(false);
		
		container.Add(selectedField);
		
		blackboard.Init();
		//create list of the names of [blackboardelements].
		var names = property.FindPropertyRelative("elementNames");

		var elementNames = new List<string>();
		for (int i = 0; i < blackboard.elements.Count; i++)
		{
			elementNames.Add(blackboard.elements[i].Name);
		}
		int index = elementNames.IndexOf(blackboard.selectedElement.Name);
		if (index < 0)
		{
			index = 0;
		}
		var dropdown = new PopupField<string>("Property", elementNames, index);
		
		dropdown.RegisterValueChangedCallback(x =>
		{
			blackboard.selectedElement = blackboard.elements.Find(be => be.Name == x.newValue);
			blackboard.blackboardPropertyName = blackboard.selectedElement?.Name;
			_property.serializedObject.ApplyModifiedProperties();
			
		});
		container.Add(new Label("Data path")) ;
		container.Add(dropdown);

		return container;
	}

	public object GetValue(object source, string name)
	{
		if (source == null)
			return null;
		var type = source.GetType();
		var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
		if (f == null)
		{
			var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
			if (p == null)
				return null;
			return p.GetValue(source, null);
		}

		return f.GetValue(source);
	}

	public object GetValue(object source, string name, int index)
	{
		var enumerable = GetValue(source, name) as IEnumerable;
		var enm = enumerable.GetEnumerator();
		while (index-- >= 0)
			enm.MoveNext();
		return enm.Current;
	}
}


