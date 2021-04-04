using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;

namespace RPG.Resources
{
	public class Health : MonoBehaviour, ISaveable
	{
		float healthPoints = -1f;
		float regenerationPercentage = 70;
		bool isDead = false;

		private void Start() 
		{
			if (healthPoints < 0)
			{
				healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
			}
			Experience experience = GetComponent<Experience>();
			if (experience != null)
			{
				//experience.onExperienceGained += UpdateLevel;
				GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
			}
		}

		public bool IsDead()
		{
			return isDead;
		}

		public void TakeDamage(GameObject instigator, float damage)
		{
			healthPoints = Mathf.Max(healthPoints - damage, 0);
			if (healthPoints == 0)
			{
				Die();
				AwardExperience(instigator);
			}
		}

		public float GetPercentage()
		{
			return 100 * (healthPoints / GetComponent<BaseStats>().GetStat(Stat.Health));
		}

		private void Die()
		{
			if (isDead) return;
			//GetComponent<Collider>().enabled = false;
			isDead = true;
			GetComponent<Animator>().SetTrigger("die");
			GetComponent<ActionScheduler>().CancelCurrentAction();
		}

		private void AwardExperience(GameObject instigator)
		{
			Experience experience = instigator.GetComponent<Experience>();
			if (experience == null) return;
			experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
		}

		private void RegenerateHealth()
		{
			float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
			healthPoints = Mathf.Max(healthPoints, regenHealthPoints);
		}

		public object CaptureState()
		{
			return healthPoints;
		}

		public void RestoreState(object state)
		{
			healthPoints = (float)state;

			if (healthPoints == 0)
			{
				Die();
			}
		}
	}		
}