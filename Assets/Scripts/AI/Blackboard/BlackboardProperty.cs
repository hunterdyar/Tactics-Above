﻿using System;
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
		public string blackboardPropertyName;
		public BlackboardElement selectedElement;
		public List<BlackboardElement> elements;
		
		//this can be serialized, but can we use it at runtime?
		public AdvancedDropdownState SelectionState;
		
		//Called Lazily
		public void Init()
		{
			if (blackboard == null)
			{
				Debug.LogError("No context injected for blackboard property... How?");
			}
			elements = FindElements(blackboard);

			foreach (var e in elements)
			{
				if (e.Name == blackboardPropertyName)
				{
					selectedElement = e;
					break;
				}
			}
			
			if (selectedElement == null) return;
			if (selectedElement.Name != blackboardPropertyName)
			{
				selectedElement = elements.Find(x => x.Name == blackboardPropertyName);
			}
		}

		public static List<BlackboardElement> FindElements(object blackboardContext)
		{
			if (blackboardContext == null) return null;
			var blackboard = blackboardContext.GetType();
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
					attribute.GetValue = () => methods[i].Invoke(blackboardContext, null);
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
					attribute.GetValue = () => props[i].GetMethod.Invoke(blackboardContext, null);
					attribute.attribueType = props[i].PropertyType;
					// Debug.Log(attribute.Name + "--" + attribute.GetValue?.Invoke()?.ToString()); // The name of the flagged variable.
					elements.Add(attribute);
				}
			}

			return elements;
		}

		public float GetFloat()
		{
			if (!selectedElement.IsInitiated)
			{
				Init();
			}
			var f = selectedElement.GetValueAsFloat();
			return f;
		}
	}
}