using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
  [SerializeField] Transform target;
  NavMeshAgent agent;

  // Start is called before the first frame update
  void Start()
  {
    agent = gameObject.GetComponent<NavMeshAgent>();
  }

  // Update is called once per frame
  void Update()
  {
    UpdateAnimator();
    //Debug.DrawRay(lastRay.origin, lastRay.direction * 100);
  }

  public void MoveTo(Vector3 destination)
  {
  agent.SetDestination(destination);
  }

  private void UpdateAnimator()
  {
    Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
    Vector3 localVelocity = transform.InverseTransformDirection(velocity);
    float speed = localVelocity.z;

    GetComponent<Animator>().SetFloat("forwardSpeed", speed); //string reference
  }
}
