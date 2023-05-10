using System.Collections.Generic;
using Tactics.Pathfinding;
using UnityEngine;

namespace Tactics
{
	public class NavNode : INode
	{
		//Tile/Configuration things.
		
		public NavMap NavMap => _navigation;
		private NavMap _navigation;

		public int WalkCost => _terrain.WalkCost;
		// private int _walkcost;
		public bool Walkable => _terrain.Walkable;
		// private bool _walkable;

		public Terrain Terrain => _terrain;
		private Terrain _terrain;
		
		// Positions
		
		/// <summary>
		/// TilemapPosition is the cell position in the Grid component. NavPosition is different for hex grids, and internal.
		/// </summary>
		public Vector3Int GridPosition => _gridPosition;
		private Vector3Int _gridPosition;
		// public Vector3 WorldPosition => _navigation.Tilemap.CellToWorld(GridPosition)+_navigation.Tilemap.tileAnchor;

		public NavNode(Terrain terrain,Vector3Int gridPosition,NavMap navigation)
		{
			// this._walkable = walkable;
			// this._walkcost = cost;
			this._terrain = terrain;
			_gridPosition = gridPosition;
			this._navigation = navigation;
		}
		
	}
}