using UnityEngine;

namespace Tactics.Entities
{
	public class FactionDebugViewer : MonoBehaviour
	{
		public Faction Faction;
		void OnDrawGizmosSelected()
		{
			// // Draw a semitransparent red cube at the transforms position
			// if (Faction.TerritoryMap == null)
			// {
			// 	return;
			// }

			// foreach (var node in Faction.NavMap.Nodes)
			// {
			// 	var influence = Faction.TerritoryMap.GetValue(node.GridPosition.x, node.GridPosition.z);
			// 	Gizmos.color = Faction.TerritoryGradient.Evaluate(Mathf.InverseLerp(Faction.territoryMinDisplay, Faction.territoryMaxDisplay, influence));
			// 	Gizmos.DrawCube(node.WorldPosition+Vector3.up, new Vector3(0.9f, 0.1f, 0.9f));
			// }

		}
	}
}