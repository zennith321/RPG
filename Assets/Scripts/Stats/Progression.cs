using UnityEngine;

namespace RPG.Stats
{
	[CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
	public class Progression : ScriptableObject
	{
		[SerializeField] ProgressionCharacterClass[] characterClasses = null;

		public float GetStat(Stat stat, CharacterClass characterClass, int level)
		{
			foreach(ProgressionCharacterClass progressionClass in characterClasses)
			{
				if (progressionClass.characterClass != characterClass) continue;
				
				foreach (ProgressionStat progressionStat in progressionClass.stats) //TODO review all this nonsense to try to understand it, lecture 136
				{
					if (progressionStat.stat != stat) continue;

					if(progressionStat.levels.Length < level)	continue;

					return progressionStat.levels[level - 1];
				}
			}
			return 0;
		}

		[System.Serializable]
		class ProgressionCharacterClass
		{
			public CharacterClass characterClass;
			public ProgressionStat[] stats;
			//public float[] health;
		}

		[System.Serializable]
		class ProgressionStat
		{
			public Stat stat;
			public float[] levels;
		}
	}
}