using Tactics.AI.Blackboard;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace AI.Blackboard.Editor
{
	public class BlackboardPropertyDropdownItem:AdvancedDropdownItem
	{
		private BlackboardElement _element;
		public BlackboardElement Element => _element;

		public BlackboardPropertyDropdownItem(BlackboardElement element) : base(element.Name)
		{
			_element = element;
			var o = _element.GetValue;
			var children = BlackboardProperty.FindElements(_element.attribueType);
			foreach (var be in children)
			{
				var e = new BlackboardPropertyDropdownItem(be);
				this.AddChild(e);
			}
		}
	}
}