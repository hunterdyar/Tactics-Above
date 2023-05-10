using System;
using UnityEngine;

namespace Tactics
{
	public class Tile : MonoBehaviour
	{

		[SerializeField] private NavMap _map;
		//we can change worldPos to include some offset here.
		public Vector3 WorldPosition => transform.position;
		
		public NavNode NavNode => _navNode;
		private NavNode _navNode;

		[SerializeField] private Terrain _terrain;

		private void OnEnable()
		{
			_map.CallForTiles += CallForTiles;
		}

		private void OnDisable()
		{
			_map.CallForTiles -= CallForTiles;
		}

		private void CallForTiles(NavMap.RegisterTile registerTile)
		{
			registerTile.Invoke(_terrain,this);
		}


		public void SetNavNode(NavNode node)
		{
			_navNode = node;
		}
	}
}