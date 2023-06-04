﻿using Tactics.AI.Blackboard;
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
			
			var root = new AdvancedDropdownItem(_blackboardProperty.blackboard.name +" Blackboard");
			
			var elements = BlackboardProperty.FindElements(_blackboardProperty.blackboard.GetType());
			foreach (var be in elements)
			{
				var element = new BlackboardPropertyDropdownItem(be);
				root.AddChild(element);
			}

			return root;
		}

		protected override void ItemSelected(AdvancedDropdownItem item)
		{
			if (item is BlackboardPropertyDropdownItem selected)
			{
				_blackboardProperty.selectedElement = selected.Element;
			}

			base.ItemSelected(item);
		}
	}
}