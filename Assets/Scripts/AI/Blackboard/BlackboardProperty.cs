using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Authentication.ExtendedProtection;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace Tactics.AI.Blackboard
{
	[Serializable]
	public class BlackboardProperty
	{
		//A blackboard property is a float value that represents a way to look up some piece of information about the world and serialize it.
		//It's similar to input paths in the input system.
		//Data goes into the blackboard by some provider, and can be retrieved with a lookup.
		//Blackboards are basically string dictionaries, but can be nested instead.
		
		//We also have a context in the backboard lookup so we can reference "allies" or "agentCurrentLocation".
		//So "Turn Number", "Faction/Energy" or "Faction/Units/Count" or Faction/TerritoryMap/CurrentLocation" are all valid blackboard properties.
		 
		//I want to build an editor that looks like a tree structure...
		//public FactionContext factionContext;
		
		
		//So a blackboard property can return an object, a float, a node, or another blackboard - hence nesting.
		
		//blackboard.Get("Faction/Units/Count") does a split by /, then does Faction.Get("Units/Count") which does a split by / and then Count.Get();
		//I think we can serialize it with references to hash values in an array, some preprocessing step... or store a lookup cache from the whole string to the final blackboard cache.
		
		
		
		////////////////////////////////////////
		//
		//
		//The other way to do it is to serialize a flow chart of dropdowns, and grab various values from it. Some dynamic pulling of values from Code attributes [BlackboardProperty("Faction/Units/Count")].
		//All the attributes get added to a list by path...and are... serialized?
		public Object blackboard;//this will get assigned to the target object that has this property. todo: properly inject it at runtime or onValidate.
		
		//these get serialized but only their name. At runtime we recreate them by marching through this list as a set of nested function calls and re-find (with all the nonserialized stuff) their runtime properties.
		public BlackboardElement[] SelectedElements = new BlackboardElement[0];
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