﻿using System.Collections;
using Attacks;
using BTween;
using Tactics.DamageSystem;
using Tactics.Entities;
using Tactics.GridShapes;
using UnityEngine;

namespace Tactics.Turns
{
	public class AttackOnShape : MoveBase
	{
		private ScriptableShape _shape;
		private Vector2Int _facingDirection;
		private Attack _attack;
		public AttackOnShape(Agent agent, ScriptableShape shape, Vector2Int facingDirection,Attack attack) : base(agent)
		{
			_shape = shape;
			_facingDirection = facingDirection;
			_attack = attack;
		}

		public override IEnumerator DoMove()
		{
			var nodes = _shape.GetNodesOnTilemapInFacingDirection(_agent.CurrentNode, _facingDirection);
			
			Playback.Playback movePlayback = new Playback.Playback();
			Debug.Log(_agent.name+ " is attacking " + nodes.Count + " nodes with " + _attack.name);
			foreach (var node in nodes)
			{
				Damage.DealDamageToNode(node,_attack, ref movePlayback);
			}
			movePlayback.Start();
			while (movePlayback.IsRunning())
			{
				yield return null;
			}

		}
	}
}