using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
	public class AIController : MonoBehaviour 
	{
		[SerializeField] float chaseDistance = 5f;
		[SerializeField] float suspicionTime = 3f;
		[SerializeField] PatrolPath patrolPath;
		[SerializeField] float waypointTolerance = 1f;
		
		GameObject player;
		ActionScheduler actionScheduler;
		Health health;
		Fighter fighter;
		Mover mover;

		Vector3 guardPosition;
		float timeSinceLastSawPlayer = Mathf.Infinity;
		int currentWaypointIndex = 0;

		private void Start() {
			actionScheduler = GetComponent<ActionScheduler>();
			fighter = GetComponent<Fighter>();
			health = GetComponent<Health>();
			mover = GetComponent<Mover>();
			player = GameObject.FindWithTag("Player");

			guardPosition = transform.position;
		}
		
		private void Update()
		{
			if (health.IsDead()) return;
			
			if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
			{
				timeSinceLastSawPlayer = 0;
				AttackBehaviour();
			}
			else if (timeSinceLastSawPlayer < suspicionTime)
			{
				SuspicionBehaviour();
			}
			else
			{
				PatrolBehaviour();
			}

			timeSinceLastSawPlayer += Time.deltaTime;
		}

		private void PatrolBehaviour()
		{
			Vector3 nextPosition = guardPosition;

			if(patrolPath != null)
			{
				if(AtWaypoint())
				{
					CycleWaypoint();
				}
				nextPosition = GetCurrentWaypoint();
			}

			mover.StartMoveAction(nextPosition);
		}

		private bool AtWaypoint()
		{
			float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
			return distanceToWaypoint < waypointTolerance;
		}

		private void CycleWaypoint()
		{
			currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
		}

		private Vector3 GetCurrentWaypoint()
		{
			return patrolPath.GetWaypoint(currentWaypointIndex);
		}

		private void SuspicionBehaviour()
		{
			actionScheduler.CancelCurrentAction();
		}

		private void AttackBehaviour()
		{
			fighter.Attack(player);
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