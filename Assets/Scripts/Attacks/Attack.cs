﻿using System.Collections.Generic;
using Tactics.AI;
using Tactics.AI.Actions;
using Tactics.DamageSystem;
using Tactics.Entities;
using Tactics.GridShapes;
using Tactics.Turns;
using Tactics.Utility;
using UnityEngine;

namespace Attacks
{
	//definition of an attack. Used to get relevant info/objects for agents, ai, etc.
	[CreateAssetMenu(fileName = "Attack", menuName = "Tactics/Attack", order = 0)]
	public class Attack : ScriptableObject
	{
		[SerializeField] private ScriptableShape shape;
		[SerializeField] private AttackLocationType locationType;
		public DamageDescription Damage => damage;
		[SerializeField] private DamageDescription damage; 
		
		//what does the AIAction need to do to choose to attack?
		
		
		// public MoveBase GetMove()
		// {

		// } 
		public List<AIAction> GetAIActions(Agent agent, AIContext context)
		{
			List<AIAction> actions = new List<AIAction>();
			//foreach direction.... all possible attacks.
			switch (locationType)
			{
				case AttackLocationType.OneOfShapeNoFacing:
					foreach (var node in shape.GetNodesOnTilemap(agent.CurrentNode))
					{
						actions.Add(new AttackAIAction(this,node, agent));	
					}
					break;
				case AttackLocationType.EntireShapeNoFacing:
					actions.Add(new AttackAIAction(this,shape.GetNodesOnTilemap(agent.CurrentNode), agent));
					break;
				case AttackLocationType.OneOfShapeAllFacing:
					foreach (var facing in RectUtility.CardinalDirectionsXY)
					{
						foreach (var node in shape.GetNodesOnTilemapInFacingDirection(agent.CurrentNode,facing))
						{
							actions.Add(new AttackAIAction(this,node, agent));
						}
					}
					break;
				case AttackLocationType.EntireShapeAllFacing:
					foreach (var facing in RectUtility.CardinalDirectionsXY)
					{
						actions.Add(new AttackAIAction(this, shape.GetNodesOnTilemapInFacingDirection(agent.CurrentNode,facing), agent));
					}

					break;
			}
			return actions;
		}
	}
}