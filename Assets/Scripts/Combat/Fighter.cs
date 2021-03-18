using RPG.Movement;
using UnityEngine;
using RPG.Core;


namespace RPG.Combat
{
	public class Fighter : MonoBehaviour, IAction
	{
		[SerializeField] float weaponRange = 2f;
		[SerializeField] float timeBetweenAttacks = 1f;
		[SerializeField] float weaponDamage = 5f;
		[SerializeField] Transform handTransform = null;
		[SerializeField] Weapon weapon = null;
		Health target;
		float timeSinceLastAttack = Mathf.Infinity;

		private void Start() 
		{
			SpawnWeapon();
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
			target.TakeDamage(weaponDamage);
		}

		private bool GetIsInRange()
		{
			return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
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

		private void StopAttack()
		{
			GetComponent<Animator>().ResetTrigger("attack");
			GetComponent<Animator>().SetTrigger("stopAttack");
		}

		private void SpawnWeapon()
		{
			if (weapon == null) return;
			Animator animator = GetComponent<Animator>();
			weapon.Spawn(handTransform, animator);
		}
	}
}