using System.Collections;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
	public class WeaponPickup : MonoBehaviour, IRaycastable
	{
		[SerializeField] WeaponConfig weapon = null;
		[SerializeField] float respawnTime = 5;
		private void OnTriggerEnter(Collider other)
		{
			if (other.tag == "Player")
			{
				Pickup(other.GetComponent<Fighter>());
			}
		}

		private void Pickup(Fighter fighter)
		{
			fighter.EquipWeapon(weapon);
			StartCoroutine(HideForSeconds(respawnTime));
		}

		private IEnumerator HideForSeconds(float seconds)
		{
			ShowPickup(false);
			yield return new WaitForSeconds(seconds);
			ShowPickup(true);
		}

		private void HidePickup()
		{
			GetComponent<Collider>().enabled = false;
			foreach(Transform child in this.gameObject.transform)
			{
				child.gameObject.SetActive(false);
			}
		}

		private void ShowPickup(bool shouldShow)
		{
			GetComponent<Collider>().enabled = shouldShow;
			foreach (Transform child in transform)
			{
				child.gameObject.SetActive(shouldShow);
			}
		}

		public bool HandleRaycast(PlayerController callingController)
		{
			if (Input.GetMouseButtonDown(0))
			{
				Pickup(callingController.GetComponent<Fighter>());
			}
			return true;
		}

		public CursorType GetCursorType()
		{
			return CursorType.Pickup;
		}
	}
}
