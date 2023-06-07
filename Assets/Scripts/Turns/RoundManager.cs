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

		public bool gameOver = false;
		private bool waitingForPlayer = false;
		private int round = 0;

		private void Awake()
		{
			round = 0;
		}

		[ContextMenu("Start Round")]
		public void StartRound()
		{
			if (round == 0)
			{
				StartCoroutine(ExecuteRound());//store reference to coroutine and status as a state machine.
			}
			else
			{
				waitingForPlayer = false;
			}
			
		}
		
		public IEnumerator ExecuteRound()
		{
			while (!gameOver)
			{
				Debug.Log($"--Round {round} Prepare Phase--");
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
				
				//Does PrepareTurn turn on the previews?

				round++;
				waitingForPlayer = true;

				while (waitingForPlayer)
				{
					yield return null;
				}

				

				Debug.Log("Action!");
				foreach (var faction in AgentCollections)
				{
					yield return StartCoroutine(faction.TakeTurn());
				}

				//hacky temp check for game over.
				foreach (var faction in AgentCollections)
				{
					if (faction.Count == 0)
					{
						gameOver = true;
						break;
					}
				}
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