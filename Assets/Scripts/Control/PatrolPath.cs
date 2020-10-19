using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{	
	public class PatrolPath : MonoBehaviour 
	{
		private void OnDrawGizmos() 
		{
			const float waypointGizmoRadius = 0.3f;

			for (int i = 0; i < transform.childCount; i++)
			{
				// transform.GetChild(i);;
				Gizmos.color = Color.green;
				Gizmos.DrawSphere(transform.GetChild(i).position, waypointGizmoRadius);
			}
		}
	}
}
