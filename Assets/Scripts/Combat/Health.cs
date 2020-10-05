using UnityEngine;

namespace RPG.Combat
{
	public class Health : MonoBehaviour
	{
		[SerializeField] float health = 100f;
		bool isDead = false;
		public bool IsDead()
		{
			return isDead;
		}

		public void TakeDamage(float damage)
		{
			//health = Mathf.Max(health - damage, 0); //one line method of incrementing damage
			health -= damage;
			if(health <= 0f)
			{
				health = 0f;
				Die();
			}
		}

		private void Die()
		{
			if(isDead) return;
			//GetComponent<Collider>().enabled = false;
			isDead = true;
			GetComponent<Animator>().SetTrigger("die");
		}
	}		
}