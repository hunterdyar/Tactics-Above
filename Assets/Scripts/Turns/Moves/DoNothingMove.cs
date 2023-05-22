using System.Collections;
using Tactics.Entities;
using UnityEngine;

namespace Tactics.Turns
{
	//This may seem silly, because it is. BUT - we probably want to still end up triggering animations and cameras and a little delay.
	
	public class DoNothingMove : MoveBase
	{
		public DoNothingMove(Agent agent) : base(agent)
		{
		}

		public override IEnumerator DoMove()
		{
			Debug.Log($"{_agent} doing nothing.");
			yield return new WaitForSeconds(0.1f);
		}
	}
}