using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
	public class Mover : MonoBehaviour
	{
		[SerializeField] Transform target;
		NavMeshAgent navMeshAgent;

		void Start()
		{
			navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
		}

		void Update()
		{
			UpdateAnimator();
			//Debug.DrawRay(lastRay.origin, lastRay.direction * 100);
		}

		public void MoveTo(Vector3 destination)
		{
			navMeshAgent.SetDestination(destination);
			navMeshAgent.isStopped = false;
			if(Vector3.Distance(destination, gameObject.transform.position) <= 2f) //TODO put in weapon range
			{
				Stop();
			}
		}

		public void Stop()
		{
			navMeshAgent.isStopped = true;
		}

		private void UpdateAnimator()
		{
			Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
			Vector3 localVelocity = transform.InverseTransformDirection(velocity);
			float speed = localVelocity.z;

			GetComponent<Animator>().SetFloat("forwardSpeed", speed); //string reference
		}
	}
}