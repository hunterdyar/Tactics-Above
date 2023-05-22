using System.Collections.Generic;
using Tactics.AI.Considerations;
using Tactics.AI.InfluenceMaps;
using Tactics.Entities;
using Tactics.Turns;
using UnityEngine;

namespace Tactics.AI.Actions
{
	public interface IAIAction 
	{
		//score will be updated continuously as different agents consider the action.
		public float Score { get; set; }
		
		public List<Consideration> GetConsiderations();
		//affect influence map. This is a processing step for before move planning.
		public abstract void AffectInfluenceMap(Agent agent, ref InfluenceMap map, InfluenceMapType mapType);

		public abstract MoveBase GetMove();
	}
}