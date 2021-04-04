using RPG.Saving;
using UnityEngine;
using System;

namespace RPG.Stats
{
	public class Experience : MonoBehaviour, ISaveable
	{
		[SerializeField] float experiencePoints = 0;

		//public delegate void ExperienceGainedDelegate();
		public event Action onExperienceGained; // Action is a predefined delegate with no return value

		public void GainExperience(float experience)
		{
			experiencePoints += experience;
			onExperienceGained();
		}

		public float GetExperience()
		{
			return experiencePoints;
		}

		public object CaptureState()
		{
			return experiencePoints;
		}

		public void RestoreState(object state)
		{
			experiencePoints = (float)state;
		}
	}
}