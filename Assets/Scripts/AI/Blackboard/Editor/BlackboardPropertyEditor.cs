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
		var prop = property.serializedObject.targetObject;

		var targetObject = property.serializedObject.targetObject;


		
		
		
			var objectField = new PropertyField(property.FindPropertyRelative("blackboard"));
		container.Add(objectField);
		var bp = property.FindPropertyRelative("elements");
		var targetObjectClassType = bp.GetType();
		var field = targetObjectClassType.GetField(bp.propertyPath);
		if (field != null)
		{
			var elements = field.GetValue(targetObject) as List<BlackboardElement>;
			if (elements != null)
			{
				
				var elementNames = new List<string>();
				for (int i = 0; i < elements.Count; i++)
				{
					elementNames.Add(elements[i].Name);
				}

				var dropdown = new PopupField<string>("Property", elementNames, 0);

				container.Add(dropdown);
			}
		}
		return container;
		return base.CreatePropertyGUI(property);
	}
}
