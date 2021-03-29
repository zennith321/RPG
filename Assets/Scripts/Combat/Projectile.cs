using System;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{		
	public class Projectile : MonoBehaviour 
	{
		[SerializeField] float speed = 1f;
		Health target = null;
		float damage = 0;

		private void Update() 
		{
			if(target == null) return;

			transform.LookAt(GetAimLocation());
			transform.Translate(Vector3.forward * speed * Time.deltaTime);
		}

		public void SetTarget(Health target, float damage)
		{
			this.target = target;
			this.damage = damage;
		}

		private Vector3 GetAimLocation()
		{
			CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
			if (targetCapsule == null)
			{
				return target.transform.position;
			}
			return target.transform.position + Vector3.up * targetCapsule.height / 2; //Aim at the middle of the capsule collider
		}

		private void OnTriggerEnter(Collider other) 
		{
			//is collider other of type health target
			if(other.GetComponent<Health>() != target) return;
			target.TakeDamage(damage);
			Destroy(this.gameObject);
		}
	}
}