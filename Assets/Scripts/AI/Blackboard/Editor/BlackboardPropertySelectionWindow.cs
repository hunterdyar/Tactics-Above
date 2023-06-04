using System.Collections.Generic;
using Tactics.AI.Blackboard;
using UnityEditor.IMGUI.Controls;

namespace AI.Blackboard.Editor
{
	public class BlackboardPropertySelectionWindow : AdvancedDropdown
	{
		private BlackboardProperty _blackboardProperty;
		public BlackboardPropertySelectionWindow(BlackboardProperty blackboardProperty, AdvancedDropdownState state) : base(state)
		{
			_blackboardProperty = blackboardProperty;
		}

		protected override AdvancedDropdownItem BuildRoot()
		{
			if (_blackboardProperty.blackboard == null)
			{
				return new AdvancedDropdownItem("Empty");
			}
			var root = new AdvancedDropdownItem(_blackboardProperty.blackboard.name +" Blackboard");
			
			var elements = BlackboardProperty.FindElements(_blackboardProperty.blackboard.GetType(),_blackboardProperty.blackboard);
			foreach (var be in elements)
			{
				var element = new BlackboardPropertyDropdownItem(null,be);
				root.AddChild(element);
			}

			return root;
		}

		protected override void ItemSelected(AdvancedDropdownItem item)
		{
			if (item is BlackboardPropertyDropdownItem selected)
			{
				var selectedElement = new List<BlackboardElement>();
				var s = selected;
				while (s.Parent != null)
				{
					selectedElement.Add(s.Element);
					s = s.Parent;
				}
				selectedElement.Add(s.Element);
				selectedElement.Reverse();
				_blackboardProperty.SelectedElements = selectedElement.ToArray();
				
				_blackboardProperty.blackboardPropertyName = selected.Element.Name;
			}

			base.ItemSelected(item);
		}
	}
}