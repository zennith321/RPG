using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Control;

namespace RPG.Cinematics
{
	public class CinematicControlRemover : MonoBehaviour
	{
		GameObject player = null;
		PlayerController playerController = null;
		ActionScheduler actionScheduler = null;

		private void Awake() 
		{
			player = GameObject.FindWithTag("Player");
			playerController = player.GetComponent<PlayerController>();
			actionScheduler = player.GetComponent<ActionScheduler>();	
		}

		private void Start() 
		{
			
		}	

		private void OnEnable() 
		{
			GetComponent<PlayableDirector>().played += DisableControl;
			GetComponent<PlayableDirector>().stopped += EnableControl;
		}

		private void OnDisable()
		{
			GetComponent<PlayableDirector>().played -= DisableControl;
			GetComponent<PlayableDirector>().stopped -= EnableControl;
		}

		void DisableControl(PlayableDirector playableDirector)
		{
			actionScheduler.CancelCurrentAction();
			playerController.enabled = false;
		}

		void EnableControl(PlayableDirector playableDirector)
		{
			playerController.enabled = true;
		}
	}
}
