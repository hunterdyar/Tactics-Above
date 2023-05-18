using System;
using System.Linq;
using Tactics.Entities;
using UnityEngine;

namespace Tactics.Damage
{
	public class Health : MonoBehaviour, ITakeDamage
	{
		[SerializeField] private int StartingHealth = 3;
		public int CurrentHealth => _health;
		private int _health = 3;

		[SerializeField] private DamageType[] ImmuneToDamageTypes;
		private void Awake()
		{
			_health = StartingHealth;
		}

		public void TakeDamage(int amount, DamageType type)
		{
			if (!ImmuneToDamageTypes.Contains(type))
			{
				//oof
				_health -= amount;
				if (_health < 0)
				{
					_health = 0;
					//todo... handle this dependency properly.
					GetComponent<Agent>().Die();
				}
			}
			else
			{
				//sheen sheen sparkle sparkle
			}
		}
	}
}