using System;
using System.Collections;
using System.Linq;
using BTween;
using Tactics.Entities;
using UnityEngine;

namespace Tactics.DamageSystem
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

		public void TakeDamage(DamageDescription damageDescription, ref Playback.Playback damagePlayback)
		{
			if (!ImmuneToDamageTypes.Contains(damageDescription.DamageType))
			{
				_health -= damageDescription.Amount;
				if (_health <= 0)
				{
					_health = 0;
					//todo... handle this dependency properly.
					//If animating with a tween... cancel that?
					GetComponent<Agent>().Die();
				}
				else
				{
					//oof
					_health -= damageDescription.Amount;

					var tween = transform.BScaleTo(new Vector3(1.2f, 1.2f, 1.2f), 0.1f, Ease.EaseInQuad).Then(transform.BScaleTo(Vector3.one, 0.1f, Ease.EaseOutQuad));
					damagePlayback.AddTween(tween);
				}
			}
			else
			{
				//sheen sheen sparkle sparkle
			}
		}
	}
}