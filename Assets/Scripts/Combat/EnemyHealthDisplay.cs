using System;
using RPG.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
	public class EnemyHealthDisplay : MonoBehaviour
	{
		Fighter playerFighter;
		//TODO have fader fade this out
		private void Awake() 
		{
			playerFighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
		}

		private void Update() 
		{
			Health target = playerFighter.GetTarget();
			if (target == null)
			{
				GetComponent<Text>().text = "No Target";
			}
			else
			{
				GetComponent<Text>().text = String.Format("{0:0}/{1:0}", target.GetHealthPoints(), target.GetMaxHealthPoints());
			}
		}
	}
}