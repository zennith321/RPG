using RPG.Movement;
using UnityEngine;
using RPG.Core;


namespace RPG.Combat
{
	public class Fighter : MonoBehaviour, IAction
	{
		[SerializeField] float timeBetweenAttacks = 1f;
		[SerializeField] Transform rightHandTransform = null;
		[SerializeField] Transform leftHandTransform = null;
		[SerializeField] Weapon defaultWeapon = null;
		[SerializeField] GameObject defaultHitEffect = null;

		Health target;
		float timeSinceLastAttack = Mathf.Infinity;
		Weapon currentWeapon = null;

		private void Start() 
		{
			EquipWeapon(defaultWeapon);
		}

		private void Update()
		{
			timeSinceLastAttack += Time.deltaTime;

			if (target == null) return;
			if (target.IsDead()) return;

			if (!GetIsInRange())
			{
				GetComponent<Mover>().MoveTo(target.transform.position, 1f);
			}
			else
			{
				GetComponent<Mover>().Cancel();
				AttackBehaviour();
			}
		}

		private void AttackBehaviour()
		{
			transform.LookAt(target.transform);
			if (timeSinceLastAttack > timeBetweenAttacks)
			{
				// This will trigger the Hit() event.
				TriggerAttack();
				timeSinceLastAttack = 0;
			}
		}

		private void TriggerAttack()
		{
			GetComponent<Animator>().ResetTrigger("stopAttack");
			GetComponent<Animator>().SetTrigger("attack");
		}

		// Animation Event
		void Hit()
		{
			if (target == null) return;
			if (currentWeapon.HasProjectile())
			{
				currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target);
			}
			else
			{
				target.TakeDamage(currentWeapon.GetWeaponDamage());
				if (defaultHitEffect != null) Instantiate(defaultHitEffect, GetAimLocation(), transform.rotation);
			}
		}

		void Shoot()
		{
			Hit();
		}

		private bool GetIsInRange()
		{
			return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.GetWeaponRange();
		}

		public void Attack(GameObject combatTarget)
		{
			GetComponent<ActionScheduler>().StartAction(this);
			target = combatTarget.GetComponent<Health>();
		}

		public bool CanAttack(GameObject combatTarget)
		{
			if (combatTarget == null) return false;
			Health targetToTest = combatTarget.GetComponent<Health>();
			return targetToTest != null && !targetToTest.IsDead();
		}

		public void Cancel()
		{
			StopAttack();
			target = null;
			GetComponent<Mover>().Cancel();
		
		}
		public void EquipWeapon(Weapon weapon)
		{
			currentWeapon = weapon;
			Animator animator = GetComponent<Animator>();
			weapon.Spawn(rightHandTransform, leftHandTransform, animator);
		}

		private void StopAttack()
		{
			GetComponent<Animator>().ResetTrigger("attack");
			GetComponent<Animator>().SetTrigger("stopAttack");
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
	}
}