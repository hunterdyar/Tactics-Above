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
			
			var elements = BlackboardProperty.FindElements(_blackboardProperty.blackboard);
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
				_blackboardProperty.blackboardPropertyName = selected.Element.Name;
			}

			base.ItemSelected(item);
		}
	}
}