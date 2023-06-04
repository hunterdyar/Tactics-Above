using Tactics.AI.Blackboard;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace AI.Blackboard.Editor
{
	public class BlackboardPropertyDropdownItem:AdvancedDropdownItem
	{
		private BlackboardElement _element;
		public BlackboardElement Element => _element;

		public BlackboardPropertyDropdownItem Parent => _parent;
		private BlackboardPropertyDropdownItem _parent;
		public BlackboardPropertyDropdownItem(BlackboardPropertyDropdownItem parent, BlackboardElement element) : base(element.Name)
		{
			_element = element;
			_parent = parent;
			//todo: this is the issue. We need to store the elements context, but we can't get that at runtime.
			var children = BlackboardProperty.FindElements(_element.attribueType,_element.context);
			foreach (var be in children)
			{
				var e = new BlackboardPropertyDropdownItem(this,be);
				this.AddChild(e);
			}
		}
	}
}