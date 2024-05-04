﻿using System.Diagnostics;

namespace Invasion1D.Models
{
	public class Enemy : Character
	{
		App Game => (App)App.Current!;
		public Enemy(
			Dimension shape,
			double position,
			double speed) :
				base(
					shape,
					position,
					GetResourcesColor(nameof(Enemy))!,
					speed)
		{
			direction = ((App)Application.Current!).RandomDirection();
		}

		public override void Attack()
		{
			throw new NotImplementedException();
		}

		public override void TakeDamage(double damage)
		{
			health -= damage;
			if (health > 0)
			{
				return;
			}

			Game.universe.enemyCount--;
			toDispose = true;
		}

		public override void Move()
		{
			List<Type> ignore = [];

			Interactive? target = FindInteractive(out double distanceFromTarget, ignoreTypes: [.. ignore]);
			double tryStep = stepDistance;
			if (distanceFromTarget < tryStep)
			{
				if (target is Item item)
				{
					if (!item.Power(this))
					{
						switch (item)
						{
							case Vitalux:
								ignore.Add(typeof(Vitalux));
								break;
							case Warpium:
								ignore.Add(typeof(Warpium));
								break;
						}
					}
				}
				else
				{
					tryStep = distanceFromTarget;
					StopMovement();
				}
			}

			if (direction)
			{
				PercentageInShape += CurrentDimension.GetPercentageFromDistance(tryStep);
			}
			else
			{
				PercentageInShape -= CurrentDimension.GetPercentageFromDistance(tryStep);
			}
		}
	}
}