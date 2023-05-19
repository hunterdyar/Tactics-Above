using System.Collections;
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
		private DamageDescription _damageDescription;
		public AttackOnShape(Agent agent, ScriptableShape shape, Vector2Int facingDirection,DamageDescription damage) : base(agent)
		{
			_shape = shape;
			_facingDirection = facingDirection;
			_damageDescription = damage;
		}

		public override IEnumerator DoMove()
		{
			var nodes = _shape.GetNodesOnTilemapInFacingDirection(_agent.CurrentNode, _facingDirection);
			
			//todo: replace tweens with some kind of "playback" wrapper/interface that we can add tweens, Animations, and more to, and it can play, be skipped, speed up, etc.
			
			Playback.Playback movePlayback = new Playback.Playback();

			foreach (var node in nodes)
			{
				Damage.DealDamageToNode(node,_damageDescription, ref movePlayback);
			}

			while (movePlayback.IsRunning())
			{
				yield return null;
			}

		}
	}
}