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
		var blackboardProperty = GetTargetObjectOfProperty(property) as BlackboardProperty;
		
		//add object assignment field. to be replaced with parent object later.
		var objectField = new PropertyField(property.FindPropertyRelative("blackboard"));
		container.Add(objectField);
		
		//selected object for debugging.
		var selectedField = new PropertyField(property.FindPropertyRelative("selectedElement"));
		selectedField.SetEnabled(false);
		
		container.Add(selectedField);
		
		blackboardProperty.Init();
		//create list of the names of [blackboardelements].
		var names = property.FindPropertyRelative("elementNames");

		var elementNames = new List<string>();
		for (int i = 0; i < blackboardProperty.elements.Count; i++)
		{
			elementNames.Add(blackboardProperty.elements[i].Name);
		}

		int index = 0;
		if (blackboardProperty.selectedElement != null)
		{
			index = elementNames.IndexOf(blackboardProperty.selectedElement.Name);
			if (index < 0)
			{
				index = 0;
			}
		}
		
		var dropdown = new PopupField<string>("Property", elementNames, index);
		
		dropdown.RegisterValueChangedCallback(x =>
		{
			blackboardProperty.selectedElement = blackboardProperty.elements.Find(be => be.Name == x.newValue);
			blackboardProperty.blackboardPropertyName = blackboardProperty.selectedElement?.Name;
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
	
	/// <summary>
        /// Gets the object the property represents.
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static object GetTargetObjectOfProperty(SerializedProperty prop)
        {
            var path = prop.propertyPath.Replace(".Array.data[", "[");
            object obj = prop.serializedObject.targetObject;
            var elements = path.Split('.');
            foreach (var element in elements)
            {
                if (element.Contains("["))
                {
                    var elementName = element.Substring(0, element.IndexOf("["));
                    var index = System.Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                    obj = GetValue_Imp(obj, elementName, index);
                }
                else
                {
                    obj = GetValue_Imp(obj, element);
                }
            }
            return obj;
        }
 
        private static object GetValue_Imp(object source, string name)
        {
            if (source == null)
                return null;
            var type = source.GetType();
 
            while (type != null)
            {
                var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (f != null)
                    return f.GetValue(source);
 
                var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (p != null)
                    return p.GetValue(source, null);
 
                type = type.BaseType;
            }
            return null;
        }
 
        private static object GetValue_Imp(object source, string name, int index)
        {
            var enumerable = GetValue_Imp(source, name) as System.Collections.IEnumerable;
            if (enumerable == null) return null;
            var enm = enumerable.GetEnumerator();
            //while (index-- >= 0)
            //    enm.MoveNext();
            //return enm.Current;
 
            for (int i = 0; i <= index; i++)
            {
                if (!enm.MoveNext()) return null;
            }
            return enm.Current;
        }
}


