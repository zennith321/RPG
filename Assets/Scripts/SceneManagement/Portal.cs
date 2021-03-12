using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
	public class Portal : MonoBehaviour 
	{
		[SerializeField] int sceneToLoad = -1;

		// bool isTriggered = false;

		private void OnTriggerEnter(Collider other)
		{
			// if (isTriggered) return;
			if (other.tag == "Player")
			{
				StartSequence();
			}
		}

		private void StartSequence()
		{
			// isTriggered = true;
			SceneManager.LoadScene(sceneToLoad);
		}
	}
}