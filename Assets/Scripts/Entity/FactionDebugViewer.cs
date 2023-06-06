using Tactics.AI.InfluenceMaps;
using UnityEngine;

namespace Tactics.Entities
{
	public class FactionDebugViewer : MonoBehaviour
	{
		public Faction Faction;
		public InfluenceMapType MapType;
		void OnDrawGizmosSelected()
		{
			var map = Faction.GetMap(MapType);
				
			if (map == null) return;

			foreach (var node in Faction.NavMap.Nodes)
			{
				var influence = map.GetValue(node.GridPosition.x, node.GridPosition.z);
				Gizmos.color = Faction.TerritoryGradient.Evaluate(Mathf.InverseLerp(Faction.territoryMinDisplay, Faction.territoryMaxDisplay, influence));
				Gizmos.DrawCube(node.WorldPosition, new Vector3(0.9f, 0.1f, 0.9f));
			}

		}
	}
}