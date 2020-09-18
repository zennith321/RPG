using UnityEngine;

namespace RPG.Combat
{
	public class Health : MonoBehaviour
	{
		[SerializeField] float health = 100f;

		public void TakeDamage(float damage)
		{
			//health = Mathf.Max(health - damage, 0); //one line way of incrementing damage
			health -= damage;
			if(health < 0f) {health = 0f;}
			print(health);
		}
	}		
}