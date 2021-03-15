using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
	public class CinematicTrigger : MonoBehaviour, ISaveable
	{
		bool isTriggered = false;

		private void OnTriggerEnter(Collider other) 
		{
			if (isTriggered) return;
			if (other.tag == "Player")
			{
				StartSequence();
			}
		}

		private void StartSequence()
		{
			GetComponent<PlayableDirector>().Play();
			isTriggered = true;
		}

		public object CaptureState()
		{
			return isTriggered;
		}

		public void RestoreState(object state)
		{
			isTriggered = (bool)state;
		}
	}
}
