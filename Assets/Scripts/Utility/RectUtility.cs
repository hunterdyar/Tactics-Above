using UnityEngine;

namespace Tactics.Utility
{
	public static class RectUtility
	{
		public static readonly Vector3Int[] CardinalDirections = new[]
		{
			new Vector3Int(1, 0, 0),
			new Vector3Int(0, 0, 1),
			new Vector3Int(-1, 0, 0),
			new Vector3Int(0, 0, -1),
		};

		public static readonly Vector3Int[] CardinalAndDiagonalDirections = new[]
		{
			new Vector3Int(1, 0, 0),
			new Vector3Int(0, 0, 1),
			new Vector3Int(-1, 0, 0),
			new Vector3Int(0, 0, -1),
			new Vector3Int(1, 0, 1),
			new Vector3Int(-1, 0, 1),
			new Vector3Int(-1, 0, -1),
			new Vector3Int(1, 0, -1),
		};
	}
}