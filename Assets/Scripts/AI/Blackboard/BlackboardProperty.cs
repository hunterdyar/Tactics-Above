using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tactics.AI.Blackboard
{
	[Serializable]
	public class BlackboardProperty
	{
		//A blackboard property is a function call that represents a way to look up some piece of information about the world and serialize it.
		//used in this context, it's a float value. 
		
		//It works by giving it some grand context, in this the object that is serializing it, 'Blackboard'; although I might refactor this to be the AIContext class


		//So a blackboard property can return any function that returns float and has the [BlackboardElement] attribute on it, in it's context, or...
		//and this is the real trick. Or it can go through any class (through [blackboardelement] functions) that itself has blackboard functions with it, where you select - eventually - some float.
		
		//SelectedElements is an array, where we basically save the name of functions in a list. Now, we are doing some clever tricks here, using the Attribute class but serializing it, which only serializes the name of the functions, then rediscovering selected methods at runtime.

		public Object blackboard;//this will get assigned to the target object that has this property. todo: properly inject it at runtime or onValidate. Or change to use AIContext type off the bat.
		
		//these get serialized but only their name. At runtime we recreate them by marching through this list as a set of nested function calls and re-find (with all the nonserialized stuff) their runtime properties.
		public BlackboardElement[] SelectedElements = Array.Empty<BlackboardElement>();
		public BlackboardElement selectedElement => SelectedElements[^1];
		
		//this can be serialized, but can we use it at runtime?
		public AdvancedDropdownState SelectionState;
		
		//Called Lazily
		public void Init(object context = null, object[] parameters = null)
		{
			if (context == null)
			{
				context = blackboard;
			}
			if (context == null)
			{
				Debug.LogError("No context injected for blackboard property... How?");
			}

			//this is sort of like the recursive Blackboard Property Dropdown Item, but depth-first, and only to the selected? 
			for (int i = 0; i < SelectedElements.Length; i++)
			{
				//todo: We could do FindElements and search for just one with the name, instead of finding all with attributes and looping through those

				var els = FindElements(context.GetType(), context);
				foreach (var e in els)
				{
					if (e.Name == SelectedElements[i].Name)
					{
						SelectedElements[i] = e;
						context = SelectedElements[i].GetValueObject(parameters);//used in next step of loop/
					}
				}
			}

		}

		public float GetFloat(object[] parameters = null)
		{
			if (SelectedElements.Length == 0)
			{
				Debug.LogError("No blackboard property selected in dropdown",blackboard);
				return 0;
			}

			if (!selectedElement.IsInitiated)
			{
				Init(null,parameters);
			}
			
			//its still possible this might break - the final context we use has to be a reference value that we can save as an 'object'.
			
			var f = selectedElement.GetValueAsFloat(0,parameters);
			return f;
		}
		

		public static List<BlackboardElement> FindElements(Type blackboard, object blackboardContext = null)
		{
			if (blackboardContext == null) return new List<BlackboardElement>();
			var elements = new List<BlackboardElement>();

			MethodInfo[] methods = blackboard.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
			for (int i = 0; i < methods.Length; i++)
			{
				if (Attribute.GetCustomAttribute(methods[i], typeof(BlackboardElement)) is BlackboardElement attribute)
				{
					if (attribute.Name == "")
					{
						attribute.Name = ObjectNames.NicifyVariableName(methods[i].Name);
					}

					attribute.context = blackboardContext;
					attribute.method = methods[i];
					//todo passing a type into here is wrong.
					// attribute.GetValue = () => methods[i].Invoke(attribute.context, null);
					attribute.attribueType = methods[i].ReturnType;
					// Debug.Log(attribute.Name + "--" + attribute.GetValue?.Invoke()?.ToString()); // The name of the flagged variable.
					elements.Add(attribute);
				}
			}

			PropertyInfo[] props = blackboard.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
			for (int i = 0; i < props.Length; i++)
			{
				if (Attribute.GetCustomAttribute(props[i], typeof(BlackboardElement)) is BlackboardElement attribute)
				{
					if (attribute.Name == "")
					{
						attribute.Name = ObjectNames.NicifyVariableName(props[i].Name);
					}

					attribute.context = blackboardContext;
					attribute.method = props[i].GetMethod;
					// attribute.GetValue = () => props[i].GetMethod.Invoke(attribute.context, null);
					attribute.attribueType = props[i].PropertyType;
					// Debug.Log(attribute.Name + "--" + attribute.GetValue?.Invoke()?.ToString()); // The name of the flagged variable.
					elements.Add(attribute);
				}
			}

			return elements;
		}
	}
}