using UnityEngine;

namespace RPG.Combat
{
	public class Health : MonoBehaviour
	{
		[SerializeField] float health = 100f;
		bool isDead = false;

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
			isDead = true;
			GetComponent<Animator>().SetTrigger("die");
		}
	}		
}