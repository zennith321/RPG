using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{    
  public class AIController : MonoBehaviour 
  {
    [SerializeField] float chaseDistance = 5f;
    
    private void Update() 
    {
      GameObject player = GameObject.FindWithTag("Player");
      //calculate distance here
      if (//is within distance)
      {
        print(//something + "Should give chase.");
      }
    }
  }
}