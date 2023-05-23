using System;
using System.Collections;
using Attacks;
using Tactics.AI;
using Tactics.AI.Actions;
using Tactics.AI.InfluenceMaps;
using Tactics.Turns;
using UnityEngine;

namespace Tactics.Entities
{
	/// <summary>
	/// Agents Are GridEntities (live on a layer in the grid)
	/// Can take turns.
	/// </summary>
	public class Agent : GridEntity
	{
		public EntityMap AgentLayer => _agentLayer;
		[SerializeField] private EntityMap _agentLayer;
		public Faction EnemyLayer => _enemyLayer;//todo: remove when we can.
		[SerializeField] private Faction _enemyLayer;
		public Attack[] Attacks => _attacks;
		[SerializeField] private Attack[] _attacks;
		private NavMap NavMap => _agentLayer.NavMap;
		public NavNode CurrentNode => _currentNode;

		private NavNode _currentNode;

		private MoveDecider _moveDecider;
		private MoveBase _nextMove;

		public int range = 3;
		private void Awake()
		{
			_moveDecider = GetComponent<MoveDecider>();
			if (_moveDecider == null)
			{
				Debug.LogWarning($"No move decider for {name}",this);
				_moveDecider = gameObject.AddComponent<DoNothingMoveDecider>();
			}
		}

		void Start()
		{
			//		
			if (NavMap == null)
			{
				Debug.LogWarning("No tilemap for agent. You probably need to add the entity layer to the Map component to initialize it.");
			}
			if (NavMap.TryGetNavNodeAtWorldPos(transform.position, out var node))
			{
				_currentNode = node;
				_agentLayer.AddEntityToMap(_currentNode,this);
			}
			else
			{
				Debug.LogWarning("Agent not on map",this);
			}


			//inject dependency. This has to happen AFTER we get a currentNode.
			//(well it doesn't half too, but i'd rather just reference the navmap through the currentNode, and not pass it explicitly.
			_moveDecider.Initiate(this);

		}
		// public bool TryMoveInDirection(Vector3Int direction)
		// {
		// 	if (NavMap.TryGetNavNode(_currentNode.GridPosition + direction,out var node))
		// 	{
		// 		if (node.Walkable && !_agentLayer.HasAnyEntity(node))
		// 		{
		// 			MoveToNode(node, true);
		// 			return true;
		// 		}
		// 	}
		// 	return false;
		// }

		public void SetOnNode(NavNode node, bool snap = false)
		{
			_agentLayer.MoveEntityToNode(this,node,snap);
			_currentNode = node;
			//snap... for now
			if (snap)
			{
				SnapToNodePosition(_currentNode);
			}

		}

		public void PrepareTurn(AIContext context)
		{
			//ai!
			
			//do this when creating node?
			_nextMove = _moveDecider.DecideMove(context);
		}

		//What nodes can THIS agent walk on?
		public bool IsNodeWalkable(NavNode node)
		{
			if (node == _currentNode)
			{
				return node.Walkable;
			}
			if (node.Walkable)
			{
				//already know it isn't itself, so we can do the faster 'contains'.
				if (_agentLayer.HasAnyEntity(node))
				{
					return false;
				}
				//check enemy layer... but ACTUALLY we want to have a 'layers that block pathfinding' collection collection, configured in some settings somewhere.

				//TESTING
				if (_enemyLayer.HasAnyEntity(node))
				{
					return false;
				}
				//What other checks do we need to do?
				
				return true;
			}

			return false;
		}

		public IEnumerator TakeTurn()
		{
			if (_nextMove == null)
			{
				Debug.LogWarning("Null next move.",this);
				yield break;
			}
			yield return StartCoroutine(_nextMove.DoMove());
			yield break;
		}

		public void Die()
		{
			_agentLayer.RemoveEntity(this);
			Destroy(gameObject);
		}

		public InfluenceMap GetTerritoryInfluence()
		{
			float range = 6f;
			//todo hold movement options somewhere sensible for this to be determined by those... as attacks will be 
			var map = new InfluenceMap(NavMap);
			var center = new Vector2Int(CurrentNode.GridPosition.x,CurrentNode.GridPosition.z);
			map.AddPropagation(center,range,DistanceFalloff.Exponential);

			return map;
		}

		public InfluenceMap GetInfluenceFromAttacks(InfluenceMapType mapType)
		{
			float range = 6f;
			//todo hold movement options somewhere sensible for this to be deterimed by those... as attacks will be 
			var map = new InfluenceMap(NavMap,0);
			var center = new Vector2Int(CurrentNode.GridPosition.x, CurrentNode.GridPosition.z);
			foreach (var attack in _attacks)
			{
				foreach (var action in attack.GetAIActions(this))
				{
					action.AffectInfluenceMap(this, ref map, mapType);
				}
				
			}
			return map;
		}

		/// <summary>
		/// 1 in locations that can be moved to, 0 in locations that can't.
		/// </summary>
		public InfluenceMap GetMovementRangeMap()
		{
			var map = InfluenceMap.New(_currentNode.NavMap);
			var center = new Vector2Int(CurrentNode.GridPosition.x, CurrentNode.GridPosition.z);
			map.AddPropagation(center,range,DistanceFalloff.None);
			return map;
		}
	}
}