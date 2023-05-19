﻿using System;
using System.Collections;
using Tactics.AI;
using Tactics.Pathfinding;
using Tactics.Turns;
using UnityEngine;

namespace Tactics.Entities
{
	/// <summary>
	/// Agents Are GridEntities (live on a layer in the grid)
	/// Can take turns.
	/// </summary>
	public class Agent : GridEntity, ITurnTaker
	{
		public EntityMap AgentLayer => _agentLayer;
		[SerializeField] private EntityMap _agentLayer;
		private NavMap NavMap => _agentLayer.NavMap;
		public NavNode CurrentNode => _currentNode;

		private NavNode _currentNode;

		private MoveDecider _moveDecider;
		private MoveBase nextMove;

		[Header("Testing")] public Transform goal;
		public int movesPerTurn;

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

		public void PrepareTurn()
		{
			//ai!
			
			//do this when creating node?
			nextMove = _moveDecider.DecideMove();
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

				//What other checks do we need to do?
				
				return true;
			}

			return false;
		}

		public IEnumerator TakeTurn()
		{
			if (nextMove == null)
			{
				Debug.LogWarning("Null next move.",this);
				yield break;
			}
			yield return StartCoroutine(nextMove.DoMove());
			yield break;
		}

		public void Die()
		{
			_agentLayer.RemoveEntity(this);
			Destroy(gameObject);
		}
	}
}