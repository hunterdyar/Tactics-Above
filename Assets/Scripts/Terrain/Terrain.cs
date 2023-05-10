using UnityEngine;

namespace Tactics
{
	[CreateAssetMenu(fileName = "Terrain", menuName = "Tactics/Terrain", order = 0)]
	public class Terrain : ScriptableObject
	{
		public int WalkCost;
		public bool Walkable;
	}
}