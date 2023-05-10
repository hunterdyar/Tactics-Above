using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HDyar.RougeLevelGen;
using Tactics.Entities;
using Tactics.Pathfinding;
using Tactics.Utility;
using UnityEngine;

namespace Tactics
{
	[CreateAssetMenu(fileName = "Map", menuName = "Tactics/Nav Map", order = 0)]
	public class NavMap : ScriptableObject, IGraph
	{
		public static Action<NavMap, BoundsInt> OnMapSizeChanged;
		
		public delegate void RegisterTile(Terrain terrain, Tile tile);
		
		public Action<RegisterTile> CallForTiles;
		[Header("Config")]
		[SerializeField] private Terrain _defaultTerrain;
		[SerializeField] private EntityMap[] _entityMaps;
		private readonly Dictionary<Vector3Int, NavNode> _map = new Dictionary<Vector3Int, NavNode>();

		public Grid Grid => _grid;
		private Grid _grid;

		public BoundsInt GridSpaceBounds;
		private BoundsInt _gridSpaceBounds;
#region InitiateFlow

		//First: Agents/Entities subscribe to actions on the scriptableObject.

		public void Initiate()
		{
			_gridSpaceBounds = new BoundsInt(Vector3Int.zero,Vector3Int.zero);
			//clear self
			_map.Clear();
			//clear entity maps
			InitiateEntityMaps();
			
			//Fire off initiation events. A callback will register tiles.
			CallForTiles?.Invoke(RegisterTileOnMap);	
			
			//Now we will update the map size.
			//todo: we should make this functional or not.
			OnMapSizeChanged?.Invoke(this, _gridSpaceBounds);
		}

		private void InitiateEntityMaps()
		{
			if (_entityMaps.Length == 0)
			{
				Debug.LogWarning("No entity maps configured in Tilemap Navigation", this);
			}

			foreach (var map in _entityMaps)
			{
				map.Initiate(this);
			}
		}

		/// <summary>
		/// A tile tells the navmap that it exists. GridPosition is calculated (i know this feels backwards, see overload) and A NavNode object is created.
		/// </summary>
		public void RegisterTileOnMap(Tile tile)
		{
			var gridPos = _grid.WorldToCell(tile.WorldPosition);
			var navNode = new NavNode(_defaultTerrain, gridPos, this);
			_gridSpaceBounds.SetMinMax(Vector3Int.Min(_gridSpaceBounds.min, gridPos), Vector3Int.Max(_gridSpaceBounds.max, gridPos));

			tile.SetNavNode(navNode);
		}
		public void RegisterTileOnMap(Terrain terrain, Tile tile)
		{
			var gridPos = _grid.WorldToCell(tile.WorldPosition);
			var navNode = new NavNode(terrain,gridPos,this);
			tile.SetNavNode(navNode);
			//navnode.settile...
			_map.Add(gridPos,navNode);

			//update bounds
			_gridSpaceBounds.SetMinMax(Vector3Int.Min(_gridSpaceBounds.min, gridPos), Vector3Int.Max(_gridSpaceBounds.max, gridPos));

			
			Debug.Log($"Tile registered on {gridPos}");
		}
		//overload if we already know the grid position.
		public void RegisterTileOnMap(Terrain terrain, Vector3Int gridPos, Tile tile)
		{
			var navNode = new NavNode(terrain, gridPos, this);
			tile.SetNavNode(navNode);
			_gridSpaceBounds.SetMinMax(Vector3Int.Min(_gridSpaceBounds.min, gridPos), Vector3Int.Max(_gridSpaceBounds.max, gridPos));
		}
#endregion

		public void SetWorldGrid(Grid grid)
		{
			_grid = grid;
		}

#region Getters

		

		public List<GridEntity> GetAllEntitiesOnNode(NavNode node)
		{
			var entities = new List<GridEntity>();

			foreach (var map in _entityMaps)
			{
				if (map.TryGetEntity(node, out var entity))
				{
					entities.Add(entity);
				}
			}

			return entities;
		}
		public INode[] GetNeighborNodes(INode center, bool walkableOnly = true)
		{
			throw new System.NotImplementedException();
		}

		public bool TryGetNavNodeAtWorldPos(Vector3 worldPos, out NavNode node)
		{
			var pos = _grid.WorldToCell(worldPos);
			return TryGetNavNode(pos, out node);
		}

		public bool TryGetNavNode(Vector3Int gridCellPosition, out NavNode node)
		{
			return _map.TryGetValue(gridCellPosition, out node);
		}

		public NavNode GetNavNode(Vector3Int gridPosition)
		{
			if (_map.TryGetValue(gridPosition, out var node))
			{
				return node;
			}

			return null;
		}
		
		public bool HasNavCellLocation(Vector3Int navPosition)
		{
			return _map.ContainsKey(navPosition);
		}

#endregion

	}
}