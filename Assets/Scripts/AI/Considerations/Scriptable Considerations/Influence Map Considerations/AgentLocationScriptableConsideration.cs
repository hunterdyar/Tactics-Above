﻿using Tactics.AI.Actions;
using Tactics.AI.InfluenceMaps;
using Tactics.Entities;
using UnityEngine;

namespace Tactics.AI.Considerations
{
	[CreateAssetMenu(fileName = "Agent Current Location Value", menuName = "Tactics/Considerations/Agent Location Consideration")]

	public class AgentLocationScriptableConsideration: ScriptableConsideration
	{
		public InfluenceMapType MapType;
		public AnimationCurve Curve;
		
		public override float ScoreConsideration(AIContext context)
		{
			InfluenceMap map;
			switch(MapType)
			{
				case InfluenceMapType.Territory:
					map = context.TerritoryMap;
					break;
				case InfluenceMapType.Threat:
					map = context.ThreatMap;
					break;
				case InfluenceMapType.Battle:
					map = context.BattleMap;
					break;
				case InfluenceMapType.Attack:
					map = context.AttackMap;
					break;
				default:
					Debug.LogError("Tried To Consider Map Type That Does Not Exist");
					return 0f;
			}
			//todo: clamp,normalize, and scale
			float mapValue = map.GetValue(context.OperatingAgent.CurrentNode);
			return Mathf.Clamp01(Curve.Evaluate(mapValue));
		}
	}
}