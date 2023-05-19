using System;

namespace Tactics.DamageSystem
{
	[Serializable]
	public struct DamageDescription
	{
		public int Amount;
		public DamageType DamageType;
	}
}