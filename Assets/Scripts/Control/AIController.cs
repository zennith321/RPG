using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Resources;
using UnityEngine;

namespace RPG.Control
{
	public class AIController : MonoBehaviour 
	{
		[SerializeField] float chaseDistance = 5f;
		[SerializeField] float suspicionTime = 3f;
		[SerializeField] PatrolPath patrolPath;
		[SerializeField] float waypointTolerance = 1f;
		[SerializeField] float waypointDwellTime = 2f;
		[Range(0,1)]
		[SerializeField] float patrolSpeedFraction = 0.2f;
		
		GameObject player;
		ActionScheduler actionScheduler;
		Health health;
		Fighter fighter;
		Mover mover;

		Vector3 guardPosition;
		float timeSinceLastSawPlayer = Mathf.Infinity;
		float timeSinceArrivedAtWaypoint = Mathf.Infinity;
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
		}

		private void PatrolBehaviour()
		{
			Vector3 nextPosition = guardPosition;

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