using System;
using System.Collections;
using Tactics.AI;
using Tactics.Entities;
using UnityEngine;

namespace Tactics.Turns
{
	public class RoundManager : MonoBehaviour
	{
		//turn this into interface, serialize by reference? We probably don't need to serialize all of that, we can hard code it.
		//These agents, then the player, then these agents...
		
		//the player is one of these (ITurnTaker), but they won't be an agentCollection.
		
		//I might have a wrapper object too that would handle events and names and things for like "Player Turn" etc.
		
		public Faction[] AgentCollections;

		[ContextMenu("Start Round")]
		public void StartRound()
		{
			StartCoroutine(ExecuteRound());
		}
		
		public IEnumerator ExecuteRound()
		{
			//Initiate everything for AIContext.
			foreach (var faction in AgentCollections)
			{
				var enemies = Array.FindAll(AgentCollections, x => x != faction);
				faction.PrepareKnowledge(enemies);
			}

			//decide moves.
			foreach (var ac in AgentCollections)
			{
				ac.PrepareTurn();
			}

			foreach (var faction in AgentCollections)
			{
				yield return StartCoroutine(faction.TakeTurn());
			}
		}

#if UNITY_EDITOR
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.N))
			{
				StartRound();
			}
		}	
#endif
		
	}
}