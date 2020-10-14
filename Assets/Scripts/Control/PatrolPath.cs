using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{	
	public class PatrolPath : MonoBehaviour 
	{
		private void OnDrawGizmos() 
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				// transform.GetChild(i);;
				Gizmos.color = Color.green;
				Gizmos.DrawSphere(transform.GetChild(i), 1f);
			}
		}
	}
}
