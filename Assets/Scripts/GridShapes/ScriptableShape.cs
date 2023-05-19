using System.Collections.Generic;
using System.Linq;
using Tactics.Utility;
using UnityEngine;

namespace Tactics.GridShapes
{ 
	public abstract class ScriptableShape : ScriptableObject
	{
		public abstract List<Vector2Int> Shape { get; }

		/// <summary>
		/// Returns the shape as NavNodes filtering out positions that the tilemap doesn't have, and absolute tilemap positions.
		/// Required when using procedural shapes, like "straight line until wall".
		/// </summary>

		/// <summary>
		/// Calculates and returns the shape rotated around the origin, as if it started facing up v2(0,1). Does not modify original shape.
		/// </summary>
		public virtual List<Vector2Int> GetShapeInCardinalFacingDirection(Vector2Int facing)
		{
			// "normalize"
			int fx = Mathf.Clamp(facing.x, -1, 1);
			int fy = Mathf.Clamp(facing.y, -1, 1);

			if (fx == 0)
			{
				if (fy == 1)
				{
					return Shape;
				}
				else if (fy == -1)
				{
					return Shape.ConvertAll(v => v.Rotate180());
				}
			}

			if (fy == 0)
			{
				if (fx == 1)
				{
					return Shape.ConvertAll(v => v.RotateRight());
				}
				else if (fx == -1)
				{
					//return rotated left.
					return Shape.ConvertAll(v => v.RotateLeft());
				}
			}

			Debug.LogWarning("GetShapeInFacingDir requires input facing dir to be cardinal.");
			return Shape;
		}

		public virtual List<NavNode> GetNodesOnTilemapInFacingDirection(NavNode center, Vector2Int facing)
		{
			List<NavNode> nodes = new List<NavNode>();
			foreach (var offset in GetShapeInCardinalFacingDirection(facing))
			{
				if (center.NavMap.TryGetNavNode(center.GridPosition + new Vector3Int(offset.x,0,offset.y), out var node))
				{
					nodes.Add(node);
				}
			}

			return nodes;
		}
		public virtual List<NavNode> GetNodesOnTilemap(NavNode center)
		{
			List<NavNode> nodes = new List<NavNode>();
			foreach (var offset in Shape)
			{
				if(center.NavMap.TryGetNavNode(center.GridPosition+new Vector3Int(offset.x,0,offset.y),out var node))
				{
					nodes.Add(node);	
				}
			}

			return nodes;
		}
	}
}