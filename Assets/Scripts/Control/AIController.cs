using GameDevTV.Utils;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;
using UnityEngine;
using System;

namespace RPG.Control
{
	public class AIController : MonoBehaviour 
	{
		[SerializeField] float chaseDistance = 5f;
		[SerializeField] float suspicionTime = 3f;
		[SerializeField] float aggroCooldownTime = 5f;
		[SerializeField] PatrolPath patrolPath;
		[SerializeField] float waypointTolerance = 1f;
		[SerializeField] float waypointDwellTime = 2f;
		[Range(0,1)]
		[SerializeField] float patrolSpeedFraction = 0.2f;
		[SerializeField] float shoutDistance = 5f;
		
		GameObject player;
		ActionScheduler actionScheduler;
		Health health;
		Fighter fighter;
		Mover mover;

		LazyValue<Vector3> guardPosition;
		float timeSinceLastSawPlayer = Mathf.Infinity;
		float timeSinceArrivedAtWaypoint = Mathf.Infinity;
		float timeSinceAggrevated = Mathf.Infinity;
		int currentWaypointIndex = 0;

		private void Awake() 
		{
			actionScheduler = GetComponent<ActionScheduler>();
			fighter = GetComponent<Fighter>();
			health = GetComponent<Health>();
			mover = GetComponent<Mover>();
			player = GameObject.FindWithTag("Player");

			guardPosition = new LazyValue<Vector3>(GetGuardPosition);
		}

		private Vector3 GetGuardPosition()
		{
			return transform.position;
		}

		private void Start() 
		{
			guardPosition.ForceInit();
		}
		
		private void Update()
		{
			if (health.IsDead()) return;

			if (IsAggrevated() && fighter.CanAttack(player))
			{
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

			UpdateTimers();
		}

		private void UpdateTimers()
		{
			timeSinceLastSawPlayer += Time.deltaTime;
			timeSinceArrivedAtWaypoint += Time.deltaTime;
			timeSinceAggrevated += Time.deltaTime;
		}

		private void PatrolBehaviour()
		{
			Vector3 nextPosition = guardPosition.value;

			if(patrolPath != null)
			{
				if(AtWaypoint())
				{
					timeSinceArrivedAtWaypoint = 0;
					CycleWaypoint();
				}
				nextPosition = GetCurrentWaypoint();
			}

			if (timeSinceArrivedAtWaypoint > waypointDwellTime)
			{
				mover.StartMoveAction(nextPosition, patrolSpeedFraction);
			}
		}

		public void Aggrevate()
		{
			timeSinceAggrevated = 0;
		}

		private bool AtWaypoint()
		{
			float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
			return distanceToWaypoint < waypointTolerance;
		}

		private void CycleWaypoint()
		{
			currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
			timeSinceArrivedAtWaypoint = 0f;
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
			timeSinceLastSawPlayer = 0;
			fighter.Attack(player);

			AggrevateNearbyEnemies();
		}

		private void AggrevateNearbyEnemies()
		{
			RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
			foreach (RaycastHit hit in hits)
			{
				AIController ai = hit.collider.GetComponent<AIController>();
				if (ai == null) continue;

				ai.Aggrevate();
			}
		}

		private bool IsAggrevated()
		{
			float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
			return distanceToPlayer < chaseDistance || timeSinceAggrevated < aggroCooldownTime;
		}
		// Called by Unity
		private void OnDrawGizmosSelected() {
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, chaseDistance); 
		}
	}
}