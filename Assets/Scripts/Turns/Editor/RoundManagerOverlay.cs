using Tactics.Turns;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UIElements;

namespace Turns.Editor
{
	[Overlay(typeof(SceneView), "Round Manager View", true)]
	public class RoundManagerOverlay : Overlay
	{
		public override VisualElement CreatePanelContent()
		{
			var root = new VisualElement(){name = "Round Manager Overlay"};
			root.Add(new Label(){text = "Round Manager"});

			var startRound = new Button(() => { GameObject.FindObjectOfType<RoundManager>()?.StartRound(); })
			{
				text = "Start Round"
			};
			root.Add(startRound);

			return root;
		}
	}
}