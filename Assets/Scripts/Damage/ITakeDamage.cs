using System.Collections;
using BTween;
using UnityEngine;

namespace Tactics.DamageSystem
{
	public interface ITakeDamage
	{
		public void TakeDamage(DamageDescription damageDescription, ref Playback.Playback reactionPlayback);
	}
}