using System.Collections.Generic;
using System.Linq;
using BTween;

namespace Tactics.Playback
{
	/// <summary>
	/// A playback is a element that hold references to animations, coroutines, tweens, camera moves, UI effects, and more. It can be started, skipped to end, and so on.
	/// </summary>
	public class Playback
	{
		private List<Tween> _tweens = new List<Tween>();

		public void AddTween(Tween tween)
		{
			_tweens.Add(tween);
		}

		public bool IsRunning()
		{
			if (_tweens.Any(x => x.Running))
			{
				return true;
			}

			return false;
		}
	}
}