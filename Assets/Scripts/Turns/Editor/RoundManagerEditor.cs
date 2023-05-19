using Tactics.Turns;
using UnityEditor;
using UnityEngine.UIElements;


	[CustomEditor(typeof(RoundManager))]
	public class RoundManagerEditor : Editor
	{
		public override VisualElement CreateInspectorGUI()
		{
			var container = new VisualElement();

			container.Add(new IMGUIContainer(OnInspectorGUI));

			var startRound = new Button(() =>
			{
				(target as RoundManager)?.StartRound();
			})
			{
				text = "Start Round"
			};
			container.Add(startRound);
			return container;
		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
		}
	}
