using System;
using System.Collections;
using System.Linq;
using BTween;
using Tactics.Entities;
using UnityEngine;

namespace Tactics.DamageSystem
{
	[RequireComponent(typeof(Agent))]

	public class Health : MonoBehaviour, ITakeDamage
	{
		[SerializeField] private int StartingHealth = 3;
		public int CurrentHealth => _health;
		private int _health = 3;
		private Agent _agent;

		[SerializeField] private DamageType[] ImmuneToDamageTypes;
		[Tooltip("If this entity is weak to a damage type, it will take double damage from that type.")]
		[SerializeField] private DamageType[] WeakToDamageTypes;

		[Tooltip("If this entity is strong to a damage type, it will take one less damage from that type.")] [SerializeField]
		private DamageType[] StrongToDamageTypes;
		private void Awake()
		{
			_health = StartingHealth;
			_agent = GetComponent<Agent>();
		}

		public void TakeDamage(DamageDescription damageDescription, ref Playback.Playback damagePlayback)
		{
			if (!ImmuneToDamageTypes.Contains(damageDescription.DamageType))
			{
				if (WeakToDamageTypes.Contains(damageDescription.DamageType))
				{
					damageDescription.Amount *= 2;
				}

				if (StrongToDamageTypes.Contains(damageDescription.DamageType))
				{
					damageDescription.Amount = Mathf.Max(damageDescription.Amount - 1,0);
				}
				_health -= damageDescription.Amount;
				if (_health <= 0)
				{
					_health = 0;
					//If animating with a tween... cancel that?
					_agent.Die();
				}
				else
				{
					//oof
					var tween = transform.BScaleTo(new Vector3(1.2f, 1.2f, 1.2f), 0.1f, Ease.EaseInQuad).Then(transform.BScaleTo(Vector3.one, 0.1f, Ease.EaseOutQuad));
					damagePlayback.AddTween(tween);
				}
			}
			else
			{
				damageDescription.Amount = 0;
				//sheen sheen sparkle sparkle
			}
		}
	}
}