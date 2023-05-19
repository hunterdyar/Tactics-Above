using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
#if UNITY_EDITOR
using UnityEditorInternal;
#endif

namespace Tactics.Entities
{
	/// <summary>
	/// An Entity Map is like a layer on top of the tilemap. It's a dictionary of entities and their positions.
	/// An entity map cannot be used by multiple tilemaps at the same time.
	/// </summary>
	[CreateAssetMenu(fileName = "Entity Map", menuName = "Tactics/Entities/Entity Map", order = 0)]
	public class EntityMap : ScriptableObject
	{
		//Keep a dictionary of GridEntities and NavNodes.
		
		//Todo: wrap data in sortable bidirectional dictionary.
		protected readonly Dictionary<NavNode, GridEntity> _entities = new Dictionary<NavNode, GridEntity>();
		protected readonly Dictionary<GridEntity, NavNode> _inverseEntities = new Dictionary<GridEntity, NavNode>();
		public int Count => _entities.Count;

		//We don't actually need this injected! yet...
		//I think we will for listening to events, moving between maps, or so on.
		public NavMap NavMap => _navMap;
		private NavMap _navMap;

		public void Initiate(NavMap navMap)
		{
			_entities.Clear();
			_inverseEntities.Clear();
			this._navMap = navMap;
		}

		public bool TryGetEntity(NavNode node, out GridEntity entity)
		{
			return _entities.TryGetValue(node, out entity);
		}

		public bool HasAnyEntity(NavNode node)
		{
			return _entities.ContainsKey(node);
		}
		public void AddEntityToMap(NavNode node, GridEntity entity, bool snapToPosition = true)
		{
			if (_entities.ContainsValue(entity))
			{
				if (_entities.ContainsKey(node) && _entities[node] != entity)
				{
					Debug.LogWarning("Trying to add entity to map, but that entity is already on map somewhere else.");
				}
			}
			
			//Assert.IsFalse(_entities.ContainsValue(entity) && _entities.ContainsKey(node) && _entities[node] != entity, "Trying to add entity to map, but that entity is already on map somewhere else.");
			
			if (!_entities.ContainsKey(node))
			{
				_entities.Add(node,entity);
				_inverseEntities.Add(entity,node);
				if (snapToPosition)
				{
					entity.SnapToNodePosition(node);
				}
#if UNITY_EDITOR
				//The editor window contains an info box telling us how many items are in the dictionary, which is helpful for debugging.
				//This wouldn't update unless we mouse over it, which makes it far less helpful for debugging, so we force a repaint.
				InternalEditorUtility.RepaintAllViews();
#endif
			}
			else
			{
				Debug.LogError("Can't add entity, already an entity on this layer at this position (map)");
			}
		}

		public void RemoveEntityOnNode(NavNode node)
		{
			if (_entities.ContainsKey(node))
			{
				var value = _entities[node];
				_entities.Remove(node);
				_inverseEntities.Remove(value);
			}

#if UNITY_EDITOR
			InternalEditorUtility.RepaintAllViews();
#endif
		}

		public void RemoveEntity(GridEntity entity)
		{
			if (_inverseEntities.ContainsKey(entity))
			{
				var value = _inverseEntities[entity];
				_inverseEntities.Remove(entity);
				_entities.Remove(value);
			}
#if UNITY_EDITOR
			InternalEditorUtility.RepaintAllViews();
#endif
		}

		/// <summary>
		/// Removes entity from the node they are on, adds them to this new node. Won't throw an error if the entity isn't on a node to begin with.
		/// </summary>
		public void MoveEntityToNode(GridEntity entity,NavNode node,bool snap = true)
		{
			RemoveEntity(entity);
			AddEntityToMap(node,entity,snap);
		}
		
	}
}