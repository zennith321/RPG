using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using UnityEngine;

namespace RPG.Control
{
	public class AIController : MonoBehaviour 
	{
		[SerializeField] float chaseDistance = 5f;
		GameObject player;
		Health health;
		Fighter fighter;

		private void Start() {
			fighter = GetComponent<Fighter>();
			health = GetComponent<Health>();
			player = GameObject.FindWithTag("Player");
		}
		
		private void Update()
		{
			if (health.IsDead()) return;
			
			if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
			{
				fighter.Attack(player);
			}
			else
			{
				fighter.Cancel();
			}
		}

		private bool InAttackRangeOfPlayer()
		{
			float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
			return distanceToPlayer < chaseDistance;
		}
		// Called by Unity
		private void OnDrawGizmosSelected() {
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, chaseDistance); 
		}
	}
}