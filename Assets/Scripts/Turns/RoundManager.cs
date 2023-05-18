using System;
using System.Collections;
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
		
		public AgentCollection[] AgentCollections;

		[ContextMenu("Start Round")]
		void StartRound()
		{
			StartCoroutine(ExecuteRound());
		}
		
		public IEnumerator ExecuteRound()
		{
			foreach (var ac in AgentCollections)
			{
				ac.PrepareTurn();
			}

			foreach (var ac in AgentCollections)
			{
				yield return StartCoroutine(ac.TakeTurn());
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