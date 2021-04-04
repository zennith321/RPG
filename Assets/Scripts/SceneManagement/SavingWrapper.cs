using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.SceneManagement
{
	public class SavingWrapper : MonoBehaviour
	{
		const string defaultSaveFile = "save";
		[SerializeField] float fadeInTime = 0.2f;
		[SerializeField] float fadeWaitTime = .01f;

		private void Awake() 
		{
			StartCoroutine(LoadLastScene());	
		}

		IEnumerator LoadLastScene() 
		{
			Fader fader = FindObjectOfType<Fader>();
			fader.FadeOutImmediate();
			yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
			yield return new WaitForSeconds(fadeWaitTime);
			yield return fader.FadeIn(fadeInTime);
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.L))
			{
				Load();
			}
			if (Input.GetKeyDown(KeyCode.S))
			{
				Save();
			}
			if (Input.GetKeyDown(KeyCode.O))
			{
				Delete();
			}
		}

		public void Save()
		{
			GetComponent<SavingSystem>().Save(defaultSaveFile);
		}

		public void Load()
		{
			GetComponent<SavingSystem>().Load(defaultSaveFile);
		}

		private void Delete()
		{
			GetComponent<SavingSystem>().Delete(defaultSaveFile);
			print("Deleted save file!");
		}
	}
}
