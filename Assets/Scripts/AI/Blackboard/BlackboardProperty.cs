using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Authentication.ExtendedProtection;
using UnityEditor.IMGUI.Controls;
using UnityEditor.Timeline.Actions;
using UnityEngine;
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
		public Object blackboard;//this will get assigned to the target object.
		public string blackboardPropertyName;
		public BlackboardElement selectedElement;
		public List<BlackboardElement> elements;
		
		public AdvancedDropdownState SelectionState;
		//todo: This is not being called.
		public void Init()
		{
			elements = FindElements(blackboard.GetType());
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

		public static List<BlackboardElement> FindElements(Type blackboard)
		{
			if (blackboard == null) return null;
			var elements = new List<BlackboardElement>();
			
			HashSet<string> elementNames = new HashSet<string>();

			MethodInfo[] methods = blackboard.GetMethods(BindingFlags.Instance | BindingFlags.Public);
			for (int i = 0; i < methods.Length; i++)
			{
				if (Attribute.GetCustomAttribute(methods[i], typeof(BlackboardElement)) is BlackboardElement attribute)
				{
					attribute.Name = methods[i].Name;
					attribute.GetValue = () => methods[i].Invoke(blackboard, null);
					attribute.attribueType = methods[i].ReturnType;
					// Debug.Log(attribute.Name + "--" + attribute.GetValue?.Invoke()?.ToString()); // The name of the flagged variable.
					elements.Add(attribute);
					elementNames.Add(attribute.Name);
				}
			}


			PropertyInfo[] props = blackboard.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			for (int i = 0; i < props.Length; i++)
			{
				if (Attribute.GetCustomAttribute(props[i], typeof(BlackboardElement)) is BlackboardElement attribute)
				{
					attribute.Name = props[i].Name;
					attribute.GetValue = () => props[i].GetMethod.Invoke(blackboard, null);
					attribute.attribueType = props[i].PropertyType;
					// Debug.Log(attribute.Name + "--" + attribute.GetValue?.Invoke()?.ToString()); // The name of the flagged variable.
					elements.Add(attribute);
					elementNames.Add(attribute.Name);
				}
			}

			return elements;
		}
	}
}