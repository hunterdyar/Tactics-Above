using System.Collections.Generic;
using Attacks;
using Tactics.AI.InfluenceMaps;
using Tactics.DamageSystem;
using Tactics.Entities;
using Tactics.GridShapes;
using Tactics.Turns;

namespace Tactics.AI.Actions
{
	public class AttackAIAction : AIAction
	{
		private Attack _attack;
		private Agent _agent;
		public List<NavNode> TargetNodes => _targetNodes;
		private List<NavNode> _targetNodes;
		public AttackAIAction(Attack attack,NavNode node, Agent agent)
		{
			_attack = attack;
			_agent = agent;
			_targetNodes = new List<NavNode>();
			_targetNodes.Add(node);
		}

		public AttackAIAction(Attack attack,List<NavNode> nodes, Agent agent)
		{
			_attack = attack;
			_agent = agent;
			_targetNodes = new List<NavNode>();
			foreach (var node in nodes)
			{
				_targetNodes.Add(node);
			}
		}

		public override void AffectInfluenceMap(Agent agent, InfluenceMap map, InfluenceMapType mapType)
		{	
			if(mapType == InfluenceMapType.Threat)
			{
				foreach (var node in _targetNodes)
				{
					map.AddValue(node.GridPosition.x,node.GridPosition.y, _attack.Damage.Amount);
				}
			}
		}

		public override MoveBase GetMove()
		{
			if(_targetNodes.Count == 1)
			{
				return new AttackOnNode(_agent, _targetNodes[0], _attack);
			}
			else
			{
				return new AttackOnNodes(_agent, _targetNodes, _attack);
			}
		}
	}
}