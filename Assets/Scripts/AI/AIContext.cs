using Tactics.AI.Actions;
using Tactics.AI.Blackboard;
using Tactics.AI.InfluenceMaps;
using Tactics.Entities;
using Unity.VisualScripting;

namespace Tactics.AI
{
	public class AIContext
	{
		[BlackboardElement]
		public Faction MyFaction;
		[BlackboardElement]
		public Faction EnemyFaction;
		public InfluenceMap TerritoryMap;
		public InfluenceMap ThreatMap;
		public InfluenceMap AttackMap;
		public InfluenceMap BattleMap;


		public Agent OperatingAgent => _operatingAgent;
		private Agent _operatingAgent;

		public IAIAction Action => _action;
		private IAIAction _action;
		
		
		public AIContext(Faction myFaction, Faction[] enemyFactions)
		{
			MyFaction = myFaction;
			//we are only supporting one faction for now.
			EnemyFaction = enemyFactions[0];
			
			//Calculate territory map
			TerritoryMap = InfluenceMap.Clone(MyFaction.MyTerritoryMap);
			TerritoryMap.AddInfluence(EnemyFaction.MyTerritoryMap,-1f);//inverts; so subtracts enemy territory from map.

			ThreatMap = InfluenceMap.Clone(EnemyFaction.AttackMap);//todo evaluate if clone is proper here.
			AttackMap = InfluenceMap.Clone(MyFaction.AttackMap);

			BattleMap = InfluenceMap.Clone(TerritoryMap);
			BattleMap.MultiplyInfluence(EnemyFaction.MyTerritoryMap);//high in contested areas/frontlines
		}

		public Faction GetFactionFromContext(FactionContext context)
		{
			switch (context)
			{
				case FactionContext.Allies:
					return MyFaction;
				case FactionContext.Enemy:
				default:
					return EnemyFaction;
			}
		}

		public void SetOperatingAgent(Agent agent)
		{
			_operatingAgent = agent;
		}

		public void SetAction(IAIAction action)
		{
			_action = _action;
		}

		#region Blackboard

		
//These are static to make it clear we are pulling from injected context, not direct references.

		[BlackboardElement]
		public static InfluenceMap GetTerritoryMap(AIContext context)
		{
			return context.TerritoryMap;
		}

		[BlackboardElement]
		public static InfluenceMap GetThreatMap(AIContext context)
		{
			return context.ThreatMap;
		}

		[BlackboardElement]
		public static InfluenceMap GetBattleMap(AIContext context)
		{
			return context.BattleMap;
		}

		[BlackboardElement]
		public static InfluenceMap GetAttackMap(AIContext context)
		{
			return context.AttackMap;
		}
		
		#endregion
	}
}