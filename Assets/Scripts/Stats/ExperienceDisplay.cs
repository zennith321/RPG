using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
	public class ExperienceDisplay : MonoBehaviour
	{
		Experience experience;
    //TODO have fader fade this out
		private void Awake() 
		{
			experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
		}

		private void Update() 
		{
			GetComponent<Text>().text = String.Format("{0:0}", experience.GetExperience());
		}
	}
}