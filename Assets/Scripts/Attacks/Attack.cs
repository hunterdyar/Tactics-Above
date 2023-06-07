using System.Collections.Generic;
using Tactics;
using Tactics.AI;
using Tactics.AI.Actions;
using Tactics.AI.Considerations;
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
		public ScriptableShape Shape => shape;
		[SerializeField] private ScriptableShape shape;

		public AttackLocationType LocationType => locationType;
		[SerializeField] private AttackLocationType locationType;
		public DamageDescription Damage => damage;
		[SerializeField] private DamageDescription damage; 
		//
		
		[SerializeField] private List<ScriptableConsideration> considerations;
		//what does the AIAction need to do to choose to attack?
		
		
		// public MoveBase GetMove()
		// {

		// } 
		public List<AttackAIAction> GetAIActions(Agent agent)
		{
			List<AttackAIAction> actions = new List<AttackAIAction>();
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

		public List<MoveToAttackOption> GetAttackOptionsForLocation(NavNode attackPos, int stepsToAttackPos = 0)
		{
			List<MoveToAttackOption> options = new List<MoveToAttackOption>();
			//foreach direction.... all possible attacks.
			switch (locationType)
			{
				case AttackLocationType.OneOfShapeNoFacing:
					foreach (var node in shape.GetNodesOnTilemap(attackPos))
					{
						options.Add(new MoveToAttackOption(this, attackPos, node,Vector3Int.zero, stepsToAttackPos));
					}

					break;
				case AttackLocationType.EntireShapeNoFacing:
					options.Add(new MoveToAttackOption(this, attackPos, shape.GetNodesOnTilemap(attackPos), Vector3Int.zero));
					break;
				case AttackLocationType.OneOfShapeAllFacing:
					foreach (var facing in RectUtility.CardinalDirectionsXY)
					{
						foreach (var node in shape.GetNodesOnTilemapInFacingDirection(attackPos, facing))
						{
							//todo: is this how im storing facing? i think it should be v2int.
							options.Add(new MoveToAttackOption(this, attackPos,node, facing.V2ToV3XZ(), stepsToAttackPos));
						}
					}

					break;
				case AttackLocationType.EntireShapeAllFacing:
					foreach (var facing in RectUtility.CardinalDirectionsXY)
					{
						options.Add(new MoveToAttackOption(this, attackPos, shape.GetNodesOnTilemapInFacingDirection(attackPos, facing), Vector3Int.zero));
					}
					break;
			}

			return options;
		}


		public List<ScriptableConsideration> GetConsiderations()
		{
			return considerations;
		}
	}
}