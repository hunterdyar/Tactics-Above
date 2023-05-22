﻿using Tactics.AI.InfluenceMaps;
using Tactics.Entities;
using Unity.VisualScripting;

namespace Tactics.AI
{
	public class AIContext
	{
		public Faction MyFaction;
		public Faction EnemyFaction;
		public InfluenceMap TerritoryMap;
		public InfluenceMap ThreatMap;
		public InfluenceMap AttackMap;

		public InfluenceMap BattleMap;
		public AIContext(Faction myFaction, Faction[] enemyFactions)
		{
			MyFaction = myFaction;
			//we are only supporting one faction for now.
			EnemyFaction = enemyFactions[0];
			
			//Calculate territory map
			TerritoryMap = InfluenceMap.Clone(MyFaction.TerritoryMap);
			TerritoryMap.AddInfluence(EnemyFaction.TerritoryMap,-1f);//inverts; so subtracts enemy territory from map.

			ThreatMap = InfluenceMap.Clone(EnemyFaction.AttackMap);//todo evaluate if clone is proper here.
			AttackMap = InfluenceMap.Clone(MyFaction.AttackMap);

			BattleMap = InfluenceMap.Clone(TerritoryMap);
			BattleMap.MultiplyInfluence(EnemyFaction.TerritoryMap);//hi in contested areas/frontlines
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
	}
}