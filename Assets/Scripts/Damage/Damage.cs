using Tactics;
using Tactics.Turns;

namespace Tactics.Damage
{
	//Convenience Wrapper class. I think it will end up making sense to make this a mono-behaviour that also handles animation of damage taking place.
	public class Damage
	{
		public static void DealDamageToNode(NavNode targetNode, int amount, DamageType dType)
		{
			foreach (var entity in targetNode.NavMap.GetAllEntitiesOnNode(targetNode))
			{
				//Flash Node with color animation. Maybe Damage 
				if (entity.TryGetComponent<ITakeDamage>(out var target))
				{
					target.TakeDamage(amount,dType);
				}
			}
		}
	}
}